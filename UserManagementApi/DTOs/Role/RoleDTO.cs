﻿namespace UserManagementApi.DTOs.Role
{
    public class RoleDTO
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public byte Priority { get; set; }

        public Guid? LastActionUserId { get; set; }
    }
}
