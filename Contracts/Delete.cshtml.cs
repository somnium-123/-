using CrmLite.Data;
using CrmLite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CrmLite.Pages.Contracts;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _db;
    public DeleteModel(AppDbContext db) => _db = db;

    public Contract Contract { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Contract = await _db.Contracts
            .Include(c => c.Customer)
            .FirstOrDefaultAsync(c => c.ContractId == id);
        if (Contract == null) return NotFound();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var c = await _db.Contracts.FindAsync(id);
        if (c == null) return NotFound();

        _db.Contracts.Remove(c);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"合同「{c.Name}」已删除";
        return RedirectToPage("Index");
    }
}
