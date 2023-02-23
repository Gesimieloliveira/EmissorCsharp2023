using System;

namespace Fusion.Visao.Veiculos
{
    public partial class OpcoesDono
    {
        public OpcoesDono(OpcoesDonoModel vm)
        {
            DataContext = vm;
            InitializeComponent();

            vm.Fechar += FecharAction;
        }

        private void FecharAction(object sender, EventArgs e)
        {
            Close();
        }
    }
}