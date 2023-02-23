using System;
using System.ComponentModel.DataAnnotations;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.ProdutoUnidade
{
    public class ProdutoUnidadeFormModel : ViewModel
    {
        private readonly ProdutoUnidadeDTO _unidade;

        public ProdutoUnidadeFormModel(ProdutoUnidadeDTO produtoUnidade)
        {
            _unidade = produtoUnidade;
            NovoRegistro = produtoUnidade.Id == 0;
        }

        [Required(ErrorMessage = @"Preciso de um nome para a unidade")]
        public string Nome
        {
            get => GetValue();
            set
            {
                SetValue(value);
                PropriedadeAlterada();
            }
        }

        [Required(ErrorMessage = @"Preciso de uma sigla para a unidade")]
        [MinLength(2, ErrorMessage = @"Sigla precisa ter no mínimo 2 caracteres")]
        public string Sigla
        {
            get => GetValue();
            set
            {
                SetValue(value);
                PropriedadeAlterada();
            }
        }

        public int PodeFracionar
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public bool SolicitarTotal
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool SolicitarPeso
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool NovoRegistro
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public void Inicializa()
        {
            Nome = _unidade.Nome;
            Sigla = _unidade.Sigla;
            PodeFracionar = _unidade.PodeFracionar;
            SolicitarTotal = _unidade.SolicitaTotalPdv;
            SolicitarPeso = _unidade.SolicitarPeso;
        }

        public void SalvarModel()
        {
            ThrowExceptionSeExistirErros();

            _unidade.Alterar(
                nome: Nome,
                sigla: Sigla,
                podeFacionar: PodeFracionar,
                solicitarTotal: SolicitarTotal,
                solicitarPeso: SolicitarPeso);

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProdutoUnidade(sessao);

                if (NovoRegistro && repositorio.JaExisteSigla(_unidade.Sigla))
                {
                    throw new InvalidOperationException("Sigla unidade já cadastrada!");
                }

                if (repositorio.JaExisteNome(_unidade.Nome, _unidade.Id))
                {
                    throw new InvalidOperationException("Nome unidade já cadastrada!");
                }

                repositorio.Salva(_unidade);
                NovoRegistro = false;
            }
        }

        public void DeletarModel()
        {
            if (_unidade.Id == 0)
            {
                return;
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProdutoUnidade(sessao);
                repositorio.Deleta(_unidade);
            }
        }
    }
}