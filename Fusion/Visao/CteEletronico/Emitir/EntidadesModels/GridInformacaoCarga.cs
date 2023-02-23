using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.CteEletronico.Emitir.EntidadesModels
{
    public class GridInformacaoCarga : ViewModel
    {
        public UnidadeMedida UnidadeMedida { get; set; }
        public string TipoMedida { get; set; }
        public decimal Quantidade { get; set; }
        public CteInfoQuantidadeCarga InfoQuantidadeCarga { get; set; }

        public static GridInformacaoCarga Cria(CteInfoQuantidadeCarga infoQuantidadeCarga)
        {
            var carga = new GridInformacaoCarga
            {
                Quantidade = infoQuantidadeCarga.Quantidade,
                UnidadeMedida = infoQuantidadeCarga.UnidadeMedida,
                TipoMedida = infoQuantidadeCarga.TipoUnidadeMedidaDescricao,
                InfoQuantidadeCarga = infoQuantidadeCarga
            };

            return carga;
        }
    }
}