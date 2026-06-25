# Completion Report: SQLite 支援

## Metadata
- **Report ID**: 0003
- **Task**: 加入 SQLite 支援
- **Date**: 2026-06-25
- **Status**: Completed

## Scope
讓 MyTwse 可以不依賴 SQL Server Express 直接跑起來，改用 SQLite 作為開發期資料庫。

## Files Created

| File | Purpose |
|------|---------|
| `MyTwse/Infrastructure/SqliteHelper.cs` | Provider-neutral `ExecSQL<T>` (uses `command.CreateParameter()` instead of `SqlParameter`) |
| `MyTwse/Repositories/SqliteStockInfoRepository.cs` | SQLite 版本的 StockInfoRepository |

## Files Modified

| File | Changes |
|------|---------|
| `MyTwse/MyTwse.csproj` | 新增 `Microsoft.EntityFrameworkCore.Sqlite` 8.0.11 |
| `MyTwse/Models/TwseStockContext.cs` | 移除 `.HasColumnType("date")` 和 `.HasColumnType("decimal(18,2)")` 以相容 SQLite |
| `MyTwse/Startup.cs` | 依 `DatabaseProvider` 設定條件註冊 EF Core provider 與 Repository；SQLite 模式自動 `EnsureCreated()` |
| `MyTwse/appsettings.json` | 新增 `DatabaseProvider` 開關（預設 `"Sqlite"`）+ SQLite 連線字串 |

## Strategy Alignment

| Item | Result |
|------|--------|
| **Task Goal** | ✅ API 可在 SQLite 模式啟動，不須 SQL Server |
| **Non-Goals** | ✅ `BaseRepository.cs` 未修改，SQL Server 路徑保留，無新功能 |
| **Invariants** | ✅ 原始 SQL Server 程式碼完整保留不受影響 |
| **Checkpoints** | ✅ Helper → Repo → Build → Test → API 啟動，全部通過 |

## Checkpoint Results

| Checkpoint | Result |
|------------|--------|
| SqliteHelper 可執行 provider-neutral SQL | ✅ |
| SqliteStockInfoRepository 實作完成 | ✅ |
| `dotnet build` + `dotnet test` 通過 | ✅ (0 errors, 4/4 tests) |
| API 可啟動（不需 SQL Server） | ✅ EnsureCreated OK, 監聽 :5000 / :5001 |

## Test Results
```
Passed! - Failed: 0, Passed: 4, Skipped: 0, Total: 4
```

## Risks / Open Issues
- TwseStock.db 檔案由 `EnsureCreated()` 自動在啟動目錄建立，已加入 `.gitignore` 建議（未強制）
- SQLite 無原生 `date` / `decimal` 型別，改用 TEXT / REAL，精確度可能有些微差異但不影響顯示
- 原 `IBaseRepository<T>` 提取到獨立檔案的 refactoring 未包含在此 commit（保留給後續任務）

## Strategy Card Reference
See `docs/ai/strategy.md` — 加入 SQLite 支援

## Git Status
- Not committed (pending `/finish-task`)
