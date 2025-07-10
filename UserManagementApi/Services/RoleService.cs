using Microsoft.EntityFrameworkCore;
using UserManagementApi.DTOs.Role;
using UserManagementApi.Models;
using UserManagementApi.Repo;

namespace UserManagementApi.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManagementDbContext _Db;
        private readonly DbSet<Role> _Table;
        private readonly ILogService _Log;

        public RoleService(UserManagementDbContext Db, ILogService Log)
        {
            _Db = Db;
            _Table = Db.Roles;
            _Log = Log;
        }

        public async Task<Role?> GetByIDAsync(Guid Id)
            => await _Table.FirstOrDefaultAsync(R => R.RoleId == Id);

        public async Task<IReadOnlyList<Role>> GetAllAsync()
            => await _Table
                .AsNoTracking()
                .OrderBy(R => R.CreatedAt)
                .ToListAsync();

        public async Task<IReadOnlyList<Role>> GetAllVisibleAsync()
            => await _Table
                .AsNoTracking()
                .Where(R => R.IsVisible)
                .OrderBy(R => R.CreatedAt)
                .ToListAsync();

        public async Task<IReadOnlyList<Permission>> GetPermissionsByRole(Guid RoleId)
            => await _Db.RolesPermissions
                .Where(Rp => Rp.RoleId == RoleId)
                .Select(Rp => Rp.Permission)
                .AsNoTracking()
                .ToListAsync();

        public async Task<bool> CreateAsync(RoleDTO Dto)
        {
            Role Entity = new Role
            {
                RoleId = Guid.NewGuid(),
                Title = Dto.Title,
                Description = Dto.Description,
                Priority = Dto.Priority,
                IsVisible = true,
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
                        "Role Created Successfully",
                        "Roles",
                        Entity.RoleId,
                        Dto.LastActionUserId ?? Guid.Empty
                    );
                }
                return saved;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Creating Role: {ex.Message}",
                    "Roles",
                    Entity.RoleId,
                    Dto.LastActionUserId ?? Guid.Empty
                );
                return false;
            }
        }

        public async Task<bool> UpdateAsync(RoleDTO Dto)
        {
            try
            {
                Role? Entity = await _Table.FindAsync(Dto.RoleId);

                if (Entity is null)
                {
                    throw new ArgumentNullException($"No Entity Found With ID: {Dto.RoleId}");
                }

                Entity.Title = Dto.Title;
                Entity.Description = Dto.Description;
                Entity.Priority = Dto.Priority;
                Entity.UpdatedAt = DateTime.Now;
                Entity.LastActionUserId = Dto.LastActionUserId;

                _Table.Update(Entity);
                bool saved = await _Db.SaveChangesAsync() > 0;

                if (saved)
                {
                    await _Log.LogAsync(
                        "Update",
                        "Role Updated Successfully",
                        "Roles",
                        Entity.RoleId,
                        Dto.LastActionUserId ?? Guid.Empty
                    );
                }
                return saved;
            }
            catch (ArgumentNullException ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Fetching Role: {ex.Message}",
                    "Roles",
                    Dto.RoleId ?? Guid.Empty,
                    Dto.LastActionUserId ?? Guid.Empty
                );
                return false;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Updating Role: {ex.Message}",
                    "Roles",
                    Dto.RoleId ?? Guid.Empty,
                    Dto.LastActionUserId ?? Guid.Empty
                );
                return false;
            }
        }

        public async Task<bool> ToggleVisibility(Guid Id, Guid ActionId)
        {
            try
            {
                Role? Entity = await _Table.FindAsync(Id);

                if (Entity is null)
                {
                    throw new ArgumentNullException($"No Entity Found With ID: {Id}");
                }

                Entity.IsVisible = !Entity.IsVisible;
                Entity.UpdatedAt = DateTime.Now;
                Entity.LastActionUserId = ActionId;

                _Table.Update(Entity);
                bool saved = await _Db.SaveChangesAsync() > 0;

                if (saved)
                {
                    await _Log.LogAsync(
                        "Toggle",
                        "Role Toggled Successfully",
                        "Roles",
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
                    $"Error Fetching Role: {ex.Message}",
                    "Roles",
                    Id,
                    ActionId
                );
                return false;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Toggling Role: {ex.Message}",
                    "Roles",
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
                Role? Entity = await _Table.FindAsync(Id);

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
                       "Role Deleted Successfully",
                       "Roles",
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
                    $"Error Fetching Role: {ex.Message}",
                    "Roles",
                    Id,
                    ActionId
                );
                return false;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Deleting Role: {ex.Message}",
                    "Roles",
                    Id,
                    ActionId
                );
                return false;
            }
        }
    }
}
