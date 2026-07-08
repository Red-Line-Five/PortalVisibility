# PortalVisibility - Backend Exercise

This repository contains a small ASP.NET minimal API implementing a content visibility exercise for shareholders.

## What I implemented
- Models: `Shareholder`, `Group`, `Membership`, `ContentItem`, `ContentAudienceGroup` (see `src/PortalVisibility.Api/Models`).
- API endpoints:
  - `GET /api/content` - returns the caller's visible content (supports `limit`, `offset`, and `q` search).
  - `GET /api/content/{id}` - returns content details if visible or `403` if restricted.
- Fake auth: callers identify using `X-Shareholder-Id` header (see `Program.cs`).
- In-memory EF Core database seeded in `Data/SeedData.cs`.
- Integration tests added in `tests/PortalVisibility.Tests` verifying allowed and denied access.

## Getting started
Requirements: .NET SDK (matching the project target). From repository root:

```bash
dotnet build
dotnet run --project src/PortalVisibility.Api
```

To run tests:

```bash
dotnet test
```

## Assumptions
- No real authentication required; `X-Shareholder-Id` header is used to identify the caller.
- In-memory database is acceptable for the exercise.
- Pagination implemented with `limit`/`offset`. A simple text `q` search is provided on title.

## Time spent
 90 minutes total
- Modeling (Shareholder, Group, Membership, ContentItem, ContentAudienceGroup) + DbContext: 25 min
- API endpoints (list + detail, pagination/search, fake auth): 35 min
- Tests: 15 min
- README + review: 15 min

## Notes
- Visiting `/` will return 404 because there's no root route by design; use `/api/content` instead.
- Seeded shareholders in `Data/SeedData.cs`: ids `1`, `2`, `3`.

***

If you want, I can:
- Add a simple root endpoint or redirect to `/api/content`.
- Add Swagger UI for interactive exploration.
- Replace the in-memory DB with SQLite or another persistent store.


