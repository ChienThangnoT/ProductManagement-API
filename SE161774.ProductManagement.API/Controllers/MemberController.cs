using AutoMapper.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SE161774.ProductManagement.Repo.Interface;
using SE161774.ProductManagement.Repo.Models;
using SE161774.ProductManagement.Repo.ResponeModel;
using SE161774.ProductManagement.Repo.ResponeModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static SE161774.ProductManagement.Repo.ResponeModel.ResponeModel;

namespace SE161774.ProductManagement.API.Controllers
{
    [Route("api/members")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public MemberController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult> GenerateJWTtoken(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    return BadRequest(new FailedResponseModel
                    {
                        Status = BadRequest().StatusCode,
                        Message = "Username or password not provided!!"
                    });
                }

                var members = await _unitOfWork.MembersRepository.GetAsync(c => c.Email.Equals(email) && c.Password.Equals(password), null, "", null, null);
                var member = members.FirstOrDefault();

                if (member == null)
                {
                    return BadRequest(new FailedResponseModel { 
                        Status = StatusCodes.Status401Unauthorized, 
                        Message = "Invalid username or password!!" });
                }

                var authClaims = new List<Claim>
                {
                    new(ClaimTypes.Name, email),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(ClaimTypes.Role, member.Role.ToString())
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
                _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );


                return Ok(new TokenExpireTime
                {         
                    Status = StatusCodes.Status201Created,
                    Message = "Create jwt token success",
                    Result = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = TimeZoneInfo.ConvertTimeFromUtc(token.ValidTo, TimeZoneInfo.Local)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }
    }                                                                  
}

