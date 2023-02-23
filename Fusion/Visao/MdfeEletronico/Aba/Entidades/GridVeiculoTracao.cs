using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.MdfeEletronico;

namespace Fusion.Visao.MdfeEletronico.Aba.Entidades
{
    public class GridVeiculoTracao
    {
        public string CodigoInterno { get; set; }
        public string Renavam { get; set; }
        public string Placa { get; set; }
        public int Tara { get; set; }
        public int CapacidadeEmKg { get; set; }
        public int CapacidadeEmM3 { get; set; }
        public TipoPropriedadeVeiculo TipoPropriedadeVeiculo { get; set; }
        public TipoVeiculo TipoVeiculo { get; set; }
        public TipoRodado TipoRodado { get; set; }
        public TipoCarroceria TipoCarroceria { get; set; }
        public string SiglaUf { get; set; }
        public Veiculo Veiculo { get; set; }
        public MDFeVeiculoTracao MDFeVeiculoTracao { get; set; }
    }
}