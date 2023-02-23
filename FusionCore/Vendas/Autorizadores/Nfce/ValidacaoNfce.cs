using System;
using System.Text;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Vendas.Autorizadores.Infra;

namespace FusionCore.Vendas.Autorizadores.Nfce
{
    public class ValidacaoNfce : IValidacaoAntesAbrirFaturamento
    {
        public void Valiar()
        {
            var mensagensDeErros = new StringBuilder();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

                if (repositorioCupomFiscal.PossuiCupomEmitidoEmContingencia())
                {
                    mensagensDeErros.Append("Você possui cupons emitidos em contingência.");
                }

                if (repositorioCupomFiscal.PossuiCupomNaoAutorizados())
                {
                    mensagensDeErros.Append("\nVocê possui cupons pendentes, não autorizados.");
                }
            }

            if (mensagensDeErros.Length != 0)
                throw new InvalidOperationException(mensagensDeErros.ToString());
        }
    }
}