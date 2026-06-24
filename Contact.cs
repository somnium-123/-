using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrmLite.Models;

/// <summary>
/// 联系人实体 — 属于 Customer 聚合内部
/// </summary>
public class Contact
{
    [Key]
    public int ContactId { get; set; }

    [Required]
    [Display(Name = "所属客户")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "联系人姓名不能为空")]
    [StringLength(50, ErrorMessage = "联系人姓名最长50个字符")]
    [Display(Name = "联系人姓名")]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    [Display(Name = "职务")]
    public string? Position { get; set; }

    [StringLength(50)]
    [Display(Name = "所属部门")]
    public string? Department { get; set; }

    [Phone(ErrorMessage = "联系电话格式不正确")]
    [StringLength(20)]
    [Display(Name = "联系电话")]
    public string? Phone { get; set; }

    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    [StringLength(100)]
    [Display(Name = "电子邮箱")]
    public string? Email { get; set; }

    [Display(Name = "主要联系人")]
    public bool IsPrimary { get; set; } = false;

    // ===== 导航属性 =====
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; } = null!;

    public List<Contract> Contracts { get; set; } = new();
}
