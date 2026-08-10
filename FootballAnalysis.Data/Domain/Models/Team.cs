using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Domain.Models
{
    public class Team
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "";

        public string ShortName { get; set; } = "";

        public string Stadium { get; set; } = "";

        public string City { get; set; } = "";
        public int FoundedYear { get; set; }

        public string Coach { get; set; } = "";

        public Guid? CaptainId { get; set; }
        public Player? Captain { get; set; } = default!;

        public string PreferredFormation { get; set; } = "";

        public string PlayingStyle { get; set; } = "";

        public ICollection<Player> Players { get; set; } = new List<Player>();

        public ICollection<TeamMatchStats> MatchStats { get; set; } = new List<TeamMatchStats>();
    }
}
