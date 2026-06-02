# APBD Test 2 — Cheatsheet

The exam is almost always the **same 5-table shape** —
your job is to spot which table plays which role, then rename + fill in.

---

## 0. Target framework

Template targets **net9.0** (the exam/school machine). If a clone won't build at home,
change the one line in `.csproj` to `net8.0` / `net10.0`. EF Core packages + Swashbuckle
must be present (CodeFirst needs EF; no migrations = 0 points).

---

## 1. Read the ER diagram — the 5 roles

```
        LookupA            LookupB        <- two reference tables (own data + PK)
            \               /
             LinkTable (middle)           <- own PK + 2 FKs + extra data (e.g. Price)
                  |
              JoinTable                   <- COMPOSITE PK (two "PK FK" columns) + extra data
                  |
              MainEntity                  <- bottom table, no FKs of its own
```

| Role        | How to identify                                          | B = Customers    | D = Players  |
|-------------|----------------------------------------------------------|------------------|--------------|
| MainEntity  | bottom table, **no FK columns**                          | Customer         | Player       |
| LookupA     | reference table pointed at by the middle table           | Concert          | Tournament   |
| LookupB     | second reference table pointed at by the middle table    | Ticket           | Map          |
| LinkTable   | **own PK** + **two FK** columns + extra data             | Ticket_Concert   | Match        |
| JoinTable   | **two columns marked `PK FK`** (composite key)           | Purchased_Ticket | Player_Match |

> Trick: find the table with **two `PK FK` columns** first — that's the JoinTable.
> What it links to = MainEntity (one side) and LinkTable (other side).

## Order of work (90 min)

| Step | What | Time |
|------|------|------|
| 1 | Identify the 5 tables from the diagram | 2 min |
| 2 | Create 5 Models | 10 min |
| 3 | DatabaseContext + seed data | 10 min |
| 4 | DTOs (response + request) | 5 min |
| 5 | IDbService + DbService | 20 min |
| 6 | Controller | 5 min |
| 7 | appsettings.json — change `Initial Catalog` | 1 min |
| 8 | `dotnet ef migrations add Init` → `dotnet ef database update` | 3 min |
| 9 | Run, test in `/swagger`, push to GitHub | 5 min |

---

## 2. Models

**Simple / lookup table:**
```csharp
[Table("Player")]
public class Player {
    [Key] public int PlayerId { get; set; }
    [MaxLength(50)]  public string FirstName { get; set; } = null!;
    [MaxLength(100)] public string LastName  { get; set; } = null!;
    [MaxLength(100)] public string? Phone    { get; set; }   // nullable col ("N") = string?, no = null!
    public DateTime BirthDate { get; set; }
    public ICollection<PlayerMatch> PlayerMatches { get; set; } = null!;
}
```

**Link table (own PK + 2 FKs):**
```csharp
[Table("Match")]
public class Match {
    [Key] public int MatchId { get; set; }
    [ForeignKey(nameof(Tournament))] public int TournamentId { get; set; }
    [ForeignKey(nameof(Map))]        public int MapId { get; set; }
    public int Team1Score { get; set; }
    [Precision(10, 2)] public decimal Price { get; set; }   // decimal(10,2) in diagram
    public Tournament Tournament { get; set; } = null!;
    public Map Map { get; set; } = null!;
    public ICollection<PlayerMatch> PlayerMatches { get; set; } = null!;
}
```
> `[Precision(x, y)]` and `[PrimaryKey(...)]` need `using Microsoft.EntityFrameworkCore;`

**Join table (composite PK) — always present:**
```csharp
[Table("Player_Match")]
[PrimaryKey(nameof(MatchId), nameof(PlayerId))]
public class PlayerMatch {
    [ForeignKey(nameof(Match))]  public int MatchId  { get; set; }
    [ForeignKey(nameof(Player))] public int PlayerId { get; set; }
    public int MVPs { get; set; }
    [Precision(4,2)] public decimal Rating { get; set; }
    public Player Player { get; set; } = null!;
    public Match  Match  { get; set; } = null!;
}
```

---

## 3. DatabaseContext

DbSets plural + seed data (insert parents before children, line up FK ids).

```csharp
public class DatabaseContext : DbContext {
    public DbSet<Player>      Players       { get; set; }
    public DbSet<Match>       Matches       { get; set; }
    public DbSet<PlayerMatch> PlayerMatches { get; set; }
    public DbSet<Tournament>  Tournaments   { get; set; }
    public DbSet<Map>         Maps          { get; set; }

    protected DatabaseContext() { }
    public DatabaseContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Tournament>().HasData(new List<Tournament> {
            new() { TournamentId = 1, Name = "t1", StartDate = DateTime.Today, EndDate = DateTime.Today },
        });
        // ...one HasData block per table, children last...
        modelBuilder.Entity<PlayerMatch>().HasData(new List<PlayerMatch> {
            new() { MatchId = 1, PlayerId = 1, MVPs = 5, Rating = 4 },
        });
    }
}
```

---

## 4. DTOs

DTOs reference only **other DTOs**, never the Models. Copy the JSON from the task.

**Response (GET):**
```csharp
public class PlayerDataDto {
    public int PlayerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public List<MatchDto> Matches { get; set; } = [];
}
public class MatchDto {
    public string Tournament { get; set; } = string.Empty;
    public int    MVPs       { get; set; }
    public decimal Rating    { get; set; }
}
```

**Request (POST):**
```csharp
public class CreatePlayerRequest {
    [Required][MaxLength(50)]  public string FirstName { get; set; } = null!;
    [Required][MaxLength(100)] public string LastName  { get; set; } = null!;
    [Required] public List<CreateMatchDto> Matches { get; set; } = [];
}
public class CreateMatchDto {
    public int MatchId { get; set; }
    public int MVPs    { get; set; }
    public decimal Rating { get; set; }
}
```

---

## 5. DbService

**Task 1 — GET:** load with Include chain, 404 if null, map to DTO
```csharp
public async Task<PlayerDataDto> GetPlayerDataAsync(int id) {
    var player = await context.Players
        .Include(p => p.PlayerMatches)
            .ThenInclude(pm => pm.Match)
                .ThenInclude(m => m.Tournament)
        .Include(p => p.PlayerMatches)
            .ThenInclude(pm => pm.Match)
                .ThenInclude(m => m.Map)
        .FirstOrDefaultAsync(p => p.PlayerId == id);

    if (player == null) throw new NotFoundException($"Player {id} not found.");

    return new PlayerDataDto {
        PlayerId  = player.PlayerId,
        FirstName = player.FirstName,
        Matches   = player.PlayerMatches.Select(pm => new MatchDto {
            Tournament = pm.Match.Tournament.Name,
            MVPs       = pm.MVPs,
            Rating     = pm.Rating,
        }).ToList()
    };
}
```

**Task 2 — POST:** create inside a transaction. Two shapes — pick the one your task asks.
```csharp
public async Task CreatePlayerAsync(CreatePlayerRequest request) {
    await using var transaction = await context.Database.BeginTransactionAsync();
    try {
        // 409 check (Customers style) — entity must NOT already exist:
        // var exists = await context.Players.FirstOrDefaultAsync(p => p.PlayerId == request.Id);
        // if (exists != null) throw new ConflictException("Already exists.");

        var player = new Player { FirstName = request.FirstName, LastName = request.LastName };
        context.Players.Add(player);

        foreach (var item in request.Matches) {
            // SHAPE B — link row already exists, look up by id:
            var match = await context.Matches.FirstOrDefaultAsync(m => m.MatchId == item.MatchId);
            if (match == null) throw new NotFoundException($"Match {item.MatchId} not found.");

            context.PlayerMatches.Add(new PlayerMatch {
                Player = player,   // navigation property, NOT id
                Match  = match,
                MVPs   = item.MVPs,
                Rating = item.Rating,
            });

            // optional: update a field on the existing row
            if (match.BestRating == null || match.BestRating < item.Rating)
                match.BestRating = item.Rating;

            // SHAPE A — create fresh rows instead (Customers style), look up lookup by NAME:
            // var concert = await context.Concerts.FirstOrDefaultAsync(c => c.Name == item.ConcertName);
            // if (concert == null) throw new NotFoundException(...);
            // var ticket = new Ticket { SerialNumber = $"SER{Guid.NewGuid().ToString()[..5]}", SeatNumber = item.SeatNumber };
            // context.Tickets.Add(ticket);
            // var tc = new TicketConcert { Ticket = ticket, Concert = concert, Price = item.Price };
            // context.TicketConcerts.Add(tc);
            // context.PurchasedTickets.Add(new PurchasedTicket { Customer = player, TicketConcert = tc, PurchaseDate = DateTime.Now });
        }

        await context.SaveChangesAsync();
        await transaction.CommitAsync();
    } catch (Exception) { await transaction.RollbackAsync(); throw; }  // never delete throw
}
```

---

## 5b. DbService — new patterns

**GET list with optional filter (Gr. A style):**
```csharp
public async Task<List<PatientDto>> GetPatientsAsync(string? lastName) {
    var query = context.Patients
        .Include(p => p.Appointments).ThenInclude(a => a.Doctor)
        .Include(p => p.Appointments).ThenInclude(a => a.AppointmentServices).ThenInclude(s => s.MedicalService)
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(lastName))
        query = query.Where(p => p.LastName.Contains(lastName));

    return (await query.ToListAsync()).Select(p => new PatientDto { ... }).ToList();
}
```

**POST — create + assign to existing (doctor must exist, date must not be past):**
```csharp
var doctor = await context.Doctors.FirstOrDefaultAsync(d => d.DoctorId == request.Appointment.DoctorId);
if (doctor == null) throw new NotFoundException("Doctor not found.");
if (request.Appointment.AppointmentDate < DateTime.Now) throw new BadRequestException("Date is in the past.");

var patient = new Patient { ... };
context.Patients.Add(patient);
context.Appointments.Add(new Appointment { Patient = patient, Doctor = doctor, ... });
```

**PUT — update in place (Gr. B style):**
```csharp
public async Task UpdateOrderAsync(int id) {
    await using var transaction = await context.Database.BeginTransactionAsync();
    try {
        var order = await context.Orders
            .Include(o => o.Payments)
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (order == null) throw new NotFoundException($"Order {id} not found.");
        if (order.Payments.Any()) throw new ConflictException("Order has payments, cannot update.");

        order.Status = "Processed";
        decimal total = 0;
        foreach (var item in order.OrderItems) {
            item.Product.Price = Math.Round(item.Product.Price * 0.9m, 2);  // -10%
            item.Price = item.Product.Price;
            total += item.Price * item.Quantity;
        }
        order.TotalAmount = total;

        await context.SaveChangesAsync();
        await transaction.CommitAsync();
    } catch (Exception) { await transaction.RollbackAsync(); throw; }
}
```

---

## 6. Controller

```csharp
[Route("api/[controller]")]
[ApiController]
public class PatientsController(IDbService dbService) : ControllerBase {

    // GET list with optional filter: GET /api/patients?lastName=Kow
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] string? lastName) {
        return Ok(await dbService.GetPatientsAsync(lastName));
    }

    // GET single by id: GET /api/orders/1
    // [HttpGet("{id:int}")]
    // public async Task<IActionResult> GetByIdAsync(int id) {
    //     try { return Ok(await dbService.GetOrderByIdAsync(id)); }
    //     catch (NotFoundException e) { return NotFound(e.Message); }
    // }

    // POST — create entity
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePatientRequest request) {
        try { await dbService.CreatePatientAsync(request); return Created(); }
        catch (NotFoundException e)  { return NotFound(e.Message); }
        catch (BadRequestException e){ return BadRequest(e.Message); }
    }

    // PUT — update entity (no body, id from URL)
    // [HttpPut("{id:int}")]
    // public async Task<IActionResult> UpdateAsync(int id) {
    //     try { await dbService.UpdateOrderAsync(id); return Ok(); }
    //     catch (NotFoundException e) { return NotFound(e.Message); }
    //     catch (ConflictException e) { return Conflict(e.Message); }
    // }
}
```

---

## 7. Program.cs

```csharp
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IDbService, DbService>();
// after build:
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
```

## appsettings.json
```json
"ConnectionStrings": {
  "Default": "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=YOUR_DB_NAME;Trusted_Connection=True;TrustServerCertificate=True"
}
```

## Exceptions — never change
```csharp
public class NotFoundException(string message  = "Not found")  : Exception(message);
public class ConflictException(string message  = "Conflict")   : Exception(message);
public class BadRequestException(string message = "Bad request"): Exception(message);
```

---

## Status codes

| Situation                     | Return            | Where            |
|-------------------------------|-------------------|------------------|
| entity not found (GET)        | `NotFound()` 404  | catch in ctrl    |
| related row not found (POST)  | `NotFound()` 404  | throw in service |
| entity already exists         | `Conflict()` 409  | throw in service |
| bad input (count, empty list) | `BadRequest()` 400| ctrl, before try |
| created OK                    | `Created()` 201   | end of POST      |
| got OK                        | `Ok(data)` 200    | end of GET       |

## Common mistakes

- Missing `await using` on transaction → won't compile
- Missing `throw` in catch → returns 200 instead of 404/409
- Used `Id` instead of navigation property in join record → EF error
- Missing `[PrimaryKey]` on many-to-many → EF error
- Missing `[Table]` → table name won't match diagram
- DTO referencing a Model instead of another DTO
- Forgot `AddScoped<IDbService, DbService>()` → 500 on startup
- Didn't run `dotnet ef database update` → no database
- No seed data → -50%
