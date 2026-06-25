using CrmLite.Data;
using CrmLite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CrmLite.Pages.Contacts;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public IndexModel(AppDbContext db) => _db = db;

    public List<Contact> Contacts { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int? CustomerId { get; set; }

    public async Task OnGetAsync()
    {
        var query = _db.Contacts.Include(c => c.Customer).AsQueryable();
        if (CustomerId.HasValue)
            query = query.Where(c => c.CustomerId == CustomerId.Value);
        Contacts = await query.OrderBy(c => c.Customer.Name).ThenBy(c => c.Name).ToListAsync();
    }
}
