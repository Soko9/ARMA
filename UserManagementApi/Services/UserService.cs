using Microsoft.EntityFrameworkCore;
using SharedHelpers.Helpers;
using UserManagementApi.DTOs.User;
using UserManagementApi.Models;
using UserManagementApi.Repo;

namespace UserManagementApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserManagementDbContext _Db;
        private readonly DbSet<User> _Table;
        private readonly ILogService _Log;

        public UserService(UserManagementDbContext Db, ILogService Log)
        {
            _Db = Db;
            _Table = Db.Users;
            _Log = Log;
        }

        public async Task<User?> GetByIDAsync(Guid Id)
            => await _Table.FirstOrDefaultAsync(U => U.UserId == Id);

        public async Task<IReadOnlyList<User>> GetAllAsync()
            => await _Table
                .AsNoTracking()
                .OrderBy(U => U.CreatedAt)
                .ToListAsync();

        public async Task<IReadOnlyList<User>> GetAllActiveAsync()
            => await _Table
                .AsNoTracking()
                .Where(U => U.IsActive)
                .OrderBy(U => U.CreatedAt)
                .ToListAsync();

        public async Task<IReadOnlyList<User>> GetAllLocedAsync()
            => await _Table
                .AsNoTracking()
                .Where(U => U.IsLocked)
                .OrderBy(U => U.CreatedAt)
                .ToListAsync();

        public async Task<bool> CreateAsync(UserDTO Dto)
        {
            string PasswordHashed = PasswordHelper.HashPassword(Dto.Password!);
            string Passcode = PasswordHelper.GenerateRandomPasscode();
            User Entity = new User
            {
                UserId = Guid.NewGuid(),
                FullName = Dto.FullName,
                Passcode = Passcode,
                PasswordHash = PasswordHashed,
                FailedLoginAttempts = 0,
                IsLocked = false,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                LastSuccessfulLoginAt = DateTime.Now,
                LastActionUserId = Dto.LastActionUserId!,
                RoleId = Dto.RoleId,
            };

            try
            {
                await _Table.AddAsync(Entity);
                bool saved = await _Db.SaveChangesAsync() > 0;
                if (saved)
                {
                    await _Log.LogAsync(
                        "Create",
                        "User Created Successfully",
                        "Users",
                        Entity.UserId,
                        Dto.LastActionUserId!
                    );
                }
                return saved;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Creating User: {ex.Message}",
                    "Users",
                    Entity.UserId,
                    Dto.LastActionUserId!
                );
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Guid Id, UserDTO Dto)
        {
            try
            {
                User? Entity = await _Table.FindAsync(Id);

                if (Entity is null)
                {
                    throw new ArgumentNullException($"No Entity Found With ID: {Id}");
                }

                Entity.FullName = Dto.FullName;
                Entity.UpdatedAt = DateTime.Now;
                Entity.LastActionUserId = Dto.LastActionUserId!;
                Entity.RoleId = Dto.RoleId;

                _Table.Update(Entity);
                bool saved = await _Db.SaveChangesAsync() > 0;

                if (saved)
                {
                    await _Log.LogAsync(
                        "Update",
                        "User Updated Successfully",
                        "Users",
                        Id,
                        Dto.LastActionUserId!
                    );
                }
                return saved;
            }
            catch (ArgumentNullException ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Fetching User: {ex.Message}",
                    "Users",
                    Id,
                    Dto.LastActionUserId!
                );
                return false;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Updating User: {ex.Message}",
                    "Users",
                    Id,
                    Dto.LastActionUserId!
                );
                return false;
            }
        }

        public async Task<bool> ToggleActivity(Guid Id, Guid? ActionId)
        {
            try
            {
                User? Entity = await _Table.FindAsync(Id);

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
                        "User Toggled Successfully",
                        "Users",
                        Id,
                        ActionId!
                    );
                }
                return saved;
            }
            catch (ArgumentNullException ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Fetching User: {ex.Message}",
                    "Users",
                    Id,
                    ActionId!
                );
                return false;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Toggling User: {ex.Message}",
                    "Users",
                    Id,
                    ActionId!
                );
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid Id, Guid? ActionId)
        {
            try
            {
                User? Entity = await _Table.FindAsync(Id);

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
                       "User Deleted Successfully",
                       "Users",
                       Id,
                       ActionId!
                   );
                }
                return saved;
            }
            catch (ArgumentNullException ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Fetching User: {ex.Message}",
                    "Users",
                    Id,
                    ActionId!
                );
                return false;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Deleting User: {ex.Message}",
                    "Users",
                    Id,
                    ActionId!
                );
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(Guid Id, UserDTO Dto)
        {
            try
            {
                User? Entity = await _Table.FindAsync(Id);

                if (Entity is null)
                {
                    throw new ArgumentNullException($"No Entity Found With ID: {Id}");
                }

                string PasswordHashed = PasswordHelper.HashPassword(Dto.Password!);

                Entity.PasswordHash = PasswordHashed;
                Entity.UpdatedAt = DateTime.Now;
                Entity.LastActionUserId = Dto.LastActionUserId!;

                _Table.Update(Entity);
                bool saved = await _Db.SaveChangesAsync() > 0;

                if (saved)
                {
                    await _Log.LogAsync(
                        "Reset Password",
                        "Password Updated Successfully",
                        "Users",
                        Id,
                        Dto.LastActionUserId!
                    );
                }
                return saved;
            }
            catch (ArgumentNullException ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Fetching User: {ex.Message}",
                    "Users",
                    Id,
                    Dto.LastActionUserId!
                );
                return false;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Updating Password: {ex.Message}",
                    "Users",
                    Id,
                    Dto.LastActionUserId!
                );
                return false;
            }
        }

        public async Task<bool> ResetPasscodeAsync(Guid Id, Guid? ActionId)
        {
            try
            {
                User? Entity = await _Table.FindAsync(Id);

                if (Entity is null)
                {
                    throw new ArgumentNullException($"No Entity Found With ID: {Id}");
                }

                string Passcode = PasswordHelper.GenerateRandomPasscode();

                Entity.Passcode = Passcode;
                Entity.UpdatedAt = DateTime.Now;
                Entity.LastActionUserId = ActionId!;

                _Table.Update(Entity);
                bool saved = await _Db.SaveChangesAsync() > 0;

                if (saved)
                {
                    await _Log.LogAsync(
                        "Reset Passcode",
                        "Passcode Updated Successfully",
                        "Users",
                        Id,
                        ActionId!
                    );
                }
                return saved;
            }
            catch (ArgumentNullException ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Fetching User: {ex.Message}",
                    "Users",
                    Id,
                    ActionId!
                );
                return false;
            }
            catch (Exception ex)
            {
                await _Log.LogAsync(
                    "Error",
                    $"Error Updating Passcode: {ex.Message}",
                    "Users",
                    Id,
                    ActionId!
                );
                return false;
            }
        }
    }
}
