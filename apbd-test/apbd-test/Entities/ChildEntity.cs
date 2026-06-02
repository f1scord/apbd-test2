// ============================================================
//  CHILD ENTITY — has FK to MainEntity + FK to LookupEntity
//  and may have its own composite PK (PK FK columns).
//
//  Examples:
//    Gr. A: Appointment  (AppointmentId PK, PatientId FK, DoctorId FK, Date, Status)
//    Gr. B: Order        (OrderId PK, Users_UserId FK, Date, Status, TotalAmount)
//           Payment      (PaymentId PK, Orders_OrderId FK, Method, Amount, Status)
// ============================================================
// TODO: rename class + [Table] + fields

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApbdTest.Entities;

[Table("ChildEntity")]
public class ChildEntity
{
    [Key] public int ChildEntityId { get; set; }

    [ForeignKey(nameof(MainEntity))]   public int MainEntityId   { get; set; }
    [ForeignKey(nameof(LookupEntity))] public int LookupEntityId { get; set; }

    // TODO: extra fields (Status, Date, Amount, etc.)
    [MaxLength(100)] public string Status { get; set; } = null!;
    [Precision(10, 2)] public decimal Amount { get; set; }

    public MainEntity   MainEntity   { get; set; } = null!;
    public LookupEntity LookupEntity { get; set; } = null!;

    // if this child has its own children (e.g. Appointment → AppointmentServices)
    public ICollection<JoinEntity> JoinEntities { get; set; } = null!;
}
