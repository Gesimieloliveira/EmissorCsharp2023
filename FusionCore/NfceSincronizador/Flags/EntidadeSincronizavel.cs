using System;

namespace FusionCore.NfceSincronizador.Flags
{
    [Serializable]
    public enum EntidadeSincronizavel : short
    {
        Usuario = 1,
        Produto = 2,
        Pessoa = 3,
        TerminalOffline = 4,
        EstadoUf = 5,
        Cidade = 6,
        Empresa = 7,
        Cfop = 8,
        EmissorFiscal = 9,
        ProdutoUnidade = 10,
        ConfiguracaoEmail = 11,
        Nfce = 12,
        EmissorFiscalNfce = 13,
        ProdutoEstoqueEvento = 14,
        Ibpt = 15,
        TipoDocumento = 16,
        ConfiguracaoFinanceiro = 17,
        ConfiguracaoFrenteCaixa = 18,
        RegraIcms = 19,
        ConfiguracaoEstoque = 20,
        Pos = 21,
        Inutilizacao = 22,
        RegraTributacaoSaida = 23,
        Balanca = 24,
        ResponsavelTecnico = 25,
        Papel = 26,
        ConfiguracaoCaixa = 27,
        TabelaPreco = 28,
        NaoSincronizar = 100
    }
}