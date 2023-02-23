using System;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.GridPicker.Contrato;

namespace Fusion.Visao.Pessoa.Picker
{
    public class FornecedorEngine : IPessoaPickerEngine
    {
        public GridPickerEventArgs ConverterItemPicked(IGridPickerItem item)
        {
            var pessoaGrid = item.GetItemReal<PessoaGrid>();

            if (pessoaGrid.IsFornecedor == false)
            {
                throw new InvalidOperationException("Pessoa não é um fornecedor!");
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPessoa(sessao);
                var fornecedor = repositorio.GetFornecedorPeloId(pessoaGrid.Id);

                item.ItemReal = fornecedor;
            }

            return new GridPickerEventArgs(item);
        }

        public GridPickerEventArgs ConverterItem(PessoaEntidade pessoa)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPessoa(sessao);
                var fornecedor = repositorio.GetFornecedorPeloId(pessoa.Id);

                if (fornecedor == null)
                {
                    throw new InvalidOperationException("Pessoa não é uma fornecedor!");
                }

                return new GridPickerEventArgs(fornecedor);
            }
        }
    }
}