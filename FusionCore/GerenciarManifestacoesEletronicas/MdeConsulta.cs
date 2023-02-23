using System;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.Base;
using NFe.Servicos.Retorno;

namespace FusionCore.GerenciarManifestacoesEletronicas
{
    public class MdeConsulta : EntidadeBase<Guid>
    {
        private MdeConsulta()
        {
            // nhibernate
        }

        public MdeConsulta(string documentoUnico, TipoAmbiente ambiente, string uf, long nsuPesquisado) : this()
        {
            Id = Guid.NewGuid();
            DocumentoUnico = documentoUnico;
            DataCadastro = DateTime.Now;
            NsuPesquisado = nsuPesquisado;
            AmbienteSefaz = ambiente;
            Uf = uf;
        }

        protected override Guid ChaveUnica => Id;
        public Guid Id { get; private set; }
        public string DocumentoUnico { get; private set; }
        public TipoAmbiente AmbienteSefaz { get; private set; }
        public string Uf { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataResposta { get; private set; }
        public long NsuPesquisado { get; private set; }
        public long UltimoNsu { get; private set; }
        public long MaiorNsu { get; private set; }
        public int CodigoStatus { get; private set; }
        public string MotivoStatus { get; private set; }
        public string XmlResposta { get; private set; }

        public string MotivoStatusCompleto => $"{CodigoStatus} - {MotivoStatus}";

        public void AddResposta(RetornoNfeDistDFeInt retorno)
        {
            DataResposta = retorno.Retorno.dhResp;
            UltimoNsu = retorno.Retorno.ultNSU;
            MaiorNsu = retorno.Retorno.maxNSU;
            CodigoStatus = retorno.Retorno.cStat;
            MotivoStatus = retorno.Retorno.xMotivo;
            XmlResposta = retorno.RetornoCompletoStr;
        }
    }
}