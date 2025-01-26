namespace ApiControleLancamentos.Application.UseCases.AtualizarLancamento
{
    using FluentValidation;

    public class AtualizarLancamentoValidator : AbstractValidator<AtualizarLancamentoCommand>
    {
        public AtualizarLancamentoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("O ID deve ser maior que zero.");

            RuleFor(x => x.Valor)
                .GreaterThan(0)
                .WithMessage("O valor deve ser positivo.");

            RuleFor(x => x.Descricao)
                .NotEmpty()
                .WithMessage("A descrição não pode ser vazia.");
        }
    }

}
