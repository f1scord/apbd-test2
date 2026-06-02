// ============================================================
//  REQUEST DTO — the shape POST/PUT receives in the body.
//  COPY the JSON example from POST.json / task description.
//
//  If the task is PUT with no body (just id in URL) — no DTO needed,
//  all logic is driven by the id from the route.
// ============================================================
//  COPY the JSON example from the "POST" part of the task.
//
//  [Required] / [MaxLength] here = automatic validation:
//  if it fails, [ApiController] returns 400 for you.
// ============================================================
// TODO: rename classes + fields to match the task's JSON

using System.ComponentModel.DataAnnotations;

namespace ApbdTest.DTOs;

public class CreateMainEntityRequest
{
    public CreateMainDto      Main  { get; set; } = null!;
    public List<CreateItemDto> Items { get; set; } = [];
}

public class CreateMainDto
{
    public int     Id        { get; set; }   // used for "already exists" 409 check
    [MaxLength(50)]  public string  FirstName { get; set; } = string.Empty;
    [MaxLength(100)] public string  LastName  { get; set; } = string.Empty;
    [MaxLength(100)] public string? Phone     { get; set; }
    public DateTime Date { get; set; }       // used for "date in past" 400 check
}

public class CreateItemDto
{
    // TODO: the fields sent per item. Depending on the task this is either:
    //   (A) data to look up an existing row by NAME  (e.g. ConcertName)
    //   (B) an id of an existing row                 (e.g. MatchId)
    public string LookupName { get; set; } = string.Empty;
    public int    Number     { get; set; }
    public decimal Price      { get; set; }
}
