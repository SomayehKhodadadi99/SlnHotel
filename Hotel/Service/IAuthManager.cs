using Hotel.Data;
using Hotel.Moddels;
using Microsoft.AspNetCore.Identity;

namespace Hotel.Service
{
    public interface IAuthManager
    {
        Task<string> CreateRefreshToken();

        Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);

        Task<IEnumerable<IdentityError>> Register(UserDTO userDto);

        Task<AuthResponseDto> Login(LoginUserDTO loginDto);
        //----------------------------------------

    }
}
