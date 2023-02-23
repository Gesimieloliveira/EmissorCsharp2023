using FusionCore.Repositorio.Base;

namespace FusionCore.FusionNfce.Uf
{
    public class UfNfce : Entidade
    {
        // ReSharper disable once EmptyConstructor
        public UfNfce() { }

        public byte Id { get; set; }
        public string Sigla { get; set; }
        public string Nome { get; set; }
        public byte CodigoIbge { get; set; }
        protected override int ReferenciaUnica => Id;
    }
}