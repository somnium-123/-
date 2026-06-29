using CrmLite.Data;
using CrmLite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CrmLite.Pages.Customers;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _db;
    public DeleteModel(AppDbContext db) => _db = db;

    public Customer Customer { get; set; } = new();
    public bool HasContracts { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var customer = await _db.Customers
            .Include(c => c.Contracts)
            .FirstOrDefaultAsync(c => c.CustomerId == id);
        if (customer == null) return NotFound();

        Customer = customer;
        HasContracts = customer.Contracts.Any();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var customer = await _db.Customers
            .Include(c => c.Contracts)
            .FirstOrDefaultAsync(c => c.CustomerId == id);
        if (customer == null) return NotFound();

        if (customer.Contracts.Any())
        {
            TempData["Error"] = $"客户「{customer.Name}」存在关联合同，无法删除";
            return RedirectToPage("Index");
        }

        _db.Customers.Remove(customer);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"客户「{customer.Name}」已删除";
        return RedirectToPage("Index");
    }
}
