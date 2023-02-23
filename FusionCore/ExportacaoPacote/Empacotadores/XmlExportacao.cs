using FusionCore.FusionAdm.Fiscal.NF.Cancelar;
using FusionCore.Vendas.Autorizadores.Nfce;

// ReSharper disable ClassNeverInstantiated.Global

namespace FusionCore.ExportacaoPacote.Empacotadores
{
    public class XmlExportacao : IEnvelope
    {
        private int _statusRetorno;
        private SituacaoFiscal _cupomSituacaoFiscal;
        public string Xml { get; set; }
        public string Chave { get; set; }
        public StatusCancelamento Status { get; set; }
        public string Grupo => Status?.EstaCancelado == true ? "Cancelados" : "Autorizados";
        public string Nome => $"{Chave}.xml";
        public string Conteudo
        {
            get => Xml;
            set => Xml = value;
        }

        public int StatusRetorno
        {
            get => _statusRetorno;
            set
            {
                _statusRetorno = value;
                Status = new StatusCancelamento(value, "");
            }
        }

        public string XmlCancelamento { get; set; }

        public SituacaoFiscal CupomSituacaoFiscal
        {
            get => _cupomSituacaoFiscal;
            set
            {
                _cupomSituacaoFiscal = value;

                var codigoSituacao = 100;

                if (_cupomSituacaoFiscal == SituacaoFiscal.Cancelado)
                {
                    codigoSituacao = 135;
                }

                Status = new StatusCancelamento(codigoSituacao, string.Empty);
            } 
        }
    }
}