# Day 2 — Code First 实体与数据库实现说明

> **日期**：2026年6月（Day 2）  
> **汇报人**：黄陈熙（DDD领域建模师）  
> **开发模式**：Code First + Migration  

---

## 一、今日目标

- 编写 Customer / Contact / Contract 三个实体类
- 配置 AppDbContext + Fluent API 实体映射
- 执行 Add-Migration InitSchema
- 执行 Update-Database 在 SQL Server 中创建数据表
- 验证表结构、外键关系、约束正确

---

## 二、已创建文件

| 文件 | 说明 | 行数 |
|------|------|:---:|
| `Customer.cs` | 客户聚合根 + CustomerLevel 枚举 | ~70 |
| `Contact.cs` | 联系人实体，FK→Customer | ~40 |
| `Contract.cs` | 合同聚合根 + ContractStatus 枚举 | ~55 |
| `DateAfterAttribute.cs` | 自定义验证：回款日期≥签订日期 | ~30 |
| `AppDbContext.cs` | DbContext + Fluent API 完整配置 | ~100 |

---

## 三、实体关系与删除规则

| 父表 | 关系 | 子表 | 外键 | ON DELETE |
|------|:---:|------|------|:---------:|
| Customer | 1:N | Contact | Contact.CustomerId | **CASCADE** |
| Customer | 1:N | Contract | Contract.CustomerId | **RESTRICT** |
| Contact | 0..1:N | Contract | Contract.ContactId | **SET NULL** |

---

## 四、Data Annotation 验证汇总

| 实体 | 字段 | 验证特性 |
|------|------|----------|
| Customer | Name | `[Required]` `[StringLength(100)]` |
| Customer | Code | `[Required]` `[StringLength(20)]` |
| Customer | Phone | `[Phone]` `[StringLength(20)]` |
| Customer | Website | `[Url]` `[StringLength(100)]` |
| Contact | Name | `[Required]` `[StringLength(50)]` |
| Contact | Email | `[EmailAddress]` `[StringLength(100)]` |
| Contact | Phone | `[Phone]` `[StringLength(20)]` |
| Contract | Name | `[Required]` `[StringLength(200)]` |
| Contract | Amount | `[Required]` `[Range(0.01, 99999999.99)]` |
| Contract | SignedDate | `[Required]` `[DataType(Date)]` |
| Contract | ExpectedPaymentDate | `[DateAfter("SignedDate")]` |

---

## 五、Migration 执行命令

```bash
# 在 Visual Studio Package Manager Console 中执行：

# 1. 创建初始迁移
Add-Migration InitSchema

# 2. 应用到数据库
Update-Database

# 3. 验证（SQL Server Management Studio 或 VS 中查看）
#    - Customers 表 (13列)
#    - Contacts 表 (8列)  
#    - Contracts 表 (10列)
#    - __EFMigrationsHistory 表 (1条记录)
```

---

## 六、数据库连接字符串

```json
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CrmLite;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

---

## 七、下一步（Day 3）

Day 3 将基于这些实体实现：
- CustomerController — 客户 CRUD
- ContactController — 联系人 CRUD
- Razor Pages — 客户/联系人页面

---

*Day 2 文档结束*
