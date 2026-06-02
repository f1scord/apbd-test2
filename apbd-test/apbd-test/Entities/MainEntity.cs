// ============================================================
//  MAIN ENTITY — the thing you GET (list or by id) and POST/PUT
//  New tests use folder "Entities" (not "Models").
//
//  Examples:
//    Gr. A: Patient  (PatientId, FirstName, LastName, DateOfBirth, Phone varchar(9))
//    Gr. B: User     (UserId, Username, Email, PasswordHash, CreatedAt)
//           Order    (OrderId, OrderDate, Status, TotalAmount, FK→User)
// ============================================================
// TODO: rename class + [Table] + fields

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApbdTest.Entities;

[Table("MainEntity")]
public class MainEntity
{
    [Key] public int MainEntityId { get; set; }

    // TODO: fields from ER diagram
    [MaxLength(50)]  public string FirstName { get; set; } = null!;
    [MaxLength(100)] public string LastName  { get; set; } = null!;
    public DateTime Date { get; set; }

    // navigation to child table (one-to-many)
    public ICollection<ChildEntity> ChildEntities { get; set; } = null!;
}
