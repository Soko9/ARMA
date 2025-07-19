using UserManagementApi.DTOs.User;
using UserManagementApi.Models;

namespace UserManagementApi.Repo
{
    public interface IUserService
    {
        Task<User?> GetByIDAsync(Guid Id);
        Task<IReadOnlyList<User>> GetAllAsync();
        Task<IReadOnlyList<User>> GetAllActiveAsync();
        Task<IReadOnlyList<User>> GetAllLocedAsync();
        Task<bool> CreateAsync(UserDTO Dto);
        Task<bool> UpdateAsync(Guid Id, UserDTO Dto);
        Task<bool> ResetPasswordAsync(Guid Id, UserDTO Dto);
        Task<bool> ResetPasscodeAsync(Guid Id, Guid? ActionId);
        Task<bool> ToggleActivity(Guid Id, Guid? ActionId);
        Task<bool> DeleteAsync(Guid Id, Guid? ActionId);
    }
}
