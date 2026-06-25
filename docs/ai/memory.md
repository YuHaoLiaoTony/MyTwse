# Project Memory

## History
- **2026-06-25**: Initial project review (Report 0001) identified .NET 5.0 EOL and external dependency blockers
- **2026-06-25**: .NET 8 upgrade completed (Report 0002). All external dependency code inlined. Project is now self-contained and buildable.
- **2026-06-25**: SQLite support added (Report 0003). API can run without SQL Server Express. Provider switching via `appsettings.json`.

## Key Decisions
- Inline over external package: All 6 utility APIs from `DotNetCore.Modules.Common` were reimplemented locally to remove the build blocker
- HttpClient over RestSharp: Simplified HTTP stack by removing 3rd-party dependency
- Jumped straight to .NET 8 LTS instead of incremental upgrades
- SQLite over SQL Server for dev: Zero-install local database. Conditional DI registration keeps both paths.
- Helper over inheritance chain: `SqliteHelper` static class + `SqliteStockInfoRepository` avoids duplicating the EF Core CRUD base class.
