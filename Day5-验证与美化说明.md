# Day 5 — 表单验证 + 页面美化

> **日期**：2026年6月（Day 5）  
> **负责**：关张旭（前端为主）+ 杨焱熙（后端配合）  

---

## 一、今日任务

| 类别 | 内容 | 完成 |
|------|------|:--:|
| 后端验证 | Data Annotation 完整覆盖所有实体 | ✅ |
| 后端验证 | 自定义 `[DateAfter]` 验证特性 | ✅ |
| 后端验证 | ModelState.IsValid 检查（所有 POST 方法） | ✅ |
| 前端验证 | jQuery Validation Unobtrusive | ✅ |
| 前端验证 | 必填字段红色星号标识 | ✅ |
| UI 美化 | Bootstrap 5 共享布局页（_Layout.cshtml） | ✅ |
| UI 美化 | 侧边栏导航 + 响应式折叠 | ✅ |
| UI 美化 | 统一表格样式（striped + hover + responsive） | ✅ |
| UI 美化 | 操作成功/失败 Alert 提示 | ✅ |
| UI 美化 | 客户级别/合同状态 Badge 标签 | ✅ |
| 基础设施 | Program.cs（DI + Migration + Seed） | ✅ |
| 基础设施 | appsettings.json（连接字符串） | ✅ |
| 基础设施 | SeedData（4客户 + 6联系人 + 4合同） | ✅ |

---

## 二、验证架构

```
┌─────────────────────────────────────┐
│          表单提交流程                 │
│                                     │
│   用户输入 → 前端即时验证(jQuery)     │
│          ↓                          │
│   前端验证通过？→ 否 → 红色提示       │
│          ↓ 是                       │
│   提交到服务器 → 后端验证(Data Ann)   │
│          ↓                          │
│   ModelState.IsValid？→ 否 → 返回错误 │
│          ↓ 是                       │
│   保存到数据库 ✅                     │
└─────────────────────────────────────┘
```

### 验证规则汇总

| 实体 | 字段 | 验证 |
|------|------|------|
| Customer | Name | `[Required]` `[StringLength(100)]` |
| Customer | Phone | `[Phone]` |
| Customer | Website | `[Url]` |
| Contact | Name | `[Required]` `[StringLength(50)]` |
| Contact | Email | `[EmailAddress]` |
| Contract | Name | `[Required]` `[StringLength(200)]` |
| Contract | Amount | `[Required]` `[Range(0.01, 99999999.99)]` |
| Contract | ExpectedPaymentDate | `[DateAfter("SignedDate")]` |

---

## 三、UI 布局

```
┌──────────────────────────────────────────────┐
│ NAVBAR: CRM Lite — 企业客户与合同管理系统      │
├──────────┬───────────────────────────────────┤
│ SIDEBAR  │  MAIN CONTENT                     │
│ (240px)  │                                   │
│          │  <nav breadcrumb>                 │
│ 客户管理  │  <h4 title>                       │
│ 联系人管理│  <div alert>                      │
│ 合同管理  │  <form> 或 <table>                │
│          │  ...                               │
│          │  <pagination>                     │
├──────────┴───────────────────────────────────┤
│ FOOTER: © 2026 南京工业大学 信管电子商务网站开发课程设计第10组     │
└──────────────────────────────────────────────┘
```

### 响应式断点

| 设备 | 宽度 | 行为 |
|------|:---:|------|
| 桌面 | ≥992px | 侧边栏固定 + 主内容区 |
| 平板 | 768-991px | 侧边栏折叠，汉堡菜单 |
| 手机 | <768px | 单栏，表格横向滚动 |

---

## 四、Seed 示例数据

| 类型 | 数量 | 说明 |
|------|:---:|------|
| 客户 | 4 | 苏州精工(VIP) + 上海华信(重要) + 北京长城(普通) + 深圳鹏程(VIP) |
| 联系人 | 6 | 每个客户1-2人 |
| 合同 | 4 | 金额 ¥30万~¥800万，含执行中/已完成状态 |

---

## 五、新增文件清单

| 文件 | 说明 | 作者 |
|------|------|:--:|
| `Program.cs` | 应用入口 + DI + Migration + Seed | 杨焱熙 |
| `appsettings.json` | SQL Server 连接字符串 | 杨焱熙 |
| `SeedData.cs` | 示例数据初始化 | 杨焱熙 |
| `Pages/Shared/_Layout.cshtml` | Bootstrap 5 共享布局页 | 关张旭 |
| `Pages/Shared/_ValidationScriptsPartial.cshtml` | jQuery 验证脚本引用 | 关张旭 |

---

*Day 5 文档结束*
