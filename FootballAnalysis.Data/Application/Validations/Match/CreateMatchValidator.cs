using FluentValidation;
using FootballAnalysis.Data.Application.DTOs.Match;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Validations.Match
{
    public class CreateMatchValidator : AbstractValidator<CreateMatchDTO>
    {
        public CreateMatchValidator()
        {
            RuleFor(m => m.KickOff)
                .NotEmpty().WithMessage("Kick-off date and time is required.");
            RuleFor(m => m.Venue)
                .NotEmpty().WithMessage("Venue is required.");
            RuleFor(m => m.Referee)
                .NotEmpty().WithMessage("Referee is required.");
            RuleFor(m => m.CompetitionId)
                .NotEmpty().WithMessage("Competition ID is required.");
            RuleFor(m => m.SeasonId)
                .NotEmpty().WithMessage("Season ID is required.");


            RuleFor(m => m.HomeTeamId)
                .NotEmpty().WithMessage("Home team ID is required.");
            RuleFor(m => m.AwayTeamId)
                .NotEmpty().WithMessage("Away team ID is required.");
        }
    }
}
