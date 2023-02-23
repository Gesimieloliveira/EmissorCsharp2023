using FusionCore.Repositorio.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable MemberCanBePrivate.Global

namespace FusionCore.FusionServico
{
    public class ConfiguracaoExportacao : Entidade
    {
        public byte Id { get; private set; } = 1;
        public bool ExportacaoAtiva { get; set; }
        public string DiretorioExportacao { get; set; }

        protected override int ReferenciaUnica => Id;
    }
}