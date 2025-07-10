using UserManagementApi.DTOs.Permission;
using UserManagementApi.Models;

namespace UserManagementApi.Repo
{
    public interface IPermissionService
    {
        Task<Permission?> GetByIDAsync(Guid Id);
        Task<IReadOnlyList<Permission>> GetAllAsync();
        Task<IReadOnlyList<Permission>> GetAllVisibleAsync();
        Task<bool> CreateAsync(PermissionDTO Dto);
        Task<bool> UpdateAsync(PermissionDTO Dto);
        Task<bool> ToggleVisibility(Guid Id, Guid ActionId);
        Task<bool> DeleteAsync(Guid Id, Guid ActionId);
    }
}
