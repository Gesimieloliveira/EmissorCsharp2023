using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Sessao;
using FusionCore.Repositorio.FusionAdm;

namespace Fusion.Controles
{
    public class ComboBoxEmpresa : ComboBox
    {
        private TextBox EditableTextBox => GetTemplateChild("PART_EditableTextBox") as TextBox;

        public override async void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            using (var sessao = SessaoSistema.Instancia.SessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioEmpresa(sessao);
                var empresas = await Task.Run(() => repositorio.BuscarEmpresaComboBoxDtos());

                SetValue(ItemsSourceProperty, empresas);

                if (empresas.Count() == 1)
                {
                    SelectedItem = empresas.ElementAt(0);
                }
            }

            MoverFocoParaTextBoxEditavel();
        }

        private void MoverFocoParaTextBoxEditavel()
        {
            if (!IsFocused || !IsEditable)
            {
                return;
            }

            EditableTextBox?.Focus();
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            MoverFocoParaTextBoxEditavel();
        }
    }
}