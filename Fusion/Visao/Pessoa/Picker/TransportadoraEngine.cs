using System;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.GridPicker.Contrato;

namespace Fusion.Visao.Pessoa.Picker
{
    public class TransportadoraEngine : IPessoaPickerEngine
    {
        public GridPickerEventArgs ConverterItemPicked(IGridPickerItem item)
        {
            var pessoaGrid = item.GetItemReal<PessoaGrid>();

            if (pessoaGrid.IsTransportadora == false)
            {
                throw new InvalidOperationException("Pessoa não é uma transportadora!");
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPessoa(sessao);
                var tranportaodra = repositorio.GetTransportadoraPeloId(pessoaGrid.Id);

                item.ItemReal = tranportaodra;
            }

            return new GridPickerEventArgs(item);
        }

        public GridPickerEventArgs ConverterItem(PessoaEntidade pessoa)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPessoa(sessao);
                var transportadora = repositorio.GetTransportadoraPeloId(pessoa.Id);

                if (transportadora == null)
                {
                    throw new InvalidOperationException("Pessoa não é uma transportadora!");
                }

                return new GridPickerEventArgs(transportadora);
            }
        }
    }
}