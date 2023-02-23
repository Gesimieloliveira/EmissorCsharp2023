﻿using System;
using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts
{
    public partial class FlyoutAddVeiculoReboque
    {
        public FlyoutAddVeiculoReboque()
        {
            InitializeComponent();
        }

        private void OnClickBotaoSalvaVeiculo(object sender, RoutedEventArgs e)
        {
            try
            {
                var model = DataContext as FlyoutAddVeiculoReboqueModel;
                model?.SalvarVeiculoReboque();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}
