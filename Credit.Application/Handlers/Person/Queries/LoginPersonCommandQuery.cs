using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Credit.Application.Models.DtoModels;
using Credit.Application.Settings;
using Credit.Domain.Entities;
using Credit.Infrastructure.Repositories;
using Credit.Infrastructure.Repositories.Person;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Credit.Application.Handlers.Person.Queries
{
    public class LoginPersonCommandQuery(LoginPersonDtoModel model) : IRequest<string>
    {
        public LoginPersonDtoModel Model { get; } = model;
    }

    public class LoginPersonCommandQueryHandler : IRequestHandler<LoginPersonCommandQuery, string>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly TokenSettings _tokenSettings;

        //constructor
        public LoginPersonCommandQueryHandler(IUnitOfWork unitOfWork, IPersonRepository personRepository, IMapper mapper, IOptions<TokenSettings> tokenSettings)
        {
            _personRepository = personRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenSettings = tokenSettings.Value;
        }

        public async Task<string> Handle(LoginPersonCommandQuery request,
            CancellationToken cancellationToken)
        {

            var person = await _personRepository.GetByEmail(request.Model.Email);

            if (person == null)
            {
                throw new Exception("Person not found");
            }

            var passwordVerified = BCrypt.Net.BCrypt.Verify(request.Model.Password, person.PasswordHash);

            if (!passwordVerified)
            {
                throw new Exception("Invalid password");
            }

            return BuildToken(person, _tokenSettings);
        }

        private static string BuildToken(PersonEntity person, TokenSettings tokenSettings)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, person.Email),
                new Claim(ClaimTypes.Role, person.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, person.Id.ToString()),
                new Claim(ClaimTypes.Name, person.FirstName)
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(tokenSettings.Secret));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new JwtSecurityToken(
                tokenSettings.Issuer,
                tokenSettings.Audience,
                claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

    }
}
