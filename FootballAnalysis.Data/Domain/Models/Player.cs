using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Domain.Models
{
    public class Player
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string Position { get; set; } = "";
        public List<string>? AlternativePositions { get; set; }

        public int ShirtNumber { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string Nationality { get; set; } = "";

        public bool IsCaptain { get; set; }

        public bool IsActive { get; set; }

        public Guid TeamId { get; set; }

        public Team Team { get; set; } = null!;

        public ICollection<PlayerMatchStats> MatchStats { get; set; } = new List<PlayerMatchStats>();
    }
}
