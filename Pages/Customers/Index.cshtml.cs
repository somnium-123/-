using CrmLite.Data;
using CrmLite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CrmLite.Pages.Customers;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    private const int PageSize = 10;

    public IndexModel(AppDbContext db) => _db = db;

    public List<Customer> Customers { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SearchName { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SearchIndustry { get; set; }

    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }

    public async Task OnGetAsync(int pageIndex = 1)
    {
        var query = _db.Customers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(SearchName))
            query = query.Where(c => c.Name.Contains(SearchName));

        if (!string.IsNullOrWhiteSpace(SearchIndustry))
            query = query.Where(c => c.Industry != null && c.Industry.Contains(SearchIndustry));

        var total = await query.CountAsync();
        TotalPages = (int)Math.Ceiling(total / (double)PageSize);
        CurrentPage = Math.Clamp(pageIndex, 1, Math.Max(1, TotalPages));

        Customers = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();
    }
}
