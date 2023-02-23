using System;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.GridPicker.Contrato;

namespace Fusion.Visao.Pessoa.Picker
{
    public class VendedorEngine : IPessoaPickerEngine
    {
        public GridPickerEventArgs ConverterItemPicked(IGridPickerItem item)
        {
            var pessoaGrid = item.GetItemReal<PessoaGrid>();

            if (pessoaGrid.IsVendedor == false)
            {
                throw new InvalidOperationException("Pessoa não é um vendedor!");
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPessoa(sessao);
                var vendedor = repositorio.GetVendedorPeloId(pessoaGrid.Id);

                item.ItemReal = vendedor;
            }

            return new GridPickerEventArgs(item);
        }

        public GridPickerEventArgs ConverterItem(PessoaEntidade pessoa)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPessoa(sessao);
                var vendedor = repositorio.GetVendedorPeloId(pessoa.Id);

                if (vendedor == null)
                {
                    throw new InvalidOperationException("Pessoa não é um vendedor!");
                }

                return new GridPickerEventArgs(vendedor);
            }
        }
    }
}