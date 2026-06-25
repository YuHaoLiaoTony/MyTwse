# Plan: .NET 8 Upgrade + External Dependency Inline

## Problem

The MyTwse project has two blockers preventing it from being buildable and maintainable:

1. **External dependency**: `DotNetCore.Modules.Common` (3 sub-projects) referenced via relative path `../../` — not in the repo, cannot build
2. **EOL runtime**: Targets .NET 5.0 (out of support since May 2022), EF Core 3.1.3, uses deprecated APIs like `WebClient`

## External API Inventory

Every usage of the external `DotNetCore.Modules.Common` library, categorized by file:

| File | API Used | Source Project | Replace With |
|------|----------|---------------|--------------|
| `Services/StockInfoService.cs:125` | `RestRequestHelper.Request(url).Get(...).AddParameter(...).Response<T>()` | DotNetCore.Utility.Helpers | `HttpClient` + inline helper |
| `Services/StockInfoService.cs:161-164` | `.TryToDecimal()`, `.TryToInt()` | DotNetCore.Utility.Extensions | Inline extension methods |
| `Services/StockInfoService.cs:68` | `.IsNumber()` | DotNetCore.Utility.Extensions | Inline extension method |
| `Filters/ExceptionFilter.cs:26,39` | `.GetHttpStatusCode()`, `.GetDescription()` | DotNetCore.Utility.Extensions | Inline extension using `GetCustomAttribute` |
| `Filters/ExceptionFilter.cs:48` | `.ToJson()` | DotNetCore.Utility.Extensions | `System.Text.Json.JsonSerializer.Serialize` |
| `Startup.cs:33` | `new ActionFilter()` | DotNetCore.Utility.Filters | Minimal pass-through ActionFilter |
| `Enum/MyTwseExceptionEnum.cs` | `[HttpStatusCode(...)]` attribute | DotNetCore.Attributes | Already exists as `MyTwse/HttpStatusCodeAttribute.cs` |

## Approach

### Step 1 — Create inline utility code

Create `MyTwse/Infrastructure/` with these files:

- `EnumExtensions.cs` — `GetDescription()`, `GetHttpStatusCode()` using `GetCustomAttribute`
- `StringExtensions.cs` — `TryToDecimal()`, `TryToInt()`, `IsNumber()`
- `ActionFilter.cs` — Minimal pass-through `IActionFilter` (replaces external one)

Replace `RestRequestHelper` usage in `StockInfoService.cs` with `System.Net.Http.HttpClient` directly (HTTP GET with query param, JSON deserialize).

### Step 2 — Update project files

- `MyTwse/MyTwse.csproj`: `net5.0` → `net8.0`, remove ProjectReference to external projects, update NuGet packages:
  - EF Core `3.1.3` → `8.0.x`
  - `RestSharp` `106.11.7` → **remove** (replaced by `HttpClient`)
  - `Microsoft.AspNetCore.Mvc.Core` `2.2.5` → **remove** (included in the framework)
- `MyTwseTest/MyTwseTest.csproj`: `netcoreapp5.0` → `net8.0`, update test SDK packages
- `MyTwse.sln`: Remove the three external project references

### Step 3 — Update source code imports

- `Services/StockInfoService.cs`: Remove `using DotNetCore.Utility.Extensions`, `using DotNetCore.Utility.Helpers`; add `using MyTwse.Infrastructure`
- `Filters/ExceptionFilter.cs`: Remove `using DotNetCore.Utility.Extensions`; add `using MyTwse.Infrastructure`; replace `.ToJson()` with `JsonSerializer.Serialize()`
- `Startup.cs`: Remove `using DotNetCore.Utility.Filters`

### Step 4 — Fix known issues

- `MyExtension.cs`: Replace `.Wait()/.Result` with `GetAwaiter().GetResult()` or refactor to fully async. Since changing the extension method signature would cascade changes to the service, use `GetAwaiter().GetResult()` as the minimal fix.
- `StockInfoService.cs:132`: Replace `Task.Delay(...).Wait()` with `Thread.Sleep()` or `Task.Delay(...).GetAwaiter().GetResult()`
- `StockInfoService.cs:175-178`: Move `Random` to a static field (fix repeated seed issue)

### Step 5 — Build & test

- `dotnet build` on both projects
- `dotnet test` on test project
- Fix any compilation errors

## Files Created

| File | Purpose |
|------|---------|
| `MyTwse/Infrastructure/EnumExtensions.cs` | `GetDescription()`, `GetHttpStatusCode()` |
| `MyTwse/Infrastructure/StringExtensions.cs` | `TryToDecimal()`, `TryToInt()`, `IsNumber()` |
| `MyTwse/Infrastructure/ActionFilter.cs` | Minimal pass-through filter |

## Files Modified

| File | Changes |
|------|---------|
| `MyTwse/MyTwse.csproj` | Target `net8.0`, remove external refs, update/remove packages |
| `MyTwseTest/MyTwseTest.csproj` | Target `net8.0`, update packages |
| `MyTwse.sln` | Remove 3 external project references |
| `MyTwse/Startup.cs` | Remove external using, keep `ActionFilter` (now local) |
| `MyTwse/Filters/ExceptionFilter.cs` | Replace external extensions, replace `.ToJson()` |
| `MyTwse/Services/StockInfoService.cs` | Replace external extensions, replace `RestRequestHelper` with `HttpClient`, fix `Random` |
| `MyTwse/Extensions/MyExtension.cs` | Fix `.Wait()/.Result` deadlock |

## Risks & Edge Cases

- **`ActionFilter` behavior unknown**: If it did logging/metrics, those are lost. The safe stub preserves the filter pipeline without adding behavior.
- **TWSE API format changes**: The scraper may break if TWSE changed their JSON format since 2021. This plan does not address that (out of scope).
- **Holiday API reliability**: `tool.bitefu.net` may be down or the response format may have changed. This plan does not replace it (would require a separate task).
- **EF Core 8.x breaking changes**: `FromSqlRaw` still works in EF Core 8, so the custom SQL in `StockInfoRepository.cs` should be fine.

## Scope Estimate

**Small-Medium**. ~7 files changed, 3 files created. Pure mechanical transformation, no new logic.

## Verification Strategy

1. `dotnet build` succeeds with zero errors/warnings on both `MyTwse` and `MyTwseTest`
2. `dotnet test` passes all 3 existing tests
3. Git diff review to confirm no accidental API contract changes
