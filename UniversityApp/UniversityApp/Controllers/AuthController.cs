// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using BL.Managers;
using DTOs.Auth;
using DTOs.Common;
using Microsoft.AspNetCore.Mvc;

namespace UniversityApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<TokenDto>>> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var token = await _authManager.RegisterAsync(registerDto);
                return Ok(ApiResponse<TokenDto>.SuccessResponse(token, "User registered successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<TokenDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<TokenDto>.ErrorResponse("An error occurred during registration", new List<string> { ex.Message }));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<TokenDto>>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _authManager.LoginAsync(loginDto);
                return Ok(ApiResponse<TokenDto>.SuccessResponse(token, "Login successful"));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<TokenDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<TokenDto>.ErrorResponse("An error occurred during login", new List<string> { ex.Message }));
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponse<TokenDto>>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var token = await _authManager.RefreshTokenAsync(refreshTokenDto);
                return Ok(ApiResponse<TokenDto>.SuccessResponse(token, "Token refreshed successfully"));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<TokenDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<TokenDto>.ErrorResponse("An error occurred during token refresh", new List<string> { ex.Message }));
            }
        }
    }
}
