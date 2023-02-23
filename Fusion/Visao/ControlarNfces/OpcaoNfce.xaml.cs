using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using DFe.Utils;
using Fusion.Visao.Vendas;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.Componentes;
using FusionCore.Vendas.Servicos;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.SendMail;

namespace Fusion.Visao.ControlarNfces
{
    public partial class OpcaoNfce
    {
        private readonly OpcaoNfceDados _opcaoNfceDados;
        private readonly Action _action;

        public OpcaoNfce(OpcaoNfceDados opcaoNfceDados, Action action)
        {
            InitializeComponent();
            _opcaoNfceDados = opcaoNfceDados;
            _action = action;
            _opcaoNfceDados.Fechar += delegate { Close(); };
            DataContext = _opcaoNfceDados;
        }

        private void Imprimir_Clique(object sender, RoutedEventArgs e)
        {
            try
            {
                _opcaoNfceDados.Imprimir();
            }
            catch (PreferenciaException pex)
            {
                DialogBox.MostraAviso(pex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void BaixarXml_Clique(object sender, RoutedEventArgs e)
        {
            try
            {
                _opcaoNfceDados.ThrowBaixarXml();

                var dialog = new SaveFileDialog { Filter = @"Arquivo Xml|*.xml" };
                var showDialog = dialog.ShowDialog();

                if (showDialog == System.Windows.Forms.DialogResult.Cancel)
                    return;

                if (dialog.FileName.IsNullOrEmpty())
                {
                    DialogBox.MostraInformacao("Não foi selecionado um caminho");
                    return;
                }

                var xmlAutorizado = _opcaoNfceDados.BaixarXml();
                var xmlDocumento = new XmlDocument();
                xmlDocumento.LoadXml(xmlAutorizado);

                var stringWriter = new StringWriter();
                var xmlTextWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented };
                xmlDocumento.WriteTo(xmlTextWriter);


                if (xmlAutorizado.IsNullOrEmpty())
                {
                    DialogBox.MostraInformacao("Não encontrei XML autorizado para essa NF-e");
                    return;
                }

                FuncoesXml.SalvarStringXmlParaArquivoXml(stringWriter.ToString(), dialog.FileName);

                DialogBox.MostraInformacao("XML foi salvo com sucesso");
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void EnviaPorEmail_Clique(object sender, RoutedEventArgs e)
        {
            try
            {
                _opcaoNfceDados.ThrowPodeEnviarEmail();

                var behavior = new EnvioEmailBehavior();

                behavior.DespacharEmails += DespacharEmailsHandler;
                behavior.Assunto = "NOTA FISCAL CONSUMIDOR ELETRONICA";
                behavior.CorpoMensagem = "Segue em anexo o DANFE e o XML";

                new EnvioEmailView(behavior).ShowDialog();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void DespacharEmailsHandler(object sender, IEnumerable<Email> emails)
        {
            var behavior = sender as EnvioEmailBehavior;

            if (behavior == null)
            {
                return;
            }

            _opcaoNfceDados.EnviaPorEmail(behavior, emails);
        }

        private void AvancarNumeracaoFiscal_Clique(object sender, RoutedEventArgs e)
        {
            if (DialogBox.MostraConfirmacao("Deseja avançar a númeração fiscal?", MessageBoxImage.Question) == false) return;

            try
            {
                _opcaoNfceDados.AvancarNumeracaoFiscal();

                DialogBox.MostraInformacao("Númeração fiscal avançada com sucesso");

                _opcaoNfceDados.OnFechar();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void AutorizarNFCe_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_opcaoNfceDados.EhUmFaturamento())
            {
                DialogBox.MostraAviso("Não é um faturamento.");
                return;
            }

            if (ContingenciaAtiva.EstaAtiva())
            {
                DialogBox.MostraAviso("Contingência está ativa, espere 40 minutos é tente novamente.");
                return;
            }

            try
            {
                var venda = _opcaoNfceDados.BuscarFaturamentoVenda();

                if (venda == null)
                {
                    DialogBox.MostraAviso("Não encontrei o faturamento");
                    return;
                }

                if (venda.Autorizada() || venda.EUmaVendaOffline())
                {
                    Imprimir_Clique(sender, e);
                    return;
                }

                IAutorizadorTela autorizador = new AutorizadorNfceTela();
                autorizador.EnviaSefaz(venda, _action);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            finally
            {
                _opcaoNfceDados.OnFechar();
            }
        }
    }
}