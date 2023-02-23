using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionNfce.Cidade
{
    public class CidadeNfce : Entidade
    {
        public CidadeNfce() { }

        public CidadeNfce(CidadeDTO cidade)
        {
            CopiaInformacoes(cidade);
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public int CodigoIbge { get; set; }
        public string SiglaUf { get; set; }
        protected override int ReferenciaUnica => Id;

        private void CopiaInformacoes(CidadeDTO cidade)
        {
            Id = cidade.Id;
            Nome = cidade.Nome;
            CodigoIbge = cidade.CodigoIbge;
            SiglaUf = cidade.SiglaUf;
        }
    }
}