using CrmLite.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CrmLite.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public IndexModel(AppDbContext db) => _db = db;

    public int CustomerCount { get; set; }
    public int ContactCount { get; set; }
    public int ContractCount { get; set; }
    public int InProgressCount { get; set; }
    public decimal TotalAmount { get; set; }

    public async Task OnGetAsync()
    {
        CustomerCount = await _db.Customers.CountAsync();
        ContactCount = await _db.Contacts.CountAsync();
        ContractCount = await _db.Contracts.CountAsync();
        InProgressCount = await _db.Contracts.CountAsync(c => c.Status == Models.ContractStatus.InProgress);
        TotalAmount = await _db.Contracts.SumAsync(c => c.Amount);
    }
}
