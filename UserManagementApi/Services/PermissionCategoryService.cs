using Microsoft.EntityFrameworkCore;
using UserManagementApi.DTOs.PermissionCategory;
using UserManagementApi.Models;
using UserManagementApi.Repo;

namespace UserManagementApi.Services
{
    public class PermissionCategoryService : IPermissionCategoryService
    {
        private readonly UserManagementDbContext _Db;
        private readonly DbSet<PermissionCategory> _Table;
        private readonly ILogService _Log;

        public PermissionCategoryService(UserManagementDbContext Db, ILogService Log)
        {
            _Db = Db;
            _Table = Db.PermissionCategories;
            _Log = Log;
        }

        public async Task<PermissionCategory?> GetByIDAsync(Guid Id)
            => await _Table.FirstOrDefaultAsync(Pc => Pc.PermissionCategoryId == Id);

        public async Task<IReadOnlyList<PermissionCategory>> GetAllAsync()
            => await _Table
                .AsNoTracking()
                .OrderBy(Pc => Pc.CreatedAt)
                .ToListAsync();

        public async Task<IReadOnlyList<PermissionCategory>> GetAllVisibleAsync()
            => await _Table
                .AsNoTracking()
                .Where(Pc => Pc.IsVisible)
                .OrderBy(Pc => Pc.CreatedAt)
                .ToListAsync();

        public async Task<bool> CreateAsync(PermissionCategoryDTO Dto)
        {
            PermissionCategory Entity = new PermissionCategory
            {
                PermissionCategoryId = Guid.NewGuid(),
                Title = Dto.Title,
                Priority = Dto.Priority,
                IsVisible = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                LastActionUserId = Dto.LastActionUserId
            };

            try
            {
                await _Table.AddAsync(Entity);
                bool saved = await _Db.SaveChangesAsync() > 0;
                if (saved)
                {
                    await _Log.LogAsync(
                        "Create",
                        "PermissionCategory Created Successfully",
                        "PermissionCategories",
                        Entity.PermissionCategoryId,
                        Dto.LastActionUserId
                    );
                }
                return saved;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Creating PermissionCategory: {ex.Message}",
                    "PermissionCategories",
                    Entity.PermissionCategoryId,
                    Dto.LastActionUserId
                );
                return false;
            }
        }

        public async Task<bool> UpdateAsync(PermissionCategoryDTO Dto)
        {
            try
            {
                PermissionCategory? Entity = await _Table.FindAsync(Dto.PermissionCategoryId);

                if (Entity is null)
                {
                    throw new ArgumentNullException($"No Entity Found With ID: {Dto.PermissionCategoryId}");
                }

                Entity.Title = Dto.Title;
                Entity.Priority = Dto.Priority;
                Entity.UpdatedAt = DateTime.Now;
                Entity.LastActionUserId = Dto.LastActionUserId;

                _Table.Update(Entity);
                bool saved = await _Db.SaveChangesAsync() > 0;

                if (saved)
                {
                    await _Log.LogAsync(
                        "Update",
                        "PermissionCategory Updated Successfully",
                        "PermissionCategories",
                        Entity.PermissionCategoryId,
                        Dto.LastActionUserId
                    );
                }
                return saved;
            }
            catch (ArgumentNullException ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Fetching PermissionCategory: {ex.Message}",
                    "PermissionCategories",
                    Dto.PermissionCategoryId ?? Guid.Empty,
                    Dto.LastActionUserId
                );
                return false;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Updating PermissionCategory: {ex.Message}",
                    "PermissionCategories",
                    Dto.PermissionCategoryId ?? Guid.Empty,
                    Dto.LastActionUserId
                );
                return false;
            }
        }

        public async Task<bool> ToggleVisibility(Guid Id, Guid ActionId)
        {
            try
            {
                PermissionCategory? Entity = await _Table.FindAsync(Id);

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
                        "PermissionCategory Toggled Successfully",
                        "PermissionCategories",
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
                    $"Error Fetching PermissionCategory: {ex.Message}",
                    "PermissionCategories",
                    Id,
                    ActionId
                );
                return false;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Toggling PermissionCategory: {ex.Message}",
                    "PermissionCategories",
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
                PermissionCategory? Entity = await _Table.FindAsync(Id);

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
                       "PermissionCategory Deleted Successfully",
                       "PermissionCategories",
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
                    $"Error Fetching PermissionCategory: {ex.Message}",
                    "PermissionCategories",
                    Id,
                    ActionId
                );
                return false;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Deleting PermissionCategory: {ex.Message}",
                    "PermissionCategories",
                    Id,
                    ActionId
                );
                return false;
            }
        }
    }
}
