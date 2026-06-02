// ============================================================
//  RESPONSE DTO — copy the JSON from GET.json / task example.
//
//  GET can return:
//    (A) LIST:   List<MainEntityDto>   — Gr. A (all patients, optional ?lastName= filter)
//    (B) SINGLE: MainEntityDto         — Gr. B (order by id, 404 if not found)
//
//  DTOs never reference Entity classes — only other DTOs.
// ============================================================
//  COPY the JSON example from the task and build classes that
//  match it field-for-field (names + nesting).
//
//  RULE: DTOs reference only OTHER DTOs, never the Models.
//  (no  List<JoinTable>  here — use  List<ItemDto>)
// ============================================================
// TODO: rename classes + fields to match the task's JSON

namespace ApbdTest.DTOs;

public class MainEntityDto
{
    public string   FirstName { get; set; } = string.Empty;
    public string   LastName  { get; set; } = string.Empty;
    public DateTime Date      { get; set; }
    public List<ChildDto> Items { get; set; } = [];
}

public class ChildDto
{
    public int      ChildId { get; set; }
    public string   Status  { get; set; } = string.Empty;
    public DateTime Date    { get; set; }
    public LookupDto Lookup { get; set; } = null!;
    public List<JoinDto> JoinItems { get; set; } = [];
}

public class LookupDto
{
    public string  Name        { get; set; } = string.Empty;
    public string  Description { get; set; } = string.Empty;
    public decimal Price       { get; set; }
}

public class JoinDto
{
    public int     Quantity { get; set; }
    public decimal Price    { get; set; }
    public LookupDto Lookup { get; set; } = null!;
}
