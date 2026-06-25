# TODO

## Active
| ID | Description | Notes |
|----|-------------|-------|
| T001 | Replace holiday API with Taiwan government calendar | `tool.bitefu.net` is unreliable; consider https://data.ntpc.gov.tw |

## Blocked
(none)

## Completed
| ID | Description | Resolution |
|----|-------------|-----------|
| — | Inline DotNetCore.Modules.Common dependency | Reimplemented 6 utility APIs locally in `MyTwse/Infrastructure/` |
| — | Upgrade .NET 5.0 → .NET 8.0 | Done in single jump, EF Core 3.1.3 → 8.0.11 |
| — | Fix async deadlock in MyExtension.cs | Replaced `.Wait()/.Result` with `.GetAwaiter().GetResult()` |
| — | Fix Random seed reuse | Made `_random` a static field |
| — | Add SQLite support | `SqliteHelper` + `SqliteStockInfoRepository` + conditional DI. Default provider is SQLite. |
