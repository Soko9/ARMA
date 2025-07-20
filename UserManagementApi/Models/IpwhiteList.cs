using System;
using System.Collections.Generic;

namespace UserManagementApi.Models;

public partial class IpwhiteList
{
    public Guid Ipid { get; set; }

    public string Ip { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? LastActionUserId { get; set; }

    public Guid? SystemId { get; set; }

    public virtual User? LastActionUser { get; set; }

    public virtual IntegratedSystem? System { get; set; }
}
