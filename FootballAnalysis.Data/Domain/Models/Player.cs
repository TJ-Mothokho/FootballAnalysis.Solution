using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Domain.Models
{
    public class Player
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string FullName { get; set; } = "";

        public string Position { get; set; } = "";

        public int ShirtNumber { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string Nationality { get; set; } = "";

        public bool IsCaptain { get; set; }

        public bool IsActive { get; set; }

        public int TeamId { get; set; }

        public Team Team { get; set; } = null!;

        public ICollection<PlayerMatchStats> MatchStats { get; set; } = new List<PlayerMatchStats>();
    }
}
