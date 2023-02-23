using System.Collections.Generic;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.FusionAdm.Servico.Estoque.Evento;
using FusionCore.FusionAdm.Servico.Sincronizador;
using FusionCore.FusionNfce.Produto;
using FusionCore.FusionNfce.Servico.Estoque;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Usuario;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.Produtos
{
    public class EnviarEventoEstoque : ISincronizavelPadrao
    {
        private ISession _sessaoServidor;
        private ISession _sessaoNfce;

        private void Sincroniza()
        {
            var todosEventos = BuscarTodosEventosEstoqueASincronizar();

            todosEventos.ForEach(estoque =>
            {
                _sessaoServidor = GerenciaSessaoNfce.ObterSessao(nameof(SessaoServerNfce)).AbrirSessao();
                _sessaoNfce = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();

                var transacaoNfce = _sessaoNfce.BeginTransaction();
                var transacaoServidor = _sessaoServidor.BeginTransaction();

                var repositorioNfce = new RepositorioEstoqueModelNfce(_sessaoServidor);

                using (repositorioNfce)
                using (transacaoNfce)
                {
                    EnviarEvento(estoque);

                    _sessaoServidor.Flush();
                    _sessaoNfce.Flush();

                    AtualizarEstoqueNoTerminal(estoque);

                    var servico = new SincronizacaoPendenteServico(
                        _sessaoServidor,
                        EntidadeSincronizavel.Produto,
                        estoque.Produto.Id.ToString()
                    );

                    servico.Salvar();

                    transacaoServidor.Commit();
                    transacaoNfce.Commit();
                }
            });
        }

        private IEnumerable<EstoqueModelNfce> BuscarTodosEventosEstoqueASincronizar()
        {
            var sessaoNfce = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();

            var repositorioNfce = new RepositorioEstoqueModelNfce(sessaoNfce);

            using (repositorioNfce)
            {
                return repositorioNfce.BuscarTodosEstoquesParaSincronizacao();
            }
        }

        private void EnviarEvento(EstoqueModelNfce estoqueEventoNfce)
        {
            if (estoqueEventoNfce.IdRemoto == 0)
            {
                var eventoBuilder = CriarBuilder(estoqueEventoNfce);

                var servicoAdm = EstoqueServicoAdmFactory.Cria(_sessaoServidor);
                var eventoAdm = servicoAdm.ReceberMovimentacao(eventoBuilder);
                AtualizarEventoNfce(estoqueEventoNfce, eventoAdm);

                return;
            }

            SalvarEventoEstoqueNfce(estoqueEventoNfce);
        }

        private void AtualizarEventoNfce(EstoqueModelNfce estoqueEventoNfce, EstoqueEventoDTO eventoAdm)
        {
            estoqueEventoNfce.IdRemoto = eventoAdm.Id;

            SalvarEventoEstoqueNfce(estoqueEventoNfce);
        }

        private void SalvarEventoEstoqueNfce(EstoqueModelNfce estoqueEventoNfce)
        {
            var repositorio = new RepositorioEstoqueModelNfce(_sessaoNfce);
            repositorio.SalvarENaoSincronizar(estoqueEventoNfce);
        }

        private EventoEstoqueBuilder CriarBuilder(EstoqueModelNfce estoqueEvento)
        {
            return new EventoEstoqueBuilder
            {
                CadastradoEm = estoqueEvento.CadastradoEm,
                EstoqueModel = CriaEstoqueModel(estoqueEvento),
                TipoEvento = estoqueEvento.TipoEvento
            };
        }

        private EstoqueModel CriaEstoqueModel(EstoqueModelNfce estoqueEvento)
        {
            return new EstoqueModel
            {
                OrigemEvento = estoqueEvento.OrigemEvento,
                Quantidade = estoqueEvento.Movimento,
                Usuario = BuscaUsuarioCompativel(estoqueEvento.Usuario),
                Produto = BuscaProdutoCompativel(estoqueEvento.Produto)
            };
        }

        private ProdutoDTO BuscaProdutoCompativel(ProdutoNfce produto)
        {
            var produtoDto = new RepositorioProduto(_sessaoServidor).GetPeloId(produto.Id);

            return produtoDto;
        }

        private UsuarioDTO BuscaUsuarioCompativel(UsuarioNfce usuario)
        {
            var usuarioDTO = new RepositorioUsuario(_sessaoServidor).GetPeloId(usuario.Id);

            return usuarioDTO;
        }

        private void AtualizarEstoqueNoTerminal(EstoqueModelNfce estoque)
        {
            var repositorio = new RepositorioProduto(_sessaoServidor);
            var saldoEstoque = repositorio.SaldoEstoque(estoque.Produto.Id);

            var repositorioNfce = new RepositorioProdutoNfce(_sessaoNfce);

            repositorioNfce.AlterarEstoquePara(estoque.Produto.Id, saldoEstoque);
        }

        public void RealizarSincronizacao()
        {
            Sincroniza();
        }
    }
}