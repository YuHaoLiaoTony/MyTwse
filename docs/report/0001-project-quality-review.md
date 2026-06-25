# Project Quality Review Report

## Metadata
- **Report ID**: 0001
- **Check Target**: Project quality & value assessment (MyTwse - CMoney interview take-home test)
- **Date**: 2026-06-25
- **Status**: Completed

## Scope
Evaluate the overall quality, architecture, code health, and current value of the MyTwse project — a .NET 5 Web API that scrapes Taiwan Stock Exchange (TWSE) PE ratio, dividend yield, and P/B ratio data. Originally submitted as a take-home interview test for CMoney.

## Files Inspected

| Category | Files |
|----------|-------|
| **Controllers** | `Controllers/StockInfoController.cs` |
| **Models** | `Models/StockInfo.cs`, `Models/TwseStockContext.cs`, `Models/InsertDateLog.cs`, `Models/QueryModels/YieldRateIncreaseＭaxDaysQueryModel.cs`, `Models/ReportModels/StockInfoJsonModel.cs`, `Models/ReportModels/YieldRateIncreaseＭaxDaysReportModel.cs`, `Models/ViewModels/CreateStockViewModel.cs`, `Models/ViewModels/ResultModel.cs` |
| **IRepositories** | `IRepositories/IStockInfoRepository.cs`, `IRepositories/IInsertDateLogRepository.cs` |
| **Repositories** | `Repositories/BaseRepository.cs`, `Repositories/BaseTwseStockRepository.cs`, `Repositories/StockInfoRepository.cs`, `Repositories/InsertDateLogRepository.cs` |
| **Services** | `Services/StockInfoService.cs`, `ServiceInterface/IStockInfoService.cs` |
| **Infrastructure** | `Startup.cs`, `Program.cs`, `MyTwse.csproj`, `appsettings.json`, `ApiResponseModel.cs`, `MyTwseException.cs`, `HttpStatusCodeAttribute.cs`, `Filters/ExceptionFilter.cs`, `Extensions/MyExtension.cs`, `Enum/InsertDateLogsTypeEnum.cs`, `Enum/MyTwseExceptionEnum.cs` |
| **Tests** | `MyTwseTest/GetStockInfoByRecentTest .cs`, `MyTwseTest/GetStockPERankTest.cs`, `MyTwseTest/GetYieldRateIncreaseＭaxDays.cs`, `MyTwseTest/MyTwseTest.csproj` |
| **Docs** | `README.md`, `.gitignore` |

## Commands Run
- `git log --oneline -30`
- `git status`
- `git remote -v`
- File reads across all source files

## Findings

### Strengths (Good Parts)

1. **Complete architecture**: Implements Controller → Service → Repository layering with DI, covering the full request lifecycle.
2. **Custom exception handling**: `ExceptionFilter` + `MyTwseException` + `HttpStatusCodeAttribute` is a thoughtful error-handling pattern that maps error codes to HTTP status codes and descriptions.
3. **Complex SQL**: The window-function SQL for finding the longest streak of strictly increasing yield rates (`GetYieldRateIncreaseＭaxDays`) shows solid SQL skill — CTEs, `ROW_NUMBER()`, and difference-of-row-numbers group detection.
4. **Comprehensive README**: API documentation is well-structured with request/response examples. Includes reflection notes (程式碼反饋) acknowledging known issues.
5. **Input validation**: `CreateStockViewModel` and `YieldRateIncreaseＭaxDaysQueryModel` use `[Required]` data annotations properly. Service validates business rules (holiday check, number format, date ordering).
6. **Data freshness tracking**: `InsertDateLog` table tracks which dates have been fetched, avoiding redundant API calls.
7. **Consistent API response format**: `ApiResponseModel<T>` wrapping with code/message/data pattern.
8. **External API throttling**: Deliberate 500-700ms random delay to avoid being rate-limited by TWSE.

### Issues Found

| # | Severity | Category | Description |
|---|----------|----------|-------------|
| 1 | **High** | Build | `MyTwse.csproj` references `DotNetCore.Modules.Common` from `../../` (external project not in this repo). Will not build without cloning another repo. |
| 2 | **High** | Architecture | `MyExtension.cs` uses `.Wait()` + `.Result` on async methods, risking deadlocks in ASP.NET context. |
| 3 | **Medium** | External Dependency | Holiday checking relies on `http://tool.bitefu.net/jiari/` — an unofficial Chinese API, unreliable for Taiwan holidays. README already acknowledges this. |
| 4 | **Medium** | Quality | `GetRandom()` creates a new `Random` instance on every call, losing entropy and potentially producing repeated values in tight loops. |
| 5 | **Medium** | Testing | Tests are integration tests (spin up full `IWebHost`), not pure unit tests. Only test error paths (holiday, non-numeric, date order). No success-path tests. No in-memory DB testing for query correctness. README already acknowledges this. |
| 6 | **Medium** | Layering | Controller returns domain entities (`StockInfo`) directly instead of DTOs. `StockInfoService` mixes business logic with HTTP fetching (single responsibility violation). |
| 7 | **Low** | .NET Version | Targets `net5.0` (out of support since May 2022). EF Core packages pinned to 3.1.3 (not 5.0). |
| 8 | **Low** | Code Smell | `BaseRepository` has both generic and non-generic versions with duplicated `ExecSQL` logic. Route strings are magic strings. |
| 9 | **Low** | Git | From the repo inspection: no `AGENTS.md`, `docs/ai/`, or `docs/report/` existed before this report. |

### Previous Reviewer Feedback (from README 程式碼反饋)
The README itself already documents reviewer feedback, which mostly overlaps with findings above:
- Holiday API is inaccurate for Taiwan
- `.user` / `StyleCop.Cache` files should not be committed
- Tests lack SQL correctness verification via `UseInMemoryDatabase`

## Result: **Partial Pass**

The project successfully demonstrates core competencies for an interview setting (layered architecture, DI, error handling, complex SQL, API design, tests). However, it has significant practical gaps: external dependency not in repo, unreliable holiday API, mixed concerns, and limited test coverage.

## Gaps
- No CI/CD pipeline or Docker setup
- No logging framework (only `appsettings.json` logging config)
- No authentication/authorization
- No rate-limiting middleware
- No caching strategy
- No health check endpoint
- No environment-specific configuration beyond development

## Risks
1. **Build risk**: The `DotNetCore.Modules.Common` project reference means anyone cloning this repo cannot build without separately obtaining that dependency. This is the single biggest blocker.
2. **Production risk**: The holiday API is external and unreliable; if it goes down, the service incorrectly treats holidays as working days.
3. **Maintenance risk**: .NET 5 is end-of-life. EF Core 3.1 is also older.

## Recommended Next Action
1. **(For a portfolio refresh)**: Remove the external `DotNetCore.Modules.Common` dependency and inline or replace the utilities used. Upgrade to .NET 8+.
2. **(For interview practice)**: The project is valuable as-is for studying what a take-home test looks like and what reviewers look for. The README feedback section is especially instructive.
3. **Either way**: The core idea (TWSE data scraper API) is still relevant, but the implementation needs modernization.

## Follow-up Recommendation
- **/fix-task** recommended for:
  - Removing external project dependency (or inlining it)
  - Fixing async deadlock risk in `MyExtension.cs`
  - Fixing `Random` instance reuse
- **/plan-task** recommended for:
  - Architecture modernization (.NET 8+, proper async throughout, separate HTTP client service)

## Git Status
- Branch: `master`
- Remote: `origin` → `https://github.com/YuHaoLiaoTony/MyTwse`
- Status before report: clean (nothing to commit)
- Remote tracking branch exists → push will be attempted

## Conclusion
**Is this project still valuable?** Yes, but with caveats:

- **As an interview portfolio piece**: Moderate value. It demonstrates you can build a complete Web API with proper layering, DI, error handling, and tests. The complex SQL query for streak detection is a standout. However, the external dependency and async issues would be flagged in a code review today.
- **As a real-world tool**: Low value without modernization. It needs the external dependency resolved, .NET version upgraded, and the holiday API replaced.
- **As a learning reference**: High value. It captures a realistic take-home test scenario with honest self-reflection in the README. The reviewer feedback section is rare and useful.
