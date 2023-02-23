using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionCore.Repositorio.Legacy.Flags;

namespace FusionCore.FusionPdv.Servico.Estoque
{
    public class EstoqueModel
    {
        public ProdutoDt Produto { get; set; }
        public decimal Quantidade { get; set; }
        public OrigemEventoEstoque OrigemEvento { get; set; }
        public UsuarioPdvDt Usuario { get; set; }
    }
}