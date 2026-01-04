namespace SMTChampionshipAPI.DTOS
{
    public class TeamResultDto
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public int? GroupId { get; set; }   

        public int MatchesCount { get; set; }
        public int WinCount { get; set; }
        public int LossCount { get; set; }
        public int DrawCount { get; set; }

        public int GoalsCount { get; set; }
        public int PointsCount { get; set; }
    }
}
