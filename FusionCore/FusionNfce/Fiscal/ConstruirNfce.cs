using System;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.TabelasDePrecos.Dtos;
using FusionCore.FusionAdm.TabelasDePrecos.NfceSync;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.FusionNfce.Vendedores;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.Helper.Criptografia;

namespace FusionCore.FusionNfce.Fiscal
{
    public class ConstruirNfce
    {
        private readonly Nfce _nfce;

        public ConstruirNfce(Nfce nfce, VendedorNfce vendedor)
        {
            _nfce = nfce;
            _nfce.Vendedor = vendedor;
        }

        public void Constroi()
        {
            _nfce.DestinoOperacao = DestinoOperacao.Interna;
            _nfce.IndicadorComprador = IndicadorComprador.Presencial;
            _nfce.FinalidadeEmissao = FinalidadeEmissao.Normal;
            _nfce.IndicadorConsumidorFinal = IndicadorOperacaoFinal.ConsumidorFinal;
            _nfce.InformacaoAdicional = string.Empty;
            _nfce.NaturezaOperacao = "Venda de Mercadorias";
            _nfce.ModalidadeFrete = ModalidadeFrete.SemFrete;

            if (SessaoSistemaNfce.Configuracao.EmissorFiscal.FlagNfce)
                _nfce.Modelo = ModeloDocumento.NFCe;

            if (SessaoSistemaNfce.Configuracao.EmissorFiscal.FlagSat)
                _nfce.Modelo = ModeloDocumento.SAT;


            _nfce.TipoDanfe = TipoDanfe.NFCe;
            _nfce.EmitidaEm = DateTime.Now;
            _nfce.EntradaSaidaEm = DateTime.Now;
            _nfce.FormaPagamento = FormaPagamento.Avista;
            _nfce.TipoOperacao = TipoOperacao.Saida;
            _nfce.TerminalOfflineId = SessaoSistemaNfce.Configuracao.TerminalOfflineId;
            _nfce.Uuid = GuuidHelper.Computar(DateTime.Now.ToString("O") + SessaoSistemaNfce.Configuracao.BindTerminal);
            _nfce.Observacao = SessaoSistemaNfce.ObservacaoPadrao().ToString().TrimOrEmpty();

            if (SessaoSistemaNfce.IsEmissorNFce())
            {
                _nfce.UuidVenda = GuuidHelper.Computar("NFC-e" + DateTime.Now.ToString("O") + SessaoSistemaNfce.Configuracao.BindTerminal);
            }

            if (SessaoSistemaNfce.IsEmissorSat())
            {
                _nfce.UuidVenda = GuuidHelper.Computar("CF-e" + DateTime.Now.ToString("O") + SessaoSistemaNfce.Configuracao.BindTerminal);
            }
        }

        public ConstruirNfce ComTabelaPreco(TabelaPrecoDto tabelaDto)
        {
            if (tabelaDto == null) return this;

            _nfce.TabelaPreco = new TabelaPrecoNfce
            {
                Id = tabelaDto.Id
            };

            return this;
        }
    }
}