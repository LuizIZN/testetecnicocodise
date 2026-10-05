using FluentValidation;
using TesteTecnico.Application.Dtos.Anime;
using TesteTecnico.Application.Dtos.Diretor;

namespace TesteTecnico.Application.Validators;

public sealed class QueryAnimeValidator : AbstractValidator<QueryAnimeParameters>
{
    public QueryAnimeValidator()
    {
        RuleFor(q => q.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("O número da página deve ser maior ou igual a 1.");

        RuleFor(q => q.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("O tamanho da página deve estar entre 1 e 100.");

        RuleFor(q => q.Nome)
            .MaximumLength(200)
            .WithMessage("O nome do anime deve possuir até 200 caracteres.");

        RuleFor(q => q.AnoLancamentoMin)
            .GreaterThan(1900).When(q => q.AnoLancamentoMin.HasValue)
            .WithMessage("O ano inicial deve ser posterior a 1900.");

        RuleFor(q => q.AnoLancamentoMax)
            .LessThanOrEqualTo(DateTime.Now.Year).When(q => q.AnoLancamentoMax.HasValue)
            .WithMessage("O ano final não pode ser maior que o ano atual.")
            .GreaterThanOrEqualTo(q => q.AnoLancamentoMin)
            .When(q => q.AnoLancamentoMin.HasValue && q.AnoLancamentoMax.HasValue)
            .WithMessage("O ano final deve ser maior ou igual ao ano inicial.");

        RuleFor(q => q.NumeroEpisodiosMin)
            .GreaterThanOrEqualTo(1).When(q => q.NumeroEpisodiosMin.HasValue)
            .WithMessage("O número mínimo de episódios deve ser maior ou igual a 1.");

        RuleFor(q => q.NumeroEpisodiosMax)
            .GreaterThanOrEqualTo(1).When(q => q.NumeroEpisodiosMax.HasValue)
            .WithMessage("O número máximo de episódios deve ser maior ou igual a 1.")
            .GreaterThanOrEqualTo(q => q.NumeroEpisodiosMin)
            .When(q => q.NumeroEpisodiosMin.HasValue && q.NumeroEpisodiosMax.HasValue)
            .WithMessage("O número máximo de episódios deve ser maior ou igual ao mínimo.");
    }
}

public sealed class QueryDiretorValidator : AbstractValidator<QueryDiretorParams>
{
    public QueryDiretorValidator()
    {
        RuleFor(q => q.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("O número da página deve ser maior ou igual a 1.");

        RuleFor(q => q.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("O tamanho da página deve estar entre 1 e 100.");

        RuleFor(q => q.Nome)
            .MaximumLength(200)
            .WithMessage("O nome do diretor deve possuir até 200 caracteres.");
    }
}