using SMTChampionshipAPI.Models;

namespace SMTChampionshipAPI.Helpers
{
    public static class MatchHelperCTRL
    {

        public static object MapMatch(Match m)
        {
            return new
            {
                m.Id,
                m.MatchDateTime,
                m.Name,
                m.Active,
                Team1 = new
                {
                    m.Team1Id,
                    m.Team1!.EnName
                },
                Team2 = new
                {
                    m.Team2Id,
                    m.Team2!.EnName
                },
                m.Team1Goals,
                m.Team2Goals
            };
        }
    }
}
