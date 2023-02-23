using System;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Fiscal.NF.Autorizacao
{
    public class EmissaoDanfeDummy : IEmissaoXml
    {
        public string TagId => "NFe" + Chave.Chave;
        public string VersaoDocumento => "3.10";
        public ChaveSefaz Chave { get; }
        public int DigitoChave => Chave.Dv;
        public TipoEmissao TipoEmissao => TipoEmissao.Normal;
        public TipoAmbiente Ambiente => TipoAmbiente.Producao;
        public string MotivoContingencia => string.Empty;
        public DateTime? InicioContingencia => DateTime.Now;

        public EmissaoDanfeDummy(ChaveSefaz chave)
        {
            Chave = chave;
        }

        public bool ContingenciaAtivada() => false;
    }
}