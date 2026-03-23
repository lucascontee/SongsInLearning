using FluentValidation;
using SongsInLearning.Models;


namespace SongsInLearning.Validators;
public class MusicValidator : AbstractValidator<Music>
{

    public MusicValidator()
    {
        RuleFor(m => m.Name)
            .NotEmpty()
            .WithMessage("Song name is required")
            .MaximumLength(60)
            .WithMessage("Song name cannot exceed 60 characters."); ;

        RuleFor(m => m.Artist)
            .NotEmpty()
            .WithMessage("Artist is required")
            .MaximumLength(60)
            .WithMessage("Artist cannot exceed 60 characters."); ;

        RuleFor(m => m.Year)
            .InclusiveBetween(1, 4999)
            .WithMessage("Year must be between 1 and 4999.");

        RuleFor(m => m.InfosGeneratedByIA)
            .MaximumLength(4000)
            .WithMessage("Infos cannot exceed 4000 characters.");

        RuleFor(m => m.UserAnnotations)
            .MaximumLength(1000)
            .WithMessage("Annotations cannot exceed 1000 characters.");

        RuleFor(m => m.Bpm)
            .GreaterThan(0)
            .WithMessage("BPM must be greater than zero.");

        RuleFor(m => m.Difficulty)
            .IsInEnum()
            .WithMessage("Invalid difficulty value.");

        RuleFor(m => m.Tuning)
            .IsInEnum()
            .WithMessage("Invalid tuning value.");

        RuleFor(m => m.Instrument)
            .IsInEnum()
            .WithMessage("Invalid instrument value.");

    }

}
