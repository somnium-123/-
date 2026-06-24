using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrmLite.Models;

/// <summary>
/// 合同聚合根 — 管理合同全生命周期
/// </summary>
public class Contract
{
    [Key]
    public int ContractId { get; set; }

    [Required(ErrorMessage = "合同编号不能为空")]
    [StringLength(20)]
    [Display(Name = "合同编号")]
    public string ContractCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "合同名称不能为空")]
    [StringLength(200, ErrorMessage = "合同名称最长200个字符")]
    [Display(Name = "合同名称")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "请选择客户")]
    [Display(Name = "关联客户")]
    public int CustomerId { get; set; }

    [Display(Name = "关联联系人")]
    public int? ContactId { get; set; }

    [Required(ErrorMessage = "合同金额不能为空")]
    [Range(0.01, 99999999.99, ErrorMessage = "合同金额必须在 0.01 ~ 99,999,999.99 之间")]
    [Display(Name = "合同金额")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "签订日期不能为空")]
    [Display(Name = "签订日期")]
    [DataType(DataType.Date)]
    public DateTime SignedDate { get; set; }

    [Display(Name = "预计回款日期")]
    [DataType(DataType.Date)]
    [DateAfter(nameof(SignedDate), ErrorMessage = "预计回款日期不能早于签订日期")]
    public DateTime? ExpectedPaymentDate { get; set; }

    [Required]
    [Display(Name = "合同状态")]
    public ContractStatus Status { get; set; } = ContractStatus.InProgress;

    [StringLength(500)]
    [Display(Name = "备注说明")]
    public string? Remark { get; set; }

    // ===== 导航属性 =====
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; } = null!;

    [ForeignKey("ContactId")]
    public Contact? Contact { get; set; }
}

/// <summary>
/// 合同状态枚举
/// </summary>
public enum ContractStatus
{
    [Display(Name = "执行中")]
    InProgress = 0,

    [Display(Name = "已完成")]
    Completed = 1,

    [Display(Name = "已终止")]
    Terminated = 2
}
