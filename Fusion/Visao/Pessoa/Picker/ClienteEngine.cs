using System;
using System.Linq;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.GridPicker.Contrato;

namespace Fusion.Visao.Pessoa.Picker
{
    public class ClienteEngine : IPessoaPickerEngine
    {
        public GridPickerEventArgs ConverterItemPicked(IGridPickerItem item)
        {
            var pessoaGrid = item.GetItemReal<PessoaGrid>();

            if (pessoaGrid.IsCliente == false)
            {
                throw new InvalidOperationException("Pessoa não é um cliente!");
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPessoa(sessao);
                var cliente = repositorio.GetClientePeloId(pessoaGrid.Id);

                if (!cliente.Enderecos.Any())
                {
                    throw new InvalidOperationException("Cliente não possui endereço!");
                }

                item.ItemReal = cliente;
            }

            return new GridPickerEventArgs(item);
        }

        public GridPickerEventArgs ConverterItem(PessoaEntidade pessoa)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPessoa(sessao);
                var cliente = repositorio.GetClientePeloId(pessoa.Id);

                if (cliente == null)
                {
                    throw new InvalidOperationException("Pessoa não é um cliente!");
                }

                if (!cliente.Enderecos.Any())
                {
                    throw new InvalidOperationException("Cliente não possui endereço!");
                }

                return new GridPickerEventArgs(cliente);
            }
        }
    }
}