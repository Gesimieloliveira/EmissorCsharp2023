using System.Collections.Generic;
using System.Linq;

namespace Fusion.FastReport.Relatorios.Sistema
{
    public class LinhaTef
    {
        public LinhaTef(string conteudo)
        {
            Conteudo = conteudo;
            ConteudoSemAspas = conteudo?.Replace("\"", string.Empty) ?? string.Empty;
        }

        public string Conteudo { get; }
        public string ConteudoSemAspas { get; }
    }

    public class RImpressaoTef : RelatorioBase
    {
        private IList<string> _impressao;

        public RImpressaoTef() : base(null)
        {
        }

        public void ComImpressao(IList<string> impressao)
        {
            _impressao = impressao;
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemBytesFrx("FrImprimirTef.frx");
        }

        protected override void PrepararDados()
        {
            AtivarImpressaoModoSplit();

            var listaLinha = _impressao.Select(conteudo => new LinhaTef(conteudo)).ToList();

            RegistraDados("ImagemTef", listaLinha);
        }
    }
}