using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.FusionAdm.Servico.Estoque.Evento;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Usuario;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate.Util;

namespace FusionCore.PdvSincronizador.Sync.Estrategia
{
    public class EnviarEstoque : SincronizacaoBase
    {
        public override string Tag { get; } = "enviar-estoque";

        public override void Sincronizar(DateTime ultimaSincronizacao)
        {
            var eventos = ObterEventos();
            if (eventos.Count == 0)
                return;

            EnviarEventos(eventos);
            RegistraEvento = true;
        }

        private IList<EstoqueEventoPdvDt> ObterEventos()
        {
            var repositorio = new EstoqueEventoPdvRepositorio(SessaoPdv);
            return repositorio.BuscaParaSincronizacao();
        }

        private void EnviarEventos(IList<EstoqueEventoPdvDt> eventos)
        {
            var transacaoAdm = SessaoAdm.BeginTransaction();
            var transacaoPdv = SessaoPdv.BeginTransaction();

            try
            {
                eventos.ForEach(EnviarEvento);
                transacaoAdm.Commit();
                transacaoPdv.Commit();
            }
            catch (Exception)
            {
                transacaoAdm.Rollback();
                transacaoPdv.Rollback();
                throw;
            }
        }

        private void EnviarEvento(EstoqueEventoPdvDt eventoPdv)
        {
            var eventoBuilder = CriarBuilder(eventoPdv);

            var servicoAdm = EstoqueServicoAdmFactory.Cria(SessaoAdm);
            var eventoAdm = servicoAdm.ReceberMovimentacao(eventoBuilder);
            AtualizarEventoPdv(eventoPdv, eventoAdm);
        }

        private EventoEstoqueBuilder CriarBuilder(EstoqueEventoPdvDt evento)
        {
            return new EventoEstoqueBuilder
            {
                CadastradoEm = evento.CadastradoEm,
                EstoqueModel = CriaEstoqueModel(evento),
                TipoEvento = evento.TipoEvento
            };
        }

        private EstoqueModel CriaEstoqueModel(EstoqueEventoPdvDt evento)
        {
            return new EstoqueModel
            {
                OrigemEvento = evento.OrigemEvento,
                Quantidade = evento.Movimento,
                Usuario = BuscaUsuarioCompativel(evento.UsuarioDt),
                Produto = BuscaProdutoCompativel(evento.ProdutoDt)
            };
        }

        private UsuarioDTO BuscaUsuarioCompativel(UsuarioPdvDt usuarioPdv)
        {
            var repositorio = new RepositorioComun<UsuarioDTO>(SessaoAdm);
            return repositorio.Busca(new UsuarioPeloId(usuarioPdv.Id));
        }

        private ProdutoDTO BuscaProdutoCompativel(ProdutoDt produtoDt)
        {
            var repositorio = new RepositorioProduto(SessaoAdm);
            return repositorio.GetPeloId(produtoDt.Id);
        }

        private void AtualizarEventoPdv(EstoqueEventoPdvDt eventoPdv, EstoqueEventoDTO eventoAdm)
        {
            eventoPdv.IdentificadorRemoto = eventoAdm.Id;
            var repositorio = new EstoqueEventoPdvRepositorio(SessaoPdv);
            repositorio.Alterar(eventoPdv);
        }
    }
}