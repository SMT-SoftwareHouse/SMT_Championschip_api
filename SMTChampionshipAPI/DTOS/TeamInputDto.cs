using SMTChampionshipAPI.Models;

namespace SMTChampionshipAPI.DTOS
{
    public class TeamInputDto
    {
        public string? EnName { get; set; }
        public string? ArName { get; set; }
        public string? EnDescription { get; set; }
        public string? ArDescription { get; set; }
        public IFormFile? Logo { get; set; }
    }
}
