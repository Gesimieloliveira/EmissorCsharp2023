using FusionCore.Repositorio.Base;
using static System.Globalization.CompareOptions;
using static System.Globalization.CultureInfo;

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public class CidadeDTO : Entidade
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual int CodigoIbge { get; set; }
        public virtual string SiglaUf { get; set; }
        protected override int ReferenciaUnica => Id;

        public override string ToString()
        {
            return $"{Nome} - {SiglaUf}";
        }

        public bool Compare(string nome, string uf)
        {
            return CompareNome(nome) && CompareSiglaUf(uf);
        }

        public bool CompareNome(string nome)
        {
            return string.Compare(nome, Nome, InvariantCulture, IgnoreNonSpace) == 0;
        }

        public bool CompareSiglaUf(string uf)
        {
            return string.Compare(uf, SiglaUf, InvariantCulture, IgnoreNonSpace) == 0;
        }
    }
}