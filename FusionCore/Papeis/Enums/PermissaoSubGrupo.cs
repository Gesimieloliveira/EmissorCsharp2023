using System.ComponentModel;

namespace FusionCore.Papeis.Enums
{
    public enum PermissaoSubGrupo
    {
        [Description("Gerenciar Papel de Usuários")]
        GerenciarPapelUsuario,

        [Description("Produto Unidade Medida")]
        ProdutoUnidade,

        [Description("Produto Localização")]
        ProdutoLocalizacao,

        [Description("Produto Grupo")]
        ProdutoGrupo,

        [Description("Produto Fiscal")]
        ProdutoFiscal,

        [Description("Cadastros geral")]
        CadastrosGeral,

        [Description("Pessoa")]
        CadastroPessoa,

        [Description("Produto")]
        CadastroProduto,

        [Description("Recibo")]
        FinanceiroRecibo,

        [Description("Centro de Custo")]
        FinanceiroCentroCusto,

        [Description("Centro de Lucro")]
        FinanceiroCentroLucro,

        [Description("Documento a Receber")]
        FinanceiroDocumentoReceber,

        [Description("Documento a Pagar")]
        FinanceiroDocumentoPagar,

        [Description("Gerenciar Compras")]
        GerenciarCompras,

        [Description("Gerenciar Caixa")]
        GerenciarCaixa,

        [Description("Gerenciar Faturamento")]
        GerenciarFaturamento,

        [Description("Gerenciar Configurações")]
        GerenciarConfiguracoes,

        [Description("Gerenciar Empresa")]
        CadastroEmpresa,

        [Description("Gerenciar Licenças")]
        GerenciarLicenas,

        [Description("Gerenciar Usuário")]
        CadastroUsuario,

        [Description("Gerenciar Terminal TipoEmissao")]
        CadastroTerminalOffline,

        [Description("Gerenciar Emissor Fiscal Eletrônico")]
        CadastroEmissorFiscalEletronico,

        [Description("Gerenciar Ecf")]
        CadastroEcf,

        [Description("Gerenciar Tef Pos")]
        CadastroTefPos,

        [Description("Gerenciar Pedido de Venda/Orçamento")]
        PedidoVendaOrcamento,

        [Description("Gerenciar Movimentações de Estoque")]
        MovimentoDeEstoque,

        [Description("Perfil NF-e")]
        PerfilNFe,

        [Description("Gerenciar NF-e")]
        NFe,

        [Description("Gerenciar CT-e")]
        CTe,

        [Description("CT-e OS")]
        CTeOs,

        [Description("MDF-e")]
        MDFe,

        [Description("Relatório")]
        Relatorio,

        [Description("Sintegra")]
        Sintegra,

        [Description("Dashboard")]
        Dashboard,

        [Description("NFC-e")]
        NFCe,

        [Description("PDV")]
        Pdv,

        [Description("Sat Fiscal/MF-e")]
        SatMfe,

        [Description("Manifestador Fiscal")]
        ManifestadorFiscal,

        [Description("Gerenciar Aliquota Interna Por Estado (UF)")]
        AliquotaInternaPorEstadoUf
    }
}