// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using BL.Configuration;
using DAL;
using DTOs.Auth;
using Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BL.Managers
{
    public class AuthManager : IAuthManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JWTConfiguration _jwtConfig;

        public AuthManager(IUnitOfWork unitOfWork, IMapper mapper, IOptions<JWTConfiguration> jwtConfig)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtConfig = jwtConfig.Value;
        }

        public async Task<TokenDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _unitOfWork.RepoUser.GetUserByEmailAsync(registerDto.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            var user = _mapper.Map<User>(registerDto);
            user.Id = Guid.NewGuid();
            user.Password = HashPassword(registerDto.Password);

            await _unitOfWork.RepoUser.Add(user);
            await _unitOfWork.SaveAsync();

            return GenerateToken(user);
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _unitOfWork.RepoUser.GetUserByEmailAsync(loginDto.Email);

            if (user == null || !VerifyPassword(loginDto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }
            return GenerateToken(user);
        }

        public async Task<TokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(refreshTokenDto.Token);
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("Invalid token");
            }

            var user = await _unitOfWork.RepoUser.GetUserByIdAsync(Guid.Parse(userId));

            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            return GenerateToken(user);
        }

        private TokenDto GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.TokenValidityInMinutes),
                Issuer = _jwtConfig.ValidIssuer,
                Audience = _jwtConfig.ValidAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new TokenDto
            {
                Token = tokenString,
                RefreshToken = GenerateRefreshToken(),
                ExpiresAt = tokenDescriptor.Expires.Value,
                Role = user.Role,
                UserId = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = _jwtConfig.ValidAudience,
                ValidateIssuer = true,
                ValidIssuer = _jwtConfig.ValidIssuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hashedPassword;
        }
    }
}
