# Strategy History

## Entry 1: .NET 8 Upgrade + Dependency Inline
- **Date**: 2026-06-25
- **Status**: Complete
- **Result**: Built and tested successfully. Project now self-contained on .NET 8.
- **Lessons Learned**:
  - Unknown `ActionFilter` behavior from external lib required a safe pass-through stub
  - `Enum` type name conflicts with `MyTwse.Enum` namespace — use `System.Enum` qualification
  - Direct jump .NET 5 → 8 was straightforward; no intermediate version needed

## Entry 2: 加入 SQLite 支援
- **Date**: 2026-06-25
- **Status**: Complete
- **Result**: SQLite support added. API can start without SQL Server. Provider switching via config.
- **Lessons Learned**:
  - `.HasColumnType("date")` / `.HasColumnType("decimal(18,2)")` are SQL Server-specific — need removal for cross-provider compat
  - `EnsureCreated()` is a quick way to auto-provision SQLite without migrations
  - `SqliteHelper` static class is cleaner than a parallel inheritance chain for replacing one method
