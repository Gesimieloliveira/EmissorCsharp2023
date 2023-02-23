using Fusion.FastReport.Relatorios.Sistema.Financeiro;
using FusionCore.Sessao;

namespace Fusion.Impressoes.Financeiro
{
    public class ImpressaoPromissoriaComCarne : IOpcaoImpressao
    {
        private readonly ISessaoManager _sessaoManager;

        public ImpressaoPromissoriaComCarne(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public bool PreVisualizar { get; set; }
        public string Impresora { get; set; }

        public override string ToString()
        {
            return "Impressão de promissóaria com carnês";
        }

        public void FazerImpressao(int maloteId)
        {
            using (var relatorio = new RPromissoriaCarne(_sessaoManager))
            {
                relatorio.ComMaloteId(maloteId);

                if (PreVisualizar)
                {
                    relatorio.Visualizar();
                    return;
                }

                relatorio.Imprimir(Impresora);
            }
        }
    }
}