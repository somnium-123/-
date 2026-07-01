# CRM Lite — API 接口文档

> **编写人**：张雨宣（后端助理）  
> **日期**：2026年6月  
> **说明**：本文档记录系统所有后端接口的请求方式、参数和返回格式，供前端开发和集成测试参考。

---

## 一、客户管理接口

### 1.1 客户列表

| 项目 | 内容 |
|------|------|
| 请求方式 | `GET` |
| 路由 | `/Customers` |
| 参数 | `searchName`（可选，模糊搜索客户名称）、`searchIndustry`（可选，模糊搜索行业）、`pageIndex`（可选，页码，默认1） |
| 返回 | Razor Page（服务端渲染） |

### 1.2 新增客户

| 项目 | 内容 |
|------|------|
| 请求方式 | `GET` / `POST` |
| 路由 | `/Customers/Create` |
| GET 说明 | 加载空表单 + 客户级别下拉列表 |
| POST 参数 | `Customer.Name`（必填）、`Customer.Industry`、`Customer.Scale`、`Customer.Level`（枚举0/1/2）、`Customer.Region`、`Customer.Address`、`Customer.PostalCode`、`Customer.Phone`、`Customer.Website`、`Customer.Remark` |
| POST 返回 | 成功 → 重定向到 `/Customers`（TempData[Success]提示）/ 失败 → 返回当前页（ModelState错误提示） |
| 自动生成 | `Customer.Code`（CUS-YYYYMMDD-XXX）、`Customer.CreatedAt`（DateTime.Now） |

### 1.3 编辑客户

| 项目 | 内容 |
|------|------|
| 请求方式 | `GET` / `POST` |
| 路由 | `/Customers/Edit/{id}` |
| GET 说明 | 根据 id 加载客户实体并填充表单 |
| POST 说明 | 手动逐字段更新（Code 和 CreatedAt 受保护不可修改） |
| POST 返回 | 成功 → 重定向到 `/Customers` / 失败 → 返回当前页 |

### 1.4 删除客户

| 项目 | 内容 |
|------|------|
| 请求方式 | `GET` / `POST` |
| 路由 | `/Customers/Delete/{id}` |
| GET 说明 | 加载客户信息 + Include Contracts 检查关联合同数量 |
| 业务规则 | 存在关联合同时显示警告并阻止删除 → 重定向到 `/Customers`（TempData[Error]提示）/ 无合同时执行 CASCADE 级联删除，联系人同步删除 |

### 1.5 客户详情

| 项目 | 内容 |
|------|------|
| 请求方式 | `GET` |
| 路由 | `/Customers/Details/{id}` |
| 说明 | Include 预加载 Contacts 和 Contracts 导航属性 |

---

## 二、联系人管理接口

### 2.1 联系人列表

| 项目 | 内容 |
|------|------|
| 请求方式 | `GET` |
| 路由 | `/Contacts` |
| 参数 | `customerId`（可选，按客户筛选） |
| 返回 | Razor Page（服务端渲染，含关联 Customer 信息） |

### 2.2 新增联系人

| 项目 | 内容 |
|------|------|
| 请求方式 | `GET` / `POST` |
| 路由 | `/Contacts/Create` |
| POST 参数 | `Contact.CustomerId`（必选）、`Contact.Name`（必填）、`Contact.Position`、`Contact.Department`、`Contact.Phone`、`Contact.Email`、`Contact.IsPrimary`（bool） |

### 2.3 编辑联系人

| 项目 | 内容 |
|------|------|
| 请求方式 | `GET` / `POST` |
| 路由 | `/Contacts/Edit/{id}` |
| 说明 | 允许更换所属客户（CustomerId 可修改） |

---

## 三、合同管理接口

### 3.1 合同列表

| 项目 | 内容 |
|------|------|
| 请求方式 | `GET` |
| 路由 | `/Contracts` |
| 参数 | `filterStatus`（可选，0=执行中/1=已完成/2=已终止）、`pageIndex`（可选，页码） |
| 返回 | Razor Page（Include 预加载 Customer 和 Contact，金额¥格式显示） |

### 3.2 新增合同

| 项目 | 内容 |
|------|------|
| 请求方式 | `GET` / `POST` |
| 路由 | `/Contracts/Create` |
| POST 参数 | `Contract.Name`（必填）、`Contract.CustomerId`（必选）、`Contract.ContactId`（可选，可为null）、`Contract.Amount`（必填，>0，decimal(18,2)）、`Contract.SignedDate`（必填）、`Contract.ExpectedPaymentDate`（可选）、`Contract.Status`（枚举0/1/2）、`Contract.Remark` |
| 自动生成 | `Contract.ContractCode`（CON-YYYYMMDD-XXX） |
| 业务验证 | DateAfterAttribute: ExpectedPaymentDate ≥ SignedDate |

### 3.3 ★ 客户-联系人联动 API

| 项目 | 内容 |
|------|------|
| 请求方式 | `GET`（AJAX） |
| 路由 | `/Contracts/Create?handler=ContactsByCustomer&customerId={id}` |
| 说明 | Razor Pages Named Handler: `OnGetContactsByCustomerAsync` |
| 参数 | `customerId`（int，必选） |
| 返回格式 | `JSON` — `[{ "contactId": 1, "name": "张经理" }, ...]` |
| 返回示例 | `[{"contactId":1,"name":"张经理"},{"contactId":2,"name":"李工程师"}]` |
| 空结果 | 客户无联系人时返回 `[]`，不存在的客户ID返回 `[]` |

### 3.4 编辑合同

| 项目 | 内容 |
|------|------|
| 请求方式 | `GET` / `POST` |
| 路由 | `/Contracts/Edit/{id}` |
| 说明 | ContractCode 受保护不可修改，客户更换后联系人列表需重新联动加载 |

### 3.5 删除合同

| 项目 | 内容 |
|------|------|
| 请求方式 | `GET` / `POST` |
| 路由 | `/Contracts/Delete/{id}` |
| 说明 | 合同删除不影响关联的客户和联系人（ContactId 为 SET NULL 规则在删除联系人时生效，删除合同只移除合同记录） |

### 3.6 合同详情

| 项目 | 内容 |
|------|------|
| 请求方式 | `GET` |
| 路由 | `/Contracts/Details/{id}` |
| 说明 | Include 预加载 Customer 和 Contact，含关联客户/联系人信息卡片 |

---

## 四、数据验证规则汇总

| 实体 | 字段 | 验证特性 | 错误消息 |
|------|------|----------|----------|
| Customer | Name | `[Required]` `[StringLength(100)]` | 客户名称不能为空 / 最长100个字符 |
| Customer | Phone | `[Phone]` | 联系电话格式不正确 |
| Customer | Website | `[Url]` | 网址格式不正确 |
| Contact | Name | `[Required]` `[StringLength(50)]` | 联系人姓名不能为空 / 最长50个字符 |
| Contact | Email | `[EmailAddress]` | 邮箱格式不正确 |
| Contract | Name | `[Required]` `[StringLength(200)]` | 合同名称不能为空 / 最长200个字符 |
| Contract | Amount | `[Required]` `[Range(0.01, 99999999.99)]` | 合同金额必须在 0.01 ~ 99,999,999.99 之间 |
| Contract | ExpectedPaymentDate | `[DateAfter("SignedDate")]` | 预计回款日期不能早于签订日期 |

---

## 五、外键删除规则

| 父表 | 子表 | 外键 | ON DELETE | 业务含义 |
|------|------|------|:---------:|------|
| Customers | Contacts | CustomerId | CASCADE | 删除客户 → 级联删除联系人 |
| Customers | Contracts | CustomerId | RESTRICT | 客户有关联合同时禁止删除 |
| Contacts | Contracts | ContactId | SET NULL | 删除联系人 → 合同引用置空 |

---

*API接口文档结束 — 张雨宣编写*
