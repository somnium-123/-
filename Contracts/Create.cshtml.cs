using CrmLite.Data;
using CrmLite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CrmLite.Pages.Contracts;

public class CreateModel : PageModel
{
    private readonly AppDbContext _db;
    public CreateModel(AppDbContext db) => _db = db;

    [BindProperty]
    public Contract Contract { get; set; } = new();

    public SelectList CustomerList { get; set; } = null!;
    public SelectList StatusList { get; set; } = null!;

    public async Task OnGetAsync()
    {
        CustomerList = new SelectList(await _db.Customers.OrderBy(c => c.Name).ToListAsync(), "CustomerId", "Name");
        StatusList = new SelectList(Enum.GetValues<ContractStatus>()
            .Select(e => new { Id = (int)e, Name = GetStatusName(e) }), "Id", "Name");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            CustomerList = new SelectList(await _db.Customers.OrderBy(c => c.Name).ToListAsync(), "CustomerId", "Name");
            StatusList = new SelectList(Enum.GetValues<ContractStatus>().Select(e => new { Id = (int)e, Name = GetStatusName(e) }), "Id", "Name");
            return Page();
        }

        var today = DateTime.Now.ToString("yyyyMMdd");
        var count = await _db.Contracts.CountAsync(c => c.ContractCode.StartsWith($"CON-{today}"));
        Contract.ContractCode = $"CON-{today}-{(count + 1):D3}";

        _db.Contracts.Add(Contract);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"合同「{Contract.Name}」创建成功";
        return RedirectToPage("Index");
    }

    /// <summary>AJAX: 按客户ID返回联系人列表(JSON)</summary>
    public async Task<IActionResult> OnGetContactsByCustomerAsync(int customerId)
    {
        var contacts = await _db.Contacts
            .Where(c => c.CustomerId == customerId)
            .OrderBy(c => c.Name)
            .Select(c => new { c.ContactId, c.Name })
            .ToListAsync();
        return new JsonResult(contacts);
    }

    private static string GetStatusName(ContractStatus s) => s switch
    {
        ContractStatus.InProgress => "执行中", ContractStatus.Completed => "已完成",
        ContractStatus.Terminated => "已终止", _ => "未知"
    };
}
