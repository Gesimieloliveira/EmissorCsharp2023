using System.Linq;
using System.Windows;
using MahApps.Metro.Controls;

namespace Fusion.Controles.Utilitarios.ComboBox
{
    internal class ComboBoxDados
    {
        public static readonly DependencyProperty OrigemDadosProperty =
            DependencyProperty.RegisterAttached(
                "OrigemDados",
                typeof(DadosEstrategia?),
                typeof(ComboBoxDados),
                new FrameworkPropertyMetadata(null, OrigemDadosChanged)
            );

        public static void SetOrigemDados(DependencyObject element, DadosEstrategia? value)
        {
            element.SetValue(OrigemDadosProperty, value);
        }

        public static DadosEstrategia? GetOrigemDados(DependencyObject element)
        {
            return (DadosEstrategia?)element.GetValue(OrigemDadosProperty);
        }

        private static void OrigemDadosChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is System.Windows.Controls.ComboBox cb))
            {
                return;
            }

            if (!(e.NewValue is DadosEstrategia estrategia))
            {
                return;
            }

            cb.ExecuteWhenLoaded(async () =>
            {
                var memberInfo = typeof(DadosEstrategia)
                    .GetMember(estrategia.ToString())
                    .FirstOrDefault();

                if (memberInfo == null)
                {
                    return;
                }

                var attr = (DadosAttribute)memberInfo
                    .GetCustomAttributes(typeof(DadosAttribute), false)
                    .FirstOrDefault();

                var config = attr?.CriarComportamento();
                if (config == null)
                {
                    return;
                }

                var dados = await config.DadosAsync();
                var cbItemsSource = dados as object[] ?? dados.ToArray();

                cb.ItemsSource = cbItemsSource;
                cb.DisplayMemberPath = config.NomeMembroExibicao;

                if (cbItemsSource.Count() == 1)
                {
                    cb.SelectedItem = cbItemsSource.FirstOrDefault();
                }
            });
        }
    }
}