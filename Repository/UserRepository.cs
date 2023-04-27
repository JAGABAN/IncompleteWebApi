using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyPersonalProject.Data;
using MyPersonalProject.Models;
using MyPersonalProject.Models.Dto;
using MyPersonalProject.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyPersonalProject.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _database;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private string secretKey;

        public UserRepository(ApplicationDbContext database, IConfiguration configuration, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _database = database;
            _userManager = userManager;
            _mapper = mapper;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }
        public bool IsUniqueUser(string username)
        {
            var user = _database.ApplicationUsers.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _database.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    User = null
                };
            }

            ////if user was found generate JWT Token
            //var roles = await _userManager.GetRolesAsync(user);
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(secretKey);

            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //        new Claim(ClaimTypes.Name,user.Id.ToString()),
            //        new Claim(ClaimTypes.Role,roles.FirstOrDefault())
            //    }),
            //    Expires = DateTime.UtcNow.AddDays(1),
            //    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //LoginResponseDto loginResponseDto = new LoginResponseDto()
            //{
            //    Token = tokenHandler.WriteToken(token),
            //    User = _mapper.Map<UserDto>(user),
            //    Role = roles.FirstOrDefault(),
            //};
            //return loginResponseDto
            //
            //if user was found generate JWT Token
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, user.Id.ToString()),
};

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDto>(user),
                Role = roles.FirstOrDefault(),
            };
            return loginResponseDto;

        }



        public async Task<UserDto> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.UserName,
                Email = registrationRequestDto.UserName,
                Name = registrationRequestDto.Name,
            };
            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "admin");
                    await _userManager.AddToRoleAsync(user, "user");
                    var userToReturn = _database.ApplicationUsers.FirstOrDefault(u => u.UserName == registrationRequestDto.UserName);
                    return _mapper.Map<UserDto>(userToReturn);
                    //var reponseReturn = new UserDto()
                    //{
                    //    Name = registrationRequestDto.Name,
                    //    UserName = registrationRequestDto.UserName,
                    //};
                    // return reponseReturn; 
                }

            }catch (Exception ex)
            {

            }
            //else
            //{
            //    throw new Exception("Registration not successful");
            //}
            return new UserDto();

        }


    }
}

