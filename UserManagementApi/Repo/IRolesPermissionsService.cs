using UserManagementApi.DTOs.RolesPermission;

namespace UserManagementApi.Repo
{
    public interface IRolesPermissionsService
    {
        Task<bool> CreateAsync(RolesPermissionsDTO Dto);
        Task<bool> ToggleActivity(Guid Id, Guid ActionId);
        Task<bool> DeleteAsync(Guid Id, Guid ActionId);
    }
}
