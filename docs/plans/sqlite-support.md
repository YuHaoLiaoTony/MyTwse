# Plan: 加入 SQLite 支援

## Problem

目前 API 需依賴 SQL Server Express + `TwseStock` 資料庫才能啟動，無法直接執行。需要補上 `appsettings.Development.json` 中可切換的 SQLite 支援，讓開發體驗零安裝。

## 核心挑戰

現有 `ExecSQL` 寫在 `BaseRepository` 中，使用 `SqlParameter`。使用者要求**不修改 `BaseRepository.cs`**。

且 `StockInfoRepository.GetYieldRateIncreaseＭaxDays` 使用的 CTE SQL 包含 `ROW_NUMBER()`，SQLite 目前版本（3.25+）已支援，語法基本相容。

## Approach

### 1. 新增 provider-neutral SQL helper

`MyTwse/Infrastructure/SqliteHelper.cs`

```csharp
public static class SqliteHelper
{
    // 與 BaseRepository.ExecSQL 功能相同，但使用 command.CreateParameter() 而非 SqlParameter
    public static List<T> ExecSQL<T>(DbContext db, string query, object parameters = null) where T : class
}
```

### 2. 新增 SQLite 版本的 Repository

`MyTwse/Repositories/SqliteStockInfoRepository.cs`（命名規則：`Sqlite{Entity}Repository`）

繼承自 `BaseTwseStockRepository<StockInfo>`（保留 EF Core CRUD），但 `GetYieldRateIncreaseＭaxDays` 改用 `SqliteHelper.ExecSQL`，不使用繼承來的 SqlServer `ExecSQL`。

原本的 T-SQL 語法需要調整：
- 無參數化調整（SQLite 支援 `@` 參數）
- `ROW_NUMBER()` 在 SQLite 3.25+ 已支援，沿用即可
- `[DATE]` 改用 `"Date"` 或 `[Date]`（SQLite 也支援 `[]`）

### 3. 修改 EF Core 註冊

`MyTwse/Startup.cs`

```csharp
// 從 appsettings.json 讀取 Provider 設定
var provider = Configuration.GetValue<string>("DatabaseProvider");

if (provider == "Sqlite")
{
    services.AddDbContext<TwseStockContext>(options =>
        options.UseSqlite(Configuration.GetConnectionString("SQLite")));
}
else
{
    services.AddDbContext<TwseStockContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DBConStr")));
}
```

### 4. DI 註冊條件切換

```csharp
if (provider == "Sqlite")
    services.AddScoped<IStockInfoRepository, SqliteStockInfoRepository>();
else
    services.AddScoped<IStockInfoRepository, StockInfoRepository>();
```

### 5. 修改 `MyTwse.csproj`

加入 `Microsoft.EntityFrameworkCore.Sqlite` 套件（既有 `SqlServer` 套件保留）。

### 6. 編輯 `appsettings.json`

加入兩組連線字串 + provider 開關：

```json
{
  "DatabaseProvider": "Sqlite",
  "ConnectionStrings": {
    "DBConStr": "Server=localhost\\SQLEXPRESS01;Database=TwseStock;Trusted_Connection=True;",
    "SQLite": "Data Source=TwseStock.db"
  }
}
```

預設用 SQLite，要切回 SQL Server 就改成 `"SqlServer"`。

## 關鍵決定：為什麼不用另一條繼承鏈

不新增 `BaseRepositorySqlite` → `BaseTwseStockRepositorySqlite` → `StockInfoRepositorySqlite` 的完整繼承鏈，因為：
- `BaseRepository<T>` 中的 EF Core CRUD 方法（`GetListBy`、`Create`、`Update` 等）全部是 provider-neutral，不需複製
- 唯一需要換的是 `ExecSQL`，用 statis helper + composition 處理即可，不需要整條繼承鏈
- 大幅減少重複程式碼

## Files Created

| File | Purpose |
|------|---------|
| `MyTwse/Infrastructure/SqliteHelper.cs` | Provider-neutral `ExecSQL<T>` |
| `MyTwse/Repositories/StockInfoRepositorySqlite.cs` | SQLite 版本的 StockInfoRepository |

## Files Modified

| File | Changes |
|------|---------|
| `MyTwse/MyTwse.csproj` | 新增 `Microsoft.EntityFrameworkCore.Sqlite` |
| `MyTwse/Startup.cs` | 依 `DatabaseProvider` 設定切換 EF Core provider 和 repository 實作 |
| `MyTwse/appsettings.json` | 加入 `DatabaseProvider` 開關 + SQLite 連線字串 |

## Risks & Edge Cases

- **SQLite 無 `Date` / `decimal(18,2)` 型別** — `TwseStockContext.OnModelCreating` 中的 `.HasColumnType("date")` 和 `.HasColumnType("decimal(18, 2)")` 在 SQLite 會報錯。需移除或改為條件式（例如用 `DatabaseProvider` 判斷）
- **`decimal` 精確度** — SQLite 存成 real，微小誤差可接受（本專案只顯示到小數第二位）
- **EF Core migration** — 用 `context.Database.EnsureCreated()` 在啟動時自動建表，不需手動 migration
- **自動建表** — 在 `Startup.Configure` 中加一段 bootstrap code 來做 `EnsureCreated()`
- **`YieldRateIncreaseＭaxDays` 的 SQL** — 該 CTE 使用 `ROW_NUMBER()` 和 `int` 算術，SQLite 3.25+ 已支援，語法不需改動

## Scope Estimate

**Small** — 3 個新檔 / 改檔，純 infrastructure 層變動

## Verification Strategy

1. `dotnet build` 通過
2. `dotnet test` 4/4 通過（測試使用 DI 啟動完整 WebHost，會吃到新設定）
3. 手動啟動 API 確認可回應（`curl http://localhost:5000/StockInfo`）
4. 將 `appsettings.json` 的 `DatabaseProvider` 改回 `"SqlServer"`，確認原本 SQL Server 路徑仍可 build
