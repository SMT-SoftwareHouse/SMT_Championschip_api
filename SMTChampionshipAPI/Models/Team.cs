namespace SMTChampionshipAPI.Models
{
    public class Team
    {
        public Team()
        {

        }
        public int Id { get; set; }
        public string? LogoPath { get; set; }
        public string? EnName { get; set; }
        public string? ArName { get; set; }
        public string? EnDescription { get; set; }
        public string? ArDescription { get; set; }

        public int? GroupId { get; set; }

    }
}
