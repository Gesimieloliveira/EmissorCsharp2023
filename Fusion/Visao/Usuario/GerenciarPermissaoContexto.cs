using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fusion.Sessao;
using FusionCore.Papeis;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Usuario
{
    public class GerenciarPermissaoContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;
        private readonly IEnumerable<Permissao> _todasPermissoes;

        public GerenciarPermissaoContexto()
        {
            ListaDeOpcoes = new List<OpcaoPermissao>();
            UsuarioLista = new List<UsuarioDTO>();

            _sessaoManager = SessaoSistema.Instancia.SessaoManager;
            _todasPermissoes = Enum.GetValues(typeof(Permissao)).Cast<Permissao>();
        }

        public IEnumerable<UsuarioDTO> UsuarioLista
        {
            get => GetValue<IEnumerable<UsuarioDTO>>();
            set => SetValue(value);
        }

        public IEnumerable<OpcaoPermissao> ListaDeOpcoes
        {
            get => GetValue<IEnumerable<OpcaoPermissao>>();
            private set => SetValue(value);
        }

        public IEnumerable<Papel> PapelLista
        {
            get => GetValue<IEnumerable<Papel>>();
            private set => SetValue(value);
        }

        public Papel PapelSelecionado
        {
            get => GetValue<Papel>();
            set
            {
                SetValue(value);
                AtualizarListagemDeUsuarios();
                AtualizarSituacaoDasPermissoes();
            }
        }

        public void AtualizarListagemDeUsuarios()
        {
            if (PapelSelecionado == null)
            {
                UsuarioLista = new List<UsuarioDTO>();
                return;
            }

            UsuarioLista = new List<UsuarioDTO>(PapelSelecionado.Usuarios);
        }

        private void AtualizarSituacaoDasPermissoes()
        {
            if (PapelSelecionado == null)
            {
                UsuarioLista = new List<UsuarioDTO>();

                foreach (var opcao in ListaDeOpcoes)
                {
                    opcao.DefinirPermissaoSemNotificacao(false);
                }

                return;
            }

            foreach (var opcao in ListaDeOpcoes)
            {
                opcao.DefinirPermissaoSemNotificacao(PapelSelecionado.Existe(opcao.Permissao));
            }
        }

        public void CarregarListaDeOpcoes()
        {
            var opcoes = new List<OpcaoPermissao>();

            foreach (var permissao in _todasPermissoes)
            {
                var opcao = new OpcaoPermissao(permissao);

                opcao.PermissaoAlterada += (sender, args) =>
                {
                    AlternarPermissao(args.Permissao, args.IsChecked);
                };

                opcoes.Add(opcao);
            }

            ListaDeOpcoes = opcoes;
        }

        private void AlternarPermissao(Permissao permisaoo, bool permitir)
        {
            if (PapelSelecionado == null)
            {
                return;
            }

            using (var sessao = _sessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var repositorio = new RepositorioPapel(sessao);

                if (permitir)
                {
                    repositorio.AdicionarPermissao(PapelSelecionado, permisaoo);
                    sessao.Transaction.Commit();

                    return;
                }

                repositorio.RemoverPermissao(PapelSelecionado, permisaoo);

                sessao.Transaction.Commit();
            }
        }

        public void CarregarOsPapeis()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioPapel(sessao);
                var papeis = repositorio.BuscaTodos();

                PapelLista = papeis.OrderBy(i => i.Descricao);
                PapelSelecionado = PapelLista.FirstOrDefault();
            }
        }

        public void DesvincularUsuario(UsuarioDTO usuario)
        {
            using (var sessao = _sessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                new RepositorioPapel(sessao).RemoverUsuario(PapelSelecionado, usuario);

                sessao.Transaction.Commit();
            }

            AtualizarListagemDeUsuarios();
        }

        public void PermitirTudo()
        {
            using (var sessao = _sessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var repositorio = new RepositorioPapel(sessao);

                repositorio.AdicionarVariasPermissoes(PapelSelecionado, _todasPermissoes);

                sessao.Transaction.Commit();
            }

            AtualizarSituacaoDasPermissoes();
        }

        public void NegarTudo()
        {
            using (var sessao = _sessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var repositorio = new RepositorioPapel(sessao);

                repositorio.RemoverTodasAsPermissoes(PapelSelecionado);

                sessao.Transaction.Commit();
            }

            AtualizarSituacaoDasPermissoes();
        }
    }
}