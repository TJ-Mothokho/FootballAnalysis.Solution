using FluentValidation;
using FootballAnalysis.Data.Application.DTOs.Competition;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Validations.Competition
{
    public class CreateCompetitionValidator : AbstractValidator<CreateCompetitionDTO>
    {
        public CreateCompetitionValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Competition name is required.")
                .MaximumLength(100).WithMessage("Competition name must not exceed 100 characters.");

            RuleFor(c => c.Country)
                .NotEmpty().WithMessage("Country is required.");
        }
    }
}
