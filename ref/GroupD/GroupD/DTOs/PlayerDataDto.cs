namespace GroupD.DTOs;

public class PlayerDataDto
{
    public int      PlayerId  { get; set; }
    public string   FirstName { get; set; } = string.Empty;
    public string   LastName  { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public List<MatchDto> Matches { get; set; } = [];
}

public class MatchDto
{
    public string   Tournament { get; set; } = string.Empty;
    public string   Map        { get; set; } = string.Empty;
    public DateTime Date       { get; set; }
    public int      MVPs       { get; set; }
    public decimal  Rating     { get; set; }
    public int      Team1Score { get; set; }
    public int      Team2Score { get; set; }
}
