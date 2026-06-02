// ============================================================
//  JOIN ENTITY — composite PK (two PK FK columns)
//  Links ChildEntity ↔ LookupEntity.
//
//  Examples:
//    Gr. A: Appointment_Services (AppointmentId PK FK, ServiceId PK FK, Quantity, PerformedAt)
//    Gr. B: Order_Items          (OrderId PK FK, ProductId PK FK, Quantity, Price)
// ============================================================
// TODO: rename class + [Table] + FK names + extra fields

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApbdTest.Entities;

[Table("Join_Entity")]
[PrimaryKey(nameof(ChildEntityId), nameof(LookupEntityId))]
public class JoinEntity
{
    [ForeignKey(nameof(ChildEntity))]  public int ChildEntityId  { get; set; }
    [ForeignKey(nameof(LookupEntity))] public int LookupEntityId { get; set; }

    // TODO: extra fields (Quantity, Price, PerformedAt, etc.)
    public int Quantity { get; set; }
    [Precision(10, 2)] public decimal Price { get; set; }

    public ChildEntity  ChildEntity  { get; set; } = null!;
    public LookupEntity LookupEntity { get; set; } = null!;
}
