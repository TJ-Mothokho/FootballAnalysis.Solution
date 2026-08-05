using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FootballAnalysis.Data.Domain.Models
{
    public class Competition
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "";

        public string Country { get; set; } = "";

        public ICollection<Season> Seasons { get; set; } = new List<Season>();

        public ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}
