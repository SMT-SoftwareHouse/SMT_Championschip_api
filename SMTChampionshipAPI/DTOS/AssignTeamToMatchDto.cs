namespace SMTChampionshipAPI.DTOS
{
    public class AssignTeamToMatchDto
    {
        public int TeamId { get; set; }

        // "Team1" أو "Team2"
        public string Slot { get; set; }
    }
}
