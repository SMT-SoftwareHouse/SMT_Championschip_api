namespace SMTChampionshipAPI.Models
{
    public enum MatchStatus
    {
        NotStarted=0,
        InProgress=1,
        Done=2,
    }

    public class Match
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public int? Team1Id { get; set; }
        public Team? Team1 { get; set; }

        public int? Team2Id { get; set; }
        public Team? Team2 { get; set; }

        public int? Team1Goals { get; set; } = null;
        public int? Team2Goals { get; set; } = null;

        public DateTime? MatchDateTime { get; set; }
        public bool? Active { get; set; } = true;

        public MatchStatus? MatchState { get; set; } = MatchStatus.NotStarted;

    }
}
