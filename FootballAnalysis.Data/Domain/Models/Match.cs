using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Domain.Models
{
    public class Match
    {
        public int Id { get; set; }

        public DateTime KickOff { get; set; }

        public string Venue { get; set; } = "";

        public string Referee { get; set; } = "";

        public int Attendance { get; set; }

        public int HomeGoals { get; set; }

        public int AwayGoals { get; set; }

        public string Status { get; set; } = "";

        public int HomeTeamId { get; set; }

        public Team HomeTeam { get; set; } = null!;

        public int AwayTeamId { get; set; }

        public Team AwayTeam { get; set; } = null!;

        public int CompetitionId { get; set; }

        public Competition Competition { get; set; } = null!;

        public int SeasonId { get; set; }

        public Season Season { get; set; } = null!;

        public ICollection<TeamMatchStats> TeamStats { get; set; } = new List<TeamMatchStats>();

        public ICollection<PlayerMatchStats> PlayerStats { get; set; } = new List<PlayerMatchStats>();
    }
}
