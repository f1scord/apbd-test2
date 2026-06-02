using System.ComponentModel.DataAnnotations;

namespace GroupD.DTOs;

public class CreatePlayerRequest
{
    [Required] [MaxLength(50)]  public string FirstName { get; set; } = null!;
    [Required] [MaxLength(100)] public string LastName  { get; set; } = null!;
    [Required] public DateTime BirthDate { get; set; }
    [Required] public List<CreateMatchDto> Matches { get; set; } = [];
}

public class CreateMatchDto
{
    public int     MatchId { get; set; }
    public int     MVPs    { get; set; }
    public decimal Rating  { get; set; }
}
