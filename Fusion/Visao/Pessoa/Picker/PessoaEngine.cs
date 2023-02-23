using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.GridPicker.Contrato;

namespace Fusion.Visao.Pessoa.Picker
{
    public class PessoaEngine : IPessoaPickerEngine
    {
        public GridPickerEventArgs ConverterItemPicked(IGridPickerItem item)
        {
            var pessoaGrid = item.GetItemReal<PessoaGrid>();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPessoa(sessao);
                var pessoa = repositorio.GetPeloId(pessoaGrid.Id);

                item.ItemReal = pessoa;
            }

            return new GridPickerEventArgs(item);
        }

        public GridPickerEventArgs ConverterItem(PessoaEntidade pessoa)
        {
            return new GridPickerEventArgs(pessoa);
        }
    }
}