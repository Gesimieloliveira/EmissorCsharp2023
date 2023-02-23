using System;
using Fusion.Base.Notificacoes;
using Fusion.Sessao;
using FusionCore.FusionAdm.Compras;
using FusionCore.Helpers.DocumentoXml;
using FusionWPF.Parcelamento;

namespace Fusion.Visao.DocumentoAPagar
{
    public static class GerarContasPagarModelFactory
    {
        public static GerarContasPagarModel Criar(Notificador notificador)
        {
            return new GerarContasPagarModel(notificador, SessaoSistema.Instancia);
        }

        public static GerarContasPagarModel Criar(Notificador notificador, NotaFiscalCompra nota) 
        {
            try
            {
                var model = Criar(notificador);

                model.EmpresaSelecionada = nota.Empresa.ToComboBox();
                model.EmpresaIsEnable = false;
                model.FornecedorSelecionado = nota.Fornecedor;
                model.FornecedorIsEnable = false;
                model.Descricao = $"DOCUMENTO VINCULADO A NOTA DE COMPRA #ID: {nota.Id}";

                if (!nota.XMLPossuiCobrancas() || !XmlNfeHelper.TentarAnalisar(nota.Xml, out var xml))
                {
                    return model;
                }

                model.DataEmissao = xml.infNFe.ide.dhEmi.LocalDateTime;
                model.DataEmissaoIsEnable = false;
                model.NumeroDocumento = xml.infNFe.ide.nNF.ToString("D9");
                model.ValorTotal = xml.infNFe.cobr.fat.vLiq.Value;

                byte num = 1;
                foreach (var dup in xml.infNFe.cobr.dup)
                {
                    model.ParcelasItems.Add(new ParcelaGerada(num++, dup.dVenc.Value, dup.vDup));
                }

                return model;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    "Não foi possível abrir janela de cobranças: " + e.Message);
            }
        }
    }
}