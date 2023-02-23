using FusionCore.Repositorio.Base;

namespace FusionCore.Exportacao.ItensBuscaRapida
{
    public class PreferenciaBuscaRapida : Entidade
    {
        private PreferenciaBuscaRapida()
        {
            //nhibernate
        }

        public PreferenciaBuscaRapida(string identificador, string localExportacao, string tag) : this()
        {
            Identificador = identificador;
            LocalExportacao = localExportacao;
            Tag = tag;
        }

        protected override int ReferenciaUnica => Id;
        public short Id { get; private set; }
        public string Identificador { get; private set; }
        public string Tag { get; private set; }
        public string LocalExportacao { get; private set; }
    }
}