using System.Security.Claims;
using Credit.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Credit.Application.Settings.APIAuthorization;

public sealed class ApiAuthorization : IApiAuthorization
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private string? _name;
    private int? _personId;

    public ApiAuthorization(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? GetAuthorizeId()
    {
        if (_personId != null)
        {
            return _personId.Value;
        }

        var claim = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(x => string.Equals(x.Type, ClaimTypes.NameIdentifier));

        if (int.TryParse(claim?.Value, out var personId))
        {
            _personId = personId;
            return personId;
        }

        throw new ArgumentException("Could not parse person id to int");
    }

    public RoleEnum GetAuthorizeRole()
    {
        var claim = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(x => string.Equals(x.Type, ClaimTypes.Role));

        if (Enum.TryParse(claim?.Value, out RoleEnum role))
        {
            return role;
        }

        throw new ArgumentException("Could not parse person role");
    }

    public string Name
    {
        get
        {
            if (_name is not null)
            {
                return _name;
            }

            var identity = _httpContextAccessor.HttpContext?.User.Identity;
            if (identity is null)
            {
                _name = string.Empty;
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(identity.Name))
            {
                _name = identity.Name;
                return identity.Name;
            }

            var claim = _httpContextAccessor.HttpContext!.User.Claims
                .FirstOrDefault(c => string.Equals(c.Type, ClaimTypes.Name, StringComparison.OrdinalIgnoreCase))?
                .Value;
            _name = claim ?? string.Empty;
            return _name;
        }
    }

    public string GetAuthorizeEmail()
    {
        var claim = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(x => string.Equals(x.Type, ClaimTypes.Email));

        if (!string.IsNullOrWhiteSpace(claim?.Value))
        {
            return claim.Value;
        }

        return string.Empty;
    }
}