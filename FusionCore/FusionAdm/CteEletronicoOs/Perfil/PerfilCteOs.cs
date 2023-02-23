using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using TipoServico = FusionCore.FusionAdm.CteEletronicoOs.Flags.TipoServico;

namespace FusionCore.FusionAdm.CteEletronicoOs.Perfil
{
    public class PerfilCteOs : Entidade
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public EmissorFiscal EmissorFiscal { get; set; }
        public PerfilCfopDTO PerfilCfop { get; set; }
        public Veiculo Veiculo { get; set; }
        public string NaturezaOperacao { get; set; }
        public TipoCte TipoCte { get; set; }
        public TipoServico TipoServico { get; set; }
        public string Observacao { get; set; }
        public PerfilCteOsSeguro Seguro { get; set; }
        public PessoaEntidade Tomador { get; set; }
        public LocalInicialPrestacao LocalInicialPrestacao { get; set; }
        public LocalFinalPrestacao LocalFinalPrestacao { get; set; }
        public string Taf { get; set; }
        public string NumeroRegistroEstadual { get; set; }
        public Ibpt Ibpt { get; set; }

        protected override int ReferenciaUnica => Id;
        public string DescricaoServico { get; set; }
        public decimal QuantidadePassageiroVolume { get; set; }
    }
}