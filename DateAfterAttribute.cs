using System.ComponentModel.DataAnnotations;

namespace CrmLite.Models;

/// <summary>
/// 自定义验证特性：验证日期是否在另一个日期之后
/// 用于 Contract.ExpectedPaymentDate >= Contract.SignedDate
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DateAfterAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public DateAfterAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success; // 可选字段允许为空

        var currentValue = (DateTime)value;
        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

        if (property == null)
            return new ValidationResult($"未知属性: {_comparisonProperty}");

        var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance)!;

        if (currentValue < comparisonValue)
            return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} 不能早于 {_comparisonProperty}");

        return ValidationResult.Success;
    }
}
