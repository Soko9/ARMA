using UserManagementApi.Models;

namespace UserManagementApi.Repo
{
    public interface IRolesPermissionsService
    {
        Task<IReadOnlyList<Permission>> GetPermissionsByRoleAsync(Guid RoleId);

        Task<bool> UpsertPermissionsForRoleAsync(Guid RoleId, List<Guid> PermissionsIds, Guid? ActionUser);
    }
}
