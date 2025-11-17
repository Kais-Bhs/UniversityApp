// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using DTOs.Auth;

namespace BL.Managers
{
    public interface IAuthManager
    {
        Task<TokenDto> RegisterAsync(RegisterDto registerDto);
        Task<TokenDto> LoginAsync(LoginDto loginDto);
        Task<TokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    }
}
