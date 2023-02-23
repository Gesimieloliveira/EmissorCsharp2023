using System;
using System.ComponentModel.DataAnnotations;
using FusionCore.FusionAdm.Produtos;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.ProdutoLocalizacoes
{
    public class ProdutoLocalizacaoFormModel : ViewModel
    {
        private readonly ProdutoLocalizacao _produtoLocalizacao;

        public ProdutoLocalizacaoFormModel(ProdutoLocalizacao produtoLocalizacao)
        {
            _produtoLocalizacao = produtoLocalizacao;
            AtualizaModel();
        }

        [Required(ErrorMessage = @"Preciso de um nome para localização")]
        public string Nome
        {
            get => GetValue();
            set => SetValue(value);
        }

        private void AtualizaModel()
        {
            Nome = _produtoLocalizacao.Nome;
        }

        public bool EhUmRegistroNovo()
        {
            return _produtoLocalizacao.Id == 0;
        }

        public void Salvar()
        {
            ThrowExceptionSeExistirErros();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioProdutoLocalizacao(sessao);

                if (repositorio.JaExisteEsseNomeCadastrado(Nome, _produtoLocalizacao.Id))
                {
                    throw new InvalidOperationException("Nome informado já existe. Informe outro nome!");
                }

                _produtoLocalizacao.Nome = Nome;

                repositorio.Salvar(_produtoLocalizacao);

                transacao.Commit();
            }
        }

        public void Deletar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioProdutoLocalizacao(sessao);
                repositorio.Deletar(_produtoLocalizacao);

                transacao.Commit();
            }
        }
    }
}