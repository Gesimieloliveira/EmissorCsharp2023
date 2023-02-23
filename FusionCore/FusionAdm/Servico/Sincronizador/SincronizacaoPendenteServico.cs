using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.FusionAdm.Servico.Sincronizador
{
    public class SincronizacaoPendenteServico
    {
        private readonly ISession _sessao;
        private readonly EntidadeSincronizavel _entidadeSincronizavel;
        private readonly string _referencia;

        public SincronizacaoPendenteServico(ISession sessao, EntidadeSincronizavel entidadeSincronizavel, string referencia)
        {
            _sessao = sessao;
            _entidadeSincronizavel = entidadeSincronizavel;
            _referencia = referencia;
        }

        public void Salvar()
        {
            var todosTerminalOffline = new RepositorioTerminalOffline(_sessao).TodosTerminaisSomenteComId();
            var sincroniacaoRepositorio = new RepositorioSincronizacaoPendente(_sessao);

            todosTerminalOffline.ForEach(id =>
            {
                var sincronizacaoPendente = new SincronizacaoPendente
                {
                    EntidadeSincronizavel = _entidadeSincronizavel,
                    Referencia = _referencia,
                    TerminalOfflineId = id
                };

                sincroniacaoRepositorio.Salvar(sincronizacaoPendente);
                
            });
        } 
    }
}