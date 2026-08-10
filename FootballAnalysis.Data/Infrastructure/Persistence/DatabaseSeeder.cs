using FootballAnalysis.Data.Domain.Models;
using FootballAnalysis.Data.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FootballAnalysis.Data.Infrastructure.Persistence
{
    public class DatabaseSeeder
    {
        private readonly ApplicationDbContext _context;

        public DatabaseSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            var competition = await _context.Competitions
                .Include(existingCompetition => existingCompetition.Seasons)
                .FirstOrDefaultAsync(existingCompetition => existingCompetition.Name == "Betway Premiership");

            if (competition == null)
            {
                competition = new Competition
                {
                    Id = Guid.Parse("095ca337-5fbd-4204-961b-bd3d5d8583ad"),
                    Name = "Betway Premiership",
                    Country = "South Africa"
                };

                await _context.Competitions.AddAsync(competition);
            }

            var season = await _context.Seasons
                .FirstOrDefaultAsync(existingSeason => existingSeason.Name == "2026/2027");

            if (season == null)
            {
                season = new Season
                {
                    Id = Guid.Parse("f77c6fc7-29d1-45ad-a535-625109f7711e"),
                    Name = "2026/2027",
                    StartDate = new DateOnly(2026, 7, 1),
                    EndDate = new DateOnly(2027, 6, 30),
                    IsCurrent = true
                };

                await _context.Seasons.AddAsync(season);
            }

            if (!competition.Seasons.Any(existingSeason => existingSeason.Name == season.Name))
            {
                competition.Seasons.Add(season);
            }

            if (!await _context.Teams.AnyAsync(existingTeam => existingTeam.Name == "Orlando Pirates"))
            {
                await _context.Teams.AddAsync(new Team
                {
                    Id = Guid.Parse("f5f9fc51-c2ca-49a8-a137-66a640db8937"),
                    Name = "Orlando Pirates",
                    ShortName = "ORL",
                    Stadium = "Orlando Amstel Arena",
                    City = "Johannesburg",
                    FoundedYear = 1937,
                    Coach = "Abdeslam Ouaddou",
                    PreferredFormation = "4-2-3-1",
                    PlayingStyle = "Direct Play"
                });
            }

            if(!await _context.Teams.AnyAsync(existingTeam => existingTeam.Name == "Kaizer Chiefs"))
            {
                await _context.Teams.AddAsync(new Team
                {
                    Id = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"),
                    Name = "Kaizer Chiefs",
                    ShortName = "KAI",
                    Stadium = "FNB Stadium",
                    City = "Johannesburg",
                    FoundedYear = 1970,
                    Coach = "Fernando Da Cruz",
                    PreferredFormation = "4-2-3-1",
                    PlayingStyle = "Attacking Transition"
                });
            }

            if (!await _context.Teams.AnyAsync(existingTeam => existingTeam.Name == "Mamelodi Sundowns"))
            {
                await _context.Teams.AddAsync(new Team
                {
                    Id = Guid.Parse("b2c3d4e5-f6a7-4b6c-9d0e-1f2a3b4c5d6e"),
                    Name = "Mamelodi Sundowns",
                    ShortName = "MSD",
                    Stadium = "Loftus Versfield Stadium",
                    City = "Pretoria",
                    FoundedYear = 1970,
                    Coach = "Miguel Cardoso",
                    PreferredFormation = "3-4-3",
                    PlayingStyle = "Possession Based"
                });
            }

            if (!await _context.Teams.AnyAsync(existingTeam => existingTeam.Name == "AmaZulu FC"))
            {
                await _context.Teams.AddAsync(new Team
                {
                    Id = Guid.Parse("c3d4e5f6-a7b8-4c7d-0e1f-2a3b4c5d6e7f"),
                    Name = "AmaZulu FC",
                    ShortName = "AMA",
                    Stadium = "Moses Mabhida Stadium",
                    City = "Durban",
                    Coach = "Arthur Zwane",
                    PreferredFormation = "3-4-2-1",
                    PlayingStyle = "Defensive Solidity"
                });
            }

            if (!await _context.Teams.AnyAsync(existingTeam => existingTeam.Name == "Chippa United"))
            {
                await _context.Teams.AddAsync(new Team
                {
                    Id = Guid.Parse("1b2c3d4e-5f6a-4c2b-7d8e-9f0a1b2c3d4e"),
                    Name = "Chippa United",
                    ShortName = "CHI",
                    Stadium = "Nelson Mandela Bay Stadium",
                    City = "Gqeberha",
                    FoundedYear = 2011,
                    Coach = "Brandon Truter",
                    PreferredFormation = "4-3-3",
                    PlayingStyle = "Balanced"
                });
            }

            if (!await _context.Teams.AnyAsync(existingTeam => existingTeam.Name == "Golden Arrows"))
            {
                await _context.Teams.AddAsync(new Team
                {
                    Id = Guid.Parse("0a1b2c3d-4e5f-4b1a-6c7d-8e9f0a1b2c3d"),
                    Name = "Golden Arrows",
                    ShortName = "LGA",
                    Stadium = "King Goodwill Zwelithini Stadium",
                    City = "Durban",
                    FoundedYear = 1943,
                    Coach = "",
                    PreferredFormation = "4-3-3",
                    PlayingStyle = "Wing Play"
                });
            }

            if (!await _context.Teams.AnyAsync(existingTeam => existingTeam.Name == "Marumo Gallants"))
            {
                await _context.Teams.AddAsync(new Team
                {
                    Id = Guid.Parse("4e5f6a7b-8c9d-4f5e-0a1b-2c3d4e5f6a7b"),
                    Name = "Marumo Gallants",
                    ShortName = "MAR",
                    Stadium = "Royal Bafokeng Stadium",
                    City = "Rustenburg",
                    PreferredFormation = "4-2-3-1",
                    PlayingStyle = "Ball Retention"
                });
            }

            if (!await _context.Teams.AnyAsync(existingTeam => existingTeam.Name == "Milford FC"))
            {
                await _context.Teams.AddAsync(new Team
                {
                    Id = Guid.Parse("7b8c9d0e-1f2a-4c8b-3d4e-5f6a7b8c9d0e"),
                    Name = "Milford FC",
                    ShortName = "MIL",
                    Stadium = "Richards Bay Sports Stadium",
                    City = "Richards Bay",
                    PreferredFormation = "4-1-4-1",
                    PlayingStyle = "Direct"
                });
            }

            if (!await _context.Teams.AnyAsync(existingTeam => existingTeam.Name == "Stellenbosch FC"))
            {
                await _context.Teams.AddAsync(new Team
                {
                    Id = Guid.Parse("f6a7b8c9-d0e1-4fa0-3b4c-5d6e7f8a9b0c"),
                    Name = "Stellenbosch FC",
                    ShortName = "STB",
                    Stadium = "Danie Craven Stadium",
                    City = "Clermont",
                    Coach = "Gavin Hunt",
                    PreferredFormation = "4-1-4-1",
                    PlayingStyle = "Direct Attacking"
                });
            }

            if (!await _context.Teams.AnyAsync(existingTeam => existingTeam.Name == "Sekhukhune United"))
            {
                await _context.Teams.AddAsync(new Team
                {
                    Id = Guid.Parse("d4e5f6a7-b8c9-4d8e-1f2a-3b4c5d6e7f8a"),
                    Name = "Sekhukhune United",
                    ShortName = "SEK",
                    Stadium = "Seshego Stadium",
                    City = "Polokwane",
                    Coach = "Cedric Kaze",
                    PreferredFormation = "4-4-1-1",
                    PlayingStyle = "Counter-Attacking"
                });
            }

            if (!await _context.Teams.AnyAsync(existingTeam => existingTeam.Name == "TS Galaxy"))
            {
                await _context.Teams.AddAsync(new Team
                {
                    Id = Guid.Parse("e5f6a7b8-c9d0-4e9f-2a3b-4c5d6e7f8a9b"),
                    Name = "TS Galaxy",
                    ShortName = "TSG",
                    Stadium = "Mbombela Stadium",
                    City = "Nelspruit",
                    FoundedYear = 2015,
                    Coach = "Bernard Parker",
                    PreferredFormation = "3-5-2",
                    PlayingStyle = "High Pressing"
                });
            }

            if (!await _context.Teams.AnyAsync(existingTeam => existingTeam.Name == "Durban City"))
            {
                await _context.Teams.AddAsync(new Team
                {
                    Id = Guid.Parse("5f6a7b8c-9d0e-4a6f-1b2c-3d4e5f6a7b8c"),
                    Name = "Durban City",
                    ShortName = "DUR",
                    Stadium = "Chatsworth Stadium",
                    City = "Chatsworth",
                    Coach = "khalil Ben Youssef",
                    PreferredFormation = "4-2-3-1",
                    PlayingStyle = "Counter-Press"
                });
            }

            if (!await _context.Teams.AnyAsync(existingTeam => existingTeam.Name == "Polokwane City"))
            {
                await _context.Teams.AddAsync(new Team
                {
                    Id = Guid.Parse("2c3d4e5f-6a7b-4d3c-8e9f-0a1b2c3d4e5f"),
                    Name = "Polokwane City",
                    ShortName = "PLK",
                    Stadium = "Old Peter Mokaba Stadium",
                    City = "Polokwane",
                    FoundedYear = 2005,
                    Coach = "Willy",
                    PreferredFormation = "4-1-4-1",
                    PlayingStyle = "Compact Defense"
                });
            }

            if (!await _context.Teams.AnyAsync(existingTeam => existingTeam.Name == "Kruger United"))
            {
                await _context.Teams.AddAsync(new Team
                {
                    Id = Guid.Parse("6a7b8c9d-0e1f-4b7a-2c3d-4e5f6a7b8c9d"),
                    Name = "Kruger United",
                    ShortName = "KRU",
                    Stadium = "Mbombela Stadium",
                    City = "Nelspruit",
                    Coach = "Abram Mongoya",
                    PreferredFormation = "4-3-3",
                    PlayingStyle = "High Intensity"
                });
            }

            if (!await _context.Teams.AnyAsync(existingTeam => existingTeam.Name == "Siwelele"))
            {
                await _context.Teams.AddAsync(new Team
                {
                    Id = Guid.Parse("8c9d0e1f-2a3b-4d9c-4e5f-6a7b8c9d0e1f"),
                    Name = "Siwelele",
                    ShortName = "SIW",
                    Stadium = "Dr Petrus Molemela Stadium",
                    City = "Bloemfontein",
                    FoundedYear = 2025,
                    Coach = "Lehlogonolo Seema",
                    PreferredFormation = "4-2-3-1",
                    PlayingStyle = "Fluid Attacking"
                });
            }

            if (!await _context.Teams.AnyAsync(existingTeam => existingTeam.Name == "Richards Bay"))
            {
                await _context.Teams.AddAsync(new Team
                {
                    Id = Guid.Parse("3d4e5f6a-7b8c-4e4d-9f0a-1b2c3d4e5f6a"),
                    Name = "Richards Bay",
                    ShortName = "RIC",
                    Stadium = "Richards Bay Sports Stadium",
                    City = "Richards Bay",
                    Coach = "Ronnie Gabriel",
                    PreferredFormation = "4-2-3-1",
                    PlayingStyle = "Pragmatic"
                });
            }



            await _context.SaveChangesAsync();
        }
    }
}
