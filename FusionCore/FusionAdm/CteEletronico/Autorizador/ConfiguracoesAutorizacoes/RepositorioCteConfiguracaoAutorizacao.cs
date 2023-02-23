using System;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.FusionAdm.CteEletronico.Autorizador.ConfiguracoesAutorizacoes
{
    public class RepositorioCteConfiguracaoAutorizacao : Repositorio<CteConfiguracaoAutorizacao, Guid>
    {
        public RepositorioCteConfiguracaoAutorizacao(ISession sessao) : base(sessao)
        {
        }

        public int TempoConsultaReciboMilesegundos()
        {
            var tempoMilesegundos = Sessao.QueryOver<CteConfiguracaoAutorizacao>()
                .Select(x => x.TempoEsperaConsultaRecibo).FutureValue<int>();

            return tempoMilesegundos.Value;
        }
    }
}