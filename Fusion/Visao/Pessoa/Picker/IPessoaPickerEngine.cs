using FusionCore.FusionAdm.Pessoas;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.GridPicker.Contrato;

namespace Fusion.Visao.Pessoa.Picker
{
    public interface IPessoaPickerEngine
    {
        GridPickerEventArgs ConverterItemPicked(IGridPickerItem item);
        GridPickerEventArgs ConverterItem(PessoaEntidade pessoa);
    }
}