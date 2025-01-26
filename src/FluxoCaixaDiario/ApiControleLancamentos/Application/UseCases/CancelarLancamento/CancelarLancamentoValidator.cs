namespace ApiControleLancamentos.Application.UseCases.CancelarLancamento
{
    using FluentValidation;

    public class CancelarLancamentoValidator : AbstractValidator<CancelarLancamentoCommand>
    {
        public CancelarLancamentoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("O ID deve ser maior que zero.");
        }
    }

}
