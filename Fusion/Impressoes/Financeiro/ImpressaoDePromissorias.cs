using Fusion.FastReport.Relatorios.Sistema.Financeiro;
using FusionCore.Sessao;

namespace Fusion.Impressoes.Financeiro
{
    public class ImpressaoDePromissorias : IOpcaoImpressao
    {
        private readonly ISessaoManager _sessaoManager;

        public ImpressaoDePromissorias(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public bool PreVisualizar { get; set; }
        public string Impresora { get; set; }

        public override string ToString()
        {
            return "Impressão de promissórias";
        }

        public void FazerImpressao(int maloteId)
        {
            using (var relatorio = new RPromissoria(_sessaoManager))
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