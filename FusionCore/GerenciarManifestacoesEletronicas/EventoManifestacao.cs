using FusionCore.Repositorio.Base;

namespace FusionCore.GerenciarManifestacoesEletronicas
{
    public class EventoManifestacao : EntidadeBase<int>
    {
        public int Id { get; set; }
        public NfeResumida NfeResumida { get; set; }
        public string Xml { get; set; }
        public StatusManifestacao Evento { get; set; } = StatusManifestacao.Desconhecida;
        protected override int ChaveUnica => Id;
    }
}