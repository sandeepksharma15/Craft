using Craft.TestHelper.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Craft.TestHelper.Identity;

public class TestDbContext : IdentityDbContext<TestUser, TestRole, int>
{
    public TestDbContext(DbContextOptions<TestDbContext> options): base(options)
    {
    }
}
