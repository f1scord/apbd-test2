// ============================================================
//  LOOKUP ENTITY — reference table (Doctors, Products, MedicalServices)
//  No FKs, just its own data. Referenced by ChildEntity or JoinEntity.
// ============================================================
// TODO: rename class + [Table] + fields

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApbdTest.Entities;

[Table("LookupEntity")]
public class LookupEntity
{
    [Key] public int LookupEntityId { get; set; }

    [MaxLength(100)] public string Name        { get; set; } = null!;
    [MaxLength(100)] public string Description { get; set; } = null!;
    [Precision(10, 2)] public decimal Price    { get; set; }

    public ICollection<ChildEntity> ChildEntities { get; set; } = null!;
    public ICollection<JoinEntity>  JoinEntities  { get; set; } = null!;
}
