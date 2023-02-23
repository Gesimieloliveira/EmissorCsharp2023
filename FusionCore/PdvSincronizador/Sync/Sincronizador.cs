using System;
using FusionCore.FusionPdv.Sessao;
using FusionCore.PdvSincronizador.Sync.Estrategia;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate;

namespace FusionCore.PdvSincronizador.Sync
{
    public class Sincronizador
    {
        private static Sincronizador _instancia;
        private readonly SessaoPdv _sessaoPdv;
        private readonly SessaoAdm _sessaoAdm;

        public static Sincronizador Instancia => _instancia ?? (_instancia = new Sincronizador());

        private Sincronizador()
        {
            _sessaoPdv = new SessaoPdv();
            _sessaoAdm = new SessaoAdm();
        }

        public void SincronizarTudo()
        {
            try
            {
                RealizarSincronizacao(new EnviarVendasEcf());
                RealizarSincronizacao(new EnviarEstoque());
                RealizarSincronizacao(new ReceberUsuario());
                RealizarSincronizacao(new ReceberConfiguracaoEstoque());
                RealizarSincronizacao(new ReceberEmpresa());
                RealizarSincronizacao(new ReceberProduto());
                RealizarSincronizacao(new ReceberCliente());
                RealizarSincronizacao(new ReceberEcf());
                RealizarSincronizacao(new ReceberFormaPagamento());
                RealizarSincronizacao(new ReceberConfiguracaoBalanca());
                RealizarSincronizacao(new ReceberTipoDocumento());
                RealizarSincronizacao(new ReceberConfiguracaoFinanceiro());
                RealizarSincronizacao(new ReceberConfiguracaoFrenteCaixa());
            }
            catch (Exception e)
            {
                throw new SincronizadorException(e);
            }
        }

        private void RealizarSincronizacao(ISincronizacao sincronizacao)
        {
            try
            {
                using (var sessaoAdm = _sessaoAdm.AbrirSessao())
                using (var sessaoPdv = _sessaoPdv.AbrirSessao())
                {
                    sincronizacao.IniciadoEm = DateTime.Now;
                    sincronizacao.SessaoAdm = sessaoAdm;
                    sincronizacao.SessaoPdv = sessaoPdv;

                    var ultimaSincronizacao = UltimaSincronizacao(sessaoPdv, sincronizacao.Tag);
                    sincronizacao.Sincronizar(ultimaSincronizacao);

                    RegistrarEvento(sincronizacao, sessaoPdv);
                }
            }
            catch (Exception e)
            {
                throw new SincronizadorException(e);
            }
        }

        private static DateTime UltimaSincronizacao(ISession sessaoPdv, string tag)
        {
            var repositorio = new EventoSincronizacaoRepositorio(sessaoPdv);
            var evento = repositorio.BuscaUltimoPelaTag(tag);

            return evento?.IniciadoEm ?? new DateTime(1754, 1, 1, 12, 00, 00);
        }

        private static void RegistrarEvento(ISincronizacao sincronizacao, ISession sessao)
        {
            if (sincronizacao.RegistraEvento == false)
                return;

            var repositorio = new EventoSincronizacaoRepositorio(sessao);
            var evento = CriaEventoSincronizacao(sincronizacao);
            repositorio.Persistir(evento);
        }

        private static EventoSincronizacaoDt CriaEventoSincronizacao(ISincronizacao sincronizacao)
        {
            return new EventoSincronizacaoDt
            {
                IniciadoEm = sincronizacao.IniciadoEm,
                Tag = sincronizacao.Tag,
                TerminadoEm = DateTime.Now
            };
        }
    }
}