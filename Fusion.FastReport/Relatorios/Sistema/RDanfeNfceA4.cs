using System.Drawing;
using System.IO;
using DFe.Utils;
using FastReport;
using FastReport.Barcode;
using FusionCore.Helpers.Hidratacao;
using NFe.Classes;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Danfe.Base;

namespace Fusion.FastReport.Relatorios.Sistema
{
    public class RDanfeNfceA4 : RelatorioBase, IRDanfeNfce
    {
        private nfeProc _proc;
        private bool _segundaViaContigencia;
        private bool _cancelado;
        private byte[] _logoMarca;
        private bool _forcarSegundaVia;

        public RDanfeNfceA4() : base(null)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemBytesFrx("FrDanfeNfceA4.frx");
        }

        public void SegundaViaContingencia(bool value)
        {
            _segundaViaContigencia = value;
        }

        public void ForcarSegundaVia(bool forcarSegundaVia)
        {
            _forcarSegundaVia = forcarSegundaVia;
        }

        public void ComXml(string xmlstring, bool cancelado, byte[] logoMarca, string nomeFantasiaCustomizado)
        {
            _logoMarca = logoMarca;
            _cancelado = cancelado;

            if (xmlstring.Substring(0, 8).ToLower() == "<nfeproc")
            {
                _proc = FuncoesXml.XmlStringParaClasse<nfeProc>(xmlstring);

                if (nomeFantasiaCustomizado.IsNotNullOrEmpty())
                {
                    _proc.NFe.infNFe.emit.xNome = nomeFantasiaCustomizado;
                    _proc.NFe.infNFe.emit.xFant = nomeFantasiaCustomizado;
                }
                return;
            }

            var nfe = FuncoesXml.XmlStringParaClasse<NFe.Classes.NFe>(xmlstring);

            if (nomeFantasiaCustomizado.IsNotNullOrEmpty())
            {
                nfe.infNFe.emit.xNome = nomeFantasiaCustomizado;
                nfe.infNFe.emit.xFant = nomeFantasiaCustomizado;
            }

            _proc = new nfeProc { NFe = nfe };
        }

        protected override void PrepararDados()
        {
            var emContigencia = _proc.NFe.infNFe.ide.tpEmis == TipoEmissao.teOffLine;
            var naoAutorizado = string.IsNullOrEmpty(_proc.protNFe?.infProt.nProt);

            if (_segundaViaContigencia && emContigencia && naoAutorizado)
            {
                NumeroCopias(2);
            }

            if (_forcarSegundaVia)
            {
                NumeroCopias(2);
            }

            RegistraDados("NFCe", new[] { _proc });

            RegistraParametro("NfceDetalheVendaNormal", NfceDetalheVendaNormal.DuasLinhas);
            RegistraParametro("NfceDetalheVendaContigencia", NfceDetalheVendaContigencia.DuasLinhas);
            RegistraParametro("NfceImprimeDescontoItem", true);
            RegistraParametro("NfceCancelado", _cancelado);
            RegistraParametro("NfceLayoutQrCode", NfceLayoutQrCode.Lateral);

            if (_logoMarca != null)
            {
                using (var ms = new MemoryStream(_logoMarca))
                {
                    EncontrarObjeto<PictureObject>("poEmitLogo").Image = Image.FromStream(ms);
                }
            }

            EncontrarObjeto<TextObject>("txtUrl").Text = _proc.NFe.infNFeSupl.urlChave ?? "URL CHAVE NÃO DEFINIDA";
            EncontrarObjeto<BarcodeObject>("bcoQrCode").Text = _proc.NFe.infNFeSupl.qrCode;
            EncontrarObjeto<BarcodeObject>("bcoQrCodeLateral").Text = _proc.NFe.infNFeSupl.qrCode;
        }

        public override void Imprimir(string printer, int? quantidadeCopia = null)
        {
            AtivarImpressaoModoSplit();
            base.Imprimir(printer, quantidadeCopia);
        }
    }
}