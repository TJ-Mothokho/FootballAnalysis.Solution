using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballAnalysis.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionSeason",
                columns: table => new
                {
                    CompetitionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeasonsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionSeason", x => new { x.CompetitionsId, x.SeasonsId });
                    table.ForeignKey(
                        name: "FK_CompetitionSeason_Competitions_CompetitionsId",
                        column: x => x.CompetitionsId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionSeason_Seasons_SeasonsId",
                        column: x => x.SeasonsId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KickOff = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Venue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Referee = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attendance = table.Column<int>(type: "int", nullable: true),
                    HomeGoals = table.Column<int>(type: "int", nullable: false),
                    AwayGoals = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AwayTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompetitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeasonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerMatchStats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Started = table.Column<bool>(type: "bit", nullable: false),
                    WasSubstitutedOn = table.Column<bool>(type: "bit", nullable: false),
                    WasSubstitutedOff = table.Column<bool>(type: "bit", nullable: false),
                    MinutesPlayed = table.Column<int>(type: "int", nullable: false),
                    IsCaptain = table.Column<bool>(type: "bit", nullable: false),
                    IsManOfTheMatch = table.Column<bool>(type: "bit", nullable: false),
                    Analysis_PerformanceSummary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Analysis_AnalystNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayerStats_FotmobRating = table.Column<double>(type: "float", nullable: false),
                    PlayerStats_SofascoreRating = table.Column<double>(type: "float", nullable: false),
                    PlayerStats_MinutesPlayed = table.Column<int>(type: "int", nullable: false),
                    PlayerAttack_Goals = table.Column<int>(type: "int", nullable: false),
                    PlayerAttack_XG = table.Column<double>(type: "float", nullable: false),
                    PlayerAttack_XGOT = table.Column<double>(type: "float", nullable: false),
                    PlayerAttack_TotalShots = table.Column<int>(type: "int", nullable: false),
                    PlayerAttack_ShotsOnTarget = table.Column<int>(type: "int", nullable: false),
                    PlayerAttack_TouchesInOppositionBox = table.Column<int>(type: "int", nullable: false),
                    PlayerAttack_BigChancesMissed = table.Column<int>(type: "int", nullable: false),
                    PlayerAttack_SuccessfulDribbles = table.Column<int>(type: "int", nullable: false),
                    PlayerAttack_DribblesAttempted = table.Column<int>(type: "int", nullable: false),
                    PlayerPasses_Touches = table.Column<int>(type: "int", nullable: false),
                    PlayerPasses_AccuratePasses = table.Column<int>(type: "int", nullable: false),
                    PlayerPasses_PassesAttempted = table.Column<int>(type: "int", nullable: false),
                    PlayerPasses_Assists = table.Column<int>(type: "int", nullable: false),
                    PlayerPasses_XA = table.Column<double>(type: "float", nullable: false),
                    PlayerPasses_ChancesCreated = table.Column<int>(type: "int", nullable: false),
                    PlayerPasses_PassesIntoFinalThird = table.Column<int>(type: "int", nullable: false),
                    PlayerPasses_AccurateCrosses = table.Column<int>(type: "int", nullable: false),
                    PlayerPasses_CrossesAttempted = table.Column<int>(type: "int", nullable: false),
                    PlayerPasses_AccurateLongBalls = table.Column<int>(type: "int", nullable: false),
                    PlayerPasses_LongBallsAttempted = table.Column<int>(type: "int", nullable: false),
                    PlayerDefence_DefensiveContributions = table.Column<int>(type: "int", nullable: false),
                    PlayerDefence_Tackles = table.Column<int>(type: "int", nullable: false),
                    PlayerDefence_Interceptions = table.Column<int>(type: "int", nullable: false),
                    PlayerDefence_Blocks = table.Column<int>(type: "int", nullable: false),
                    PlayerDefence_Recoveries = table.Column<int>(type: "int", nullable: false),
                    PlayerDefence_Clearance = table.Column<int>(type: "int", nullable: false),
                    PlayerDefence_HeadedClearances = table.Column<int>(type: "int", nullable: false),
                    PlayerDefence_DribbledPast = table.Column<int>(type: "int", nullable: false),
                    PlayerDuels_DuelsWon = table.Column<int>(type: "int", nullable: false),
                    PlayerDuels_DuelsLost = table.Column<int>(type: "int", nullable: false),
                    PlayerDuels_GroundDuelsWon = table.Column<int>(type: "int", nullable: false),
                    PlayerDuels_TotalGroundDuels = table.Column<int>(type: "int", nullable: false),
                    PlayerDuels_AerialDuelsWon = table.Column<int>(type: "int", nullable: false),
                    PlayerDuels_TotalAerialDuels = table.Column<int>(type: "int", nullable: false),
                    Goalkeepering_Saves = table.Column<int>(type: "int", nullable: false),
                    Goalkeepering_GoalsConceded = table.Column<int>(type: "int", nullable: false),
                    Goalkeepering_FacedxGOT = table.Column<double>(type: "float", nullable: false),
                    Goalkeepering_GoalsPrevented = table.Column<double>(type: "float", nullable: false),
                    Goalkeepering_ActedAsSweeper = table.Column<int>(type: "int", nullable: false),
                    Goalkeepering_HighClaim = table.Column<int>(type: "int", nullable: false),
                    Goalkeepering_LongBalls = table.Column<int>(type: "int", nullable: false),
                    Goalkeepering_AccurateLongBalls = table.Column<int>(type: "int", nullable: false),
                    Goalkeepering_Passes = table.Column<int>(type: "int", nullable: false),
                    Goalkeepering_AccuratePasses = table.Column<int>(type: "int", nullable: false),
                    PlayerDiscipline_YellowCards = table.Column<int>(type: "int", nullable: false),
                    PlayerDiscipline_RedCards = table.Column<int>(type: "int", nullable: false),
                    PlayerDiscipline_FoulsCommitted = table.Column<int>(type: "int", nullable: false),
                    PlayerDiscipline_WasFouled = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMatchStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerMatchStats_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlternativePositions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShirtNumber = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCaptain = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stadium = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoundedYear = table.Column<int>(type: "int", nullable: false),
                    Coach = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaptainId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PreferredFormation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayingStyle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Players_CaptainId",
                        column: x => x.CaptainId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamMatchStats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsHome = table.Column<bool>(type: "bit", nullable: false),
                    Formation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayingStyle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchStats_TeamGoals = table.Column<int>(type: "int", nullable: false),
                    MatchStats_OppositionGoals = table.Column<int>(type: "int", nullable: false),
                    MatchStats_Possession = table.Column<int>(type: "int", nullable: false),
                    MatchStats_Corners = table.Column<int>(type: "int", nullable: false),
                    MatchStats_BigChances = table.Column<int>(type: "int", nullable: false),
                    MatchStats_BigChancesMissed = table.Column<int>(type: "int", nullable: false),
                    MatchShots_TotalShots = table.Column<int>(type: "int", nullable: false),
                    MatchShots_ShotsOnTarget = table.Column<int>(type: "int", nullable: false),
                    MatchShots_ShotsOffTarget = table.Column<int>(type: "int", nullable: false),
                    MatchShots_BlockedShots = table.Column<int>(type: "int", nullable: false),
                    MatchShots_HitWoodwork = table.Column<int>(type: "int", nullable: false),
                    MatchShots_ShotsInsideBox = table.Column<int>(type: "int", nullable: false),
                    MatchShots_ShotsOutsideBox = table.Column<int>(type: "int", nullable: false),
                    MatchExpectedGoals_XG = table.Column<double>(type: "float", nullable: false),
                    MatchExpectedGoals_OpenPlayXG = table.Column<double>(type: "float", nullable: false),
                    MatchExpectedGoals_SetPlayXG = table.Column<double>(type: "float", nullable: false),
                    MatchExpectedGoals_NonPenaltyXG = table.Column<double>(type: "float", nullable: false),
                    MatchExpectedGoals_XGOT = table.Column<double>(type: "float", nullable: false),
                    MatchPasses_Passes = table.Column<int>(type: "int", nullable: false),
                    MatchPasses_AccuratePasses = table.Column<int>(type: "int", nullable: false),
                    MatchPasses_OwnHalf = table.Column<int>(type: "int", nullable: false),
                    MatchPasses_OppositionHalf = table.Column<int>(type: "int", nullable: false),
                    MatchPasses_AccurateLongBalls = table.Column<int>(type: "int", nullable: false),
                    MatchPasses_AccurateLongBallsPercentage = table.Column<int>(type: "int", nullable: false),
                    MatchPasses_AccurateCrosses = table.Column<int>(type: "int", nullable: false),
                    MatchPasses_AccurateCrossesPercentage = table.Column<int>(type: "int", nullable: false),
                    MatchPasses_Throws = table.Column<int>(type: "int", nullable: false),
                    MatchPasses_TouchesInOppositionBox = table.Column<int>(type: "int", nullable: false),
                    MatchPasses_Offsides = table.Column<int>(type: "int", nullable: false),
                    MatchDiscipline_FoulsCommitted = table.Column<int>(type: "int", nullable: false),
                    MatchDiscipline_YellowCards = table.Column<int>(type: "int", nullable: false),
                    MatchDiscipline_RedCards = table.Column<int>(type: "int", nullable: false),
                    MatchDefence_Tackles = table.Column<int>(type: "int", nullable: false),
                    MatchDefence_Interceptions = table.Column<int>(type: "int", nullable: false),
                    MatchDefence_Blocks = table.Column<int>(type: "int", nullable: false),
                    MatchDefence_Clearances = table.Column<int>(type: "int", nullable: false),
                    MatchDefence_KeeperSaves = table.Column<int>(type: "int", nullable: false),
                    MatchDuels_DuelsWon = table.Column<int>(type: "int", nullable: false),
                    MatchDuels_GroundDuelsWon = table.Column<int>(type: "int", nullable: false),
                    MatchDuels_GroundDuelsWonPercentage = table.Column<int>(type: "int", nullable: false),
                    MatchDuels_AerialDuelsWon = table.Column<int>(type: "int", nullable: false),
                    MatchDuels_AerialDuelsWonPercentage = table.Column<int>(type: "int", nullable: false),
                    MatchDuels_SuccessfulDribbles = table.Column<int>(type: "int", nullable: false),
                    MatchDuels_SuccessfulDribblesPercentage = table.Column<int>(type: "int", nullable: false),
                    MatchAttackingZones_CenterAttack = table.Column<int>(type: "int", nullable: false),
                    MatchAttackingZones_LeftAttack = table.Column<int>(type: "int", nullable: false),
                    MatchAttackingZones_RightAttack = table.Column<int>(type: "int", nullable: false),
                    MatchAnalysis_TacticalNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchAnalysis_AnalystNotes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMatchStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMatchStats_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamMatchStats_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionSeason_SeasonsId",
                table: "CompetitionSeason",
                column: "SeasonsId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AwayTeamId",
                table: "Matches",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_CompetitionId",
                table: "Matches",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamId",
                table: "Matches",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_SeasonId",
                table: "Matches",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMatchStats_MatchId",
                table: "PlayerMatchStats",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMatchStats_PlayerId",
                table: "PlayerMatchStats",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMatchStats_TeamId",
                table: "PlayerMatchStats",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMatchStats_MatchId",
                table: "TeamMatchStats",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMatchStats_TeamId",
                table: "TeamMatchStats",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CaptainId",
                table: "Teams",
                column: "CaptainId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Teams_AwayTeamId",
                table: "Matches",
                column: "AwayTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Teams_HomeTeamId",
                table: "Matches",
                column: "HomeTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerMatchStats_Players_PlayerId",
                table: "PlayerMatchStats",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerMatchStats_Teams_TeamId",
                table: "PlayerMatchStats",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Teams_TeamId",
                table: "Players",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Teams_TeamId",
                table: "Players");

            migrationBuilder.DropTable(
                name: "CompetitionSeason");

            migrationBuilder.DropTable(
                name: "PlayerMatchStats");

            migrationBuilder.DropTable(
                name: "TeamMatchStats");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
