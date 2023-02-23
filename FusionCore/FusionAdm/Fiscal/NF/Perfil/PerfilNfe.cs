using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.FusionAdm.Fiscal.NF.Perfil
{
    public class PerfilNfe : Entidade
    {
        public PerfilNfe()
        {
            Ativo = true;
            Observacao = string.Empty;
            TipoOperacao = TipoOperacao.Saida;
            FinalidadeEmissao = FinalidadeEmissao.Normal;
            SimplesNacional = new PerfilNfeSimplesNacional();
        }

        protected override int ReferenciaUnica => Id;
        public short Id { get; private set; }
        public EmissorFiscal EmissorFiscal { get; set; }
        public bool Ativo { get; set; }
        public string Descricao { get; set; }
        public TipoOperacao TipoOperacao { get; set; }
        public FinalidadeEmissao FinalidadeEmissao { get; set; }
        public string NaturezaOperacao { get; set; }
        public string Observacao { get; set; }
        public bool AutoAtivarPartilhaIcms { get; set; }
        public PerfilNfeDestinatario Destinatario { get; set; }
        public PerfilNfeTransportadora Transportadora { get; set; }
        public PerfilNfeSimplesNacional SimplesNacional { get; set; }
        public bool MovimentarEstoqueProduto { get; set; } = true;
        public PerfilCfopDTO Cfop { get; set; }
        public bool UsarIpiTagPropria { get; set; }
        public bool DesativarInfoCreditoItem { get; set; }
    }
}