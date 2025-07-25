﻿using Microsoft.EntityFrameworkCore;
using UserManagementApi.Models;
using UserManagementApi.Repo;

namespace UserManagementApi.Services
{
    public class LogService : ILogService
    {
        private readonly UserManagementDbContext _Db;
        private readonly DbSet<Log> _Table;

        public LogService(UserManagementDbContext Db)
        {
            _Db = Db;
            _Table = Db.Logs;
        }

        public async Task<IReadOnlyList<Log>> GetAllAsync()
            => await _Table
                .AsNoTracking()
                .OrderBy(L => L.ActionAt)
                .ToListAsync();

        public async Task LogAsync(string Type, string Desc, string Table, Guid RowId, Guid? UserID)
        {
            Log Entity = new Log
            {
                LogId = Guid.NewGuid(),
                ActionType = Type,
                Description = Desc,
                ActionTableName = Table,
                ActionRowId = RowId,
                ActionUserId = UserID!,
                ActionAt = DateTime.Now,
            };

            await _Table.AddAsync(Entity);
            await _Db.SaveChangesAsync();
            Console.WriteLine(Entity.ToString());
        }
    }
}
