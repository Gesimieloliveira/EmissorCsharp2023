using System.IO;
using DFe.Utils;
using FusionCore.FusionNfce.ImpressaoDireta;
using FusionCore.Helpers.Ambiente;

namespace FusionNfce.Visao.Configuracao.Impressao
{
    public class SalvarImpressaoDireta
    {
        private ImpressaoDiretaAtiva _impressaoDiretaAtiva;
        private readonly string _arquivo;
        private const string NomeArquivo = "ImpressaoDiretaAtiva.xml";

        public SalvarImpressaoDireta(ImpressaoDiretaAtiva impressaoDiretaAtiva)
        {
            _impressaoDiretaAtiva = impressaoDiretaAtiva;
            _arquivo = Path.Combine(DiretorioAssembly.GetPastaConfig(), NomeArquivo);
        }

        public SalvarImpressaoDireta()
        {
            _arquivo = Path.Combine(DiretorioAssembly.GetPastaConfig(), "ImpressaoDiretaAtiva.xml");
        }

        public ImpressaoDiretaAtiva Ler()
        {
            VerificaSeExiste();

            return FuncoesXml.ArquivoXmlParaClasse<ImpressaoDiretaAtiva>(_arquivo);
        }

        public void Salvar()
        {
            FuncoesXml.ClasseParaArquivoXml(_impressaoDiretaAtiva, _arquivo);
        }

        private void VerificaSeExiste()
        {
            if (new ManipulaArquivo(_arquivo).Existe()) return;

            CriaImpressaoDiretaAtivaDefault();
            Salvar();
        }

        private void CriaImpressaoDiretaAtivaDefault()
        {
            var impressaoDireta = ImpressaoDiretaAtiva.Default;
            _impressaoDiretaAtiva = impressaoDireta;
        }
    }
}