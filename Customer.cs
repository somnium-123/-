using System.ComponentModel.DataAnnotations;

namespace CrmLite.Models;

/// <summary>
/// 客户聚合根 — 企业客户与合同管理系统的核心实体
/// </summary>
public class Customer
{
    [Key]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "客户编号不能为空")]
    [StringLength(20)]
    [Display(Name = "客户编号")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "客户名称不能为空")]
    [StringLength(100, ErrorMessage = "客户名称最长100个字符")]
    [Display(Name = "客户名称")]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    [Display(Name = "所属行业")]
    public string? Industry { get; set; }

    [StringLength(20)]
    [Display(Name = "企业规模")]
    public string? Scale { get; set; }

    [Required]
    [Display(Name = "客户级别")]
    public CustomerLevel Level { get; set; } = CustomerLevel.Normal;

    [StringLength(50)]
    [Display(Name = "所在地区")]
    public string? Region { get; set; }

    [StringLength(200)]
    [Display(Name = "详细地址")]
    public string? Address { get; set; }

    [StringLength(10)]
    [Display(Name = "邮政编码")]
    public string? PostalCode { get; set; }

    [Phone(ErrorMessage = "联系电话格式不正确")]
    [StringLength(20)]
    [Display(Name = "联系电话")]
    public string? Phone { get; set; }

    [Url(ErrorMessage = "网址格式不正确")]
    [StringLength(100)]
    [Display(Name = "企业网址")]
    public string? Website { get; set; }

    [StringLength(500)]
    [Display(Name = "备注说明")]
    public string? Remark { get; set; }

    [Required]
    [Display(Name = "创建时间")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // ===== 导航属性 =====
    public List<Contact> Contacts { get; set; } = new();
    public List<Contract> Contracts { get; set; } = new();
}

/// <summary>
/// 客户级别枚举
/// </summary>
public enum CustomerLevel
{
    [Display(Name = "普通客户")]
    Normal = 0,

    [Display(Name = "重要客户")]
    Important = 1,

    [Display(Name = "VIP客户")]
    VIP = 2
}
