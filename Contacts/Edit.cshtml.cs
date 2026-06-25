using CrmLite.Data;
using CrmLite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CrmLite.Pages.Contacts;

public class EditModel : PageModel
{
    private readonly AppDbContext _db;
    public EditModel(AppDbContext db) => _db = db;

    [BindProperty]
    public Contact Contact { get; set; } = new();
    public SelectList CustomerList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var contact = await _db.Contacts.FindAsync(id);
        if (contact == null) return NotFound();
        Contact = contact;

        CustomerList = new SelectList(await _db.Customers.OrderBy(c => c.Name).ToListAsync(), "CustomerId", "Name");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var entity = await _db.Contacts.FindAsync(Contact.ContactId);
        if (entity == null) return NotFound();

        entity.CustomerId = Contact.CustomerId;
        entity.Name = Contact.Name;
        entity.Position = Contact.Position;
        entity.Department = Contact.Department;
        entity.Phone = Contact.Phone;
        entity.Email = Contact.Email;
        entity.IsPrimary = Contact.IsPrimary;

        await _db.SaveChangesAsync();
        TempData["Success"] = $"联系人「{entity.Name}」更新成功";
        return RedirectToPage("Index");
    }
}
