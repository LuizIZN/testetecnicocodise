using FluentValidation;
using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Diretor;

namespace TesteTecnico.Application.Validators;

public class CreateDiretorValidator : AbstractValidator<CreateDiretorRequest>
{
	public CreateDiretorValidator()
	{
		RuleFor(d => d.Nome)
			.NotEmpty().WithMessage("O nome do diretor é obrigatório.")
			.MaximumLength(200).WithMessage("O nome do diretor deve possuir até 200 caracteres.");

		RuleFor(d => d.DataNascimento)
			.NotEmpty().WithMessage("A data de nascimento do diretor é obrigatória.")
			.LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
			.WithMessage("A data de nascimento não pode ser maior que a data atual.");
	}
}

public class UpdateDiretorValidator : AbstractValidator<UpdateDiretorRequest>
{
	public UpdateDiretorValidator()
	{
		RuleFor(d => d.Nome)
			.NotEmpty().When(d => d.Nome is not null)
			.WithMessage("O nome do diretor não pode ser vazio.")
			.MaximumLength(200).WithMessage("O nome do diretor deve possuir até 200 caracteres.");

		RuleFor(d => d.DataNascimento)
			.LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).When(d => d.DataNascimento.HasValue)
			.WithMessage("A data de nascimento não pode ser maior que a data atual.");
	}
}
