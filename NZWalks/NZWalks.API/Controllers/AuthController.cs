using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;

        //Injecting user repo inside constructor
        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest )
        {
            //Validate the incoming request 
            //Used Fluent Validation here

            //Check if the user is authenticated.
            //Check UserName and Password.
            var user = await userRepository.AuthenticateAsync(
                loginRequest.UserName, loginRequest.Password);

            if (user != null)
            {
                // Create JWT Token
                var Token = await tokenHandler.CreateTokenAsync(user);
                return Ok(Token);

            }
            //return bad request
            return BadRequest("Username or password incorrect");

        }
    }
}
