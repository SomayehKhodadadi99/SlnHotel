using AutoMapper;
using Hotel.Controllers;
using Hotel.Data;
using Hotel.Moddels;
using Hotel.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hotel.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthManager> _logger;

        private ApiUser _user;
        private const string _loginProvider = "HotelListingApi";
        private const string _refreshToken = "RefreshToken";
        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration, ILogger<AuthManager> logger)
        {
            this._mapper = mapper;
            this._userManager = userManager;
            this._configuration = configuration;
            this._logger = logger;
        }


        //حذف توکن قبلی 
        //ایجاد توکن جدید
        //ست کردن توکن جدید همراه با توکن 
        public async Task<string> CreateRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);
            var result = await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);
            return newRefreshToken;
        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;
            _user = await _userManager.FindByNameAsync(username);

            if (_user == null || _user.Id != request.UserId)
            {
                return null;
            }

            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.RefreshToken);

            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseDto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshToken()
                };
            }

            await _userManager.UpdateSecurityStampAsync(_user);
            return null;
        }

        private async Task<string> GenerateToken()
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(_user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim("uid", _user.Id),
            }
            .Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<IEnumerable<IdentityError>> Register(UserDTO userDto)
        {
            _user = _mapper.Map<ApiUser>(userDto);
            _user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(_user, userDto.Password);

            if (result.Succeeded)
            {
                //await _userManager.AddToRoleAsync(_user, "User");
                await _userManager.AddToRolesAsync(_user, userDto.Roles);
            }

            return result.Errors;
        }

        public async Task<AuthResponseDto> Login(LoginUserDTO loginDto)
        {
            _logger.LogInformation($"Looking for user with email {loginDto.Email}");
            _user = await _userManager.FindByEmailAsync(loginDto.Email);
            bool isValidUser = await _userManager.CheckPasswordAsync(_user, loginDto.Password);

            if (_user == null || isValidUser == false)
            {
                _logger.LogWarning($"User with email {loginDto.Email} was not found");
                return null;
            }

            var token = await GenerateToken();
            _logger.LogInformation($"Token generated for user with email {loginDto.Email} | Token: {token}");

            return new AuthResponseDto
            {
                Token = token,
                UserId = _user.Id,
                RefreshToken = await CreateRefreshToken()
            };
        }
        //---------------------------------------------------------
        //public async Task<bool> ValidateUser(LoginUserDTO loginDTO)
        //{
        //    _user = await _userManager.FindByNameAsync(loginDTO.Email);
        //    return _user != null
        //        && await _userManager.CheckPasswordAsync(_user, loginDTO.Password);
        //}

        //public async Task<string> CreateToken()
        //{
        //    var sgningCredentials = GetSigningCredentials();
        //    var claims = await GetClaims();
        //    var tokenOption = GenerateTokenOptions(sgningCredentials, claims);

        //    return new JwtSecurityTokenHandler().WriteToken(tokenOption);
        //}

        //private JwtSecurityToken GenerateTokenOptions(SigningCredentials sgningCredentials, List<Claim> claims)
        //{
        //    var jwtSetting = _configuration.GetSection("Jwt");
        //    var expirtion = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSetting.GetSection("lifetime").Value));

        //    var jwtSecurityToken = new JwtSecurityToken(
        //        issuer: jwtSetting.GetSection("Issuer").Value,
        //        claims: claims,
        //        expires: expirtion,
        //        signingCredentials: sgningCredentials,
        //        audience: jwtSetting.GetSection("Issuer").Value
        //        );
        //    return jwtSecurityToken;
        //}
        //private async Task<List<Claim>> GetClaims()
        //{
        //    var Claims = new List<Claim>
        //   {
        //       new Claim(ClaimTypes.Name,_user.UserName)
        //   };
        //    var roles = await _userManager.GetRolesAsync(_user);
        //    foreach (var role in roles)
        //    {
        //        Claims.Add(new Claim(ClaimTypes.Role, role));
        //    }
        //    return Claims;
        //}
        //private SigningCredentials GetSigningCredentials()
        //{
        //    //var key = Environment.GetEnvironmentVariable("KEY");
        //    var key = _configuration.GetSection("JwtSettings").GetSection("Key").Value;
        //    var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        //    return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        //}
    }
}

        //این دو تا متد  پایینی مثل همن

        //private async Task<List<Claim>> GetClaims()
        //{
        //    var Claims = new List<Claim>
        //   {
        //       new Claim(ClaimTypes.Name,_user.UserName)
        //   };
        //    var roles = await _userManager.GetRolesAsync(_user);
        //    foreach (var role in roles)
        //    {
        //        Claims.Add(new Claim(ClaimTypes.Role, role));
        //    }
        //    return Claims;
        //}

        //var roles = await _userManager.GetRolesAsync(_user);
        //var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
        //var userClaims = await _userManager.GetClaimsAsync(_user);


