using CrmLite.Data;
using CrmLite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CrmLite.Pages.Contracts;

public class EditModel : PageModel
{
    private readonly AppDbContext _db;
    public EditModel(AppDbContext db) => _db = db;

    [BindProperty]
    public Contract Contract { get; set; } = new();
    public SelectList CustomerList { get; set; } = null!;
    public SelectList StatusList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var c = await _db.Contracts.FindAsync(id);
        if (c == null) return NotFound();
        Contract = c;

        CustomerList = new SelectList(await _db.Customers.OrderBy(c => c.Name).ToListAsync(), "CustomerId", "Name");
        StatusList = new SelectList(Enum.GetValues<ContractStatus>()
            .Select(e => new { Id = (int)e, Name = e switch { ContractStatus.InProgress => "执行中", ContractStatus.Completed => "已完成", _ => "已终止" } }), "Id", "Name");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var entity = await _db.Contracts.FindAsync(Contract.ContractId);
        if (entity == null) return NotFound();

        entity.CustomerId = Contract.CustomerId;
        entity.ContactId = Contract.ContactId;
        entity.Name = Contract.Name;
        entity.Amount = Contract.Amount;
        entity.SignedDate = Contract.SignedDate;
        entity.ExpectedPaymentDate = Contract.ExpectedPaymentDate;
        entity.Status = Contract.Status;
        entity.Remark = Contract.Remark;

        await _db.SaveChangesAsync();
        TempData["Success"] = $"合同「{entity.Name}」更新成功";
        return RedirectToPage("Index");
    }
}
