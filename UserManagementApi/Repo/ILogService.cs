using UserManagementApi.Models;

namespace UserManagementApi.Repo
{
    public interface ILogService
    {
        Task<IReadOnlyList<Log>> GetAllAsync();
        Task LogAsync(string Type, string Desc, string Table, Guid RowId, Guid? UserID);
    }
}
