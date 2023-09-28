# 取章排版系統 後端

## 技術說明

---

* 程式語言：C#
* 專案架構：.Net 6
* 使用第三方元件：
1. Serilog.AspNetCore：7.0.0
1. Serilog.Formatting.Compact：1.1.0
1. Serilog.Sinks.Async：1.5.0
1. RestSharp：110.2.0 (DJKeycloakWebAPILib使用到之後會拆分內部元件)
1. Keycloak.AuthServices.Authentication：1.5.2
1. Keycloak.AuthServices.Authorization：1.5.2
1. AutoMapper.Extensions.Microsoft.DependencyInjection：12.0.1
1. Microsoft.EntityFrameworkCore.Tools：7.0.10
1. Microsoft.Extensions.Hosting.WindowsServices：7.0.1
1. System.Drawing.Common：7.0.0
1. Spire.Pdf：9.7.0(之後拆分成公司內部元件)
1. EFCore.BulkExtensions：7.1.6
1. Microsoft.EntityFrameworkCore：7.0.10
1. Microsoft.Extensions.Identity.Stores：6.0.21
1. Microsoft.EntityFrameworkCore.Sqlite：7.0.10
1. Pomelo.EntityFrameworkCore.MySql：7.0.0
* 使用公司內部元件：
1. DJImageLib：1.0.1

