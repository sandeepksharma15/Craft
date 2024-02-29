﻿using Craft.Auditing;
using Craft.Security.Models;
using Microsoft.EntityFrameworkCore;

namespace Craft.Data.DbContexts;

public interface ICraftDbContext
{
    DbSet<AuditTrail> AuditTrails { get; set; }
    DbSet<LoginHistory> LoginHistories { get; set; }
    DbSet<RefreshToken> RefreshTokens { get; set; }
}
