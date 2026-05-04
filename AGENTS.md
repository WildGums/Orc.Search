# Orc.Search

Orc.Search is a library that makes it easy to add search functionality to any application. It uses [Lucene.NET](http://lucenenet.apache.org/) under the hood to provide fast, full-text search capabilities.

Orc.Search consists of the following projects:

- **Orc.Search** — Core library providing search services, models, and metadata definitions.
- **Orc.Search.Xaml** — WPF/XAML library containing views and view models for integrating search UI.

---

## Critical Rules (Read First)

These rules are **non-negotiable**. Violating them causes broken builds, crashes, or downstream breakage.

### 1. Never Edit Generated Files

Files matching `*.generated.cs` are auto-generated.

- **NEVER** manually edit these files

### 2. ABI / API Stability

This project maintains stable ABI / API. Breaking changes break downstream apps.

| Allowed | Never |
|---------|-------|
| Add new overloads | Modify existing signatures |
| Add new methods | Remove public APIs |
| Add new classes | Change return types |

### 3. Tests Are Mandatory

**Building alone is NOT sufficient.** Run tests before claiming completion (see [Commands](#commands)).

### 4. Branch Protection (COMPLIANCE REQUIRED)

**Direct commits to protected branches are a policy violation.**

| Repository | Protected Branches |
|------------|-------------------|
| Orc.Search | `master` |
| Orc.Search | `develop` |

**Required workflow:**

1. **Create a feature branch FIRST** — Preferred naming convention: `feature/issue-NNNN-description` (issue number is optional when no issue exists, e.g., `feature/add-async-search`)
2. **Make all commits on the feature branch** — Never commit directly to protected branches
3. **Submit a Pull Request** — Changes must be reviewed by a human before merging

```bash
# CORRECT — Always create a feature branch first
git checkout -b feature/issue-1234-fix-description

# NEVER DO THIS — Policy violation
git checkout develop && git commit  # FORBIDDEN

# NEVER DO THIS — Policy violation
git checkout master && git commit  # FORBIDDEN
```

The repository has protected branches that must be respected.

---

## Commands

Single source of truth for all commands:

| Task | Command |
|------|---------|
| **Build** | `dotnet cake --target=build` |
| **Test** | `dotnet cake --target=test` |
| **Build and test** | `dotnet cake --target=buildandtest` |

---

## Architecture & Directories

### Layer Overview

```
Orc.Search      => Core search library (cross-platform)
Orc.Search.Xaml => WPF/XAML UI components (Windows only)
```

### Directory Guide

| Directory | Editable? | Notes |
|-----------|-----------|-------|
| `*.generated.cs` | No | Leave as-is |
| `src/Orc.Search/` | Yes | Core search services, models, metadata |
| `src/Orc.Search/Services/` | Yes | Search service implementations |
| `src/Orc.Search/Models/` | Yes | Search history and data models |
| `src/Orc.Search/Metadata/` | Yes | Search metadata / property definitions |
| `src/Orc.Search/Providers/` | Yes | Search index providers |
| `src/Orc.Search.Xaml/` | Yes | WPF views and view models |
| `src/Orc.Search.Xaml/Views/` | Yes | Search UI views |
| `src/Orc.Search.Xaml/ViewModels/` | Yes | View models for search UI |
| `src/Orc.Search.Tests/` | Yes | Unit and integration tests |
| `src/Orc.Search.Example/` | Yes | Example application |
| `deployment/` | No | Deployment / build scripts |

---

## Writing Code

### Anti-Patterns (Never Do This)

| Anti-Pattern | Why |
|-------------|-----|
| Modifying method signatures | ABI breaking |
| Manual edits to `*.generated.cs` | Overwritten on regenerate |
| Using default parameters in public APIs | ABI breaking — existing callers break if parameters are inserted or reordered; add new overloads instead |
| **Skipping failing tests** | **Unacceptable — tests must pass** |

---

## Testing & Debugging

### Running Tests

```bash
dotnet cake --target=test
```

### Tests MUST Pass

> **NON-NEGOTIABLE:** Tests must PASS before claiming completion.
>
> - Do NOT skip failing tests
> - Do NOT claim completion if tests fail
> - Do NOT use `SkipException` to work around failures

### Writing Tests

1. Use NUnit to write tests
2. Group related tests in a class (conventionally named `<Feature>Facts`)
3. Combine Pascal / Snake case for test methods (e.g. `Feature_Does_Work`)

```csharp
[Test]
public void Feature_Does_Work()
{
    var result = 47 - 5;

    Assert.That(result, Is.EqualTo(42));
}
```

**Philosophy:** Tests FAIL when wrong, never skip (except missing hardware).

### Public API Approval

The test project includes `PublicApiFacts` which verifies no breaking changes are introduced to the public API surface. If you intentionally change the public API, update the approval files:

- `src/Orc.Search.Tests/PublicApiFacts.Orc_Search_HasNoBreakingChanges_Async.verified.txt`
- `src/Orc.Search.Tests/PublicApiFacts.Orc_Search_Xaml_HasNoBreakingChanges_Async.verified.txt`

### Debugging Methodology

1. **Establish baseline** — What's the known-good state?
2. **One change at a time** — Verify each change before proceeding
3. **Track changes in a table** — Log what you changed and the result
4. **Platform differences are signals** — If X works and Y fails, the difference IS the answer
5. **Revert if worse** — Don't pile fixes on top of failures

---

## Further Reading

| Topic | Document |
|-------|----------|
| Contributing guidelines | [CONTRIBUTING.md](CONTRIBUTING.md) |
| Project documentation | [WildGums Open Source Docs](http://opensource.wildgums.com) |
