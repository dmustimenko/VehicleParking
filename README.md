# Vehicle Parking

A vehicle parking management API built with ASP.NET Core (.NET 10) minimal APIs and
PostgreSQL (EF Core).

## Features

- Park a vehicle into the first available space
- Report the number of available and occupied spaces
- Calculate the parking charge and free the space on exit

## Solution structure

| Project | Responsibilities |
| ------- | -------------- |
| `VehicleParking.Domain` | Entities, enums, domain exceptions, charge calculation, settings |
| `VehicleParking.Application` | Services, abstractions, request/response models |
| `VehicleParking.Infrastructure` | EF Core persistence: `DbContext`, entity configurations, repository, migrations |
| `VehicleParking.Api` | minimal-API endpoints, error handling, DI, configuration |
| `VehicleParking.Tests` | Unit tests (xUnit) |

Project Dependencies (`Api -> Infrastructure -> Application -> Domain`).

## Tech stack

- .NET 10 / ASP.NET Core minimal APIs
- PostgreSQL via EF Core (Npgsql)
- xUnit for tests
- Scalar for the API reference UI (Development only)

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Docker (PostgreSQL, optionally for the API)

## Environment Setup

### Docker Compose

To set up the database credentials use a `.env` file in the root directory:

```env
POSTGRES_USER=<user>
POSTGRES_PASSWORD=<password>
```

These values are used by Docker Compose to initialize the PostgreSQL container.

> **Note:** There are **local development credentials**, kept in the repository to run exercise with no extra setup.
> In a real deployment the connection string would come from **secrets** / **environment variables**.

### Host

To set up the database credentials use the `VehicleParking.Api/appsettings.json` file:

```env
Username=<username>;Password=<password>
```
> **Note:** There are **local development credentials**, kept in the repository to run exercise with no extra setup.
> In a real deployment the connection string would come from **secrets** / **environment variables**.

## Running

### Option A - DB and API in Docker

```bash
docker compose up --build
```

Builds the API, starts PostgreSQL and the API on a shared network, applies
EF Core migrations and fills the parking spaces.

- API: <http://localhost:8088>
- Scalar UI: <http://localhost:8088/scalar/v1>

Stop containers and delete all data:

```bash
docker compose down -v
```

### Option B - DB in Docker, API on the host

```bash
docker compose up -d postgres
dotnet run --project VehicleParking.Api
```

PostgreSQL is exposed on `localhost:5432`, matching the connection string in
`VehicleParking.Api/appsettings.json`.

The API listens on the port from
`Properties/launchSettings.json` (<http://localhost:5179>), with Scalar at `/scalar/v1`.

### Database migrations

In **Development** mode the application applies migrations on startup.

You can also apply them explicitly (the `DbContext` lives in `VehicleParking.Infrastructure`, the `VehicleParking.Api` is
the startup project):

```bash
dotnet tool install --global dotnet-ef
dotnet ef database update --project VehicleParking.Infrastructure --startup-project VehicleParking.Api
```

## Endpoints

| Method | Route           | Body                                            | Success response                                  |
| ------ | --------------- | ----------------------------------------------- | ------------------------------------------------- |
| POST   | `/parking`      | `{ "VehicleReg": string, "VehicleType": int }`  | `{ VehicleReg, SpaceNumber, TimeIn }`             |
| GET    | `/parking`      | -                                               | `{ AvailableSpaces, OccupiedSpaces }`             |
| POST   | `/parking/exit` | `{ "VehicleReg": string }`                      | `{ VehicleReg, VehicleCharge, TimeIn, TimeOut }`  |

`VehicleType` must be `1` (Small), `2` (Medium) or `3` (Large).

Example (replace the port with `8088` for Docker or `5179` for a local run):

```bash
curl -X POST http://localhost:8088/parking \
  -H "Content-Type: application/json" \
  -d '{"VehicleReg":"DX8WX50","VehicleType":2}'

curl http://localhost:8088/parking

curl -X POST http://localhost:8088/parking/exit \
  -H "Content-Type: application/json" \
  -d '{"VehicleReg":"DX8WX50"}'
```

### Errors

- `400` – invalid request (`VehicleType` not in (`1`,`2`,`3`), missing/blank/invalid `VehicleReg`)
- `404` – vehicle that is not currently parked
- `409` – vehicle **already parked** or the **vehicle parking is full**

## Charge calculation

Charge is the vehicle type's per-minute rate for each **started** minute, plus `AdditionalCharge` for every **complete** `AdditionalChargeIntervalMinutes` parked:

Rates and additional charges (configurable in `appsettings.json` under `Charge`):

| Type   | Rate / minute |
| ------ | ------------- |
| Small  | £0.10         |
| Medium | £0.20         |
| Large  | £0.40         |

`AdditionalCharge` = £1

`AdditionalChargeIntervalMinutes` = 5 minutes

## Tests

```bash
dotnet test
```

Unit tests cover the charge calculation service and the parking service behaviour.

## Assumptions

- **Total spaces:** the vehicle parking has a fixed number of spaces (**50** in my implementation), filled by the initial migration. Capacity is calculated from the `parking_spaces` table.
- **Charge rounding:** any **partial minute** is **rounded up** to a **whole** minute and the £1 applies per *complete* 5-minute period.
- **Vehicle registration:** it is normalised before use (all whitespace removed and upper-cased, for example ` DX 8wx50 ` and `DX8WX50` are converted as the same vehicle).

  A missing/blank/invalid vehicle registration is rejected
  
  Vehicle may only have one active parking session at a time.
- **Spaces are interchangeable:** any vehicle type can use any space.

  Allocation is the lowest-numbered free space.
- **Time:** is persisted and returned in **UTC**.
- **Charge:** is computed as `decimal` and returned as `double` in response to match the required contract.
- **Concurrency:** a free space is picked and locked in one transaction by a PostgreSQL function (`FOR UPDATE SKIP LOCKED`) - so parallel requests get different spaces.

  Unique indexes (one active session per space and one active session per registration) prevent double parking.

## Things I would like to clarify

- Should a vehicle be charged within the first minute or not? I **assumed** - not.
- Should the total number of spaces be configurable? For testing I **assumed** this would be constant value.
- **Does `VehicleType` affect just charge?** Currently **every space** fits **every vehicle type**
 and allocation is just for the first free space.
 
  A real vehicle parking might have size compatibility where a vehicle can only be parked on the space which physically fits for it.
  This means we would need to check compatibility by size/type.
  
  But since this is not clarified in the task I **assumed** to exclude size/type compatibility.
