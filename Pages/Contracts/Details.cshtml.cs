using CrmLite.Data;
using CrmLite.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CrmLite.Pages.Contracts;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _db;
    public DetailsModel(AppDbContext db) => _db = db;

    public Contract Contract { get; set; } = new();

    public async Task OnGetAsync(int id)
    {
        Contract = await _db.Contracts
            .Include(c => c.Customer)
            .Include(c => c.Contact)
            .FirstOrDefaultAsync(c => c.ContractId == id)
            ?? new Contract();
    }
}
