using UserManagementApi.DTOs.Role;
using UserManagementApi.Models;

namespace UserManagementApi.Repo
{
    public interface IRoleService
    {
        Task<Role?> GetByIDAsync(Guid Id);
        Task<IReadOnlyList<Role>> GetAllAsync();
        Task<IReadOnlyList<Role>> GetAllVisibleAsync();
        Task<bool> CreateAsync(RoleDTO Dto);
        Task<bool> UpdateAsync(Guid Id, RoleDTO Dto);
        Task<bool> ToggleVisibility(Guid Id, Guid? ActionId);
        Task<bool> DeleteAsync(Guid Id, Guid? ActionId);
    }
}
