using UserManagementApi.DTOs.PermissionCategory;
using UserManagementApi.Models;

namespace UserManagementApi.Repo
{
    public interface IPermissionCategoryService
    {
        Task<PermissionCategory?> GetByIDAsync(Guid Id);
        Task<IReadOnlyList<PermissionCategory>> GetAllAsync();
        Task<IReadOnlyList<PermissionCategory>> GetAllVisibleAsync();
        Task<bool> CreateAsync(PermissionCategoryDTO Dto);
        Task<bool> UpdateAsync(Guid Id, PermissionCategoryDTO Dto);
        Task<bool> ToggleVisibility(Guid Id, Guid? ActionId);
        Task<bool> DeleteAsync(Guid Id, Guid? ActionId);
    }
}
