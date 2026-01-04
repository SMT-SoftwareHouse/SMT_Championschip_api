using SMTChampionshipAPI.DTOS;
using SMTChampionshipAPI.Models;

namespace SMTChampionshipAPI.Helpers
{
    public static class ResultHelperCTRL
    {
        public static TeamResultDto CalculateTeamResult(
         Team team,
         List<Match> matches)
        {
            int wins = 0, losses = 0, draws = 0;
            int goals = 0, points = 0;

            foreach (var m in matches)
            {
                bool isTeam1 = m.Team1Id == team.Id;
                bool isTeam2 = m.Team2Id == team.Id;

                if (!isTeam1 && !isTeam2)
                    continue;

                int teamGoals = isTeam1
                    ? m.Team1Goals ?? 0
                    : m.Team2Goals ?? 0;

                int opponentGoals = isTeam1
                    ? m.Team2Goals ?? 0
                    : m.Team1Goals ?? 0;

                goals += teamGoals;

                if (teamGoals > opponentGoals)
                {
                    wins++;
                    points += 3;
                }
                else if (teamGoals < opponentGoals)
                {
                    losses++;
                }
                else
                {
                    draws++;
                    points += 1;
                }
            }

            return new TeamResultDto
            {
                TeamId = team.Id,
                TeamName = team.EnName,
                GroupId = team.GroupId,

                MatchesCount = wins + losses + draws,
                WinCount = wins,
                LossCount = losses,
                DrawCount = draws,
                GoalsCount = goals,
                PointsCount = points
            };
        }
    }
}
