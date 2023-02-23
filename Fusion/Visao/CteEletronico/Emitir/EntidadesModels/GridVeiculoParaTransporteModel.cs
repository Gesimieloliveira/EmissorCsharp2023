using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.CteEletronico.Emitir.EntidadesModels
{
    public class GridVeiculoParaTransporteModel : ViewModel
    {
        public string Chassi { get; set; }
        public string Cor { get; set; }
        public string DescricaoCor { get; set; }
        public string CodigoMarcaModelo { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal FreteUnitario { get; set; }
        public CteVeiculoTransportado VeiculoTransportado { get; set; }

        public static GridVeiculoParaTransporteModel Cria(CteVeiculoTransportado veiculoTransportado)
        {
            var veiculo = new GridVeiculoParaTransporteModel
            {
                Chassi = veiculoTransportado.Chassi,
                Cor = veiculoTransportado.Cor,
                DescricaoCor = veiculoTransportado.DescricaoCor,
                CodigoMarcaModelo = veiculoTransportado.CodigoMarcaModelo,
                ValorUnitario = veiculoTransportado.ValorUnitario,
                FreteUnitario = veiculoTransportado.FreteUnitario,
                VeiculoTransportado = veiculoTransportado
            };

            return veiculo;
        }
    }
}