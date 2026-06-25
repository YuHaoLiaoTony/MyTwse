# Strategy History

## Entry 1: .NET 8 Upgrade + Dependency Inline
- **Date**: 2026-06-25
- **Status**: Complete
- **Result**: Built and tested successfully. Project now self-contained on .NET 8.
- **Lessons Learned**:
  - Unknown `ActionFilter` behavior from external lib required a safe pass-through stub
  - `Enum` type name conflicts with `MyTwse.Enum` namespace — use `System.Enum` qualification
  - Direct jump .NET 5 → 8 was straightforward; no intermediate version needed
