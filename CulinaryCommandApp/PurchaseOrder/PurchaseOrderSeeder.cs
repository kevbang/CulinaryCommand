using Microsoft.EntityFrameworkCore;
using CulinaryCommand.Data;
using CulinaryCommand.PurchaseOrder.Entities;

namespace CulinaryCommand.PurchaseOrder;

public static class PurchaseOrderSeeder
{
    public static async Task SeedSuppliersAsync(AppDbContext context)
    {
        if (await context.Suppliers.AnyAsync())
        {
            return; // Already seeded
        }

        var suppliers = new List<Supplier>
        {
            new Supplier
            {
                Name = "PCBA+",
                ContactPerson = "John Smith",
                Email = "orders@pcbaplus.com",
                Phone = "+1-555-0101",
                Address = "123 Tech Street, Silicon Valley, CA 94025",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Supplier
            {
                Name = "Fabby Boards",
                ContactPerson = "Sarah Johnson",
                Email = "info@fabbyboards.com",
                Phone = "+1-555-0102",
                Address = "456 Circuit Ave, Austin, TX 78701",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Supplier
            {
                Name = "PCBWOY",
                ContactPerson = "Mike Chen",
                Email = "sales@pcbwoy.com",
                Phone = "+1-555-0103",
                Address = "789 Component Lane, Boston, MA 02101",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Supplier
            {
                Name = "LCSC",
                ContactPerson = "Lisa Wang",
                Email = "support@lcsc.com",
                Phone = "+86-755-8888-8888",
                Address = "Building 5, Shenzhen, Guangdong, China",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Supplier
            {
                Name = "Wirey",
                ContactPerson = "Tom Wilson",
                Email = "orders@wirey.com",
                Phone = "+1-555-0104",
                Address = "321 Wire Way, Detroit, MI 48201",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Supplier
            {
                Name = "Wire-E-Coyote",
                ContactPerson = "Emily Rodriguez",
                Email = "sales@wireecoyote.com",
                Phone = "+1-555-0105",
                Address = "654 Desert Road, Phoenix, AZ 85001",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Supplier
            {
                Name = "Paint by Numbers",
                ContactPerson = "David Lee",
                Email = "orders@paintbynumbers.com",
                Phone = "+1-555-0106",
                Address = "987 Color Street, Portland, OR 97201",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Supplier
            {
                Name = "DigiKey",
                ContactPerson = "Jennifer Brown",
                Email = "sales@digikey.com",
                Phone = "+1-800-344-4539",
                Address = "701 Brooks Ave South, Thief River Falls, MN 56701",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Supplier
            {
                Name = "McMaster-Carr",
                ContactPerson = "Robert Taylor",
                Email = "orders@mcmaster.com",
                Phone = "+1-630-833-0300",
                Address = "600 County Line Rd, Elmhurst, IL 60126",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Supplier
            {
                Name = "Mouser",
                ContactPerson = "Amanda White",
                Email = "sales@mouser.com",
                Phone = "+1-817-804-3800",
                Address = "1000 N Main St, Mansfield, TX 76063",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Supplier
            {
                Name = "Future Electronics",
                ContactPerson = "Kevin Martinez",
                Email = "info@futureelectronics.com",
                Phone = "+1-514-694-7710",
                Address = "237 Hymus Blvd, Pointe-Claire, QC H9R 5C7, Canada",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Suppliers.AddRangeAsync(suppliers);
        await context.SaveChangesAsync();
    }
}
