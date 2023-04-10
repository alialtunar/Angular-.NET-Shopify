using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServerApp.Data;
using ServerApp.DTO;
using ServerApp.Models;
//using ServerApp.Models;

namespace ServerApp.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUserRepository _userRepository;
         private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager,SignInManager<User> signInManager,IConfiguration configuration,RoleManager<Role> roleManager,IUserRepository userRepository,IMapper mapper)
        {
            _userManager=userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _userRepository = userRepository;
             _mapper = mapper;
        }


     
     [HttpGet("{id}")]
    public async Task<ActionResult> GetUserById(int id)
    {
        var userDto = await _userRepository.GetUserById(id);

        if (userDto == null)
        {
            return NotFound();
        }

        return Ok(userDto);
    }


[HttpPut("{id}")]
[Consumes("multipart/form-data")]
public async Task<IActionResult> UpdateUser(int id, [FromForm] UserForUpdateDTO userForUpdateDTO, [FromForm] IFormFile profilImage)
{
    try
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            return NotFound();
        }

        if (profilImage != null && profilImage.Length > 0)
        {
            var extension = Path.GetExtension(profilImage.FileName);
            var randomName = $"{Guid.NewGuid()}{extension}";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", randomName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await profilImage.CopyToAsync(stream);
            }

            userForUpdateDTO.ImageUrl = randomName;
        }

        _mapper.Map(userForUpdateDTO, user);

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return NoContent();
        }

        // If update fails, retrieve detailed error messages
        var errors = result.Errors.Select(e => e.Description).ToList();

        // Construct error message
        var errorMessage = $"Update failed for user with ID {id}. Errors: {string.Join(",", errors)}";

        return BadRequest(errorMessage);
    }
    catch (FileNotFoundException ex)
    {
        // Handle the exception when the file is not found
        return BadRequest($"File not found. Exception message: {ex.Message}");
    }
    catch (DirectoryNotFoundException ex)
    {
        // Handle the exception when the directory is not found
        return BadRequest($"Directory not found. Exception message: {ex.Message}");
    }
    catch (PathTooLongException ex)
    {
        // Handle the exception when the path is too long
        return BadRequest($"Path too long. Exception message: {ex.Message}");
    }
    catch (IOException ex)
    {
        // Handle the exception when there is an I/O error
        return StatusCode(StatusCodes.Status500InternalServerError, $"I/O error. Exception message: {ex.Message}");
    }
    catch (UnauthorizedAccessException ex)
    {
        // Handle the exception when access is denied
        return BadRequest($"Access denied. Exception message: {ex.Message}");
    }
    catch (Exception ex)
    {
        // Handle other types of exceptions that were not anticipated
        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
}





         
[HttpPost("register")]
public async Task<IActionResult> Register(UserForRegisterDTO model)
{
    try
    {
        User user;

        if (model.IsSupplier)
        {
            var supplier = new Supplier
            {
                CompanyName = model.CompanyName,
                Products = new List<Product>()
            };

            user = supplier;
        }
        else
        {
            user = new User();
        }

        user.UserName = model.UserName;
        user.Name = model.Name;
        user.Email = model.Email;
        user.Gender = model.Gender;
        user.Created = DateTime.Now;
        user.IsSupplier = model.IsSupplier;

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        // Assign role to user
        var role = user.IsSupplier ? "Supplier" : "Customer";
        
        // Check if the role exists, and create it if it does not
       if (!await _roleManager.RoleExistsAsync(role))
{
    var newRole = new Role { Name = role };
    await _roleManager.CreateAsync(newRole);
}
        
        // Add the user to the role
        var roleResult = await _userManager.AddToRoleAsync(user, role);
        if (!roleResult.Succeeded)
        {
            return BadRequest(roleResult.Errors);
        }

        // Update security stamp
        user.SecurityStamp = Guid.NewGuid().ToString();
        var stampResult = await _userManager.UpdateAsync(user);
        if (!stampResult.Succeeded)
        {
            return BadRequest(stampResult.Errors);
        }

        return StatusCode(201);
    }
    catch (Exception ex)
    {
        // Handle the exception here, e.g. log the error, send an email, etc.
        return StatusCode(500, ex.Message);
    }
}

        
       [HttpPost("login")] 
      public async Task<IActionResult> Login(UserForLoginDTO model){
          
          var user= await _userManager.FindByNameAsync(model.UserName);
         

          if(user==null){
            return BadRequest(new{
                message="username is incorrect"
            });
          }

         var result= await _signInManager.CheckPasswordSignInAsync(user,model.Password,false);

         List<string> roles = (List<string>)await _userManager.GetRolesAsync(user);
         

         if(result.Succeeded){
            return Ok(new{
              token= GenerateJwtToken(user,roles)
             
            });
         }

         return Unauthorized();

      }

        private string GenerateJwtToken(User user, List<string> roles)
        {
            var tokenHandler= new JwtSecurityTokenHandler();
            var key= Encoding.ASCII.GetBytes( _configuration.GetSection("AppSettings:Secret").Value);
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject= new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.UserName) ,
                    new Claim(ClaimTypes.Role,String.Join(",",roles))

                }),

                
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)

            };
            
         
             

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }







}