# Completion Report: .NET 8 Upgrade + Dependency Inline

## Metadata
- **Report ID**: 0002
- **Task**: .NET 8 Upgrade + External Dependency Inline
- **Date**: 2026-06-25
- **Status**: Completed

## Scope
Upgrade MyTwse from .NET 5.0 to .NET 8.0 LTS, resolve the external `DotNetCore.Modules.Common` dependency by inlining its used APIs, and fix async deadlock issues.

## Files Created

| File | Purpose |
|------|---------|
| `MyTwse/Infrastructure/EnumExtensions.cs` | `GetDescription()`, `GetHttpStatusCode()`, `ToJson()` |
| `MyTwse/Infrastructure/StringExtensions.cs` | `TryToDecimal()`, `TryToInt()`, `IsNumber()` |
| `MyTwse/Infrastructure/ActionFilter.cs` | Minimal pass-through `IActionFilter` |

## Files Modified

| File | Changes |
|------|---------|
| `MyTwse/MyTwse.csproj` | `net5.0` → `net8.0`; EF Core `3.1.3` → `8.0.11`; removed `RestSharp`, `Microsoft.AspNetCore.Mvc.Core`, and 3 external ProjectReferences |
| `MyTwseTest/MyTwseTest.csproj` | `netcoreapp5.0` → `net8.0` |
| `MyTwse.sln` | Removed 3 external project references and their solution config |
| `MyTwse/Startup.cs` | Removed `using DotNetCore.Utility.Filters`, added `using MyTwse.Infrastructure` |
| `MyTwse/Filters/ExceptionFilter.cs` | Replaced `DotNetCore.Utility.Extensions` with local `EnumExtensions`; replaced `.ToJson()` with `JsonSerializer.Serialize()` |
| `MyTwse/Services/StockInfoService.cs` | Replaced `RestRequestHelper` with `HttpClient`; replaced `TryToDecimal/TryToInt/IsNumber` with local `StringExtensions`; fixed `Random` to static field |
| `MyTwse/Extensions/MyExtension.cs` | Replaced `.Wait()/.Result` with `.GetAwaiter().GetResult()` |

## Checkpoint Results

| Checkpoint | Result |
|------------|--------|
| All external usage cataloged and inlined → buildable without external refs | ✅ |
| .NET 8 target + all packages upgraded → `dotnet build` passes | ✅ |
| Test project upgraded → `dotnet test` passes | ✅ (4/4) |
| Solution file cleaned → no external project refs | ✅ |

## Test Results
```
Passed! - Failed: 0, Passed: 4, Skipped: 0, Total: 4
```

## Risks / Open Issues
- `WebClient` deprecation warning (SYSLIB0014) in `MyExtension.cs` — pre-existing, not addressed by this task
- `ASP0019` header warning in `ExceptionFilter.cs` — pre-existing, not addressed
- TWSE API format may have changed since 2021 (not in scope)
- Holiday API (`tool.bitefu.net`) still unreliable for Taiwan (out of scope)

## Strategy Card Reference
See `docs/ai/strategy.md` — .NET 8 Upgrade + Dependency Inline

## Final Alignment
- **Task Goal**: ✅ .NET 8 upgraded, external dependency resolved, async deadlock fixed
- **Non-Goals**: ✅ No new features, no DB changes, no CI/CD introduced
- **Invariants**: ✅ API contracts unchanged, tests preserved, no external refs
- **Completion Criteria**: ✅ `dotnet build` zero errors, `dotnet test` 4/4 passed, no `DotNetCore.Modules.Common` references anywhere

## Git Status
- Not committed (pending `/finish-task`)
