using FusionCore.Repositorio.Base;

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public class ProdutoGrupoDTO : Entidade
    {
        public int Id { get; set; }
        protected override int ReferenciaUnica => Id;
        public string Nome { get; set; }

        public override string ToString()
        {
            return Nome;
        }
    }
}