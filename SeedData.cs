using CrmLite.Models;

namespace CrmLite.Data;

/// <summary>
/// 数据库 Seed 初始化 — 预置演示用示例数据
/// </summary>
public static class SeedData
{
    public static void Initialize(AppDbContext db)
    {
        // 已有数据则跳过
        if (db.Customers.Any()) return;

        var now = DateTime.Now;
        var today = now.ToString("yyyyMMdd");

        // ===== 示例客户 =====
        var customers = new List<Customer>
        {
            new()
            {
                Code = $"CUS-{today}-001", Name = "苏州精工制造有限公司",
                Industry = "制造业", Scale = "大型企业", Level = CustomerLevel.VIP,
                Region = "华东", Address = "江苏省苏州市工业园区苏虹路100号",
                PostalCode = "215000", Phone = "0512-88888888",
                Website = "https://www.szjinggong.com", Remark = "长期合作客户，年采购额超500万",
                CreatedAt = now.AddDays(-90)
            },
            new()
            {
                Code = $"CUS-{today}-002", Name = "上海华信科技有限公司",
                Industry = "IT/互联网", Scale = "中型企业", Level = CustomerLevel.Important,
                Region = "华东", Address = "上海市浦东新区张江高科技园区",
                PostalCode = "201203", Phone = "021-66666666",
                Website = "https://www.huaxin-tech.cn", Remark = "技术合作伙伴",
                CreatedAt = now.AddDays(-60)
            },
            new()
            {
                Code = $"CUS-{today}-003", Name = "北京长城贸易有限公司",
                Industry = "服务业", Scale = "中型企业", Level = CustomerLevel.Normal,
                Region = "华北", Address = "北京市朝阳区建国路88号",
                PostalCode = "100022", Phone = "010-88888888",
                Remark = "新开发客户，有较大增长潜力",
                CreatedAt = now.AddDays(-30)
            },
            new()
            {
                Code = $"CUS-{today}-004", Name = "深圳鹏程电子科技有限公司",
                Industry = "电子制造", Scale = "大型企业", Level = CustomerLevel.VIP,
                Region = "华南", Address = "深圳市南山区科技园",
                PostalCode = "518000", Phone = "0755-99999999",
                CreatedAt = now.AddDays(-15)
            },
        };

        db.Customers.AddRange(customers);
        db.SaveChanges();

        // ===== 示例联系人 =====
        var contacts = new List<Contact>
        {
            new() { CustomerId = customers[0].CustomerId, Name = "张经理", Position = "采购部经理",
                Department = "采购部", Phone = "0512-88888801", Email = "zhang@szjinggong.com", IsPrimary = true },
            new() { CustomerId = customers[0].CustomerId, Name = "李工程师", Position = "技术部主管",
                Department = "技术部", Phone = "0512-88888802", Email = "li@szjinggong.com", IsPrimary = false },
            new() { CustomerId = customers[1].CustomerId, Name = "王总监", Position = "技术总监",
                Department = "研发部", Phone = "021-66666601", Email = "wang@huaxin-tech.cn", IsPrimary = true },
            new() { CustomerId = customers[1].CustomerId, Name = "赵经理", Position = "市场部经理",
                Department = "市场部", Phone = "021-66666602", Email = "zhao@huaxin-tech.cn", IsPrimary = false },
            new() { CustomerId = customers[2].CustomerId, Name = "陈总", Position = "总经理",
                Department = "总经办", Phone = "010-88888801", Email = "chen@beijingcc.com", IsPrimary = true },
            new() { CustomerId = customers[3].CustomerId, Name = "刘工", Position = "研发部主管",
                Department = "研发部", Phone = "0755-99999901", Email = "liu@pengcheng.com", IsPrimary = true },
        };

        db.Contacts.AddRange(contacts);
        db.SaveChanges();

        // ===== 示例合同 =====
        var contracts = new List<Contract>
        {
            new()
            {
                ContractCode = $"CON-{today}-001", Name = "精密零部件采购合同",
                CustomerId = customers[0].CustomerId, ContactId = contacts[0].ContactId,
                Amount = 500000.00m, SignedDate = now.AddDays(-60),
                ExpectedPaymentDate = now.AddDays(30), Status = ContractStatus.InProgress,
                Remark = "分三期付款，每期166,666.67元"
            },
            new()
            {
                ContractCode = $"CON-{today}-002", Name = "信息系统开发合同",
                CustomerId = customers[1].CustomerId, ContactId = contacts[2].ContactId,
                Amount = 1200000.00m, SignedDate = now.AddDays(-45),
                ExpectedPaymentDate = now.AddDays(90), Status = ContractStatus.InProgress,
                Remark = "包含OA模块与CRM模块定制开发"
            },
            new()
            {
                ContractCode = $"CON-{today}-003", Name = "物流服务年度合同",
                CustomerId = customers[2].CustomerId, ContactId = contacts[4].ContactId,
                Amount = 300000.00m, SignedDate = now.AddDays(-120),
                ExpectedPaymentDate = now.AddDays(-30), Status = ContractStatus.Completed,
                Remark = "已完成全部回款"
            },
            new()
            {
                ContractCode = $"CON-{today}-004", Name = "芯片采购框架协议",
                CustomerId = customers[3].CustomerId, ContactId = contacts[5].ContactId,
                Amount = 8000000.00m, SignedDate = now.AddDays(-10),
                ExpectedPaymentDate = now.AddDays(120), Status = ContractStatus.InProgress,
                Remark = "框架协议，按实际订单分批结算"
            },
        };

        db.Contracts.AddRange(contracts);
        db.SaveChanges();
    }
}
