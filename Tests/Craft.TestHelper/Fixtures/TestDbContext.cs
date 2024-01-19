using Craft.TestHelper.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Craft.TestHelper.Identity;

public class TestDbContext(DbContextOptions<TestDbContext> options) : IdentityDbContext<TestUser, TestRole, int>(options)
{
}
