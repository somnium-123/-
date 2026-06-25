using CrmLite.Data;
using CrmLite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmLite.Pages.Customers;

public class CreateModel : PageModel
{
    private readonly AppDbContext _db;
    public CreateModel(AppDbContext db) => _db = db;

    [BindProperty]
    public Customer Customer { get; set; } = new();

    public SelectList LevelList { get; set; } = null!;

    public IActionResult OnGet()
    {
        LevelList = new SelectList(Enum.GetValues<CustomerLevel>()
            .Select(e => new { Id = (int)e, Name = GetLevelName(e) }), "Id", "Name");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            LevelList = new SelectList(Enum.GetValues<CustomerLevel>()
                .Select(e => new { Id = (int)e, Name = GetLevelName(e) }), "Id", "Name");
            return Page();
        }

        // 自动生成客户编号 CUS-YYYYMMDD-XXX
        var today = DateTime.Now.ToString("yyyyMMdd");
        var count = await _db.Customers.CountAsync(c => c.Code.StartsWith($"CUS-{today}"));
        Customer.Code = $"CUS-{today}-{(count + 1):D3}";
        Customer.CreatedAt = DateTime.Now;

        _db.Customers.Add(Customer);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"客户「{Customer.Name}」创建成功";
        return RedirectToPage("Index");
    }

    private static string GetLevelName(CustomerLevel level) => level switch
    {
        CustomerLevel.Normal => "普通客户",
        CustomerLevel.Important => "重要客户",
        CustomerLevel.VIP => "VIP客户",
        _ => "未知"
    };
}
