using FusionCore.Helpers.Basico;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;
 
namespace FusionCore.FusionAdm.Servico.Estoque
{
    public class EstoqueModel
    {
        public EstoqueModel()
        {
        }

        public EstoqueModel(
            ProdutoDTO produto,
            decimal quantidade,
            UsuarioDTO usuario,
            OrigemEventoEstoque origemEvento,
            bool inverso = false
        )
        {
            Produto = produto;
            Quantidade = quantidade;
            Usuario = usuario;
            OrigemEvento = origemEvento;
            Inverso = inverso;
        }

        public ProdutoDTO Produto { get; set; }
        public decimal Quantidade { get; set; }
        public decimal QuantidadeReservaEstoque { get; set; }
        public OrigemEventoEstoque OrigemEvento { get; set; }
        public UsuarioDTO Usuario { get; set; }
        public bool Inverso { get; set; }
        public bool IsReservaEstoque => QuantidadeReservaEstoque != 0;
        public decimal EstoqueMinimo { get; set; }
        public decimal EstoqueMaximo { get; set; }

        public string OrigemEventoToString()
        {
            return OrigemEvento.GetDescription();
        }

        public void MarcaComoInverso()
        {
            Inverso = true;
        }
    }
}