using CrmLite.Data;
using CrmLite.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CrmLite.Pages.Customers;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _db;
    public DetailsModel(AppDbContext db) => _db = db;

    public Customer Customer { get; set; } = new();

    public async Task OnGetAsync(int id)
    {
        Customer = await _db.Customers
            .Include(c => c.Contacts)
            .Include(c => c.Contracts)
            .FirstOrDefaultAsync(c => c.CustomerId == id)
            ?? new Customer();
    }
}
