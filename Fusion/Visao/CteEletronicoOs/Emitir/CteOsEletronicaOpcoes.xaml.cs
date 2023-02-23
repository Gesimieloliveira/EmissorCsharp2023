using System;

namespace Fusion.Visao.CteEletronicoOs.Emitir
{
    public partial class CteOsEletronicaOpcoes
    {
        public CteOsEletronicaOpcoes(CteOsEletronicaOpcoesModel model)
        {
            model.Fechar += Fechar;
            DataContext = model;
            InitializeComponent();
        }

        private void Fechar(object sender, EventArgs e)
        {
            Close();
        }
    }
}
