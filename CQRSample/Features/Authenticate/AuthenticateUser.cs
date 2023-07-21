using CQRSample.Data;
using CQRSample.Domain;
using CQRSample.Features.Users.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CQRSample.Features.Authenticate
{
    public abstract class AuthenticateUser
    {
        public class AuthenticateRequest : IRequest<AuthenticateResponse>
       {

            public AuthenticateRequest(string username, string password)
            {
                Username = username;
                Password = password;         
            }

            [Required]
            public string Username
            {
                get;
            }
            [Required]
            public string Password
            {
                get;
            }


        }

        public class AuthenticateResponse
        {
            public int Id
            {
                get;
                set;
            }

            public string Fullname
            {
                get;
                set;
            }

            public string Username
            {
                get;
                set;
            }

            public string Password
            {
                get;
                set;
            }
            public string Token
            {
                get;
                set;
            }


            public AuthenticateResponse(User user, string token)
            {
                Id = user.Id;
                Fullname = user.FullName;
                Username = user.UserName;
                Password = user.Password;
                Token = token;
            }
        }

        public class Handler : IRequestHandler<AuthenticateRequest, AuthenticateResponse>
        {
            private readonly StoreContext _context;
            private readonly IConfiguration _configuration;

            public Handler(StoreContext context, IConfiguration configuration)
            {
                _context = context;
                _configuration = configuration;

            }

            public Task<AuthenticateResponse> Handle(AuthenticateRequest request, CancellationToken cancellationToken)
            {
                var user = _context.Users.SingleOrDefault(x => x.UserName == request.Username
                   && x.Password == request.Password);

                if (user == null)
                    throw new NoUserFound();
                var token = GenerateJwtToken(user);
                var result = new AuthenticateResponse(user, token);
                return Task.FromResult(result);
            }

            private string GenerateJwtToken(User user)
            {
                var key = _configuration.GetValue<string>("JwtConfig:Key");
                var audience = _configuration.GetValue<string>("JwtConfig:Audience");
                var issuer = _configuration.GetValue<string>("JwtConfig:Issuer");
                var keyBytes = Encoding.ASCII.GetBytes(key);
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("id", user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(keyBytes),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }




        }
    }
}
