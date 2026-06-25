# Strategy Card: .NET 8 Upgrade + Dependency Inline
**Status: Complete**

## Task Goal
Upgrade MyTwse from .NET 5.0 to .NET 8.0 LTS, resolve the external `DotNetCore.Modules.Common` dependency by inlining its used APIs, and fix the async deadlock issue in `MyExtension.cs`.

## Non-Goals
- No new features or API endpoints
- No database schema changes
- No test coverage expansion (tests stay as-is)
- No CI/CD pipeline setup
- No Docker containerization

## Strategic Approach
1. Inline all external dependency code into the MyTwse project first (eliminate build blocker)
2. Upgrade .NET & package versions in one go (5 → 8, no intermediate stops)
3. Fix async deadlock as part of the inlining (replace `Wait()/Result` with proper async)
4. Remove sln references to external projects
5. Verify build + existing tests pass

## Invariants
- API response contracts must remain unchanged (same JSON shape, same status codes)
- All existing test behavior must be preserved
- No external project references after migration (self-contained repo)

## Risk Points
- Unknown contents of `DotNetCore.Modules.Common` — strategies inferred from usage patterns only
- `RestSharp` version jump (106 →  latest) may have breaking API changes
- EF Core 3.1 → 8.x migration may need `OnModelCreating` or query adjustments
- `ActionFilter` behavior from external lib is unknown — may need to strip or reimplement

## Checkpoints
1. All external usage cataloged and inlined → buildable without external refs
2. .NET 8 target + all packages upgraded → `dotnet build` passes
3. Test project upgraded → `dotnet test` passes
4. Solution file cleaned (no external project refs) → sln loads in VS/Rider

## Completion Criteria
- `dotnet build` succeeds with zero warnings on both projects
- `dotnet test` passes all existing tests
- No references to `DotNetCore.Modules.Common` anywhere in the repo
- Git status clean after commit
