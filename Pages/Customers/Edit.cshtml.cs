using CrmLite.Data;
using CrmLite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CrmLite.Pages.Customers;

public class EditModel : PageModel
{
    private readonly AppDbContext _db;
    public EditModel(AppDbContext db) => _db = db;

    [BindProperty]
    public Customer Customer { get; set; } = new();
    public SelectList LevelList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var customer = await _db.Customers.FindAsync(id);
        if (customer == null) return NotFound();
        Customer = customer;

        LevelList = new SelectList(Enum.GetValues<CustomerLevel>()
            .Select(e => new { Id = (int)e, Name = GetLevelName(e) }), "Id", "Name");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var entity = await _db.Customers.FindAsync(Customer.CustomerId);
        if (entity == null) return NotFound();

        // 手动更新允许修改的字段，保留 Code 和 CreatedAt 不变
        entity.Name = Customer.Name;
        entity.Industry = Customer.Industry;
        entity.Scale = Customer.Scale;
        entity.Level = Customer.Level;
        entity.Region = Customer.Region;
        entity.Address = Customer.Address;
        entity.PostalCode = Customer.PostalCode;
        entity.Phone = Customer.Phone;
        entity.Website = Customer.Website;
        entity.Remark = Customer.Remark;

        await _db.SaveChangesAsync();
        TempData["Success"] = $"客户「{entity.Name}」更新成功";
        return RedirectToPage("Index");
    }

    private static string GetLevelName(CustomerLevel level) => level switch
    {
        CustomerLevel.Normal => "普通客户", CustomerLevel.Important => "重要客户",
        CustomerLevel.VIP => "VIP客户", _ => "未知"
    };
}
