using FluentValidation;
using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Anime;

namespace TesteTecnico.Application.Validators
{
    public class CreateAnimeValidator : AbstractValidator<CreateAnimeRequest>
    {
        public CreateAnimeValidator()
        {
            RuleFor(a => a.Nome)
                .NotEmpty().WithMessage("O nome do anime é obrigatório.")
                .MaximumLength(200).WithMessage("O nome do anime deve possuir até 200 caracteres.");
            
            RuleFor(a => a.Descricao)
                .NotEmpty().WithMessage("A descrição do anime é obrigatória.")
                .MaximumLength(500).WithMessage("A descrição do anime deve possuir até 500 caracteres.");

            RuleFor(a => a.AnoLancamento)
                .NotNull().WithMessage("O ano de lançamento do anime é obrigatório.")
                .GreaterThan(1900).WithMessage("O ano de lançamento do anime deve ser posterior a 1900.")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("A ano de lançamento do anime deve ser o mesmo ou anterior ao ano atual.");

            RuleFor(a => a.NumeroEpisodios)
                .NotNull().WithMessage("O número de episódios do anime é obrigatório.")
                .GreaterThanOrEqualTo(1).WithMessage("O número de episódios do anime deve ser maior ou igual a 1.");
            
            RuleFor(a => a.DiretorId)
                .NotEmpty().WithMessage("O ID do diretor do anime é obrigatório.");
        }
    }

    public class UpdateAnimeValidator : AbstractValidator<UpdateAnimeRequest>
    {
        public UpdateAnimeValidator()
        {
            RuleFor(a => a.Nome)
                .NotEmpty().When(a => a.Nome is not null)
                .WithMessage("O nome do anime não pode ser vazio.")
                .MaximumLength(200).WithMessage("O nome do anime deve possuir até 200 caracteres.");

            RuleFor(a => a.Descricao)
                .NotEmpty().When(a => a.Descricao is not null)
                .WithMessage("A descrição do anime não pode ser vazia.")
                .MaximumLength(500).WithMessage("A descrição do anime deve possuir até 500 caracteres.");

            RuleFor(a => a.AnoLancamento)
                .GreaterThan(1900).When(a => a.AnoLancamento.HasValue)
                .WithMessage("O ano de lançamento do anime deve ser posterior a 1900.")
                .LessThanOrEqualTo(DateTime.Now.Year).When(a => a.AnoLancamento.HasValue)
                .WithMessage("O ano de lançamento do anime deve ser o mesmo ou anterior ao ano atual.");

            RuleFor(a => a.NumeroEpisodios)
                .GreaterThanOrEqualTo(1).When(a => a.NumeroEpisodios.HasValue)
                .WithMessage("O número de episódios do anime deve ser maior ou igual a 1.");

            RuleFor(a => a.DiretorId)
                .NotEmpty().When(a => a.DiretorId.HasValue)
                .WithMessage("O ID do diretor do anime não pode ser vazio.");
        }
    }
}