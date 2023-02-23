using FusionCore.Repositorio.Base;

namespace FusionCore.Tributacoes.Federal
{
    public class EquadramentoIpi : Entidade
    {
        public string Id { get; set; }
        protected override int ReferenciaUnica => Id.GetHashCode();
        public string GrupoCst { get; set; }
        public string Descricao { get; set; }

        public string DescricaoCompleta => GetDescricaoCompleta();

        private string GetDescricaoCompleta()
        {
            return $"{Id} - {GrupoCst} - {Descricao}";
        }

        public override string ToString()
        {
            return $"{Id} - {Descricao}";
        }
    }
}