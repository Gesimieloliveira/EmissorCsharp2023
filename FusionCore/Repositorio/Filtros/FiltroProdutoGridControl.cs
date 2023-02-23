using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.Repositorio.Filtros
{
    public class FiltroProdutoGridControl
    {
        public int? CodigoIdIgualA { get; set; }
        public string NomeProdutoContenha { get; set; }
        public string CodigoBarrasIgualA { get; set; }
        public string ReferenciaContenha { get; set; }
        public bool Ativos { get; set; } = true;
        public ProdutoGrupoDTO Grupo { get; set; }

        public bool ContemCodigoId()
        {
            return CodigoIdIgualA != null;
        }

        public bool ContemCodigoBarras()
        {
            return CodigoBarrasIgualA.IsNotNullOrEmpty();
        }

        public bool ContemNome()
        {
            return NomeProdutoContenha.IsNotNullOrEmpty();
        }

        public bool ContemReferencia()
        {
            return ReferenciaContenha.IsNotNullOrEmpty();
        }

        public bool ContemGrupo()
        {
            return Grupo != null;
        }
    }
}