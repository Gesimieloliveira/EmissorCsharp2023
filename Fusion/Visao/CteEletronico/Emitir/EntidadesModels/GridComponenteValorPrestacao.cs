using FusionCore.FusionAdm.CteEletronico.Emissao;

namespace Fusion.Visao.CteEletronico.Emitir.EntidadesModels
{
    public class GridComponenteValorPrestacao
    {
        public CteComponenteValorPrestacao Componente { get; set; }


        public static GridComponenteValorPrestacao Cria(CteComponenteValorPrestacao componenteValorPrestacao)
        {
            return new GridComponenteValorPrestacao
            {
                Componente = componenteValorPrestacao
            };
        }
    }
}