# Small Similar Project

A simplified, self-contained version of the birds-fabric-services architecture. Uses in-memory data instead of Azure/Fabric dependencies to demonstrate the same layered architecture pattern: **Shared Data API -> Content Filtering API -> Frontend**.

## Architecture

```
src/
├── shared/          -> SharedDataApi (.NET 10) - Rich in-memory data source (port 5100)
├── content/         -> ContentApi (.NET 10) - Calls shared & filters/transforms data (port 5200)
└── fabric-workload/ -> React app (Vite + TypeScript) - Dashboard frontend (port 5173)
```

## Aspire + Dapr (Recommended)

Mirrors the main birds-fabric-services architecture using .NET Aspire for orchestration and Dapr for service-to-service invocation.

### Prerequisites

- .NET 10 SDK
- Aspire workload: `dotnet workload install aspire`
- Dapr CLI installed + `dapr init`

### Run

```bash
cd small-simalar-project
dotnet run --project src/aspire/AppHost
```

The Aspire dashboard opens automatically. All services (SharedDataApi, ContentApi, frontend) and their Dapr sidecars are visible in the dashboard. ContentApi calls SharedDataApi via Dapr service invocation (visible in Aspire traces).

## Manual Run (Alternative)

Open three terminals:

```bash
# Terminal 1: Start shared API
cd small-simalar-project/src/shared
dotnet run

# Terminal 2: Start content API
cd small-simalar-project/src/content
dotnet run

# Terminal 3: Start React app
cd small-simalar-project/src/fabric-workload
npm install
npm run dev
```

## Verification

1. `http://localhost:5100/api/products` - Returns ~50 products with full details
2. `http://localhost:5200/api/content/products` - Returns simplified product list (filtered fields)
3. `http://localhost:5173` - React dashboard showing data from the content API

## API Endpoints

### SharedDataApi (port 5100)
| Endpoint | Description |
|---|---|
| GET /api/products | ~50 products with full details |
| GET /api/products/{id} | Single product |
| GET /api/users | ~30 users with full profiles |
| GET /api/users/{id} | Single user |
| GET /api/orders | ~100 orders with full details |
| GET /api/orders/{id} | Single order |

### ContentApi (port 5200)
| Endpoint | Description |
|---|---|
| GET /api/content/products | Simplified product list (id, name, category, price) |
| GET /api/content/products/{id} | Product detail (subset of fields) |
| GET /api/content/users | Simplified user list (id, name, email, role) |
| GET /api/content/users/{id} | User profile (filtered fields) |
| GET /api/content/orders | Order summaries (id, userId, totalAmount, status, date) |
| GET /api/content/orders/{id} | Order detail |
| GET /api/content/dashboard | Aggregated stats: counts, revenue, order statuses |
