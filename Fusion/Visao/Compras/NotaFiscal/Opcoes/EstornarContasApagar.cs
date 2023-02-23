using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Fusion.Sessao;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Compras.NotaFiscal.Opcoes
{
    public class EstornarContasApagar : IOutraOpcao
    {
        public string Titulo { get; } = "Extornar documentos a pagar";
        public bool IsVisible { get; } = SessaoSistema.Instancia?.AcessoConcedido.PossuiFusionGestor == true;

        public void ExeuctaAcao(NotaFiscalCompraViewModel compraVm)
        {
            if (compraVm.PossuiFinanceiro == false)
            {
                DialogBox.MostraAviso("Nota não possui documentos para ser estornado.");
                return;
            }

            try
            {
                IList<DocumentoPagar> documentos;

                using (var sessao = SessaoSistema.Instancia.SessaoManager.CriaSessao())
                {
                    var repositorioPagar = new RepositorioDocumentoPagar(sessao);
                    documentos = repositorioPagar.BuscarDocumentoPagarDeMalote(compraVm.ObtemMaloteDaNota());

                    var total = documentos.Sum(i => i.ValorAjustado);
                    var msgConfirmacao = $"Estornar os documentos a apagar vinculados a essa Nota, total dos documentos: {total}. Deseja continuar?";

                    if (DialogBox.MostraConfirmacao(msgConfirmacao) != MessageBoxResult.Yes)
                    {
                        return;
                    }
                }

                compraVm.EstornaDocumentosApagar(documentos);
                DialogBox.MostraInformacao("Documentos foram estornados.");
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }
    }
}