using SMTChampionshipAPI.Models;

namespace SMTChampionshipAPI.DTOS
{
    public class MatchCreateDTO
    {
        public string MatchName { get; set; }
        public int? Team1Id { get; set; }
        public int? Team2Id { get; set; }

        public DateTime? MatchDateTime { get; set; }

    }
}
