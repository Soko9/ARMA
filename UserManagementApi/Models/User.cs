﻿using System;
using System.Collections.Generic;

namespace UserManagementApi.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Passcode { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public byte FailedLoginAttempts { get; set; }

    public bool IsLocked { get; set; }

    public bool IsActive { get; set; }

    public DateTime LastSuccessfulLoginAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? LastActionUserId { get; set; }

    public Guid? RoleId { get; set; }

    public virtual ICollection<IntegratedSystem> IntegratedSystems { get; set; } = new List<IntegratedSystem>();

    public virtual ICollection<User> InverseLastActionUser { get; set; } = new List<User>();

    public virtual ICollection<IpwhiteList> IpwhiteLists { get; set; } = new List<IpwhiteList>();

    public virtual User? LastActionUser { get; set; }

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual ICollection<PermissionCategory> PermissionCategories { get; set; } = new List<PermissionCategory>();

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<RolesPermission> RolesPermissions { get; set; } = new List<RolesPermission>();
}
