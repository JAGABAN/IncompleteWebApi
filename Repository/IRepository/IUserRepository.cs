using MyPersonalProject.Models;
using MyPersonalProject.Models.Dto;

namespace MyPersonalProject.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);

        // Task<LoginResponseDto>Login(LoginResponseDto loginResponseDto);
        public  Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        //  Task<LocalUser> Register(RegistrationRequestDto registrationRequestDto);

        public Task<UserDto> Register(RegistrationRequestDto registrationRequestDto);
    }
}
