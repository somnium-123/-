using CrmLite.Data;
using CrmLite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CrmLite.Pages.Contacts;

public class CreateModel : PageModel
{
    private readonly AppDbContext _db;
    public CreateModel(AppDbContext db) => _db = db;

    [BindProperty]
    public Contact Contact { get; set; } = new();

    public SelectList CustomerList { get; set; } = null!;

    public async Task OnGetAsync()
    {
        CustomerList = new SelectList(
            await _db.Customers.OrderBy(c => c.Name).ToListAsync(),
            "CustomerId", "Name");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            CustomerList = new SelectList(await _db.Customers.OrderBy(c => c.Name).ToListAsync(), "CustomerId", "Name");
            return Page();
        }

        _db.Contacts.Add(Contact);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"联系人「{Contact.Name}」创建成功";
        return RedirectToPage("Index");
    }
}
