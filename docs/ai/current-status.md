# Current Status

## Project
MyTwse — TWSE stock data Web API (interview take-home test)

## .NET Version
**.NET 8.0** (upgraded from 5.0)

## Build Status
- `dotnet build`: ✅ 0 errors, 2 pre-existing warnings
- `dotnet test`: ✅ 4/4 passed

## Key Changes
- External dependency `DotNetCore.Modules.Common` fully inlined into `MyTwse/Infrastructure/`
- `RestSharp` replaced with `HttpClient`
- `System.Net.WebClient` still used in holiday check (pre-existing)
- Async deadlock (`Wait()/Result`) fixed in `MyExtension.cs`
- `Random` instance reuse fixed in `StockInfoService.cs`

## Known Issues
1. Holiday API (`tool.bitefu.net`) is unreliable for Taiwan — needs separate task
2. TWSE API format may have changed since 2021 — untested
3. `WebClient` usage triggers SYSLIB0014 deprecation warning
4. `ExceptionFilter` header append triggers ASP0019 warning
