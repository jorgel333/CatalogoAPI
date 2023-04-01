using APICatalogo.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly UserManager<IdentityUser>? _userManager;
        private readonly SignInManager<IdentityUser>? _signInManeger;
        private readonly IConfiguration _configuration;
        public AuthorizeController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManeger = signInManager;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return $"AuthorizeController :: Acessado em: " +
                $"{DateTime.Now.ToLongDateString()}";
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(UserDTO model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManeger.SignInAsync(user, false);
            return Ok(GenerateToken(model));
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserDTO userInfo)
        {
            var result = await _signInManeger.PasswordSignInAsync(userInfo.Email,
                userInfo.Password, isPersistent : false, lockoutOnFailure : false);

            if(result.Succeeded)
            {
                return Ok(GenerateToken(userInfo));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login Inválido...");
                return BadRequest(ModelState);
            }
        }

        private UserToken GenerateToken(UserDTO userInfo)
        {
            //define declarações de usuário
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("meuPet", "pipoca"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //Gera uma chave com base no algoritimo simetrico
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            //gera a assinatura digital do token usando o algoritimo Hmac e a chave privada
            var credencials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //tempo de expiração do token
            var expiration = _configuration["TokenConfiguration:ExpireHours"];
            var expirationUtc = DateTime.UtcNow.AddHours(double.Parse(expiration));

            //classe que representa um token JWT e gera o token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                claims: claims,
                expires: expirationUtc,
                signingCredentials: credencials);

            return new UserToken()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expirationUtc,
                Message = "Token Jwt Ok"
            };
        }
    }
}
