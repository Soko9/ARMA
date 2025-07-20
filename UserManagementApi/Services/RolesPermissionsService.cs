using Microsoft.EntityFrameworkCore;
using UserManagementApi.Models;
using UserManagementApi.Repo;

namespace UserManagementApi.Services
{
    public class RolesPermissionsService : IRolesPermissionsService
    {
        private readonly UserManagementDbContext _Db;
        private readonly DbSet<RolesPermission> _Table;
        private readonly ILogService _Log;

        public RolesPermissionsService(UserManagementDbContext Db, ILogService Log)
        {
            _Db = Db;
            _Table = Db.RolesPermissions;
            _Log = Log;
        }

        public async Task<IReadOnlyList<Permission>> GetPermissionsByRoleAsync(Guid RoleId)
            => await _Table
                .Where(Rp => Rp.RoleId == RoleId && Rp.IsActive)
                .Select(Rp => Rp.Permission)
                .AsNoTracking()
                .ToListAsync();

        public async Task<bool> UpsertPermissionsForRoleAsync(Guid RoleId, List<Guid> PermissionsIds, Guid? ActionUser)
        {
            DateTime now = DateTime.Now;

            List<RolesPermission> ExistingPermissionsForRole = _Table
                .Where(Rp => Rp.RoleId == RoleId)
                .ToList();

            // deactivating all existing permissions
            foreach (RolesPermission Rp in ExistingPermissionsForRole)
            {
                if (Rp.IsActive)
                {
                    Rp.IsActive = false;
                    Rp.UpdatedAt = now;
                    Rp.LastActionUserId = ActionUser;

                    await _Log.LogAsync(
                        "Decativating Permission",
                        $"Deactivated Permission {Rp.PermissionId} For Role {Rp.RoleId}",
                        "RolesPermission",
                        Rp.RolePermissionId,
                        ActionUser
                    );
                }
            }

            // re-activating / inserting given permissions
            foreach (Guid PId in PermissionsIds)
            {
                RolesPermission? ExistingPermission = ExistingPermissionsForRole
                    .FirstOrDefault(Rp => Rp.PermissionId == PId);

                if (ExistingPermission != null)
                {
                    ExistingPermission.IsActive = true;
                    ExistingPermission.UpdatedAt = now;
                    ExistingPermission.LastActionUserId = ActionUser;

                    await _Log.LogAsync(
                        "Recativating Permission",
                        $"Reactivated Permission {ExistingPermission.PermissionId} For Role {ExistingPermission.RoleId}",
                        "RolesPermission",
                        ExistingPermission.RolePermissionId,
                        ActionUser
                    );
                }
                else
                {
                    Guid NewGuid = Guid.NewGuid();
                    RolesPermission NewRolePermission = new RolesPermission
                    {
                        RolePermissionId = NewGuid,
                        RoleId = RoleId,
                        PermissionId = PId,
                        IsActive = true,
                        CreatedAt = now,
                        UpdatedAt = now,
                        LastActionUserId = ActionUser
                    };

                    await _Table.AddAsync(NewRolePermission);
                    await _Log.LogAsync(
                            "Create",
                            "RolePermission Created Successfully",
                            "RolesPermission",
                            NewGuid,
                            ActionUser
                        );
                }
            }

            await _Db.SaveChangesAsync();
            return true;
        }
    }
}
