using Microsoft.EntityFrameworkCore;
using UserManagementApi.DTOs.RolesPermission;
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

        public async Task<bool> CreateAsync(RolesPermissionsDTO Dto)
        {
            RolesPermission Entity = new RolesPermission
            {
                RolePermissionId = Guid.NewGuid(),
                RoleId = Dto.RoleId,
                PermissionId = Dto.PermissionId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                LastActionUserId = Dto.LastActionUserId,
            };

            try
            {
                await _Table.AddAsync(Entity);
                bool saved = await _Db.SaveChangesAsync() > 0;
                if (saved)
                {
                    await _Log.LogAsync(
                        "Create",
                        "RolePermission Created Successfully",
                        "RolesPermissions",
                        Entity.RolePermissionId,
                        Dto.LastActionUserId ?? Guid.Empty
                    );
                }
                return saved;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Creating RolePermission: {ex.Message}",
                    "RolesPermissions",
                    Entity.RolePermissionId,
                    Dto.LastActionUserId ?? Guid.Empty
                );
                return false;
            }
        }

        public async Task<bool> ToggleActivity(Guid Id, Guid ActionId)
        {
            try
            {
                RolesPermission? Entity = await _Table.FindAsync(Id);

                if (Entity is null)
                {
                    throw new ArgumentNullException($"No Entity Found With ID: {Id}");
                }

                Entity.IsActive = !Entity.IsActive;
                Entity.UpdatedAt = DateTime.Now;
                Entity.LastActionUserId = ActionId;

                _Table.Update(Entity);
                bool saved = await _Db.SaveChangesAsync() > 0;

                if (saved)
                {
                    await _Log.LogAsync(
                        "Toggle",
                        "RolePermission Toggled Successfully",
                        "RolesPermissions",
                        Id,
                        ActionId
                    );
                }
                return saved;
            }
            catch (ArgumentNullException ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Fetching RolePermission: {ex.Message}",
                    "RolesPermissions",
                    Id,
                    ActionId
                );
                return false;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Toggling RolePermission: {ex.Message}",
                    "RolesPermissions",
                    Id,
                    ActionId
                );
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid Id, Guid ActionId)
        {
            try
            {
                RolesPermission? Entity = await _Table.FindAsync(Id);

                if (Entity is null)
                {
                    throw new ArgumentNullException($"No Entity Found With ID: {Id}");
                }

                _Table.Remove(Entity);
                bool saved = await _Db.SaveChangesAsync() > 0;

                if (saved)
                {
                    await _Log.LogAsync(
                       "Delete",
                       "RolePermission Deleted Successfully",
                       "RolesPermissions",
                       Id,
                       ActionId
                   );
                }
                return saved;
            }
            catch (ArgumentNullException ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Fetching RolePermission: {ex.Message}",
                    "RolesPermissions",
                    Id,
                    ActionId
                );
                return false;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Deleting RolePermission: {ex.Message}",
                    "RolesPermissions",
                    Id,
                    ActionId
                );
                return false;
            }
        }
    }
}
