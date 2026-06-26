using CrmLite.Data;
using CrmLite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CrmLite.Pages.Contracts;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    private const int PageSize = 10;

    public IndexModel(AppDbContext db) => _db = db;

    public List<Contract> Contracts { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int? FilterCustomerId { get; set; }

    [BindProperty(SupportsGet = true)]
    public ContractStatus? FilterStatus { get; set; }

    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }

    public async Task OnGetAsync(int pageIndex = 1)
    {
        var query = _db.Contracts
            .Include(c => c.Customer)
            .Include(c => c.Contact)
            .AsQueryable();

        if (FilterCustomerId.HasValue)
            query = query.Where(c => c.CustomerId == FilterCustomerId.Value);

        if (FilterStatus.HasValue)
            query = query.Where(c => c.Status == FilterStatus.Value);

        var total = await query.CountAsync();
        TotalPages = (int)Math.Ceiling(total / (double)PageSize);
        CurrentPage = Math.Clamp(pageIndex, 1, Math.Max(1, TotalPages));

        Contracts = await query
            .OrderByDescending(c => c.SignedDate)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();
    }
}
