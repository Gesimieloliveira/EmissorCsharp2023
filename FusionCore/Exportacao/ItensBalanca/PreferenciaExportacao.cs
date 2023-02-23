using FusionCore.Repositorio.Base;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.Exportacao.ItensBalanca
{
    public class PreferenciaExportacao : Entidade
    {
        private PreferenciaExportacao()
        {
            //nhibernate
        }

        public PreferenciaExportacao(string identificador, string localExportacao, string tag) : this()
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