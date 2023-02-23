using System.Collections.Generic;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFeVeiculoTracao : EntidadeBase<int>
    {
        public MDFeVeiculoTracao()
        {
            Condutores = new List<MDFeCondutor>();
        }

        public int RodoviarioId { get; set; }
        protected override int ChaveUnica => RodoviarioId;
        public MDFeRodoviario Rodoviario { get; set; }
        public Veiculo Veiculo { get; set; }
        public IList<MDFeCondutor> Condutores { get; set; }
    }
}