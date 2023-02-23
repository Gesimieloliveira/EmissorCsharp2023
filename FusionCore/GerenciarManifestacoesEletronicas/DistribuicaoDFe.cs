using System;
using System.Collections.Generic;
using DFe.Utils;
using FusionCore.FusionAdm.Fiscal.Flags;
using NFe.Classes.Servicos.DistribuicaoDFe;

namespace FusionCore.GerenciarManifestacoesEletronicas
{
    public class DistribuicaoDFe
    {
        private readonly IList<ItemDistribuicaoDFe> _itens = new List<ItemDistribuicaoDFe>();

        public DistribuicaoDFe()
        {
            DataCriacao = DateTime.Now;
        }

        public int Id { get; set; }
        public string DocumentoUnicoInteressado { get; set; }
        public string UltimoNsuPesquisado { get; set; }
        public string MaiorNsu { get; set; }
        public string Xml { get; set; }
        public TipoAmbiente AmbienteSefaz { get; set; }
        public DateTime DataCriacao { get; private set; }

        public void AddItem(loteDistDFeInt docZip)
        {
            var xmlDescompactado = Compressao.Unzip(docZip.XmlNfe);

            var item = new ItemDistribuicaoDFe
            {
                Distribuicao = this,
                XmlDescompactado = xmlDescompactado,
                Nsu = docZip.NSU,
                NomeSchema = docZip.schema,
                TipoEvento = (int?) docZip.ProcEventoNFe?.evento.infEvento?.tpEvento,
                TipoDfe = TipoDfeHelper.IdentificarTipo(xmlDescompactado)
            };

            _itens.Add(item);
        }
    }
}