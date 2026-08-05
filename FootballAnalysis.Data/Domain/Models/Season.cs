using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FootballAnalysis.Data.Domain.Models
{
    public class Season
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "";

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public bool IsCurrent { get; set; }

        public ICollection<Competition> Competitions { get; set; } = new List<Competition>();

        public ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}
