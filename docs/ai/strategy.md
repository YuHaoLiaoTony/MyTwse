# Strategy Card: 加入 SQLite 支援
**Status: Complete**

## Task Goal
讓 MyTwse 可以不依賴 SQL Server Express 直接跑起來，改用 SQLite 作為開發期資料庫。

## Non-Goals
- 不修改現有的 SQL Server `BaseRepository.cs`（保留兩套並存）
- 不刪除 SQL Server 支援
- 不改動 Service / Controller / 業務邏輯
- 不改 API 回傳格式

## Strategic Approach
1. 新增 provider-neutral 的 SQL 執行 helper（避免直接依賴 `SqlParameter`）
2. 新增 `StockInfoRepositorySqlite` 使用新的 helper，保留原本的 SQL Server repo 不變
3. 切換 EF Core provider 為 SQLite + 新增 NuGet 套件
4. 透過 `appsettings.json` 開關選擇用 SQL Server 還是 SQLite

## Invariants
- 所有 EF Core 查詢（`_DB.Set<T>().Where()` 等）provider 中立，不需變動
- `StockInfoRepository.GetYieldRateIncreaseＭaxDays` 中使用的 T-SQL 需要改為 SQLite 相容語法
- 原始 SQL Server 版本的程式碼必須完整保留

## Risk Points
- SQLite 不支援 `ROW_NUMBER()` — 那個複雜的 CTE SQL 需要改寫
- `date` 型別在 SQLite 中是文字，EF Core 的行為可能不同
- `decimal` 精確度在 SQLite 中可能有微小差異

## Checkpoints
1. 新的 SQLite helper 可以執行 provider-neutral 的 SQL
2. `StockInfoRepositorySqlite` 實作完成
3. `dotnet build` + `dotnet test` 通過
4. API 可以啟動並回應（不需要 SQL Server）

## Completion Criteria
- `dotnet build` 零錯誤
- `dotnet test` 4/4 通過
- 切換到 SQLite 後 API 可以啟動（不需要 SQL Server）
- 現有 SQL Server 版本完整保留不受影響
