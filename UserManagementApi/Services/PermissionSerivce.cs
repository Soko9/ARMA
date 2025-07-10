using Microsoft.EntityFrameworkCore;
using UserManagementApi.DTOs.Permission;
using UserManagementApi.Models;
using UserManagementApi.Repo;

namespace UserManagementApi.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly UserManagementDbContext _Db;
        private readonly DbSet<Permission> _Table;
        private readonly ILogService _Log;

        public PermissionService(UserManagementDbContext Db, ILogService Log)
        {
            _Db = Db;
            _Table = Db.Permissions;
            _Log = Log;
        }

        public async Task<Permission?> GetByIDAsync(Guid Id)
            => await _Table.FirstOrDefaultAsync(P => P.PermissionId == Id);

        public async Task<IReadOnlyList<Permission>> GetAllAsync()
            => await _Table
                .AsNoTracking()
                .OrderBy(P => P.CreatedAt)
                .ToListAsync();

        public async Task<IReadOnlyList<Permission>> GetAllVisibleAsync()
            => await _Table
                .AsNoTracking()
                .Where(P => P.IsVisible)
                .OrderBy(P => P.CreatedAt)
                .ToListAsync();

        public async Task<bool> CreateAsync(PermissionDTO Dto)
        {
            Permission Entity = new Permission
            {
                PermissionId = Guid.NewGuid(),
                Title = Dto.Title,
                Description = Dto.Description,
                IsVisible = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                LastActionUserId = Dto.LastActionUserId,
                PermissionCategoryId = Dto.PermissionCategoryId,
            };

            try
            {
                await _Table.AddAsync(Entity);
                bool saved = await _Db.SaveChangesAsync() > 0;
                if (saved)
                {
                    await _Log.LogAsync(
                        "Create",
                        "Permission Created Successfully",
                        "Permissions",
                        Entity.PermissionId,
                        Dto.LastActionUserId ?? Guid.Empty
                    );
                }
                return saved;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Creating Permission: {ex.Message}",
                    "Permissions",
                    Entity.PermissionId,
                    Dto.LastActionUserId ?? Guid.Empty
                );
                return false;
            }
        }

        public async Task<bool> UpdateAsync(PermissionDTO Dto)
        {
            try
            {
                Permission? Entity = await _Table.FindAsync(Dto.PermissionId);

                if (Entity is null)
                {
                    throw new ArgumentNullException($"No Entity Found With ID: {Dto.PermissionId}");
                }

                Entity.Title = Dto.Title;
                Entity.Description = Dto.Description;
                Entity.UpdatedAt = DateTime.Now;
                Entity.LastActionUserId = Dto.LastActionUserId;
                Entity.PermissionCategoryId = Dto.PermissionCategoryId;

                _Table.Update(Entity);
                bool saved = await _Db.SaveChangesAsync() > 0;

                if (saved)
                {
                    await _Log.LogAsync(
                        "Update",
                        "Permission Updated Successfully",
                        "Permissions",
                        Entity.PermissionId,
                        Dto.LastActionUserId ?? Guid.Empty
                    );
                }
                return saved;
            }
            catch (ArgumentNullException ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Fetching Permission: {ex.Message}",
                    "Permissions",
                    Dto.PermissionId ?? Guid.Empty,
                    Dto.LastActionUserId ?? Guid.Empty
                );
                return false;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Updating Permission: {ex.Message}",
                    "Permissions",
                    Dto.PermissionId ?? Guid.Empty,
                    Dto.LastActionUserId ?? Guid.Empty
                );
                return false;
            }
        }

        public async Task<bool> ToggleVisibility(Guid Id, Guid ActionId)
        {
            try
            {
                Permission? Entity = await _Table.FindAsync(Id);

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
                        "Permission Toggled Successfully",
                        "Permissions",
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
                    $"Error Fetching Permission: {ex.Message}",
                    "Permissions",
                    Id,
                    ActionId
                );
                return false;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Toggling Permission: {ex.Message}",
                    "Permissions",
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
                Permission? Entity = await _Table.FindAsync(Id);

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
                       "Permission Deleted Successfully",
                       "Permissions",
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
                    $"Error Fetching Permission: {ex.Message}",
                    "Permissions",
                    Id,
                    ActionId
                );
                return false;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Deleting Permission: {ex.Message}",
                    "Permissions",
                    Id,
                    ActionId
                );
                return false;
            }
        }
    }
}
