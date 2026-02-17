# Small Similar Project

A simplified, self-contained version of the birds-fabric-services architecture. Uses in-memory data instead of Azure/Fabric dependencies to demonstrate the same layered architecture pattern: **Shared Data API -> Content Filtering API -> Frontend**.

## Architecture

```
src/
├── shared/          -> SharedDataApi (.NET 8) - Rich in-memory data source (port 5100)
├── content/         -> ContentApi (.NET 8) - Calls shared & filters/transforms data (port 5200)
└── fabric-workload/ -> React app (Vite + TypeScript) - Dashboard frontend (port 5173)
```

## How to Run

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

## Docker (Alternative)

Run all three services with a single command:

```bash
cd small-simalar-project
docker compose up --build
```

Then open `http://localhost:3000` to see the React dashboard.

To stop: `docker compose down`

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
