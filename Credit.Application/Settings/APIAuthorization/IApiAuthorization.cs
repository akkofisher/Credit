using Credit.Domain.Enums;

namespace Credit.Application.Settings.APIAuthorization
{
    public interface IApiAuthorization
    {
        public int? GetAuthorizeId();
        public RoleEnum GetAuthorizeRole();
        public string Name { get; }
    }
}
