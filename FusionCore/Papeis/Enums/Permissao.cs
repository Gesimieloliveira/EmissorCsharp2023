using FusionCore.Papeis.Anotacoes;

namespace FusionCore.Papeis.Enums
{
    public enum Permissao
    {
        [PermissaoDetalhe("Gerenciar Papel Usuário", PermissaoGrupo.Configuracoes, PermissaoSubGrupo.GerenciarPapelUsuario)]
        GERENCIAR_PAPEL_USUARIO = 0,

        [PermissaoDetalhe("Gerenciar Produto Unidade", PermissaoGrupo.Cadastros, PermissaoSubGrupo.ProdutoUnidade)]
        GERENCIAR_PRODUTO_UNIDADE = 1,

        [PermissaoDetalhe("Gerenciar Produto Grupo", PermissaoGrupo.Cadastros, PermissaoSubGrupo.ProdutoGrupo)]
        GERENCIAR_PRODUTO_GRUPO = 2,

        [PermissaoDetalhe("Gerenciar Produto Localizão", PermissaoGrupo.Cadastros, PermissaoSubGrupo.ProdutoLocalizacao)]
        GERENCIAR_PRODUTO_LOCALIZACAO = 3,

        [PermissaoDetalhe("Gerenciar Produto Ncm", PermissaoGrupo.Cadastros, PermissaoSubGrupo.ProdutoFiscal)]
        GERENCIAR_NCM = 4,

        [PermissaoDetalhe("Gerenciar Veículo", PermissaoGrupo.Cadastros, PermissaoSubGrupo.CadastrosGeral)]
        GERENCIAR_VEICULO = 5,

        [PermissaoDetalhe("Gerenciar Tipo Documento", PermissaoGrupo.Cadastros, PermissaoSubGrupo.CadastrosGeral)]
        GERENCIAR_TIPO_DOCUMENTO = 6,

        [PermissaoDetalhe("Gerenciar Produto Cfop", PermissaoGrupo.Cadastros, PermissaoSubGrupo.ProdutoFiscal)]
        GERENCIAR_CFOP = 7,

        [PermissaoDetalhe("Gerenciar Regra Saída", PermissaoGrupo.Cadastros, PermissaoSubGrupo.ProdutoFiscal)]
        GERENCIAR_REGRA_SAIDA = 8,

        [PermissaoDetalhe("Regra Saída Listar", PermissaoGrupo.Cadastros, PermissaoSubGrupo.ProdutoFiscal)]
        REGRA_SAIDA_LISTAR = 9,

        [PermissaoDetalhe("Cadastro Pessoa Inserir/Alterar", PermissaoGrupo.Cadastros, PermissaoSubGrupo.CadastroPessoa)]
        CADASTRO_PESSOA_INSERIR_ALTERAR = 10,

        [PermissaoDetalhe("Cadastro Pessoa Visualizar", PermissaoGrupo.Cadastros, PermissaoSubGrupo.CadastroPessoa)]
        CADASTRO_PESSOA_VISUALIZAR = 11,

        [PermissaoDetalhe("Cadastro Pessoa Listar", PermissaoGrupo.Cadastros, PermissaoSubGrupo.CadastroPessoa)]
        CADASTRO_PESSOA_LISTAR = 12,

        [PermissaoDetalhe("Cadastro Produto Inserir/Alterar", PermissaoGrupo.Cadastros, PermissaoSubGrupo.CadastroProduto)]
        CADASTRO_PRODUTO_INSERIR_ALTERAR = 13,

        [PermissaoDetalhe("Cadastro Produto Visualizar", PermissaoGrupo.Cadastros, PermissaoSubGrupo.CadastroProduto)]
        CADASTRO_PRODUTO_VISUALIZAR = 14,

        [PermissaoDetalhe("Cadastro Produto Listar", PermissaoGrupo.Cadastros, PermissaoSubGrupo.CadastroProduto)]
        CADASTRO_PRODUTO_LISTAR = 15,

        [PermissaoDetalhe("Cadastro Produto Acrescentar Estoque Avulso", PermissaoGrupo.Cadastros, PermissaoSubGrupo.CadastroProduto)]
        CADASTRO_PRODUTO_ACRESCENTAR_ESTOQUE_AVULSO = 16,

        [PermissaoDetalhe("Cadastro Produto Descontar Estoque Avulso", PermissaoGrupo.Cadastros, PermissaoSubGrupo.CadastroProduto)]
        CADASTRO_PRODUTO_DESCONTAR_ESTOQUE_AVULSO = 17,

        [PermissaoDetalhe("Financeiro Gerar Recibo", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroRecibo)]
        FINANCEIRO_GERAR_RECIBO = 18,

        [PermissaoDetalhe("Financeiro Gerenciar Centro de Custo", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroCentroCusto)]
        FINANCEIRO_GERENCIAR_CENTRO_CUSTO = 19,

        [PermissaoDetalhe("Financeiro Gerenciar Centro de Lucro", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroCentroLucro)]
        FINANCEIRO_GERENCIAR_CENTRO_LUCRO = 20,

        [PermissaoDetalhe("Financeiro Documento Receber Gerar Avulso", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoReceber)]
        [PermissaoMensagemErro("Hmm, {0}, você não tem permissão para gerar documento a receber avulso")]
        FINANCEIRO_DOCUMENTO_RECEBER_GERAR_AVULSO = 21,

        [PermissaoDetalhe("Financeiro Documento Receber Alterar", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoReceber)]
        [PermissaoMensagemErro("Hmm, {0}, você não tem permissão para alterar um documento a receber")]
        FINANCEIRO_DOCUMENTO_RECEBER_ALTERAR = 22,

        [PermissaoDetalhe("Financeiro Documento Receber Visualizar", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoReceber)]
        [PermissaoMensagemErro("Hmm, {0}, você não tem permissão para visualizar")]
        FINANCEIRO_DOCUMENTO_RECEBER_VISUALIZAR = 23,

        [PermissaoDetalhe("Financeiro Documento Receber Listar", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoReceber)]
        FINANCEIRO_DOCUMENTO_RECEBER_LISTAR = 24,

        [PermissaoDetalhe("Financeiro Documento Receber Quitar", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoReceber)]
        FINANCEIRO_DOCUMENTO_RECEBER_QUITAR = 25,

        [PermissaoDetalhe("Financeiro Documento Receber Adicionar Desconto", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoReceber)]
        FINANCEIRO_DOCUMENTO_RECEBER_ADICIONAR_DESCONTO = 26,

        [PermissaoDetalhe("Financeiro Documento Receber Adicionar Juros", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoReceber)]
        FINANCEIRO_DOCUMENTO_RECEBER_ADICIONAR_JUROS = 27,

        [PermissaoDetalhe("Financeiro Documento Receber Estornar Lançamento", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoReceber)]
        FINANCEIRO_DOCUMENTO_RECEBER_ESTORNAR_LANCAMENTO = 28,

        [PermissaoDetalhe("Financeiro Documento Receber Estornar Documento", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoReceber)]
        FINANCEIRO_DOCUMENTO_RECEBER_ESTORNAR = 29,

        [PermissaoDetalhe("Financeiro Documento Pagar Gerar Avulso", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoPagar)]
        [PermissaoMensagemErro("Hmm, {0}, você não tem permissão para gerar documento a pagar avulso")]
        FINANCEIRO_DOCUMENTO_APAGAR_GERAR_AVULSO = 30,

        [PermissaoDetalhe("Financeiro Documento Pagar Alterar", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoPagar)]
        [PermissaoMensagemErro("Hmm, {0}, você não tem permissão para alterar um documento a pagar")]
        FINANCEIRO_DOCUMENTO_APAGAR_ALTERAR = 31,

        [PermissaoDetalhe("Financeiro Documento Pagar Visualizar", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoPagar)]
        [PermissaoMensagemErro("Hmm, {0}, você não tem permissão para visualizar")]
        FINANCEIRO_DOCUMENTO_APAGAR_VISUALIZAR = 32,

        [PermissaoDetalhe("Financeiro Documento Pagar Listar", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoPagar)]
        FINANCEIRO_DOCUMENTO_APAGAR_LISTAR = 33,

        [PermissaoDetalhe("Financeiro Documento Pagar Quitar", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoPagar)]
        FINANCEIRO_DOCUMENTO_APAGAR_QUITAR = 34,

        [PermissaoDetalhe("Financeiro Documento Pagar Adicionar Desconto", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoPagar)]
        FINANCEIRO_DOCUMENTO_APAGAR_ADICIONAR_DESCONTO = 35,

        [PermissaoDetalhe("Financeiro Documento Pagar Adicionar Juros", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoPagar)]
        FINANCEIRO_DOCUMENTO_APAGAR_ADICIONAR_JUROS = 36,

        [PermissaoDetalhe("Financeiro Documento Pagar Estornar Lançamento", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoPagar)]
        FINANCEIRO_DOCUMENTO_APAGAR_ESTORNAR_LANCAMENTO = 37,

        [PermissaoDetalhe("Financeiro Documento Pagar Estornar Documento", PermissaoGrupo.Financeiro, PermissaoSubGrupo.FinanceiroDocumentoPagar)]
        FINANCEIRO_DOCUMENTO_APAGAR_ESTORNAR = 38,

        [PermissaoDetalhe("Gerenciar Compras", PermissaoGrupo.Movimentacoes, PermissaoSubGrupo.GerenciarCompras)]
        GERENCIAR_COMPRAS = 39,

        [PermissaoDetalhe("Compras Remover", PermissaoGrupo.Movimentacoes, PermissaoSubGrupo.GerenciarCompras)]
        COMPRAS_REMOVER = 40,

        [PermissaoDetalhe("Gerenciar Faturamento", PermissaoGrupo.Movimentacoes, PermissaoSubGrupo.GerenciarFaturamento)]
        GERENCIAR_FATURAMENTO = 41,

        [PermissaoDetalhe("Preferências Faturamento", PermissaoGrupo.Movimentacoes, PermissaoSubGrupo.GerenciarFaturamento)]
        FATURAMENTO_PREFERENCIAS = 42,

        [PermissaoDetalhe("Gerenciar Configurações", PermissaoGrupo.Configuracoes, PermissaoSubGrupo.GerenciarConfiguracoes)]
        GERENCIAR_CONFIGURACOES = 43,

        [PermissaoDetalhe("Gerenciar Empresa", PermissaoGrupo.Cadastros, PermissaoSubGrupo.CadastroEmpresa)]
        GERENCIAR_EMPRESA = 44,

        [PermissaoDetalhe("Gerenciar Licenças", PermissaoGrupo.Configuracoes, PermissaoSubGrupo.GerenciarLicenas)]
        GERENCIAR_LICENCA = 45,

        [PermissaoDetalhe("Gerenciar Usuário", PermissaoGrupo.Cadastros, PermissaoSubGrupo.CadastroUsuario)]
        GERENCIAR_USUARIO = 46,

        [PermissaoDetalhe("Gerenciar Terminal TipoEmissao", PermissaoGrupo.Cadastros, PermissaoSubGrupo.CadastroTerminalOffline)]
        GERENCIAR_TERMINAL_OFFLINE = 47,

        [PermissaoDetalhe("Gerenciar Emissor Fiscal Eletrônico", PermissaoGrupo.Cadastros, PermissaoSubGrupo.CadastroEmissorFiscalEletronico)]
        GERENCIAR_EMISSOR_FISCAL_ELETRONICO = 48,

        [PermissaoDetalhe("Gerenciar ECF", PermissaoGrupo.Cadastros, PermissaoSubGrupo.CadastroEcf)]
        GERENCIAR_ECF = 49,

        [PermissaoDetalhe("Gerenciar Alíquota Interestadual", PermissaoGrupo.Cadastros, PermissaoSubGrupo.ProdutoFiscal)]
        GERENCIAR_ALIQUOTA_INTERESTADUAL = 50,

        [PermissaoDetalhe("Gerenciar Importar Tabela IBPT", PermissaoGrupo.Cadastros, PermissaoSubGrupo.ProdutoFiscal)]
        GERENCIAR_IMPORTAR_TABELA_IBPT = 51,

        [PermissaoDetalhe("Gerenciar CFOP Base", PermissaoGrupo.Cadastros, PermissaoSubGrupo.ProdutoFiscal)]
        GERENCIAR_CFOP_BASE = 52,

        [PermissaoDetalhe("Gerenciar Tef Pos", PermissaoGrupo.Cadastros, PermissaoSubGrupo.CadastroTefPos)]
        GERENCIAR_TEF_POS = 53,

        [PermissaoDetalhe("Gerenciar Pedido de Venda/Orçamento", PermissaoGrupo.Movimentacoes, PermissaoSubGrupo.PedidoVendaOrcamento)]
        GERENCIAR_PEDIDO_VENDA_ORÇAMENTO = 54,

        [PermissaoDetalhe("Converter Pedido de Venda/Orçamento para Faturamento", PermissaoGrupo.Movimentacoes, PermissaoSubGrupo.PedidoVendaOrcamento)]
        CONVERTER_PEDIDO_PARA_FATURAMENTO = 55,

        [PermissaoDetalhe("Converter Pedido de Venda/Orçamento para NF-e", PermissaoGrupo.Movimentacoes, PermissaoSubGrupo.PedidoVendaOrcamento)]
        CONVERTER_PEDIDO_PARA_NFE = 56,

        [PermissaoDetalhe("Gerenciar Movimento de Estoque", PermissaoGrupo.Movimentacoes, PermissaoSubGrupo.MovimentoDeEstoque)]
        GERENCIAR_MOVIMENTACAO_ESTOQUE = 57,

        [PermissaoDetalhe("Movimento de Estoque Remover", PermissaoGrupo.Movimentacoes, PermissaoSubGrupo.MovimentoDeEstoque)]
        MOVIMENTACAO_ESTOQUE_REMOVER = 58,

        [PermissaoDetalhe("Gerenciar Perfil NF-e", PermissaoGrupo.NFe, PermissaoSubGrupo.PerfilNFe)]
        GERENCIAR_PERFIL_NFE = 59,

        [PermissaoDetalhe("Gerenciar NF-e", PermissaoGrupo.NFe, PermissaoSubGrupo.NFe)]
        GERENCIAR_NFE = 60,

        [PermissaoDetalhe("Inutilizar NF-e", PermissaoGrupo.NFe, PermissaoSubGrupo.NFe)]
        NFE_INUTILIZAR = 61,

        [PermissaoDetalhe("Exportar XML", PermissaoGrupo.NFe, PermissaoSubGrupo.NFe)]
        NFE_EXPORTAR_XML = 62,

        [PermissaoDetalhe("Gerenciar Perfil CT-e", PermissaoGrupo.Transportes, PermissaoSubGrupo.CTe)]
        GERENCIAR_PERFIL_CTE = 63,

        [PermissaoDetalhe("Cancelar NF-e", PermissaoGrupo.NFe, PermissaoSubGrupo.NFe)]
        NFE_CANCELAR = 64,

        [PermissaoDetalhe("Carta Correção NF-e", PermissaoGrupo.NFe, PermissaoSubGrupo.NFe)]
        NFE_CARTA_CORRECAO = 65,

        [PermissaoDetalhe("Gerenciar CT-e", PermissaoGrupo.Transportes, PermissaoSubGrupo.CTe)]
        GERENCIAR_CTE = 66,

        [PermissaoDetalhe("Exportar XML", PermissaoGrupo.Transportes, PermissaoSubGrupo.CTe)]
        CTE_EXPORTAR_XML = 67,

        [PermissaoDetalhe("Inutilizar CT-e", PermissaoGrupo.Transportes, PermissaoSubGrupo.CTe)]
        CTE_INUTILIZAR = 68,

        [PermissaoDetalhe("Enviar XML", PermissaoGrupo.Transportes, PermissaoSubGrupo.CTe)]
        CTE_ENVIAR_XML = 69,

        [PermissaoDetalhe("Baixar XML", PermissaoGrupo.Transportes, PermissaoSubGrupo.CTe)]
        CTE_BAIXAR_XML = 70,

        [PermissaoDetalhe("Carta Correção CT-e", PermissaoGrupo.Transportes, PermissaoSubGrupo.CTe)]
        CTE_CARTA_CORRECAO = 71,

        [PermissaoDetalhe("Cancelar CT-e", PermissaoGrupo.Transportes, PermissaoSubGrupo.CTe)]
        CTE_CANCELAR = 72,

        [PermissaoDetalhe("Gerenciar Perfil CT-e Os", PermissaoGrupo.Transportes, PermissaoSubGrupo.CTeOs)]
        GERENCIAR_PERFIL_CTE_OS = 73,

        [PermissaoDetalhe("Carta Correção CT-e Os", PermissaoGrupo.Transportes, PermissaoSubGrupo.CTeOs)]
        CTE_OS_CARTA_CORRECAO = 74,

        [PermissaoDetalhe("Cancelar CT-e Os", PermissaoGrupo.Transportes, PermissaoSubGrupo.CTeOs)]
        CTE_OS_CANCELAR = 75,

        [PermissaoDetalhe("Exportar XML", PermissaoGrupo.Transportes, PermissaoSubGrupo.CTeOs)]
        CTE_OS_EXPORTAR_XML = 76,

        [PermissaoDetalhe("Gerenciar CT-e OS", PermissaoGrupo.Transportes, PermissaoSubGrupo.CTeOs)]
        GERENCIAR_CTE_OS = 77,

        [PermissaoDetalhe("Inutilizar CT-e OS", PermissaoGrupo.Transportes, PermissaoSubGrupo.CTeOs)]
        CTE_OS_INUTILIZAR = 78,

        [PermissaoDetalhe("Gerenciar MDF-e", PermissaoGrupo.Transportes, PermissaoSubGrupo.MDFe)]
        GERENCIAR_MDFE = 79,

        [PermissaoDetalhe("Encerrar MDF-e", PermissaoGrupo.Transportes, PermissaoSubGrupo.MDFe)]
        MDFE_ENCERRAR = 80,

        [PermissaoDetalhe("Exportar XML", PermissaoGrupo.Transportes, PermissaoSubGrupo.MDFe)]
        MDFE_EXPORTAR_XML = 81,

        [PermissaoDetalhe("Enviar XML", PermissaoGrupo.Transportes, PermissaoSubGrupo.MDFe)]
        MDFE_ENVIAR_XML = 82,

        [PermissaoDetalhe("Baixar XML", PermissaoGrupo.Transportes, PermissaoSubGrupo.MDFe)]
        MDFE_BAIXAR_XML = 83,

        [PermissaoDetalhe("Incluir Condutor MDF-e", PermissaoGrupo.Transportes, PermissaoSubGrupo.MDFe)]
        MDFE_INCLUIR_CONDUTOR = 84,

        [PermissaoDetalhe("Cancelar MDF-e", PermissaoGrupo.Transportes, PermissaoSubGrupo.MDFe)]
        MDFE_CANCELAR = 85,

        [PermissaoDetalhe("Relatório Listar", PermissaoGrupo.Relatorio, PermissaoSubGrupo.Relatorio)]
        RELATORIO_LISTAR = 86,

        [PermissaoDetalhe("Sintegra Gerar", PermissaoGrupo.Sintegra, PermissaoSubGrupo.Sintegra)]
        SINTEGRA_GERAR = 87,

        [PermissaoDetalhe("Dashboard", PermissaoGrupo.Configuracoes, PermissaoSubGrupo.Dashboard)]
        DASHBOARD = 88,

        [PermissaoDetalhe("Gerenciar PDV", PermissaoGrupo.Pdv, PermissaoSubGrupo.Pdv)]
        GERENCIAR_PDV = 89,

        [PermissaoDetalhe("Cancelar Venda em Andamento PDV", PermissaoGrupo.Pdv, PermissaoSubGrupo.Pdv)]
        PDV_CANCELAR_VENDA_ANDAMENTO = 90,

        [PermissaoDetalhe("Recuperar Venda PDV", PermissaoGrupo.Pdv, PermissaoSubGrupo.Pdv)]
        PDV_RECUPERAR_VENDA = 91,

        [PermissaoDetalhe("Converter Pedido de Venda PDV", PermissaoGrupo.Pdv, PermissaoSubGrupo.Pdv)]
        PDV_CONVERTER_PEDIDO_VENDA = 92,

        [PermissaoDetalhe("Visualizar Vendas PDV", PermissaoGrupo.Pdv, PermissaoSubGrupo.Pdv)]
        PDV_VISUALIZAR_VENDAS = 93,

        [PermissaoDetalhe("Cancelar Venda Finalizada PDV", PermissaoGrupo.Pdv, PermissaoSubGrupo.Pdv)]
        PDV_CANCELAR_VENDA_FINALIZADA = 94,

        [PermissaoDetalhe("Avançar Númeração NFC-e", PermissaoGrupo.NFCe, PermissaoSubGrupo.NFCe)]
        PDV_AVANCAR_NUMERACAO_NFCE = 95,

        [PermissaoDetalhe("Contingência TipoEmissao NFC-e", PermissaoGrupo.NFCe, PermissaoSubGrupo.NFCe)]
        PDV_CONTINGENCIA_OFFLINE_NFCE = 96,

        [PermissaoDetalhe("Configuração Impressora PDV", PermissaoGrupo.Pdv, PermissaoSubGrupo.Pdv)]
        PDV_CONFIGURACAO_IMPRESSORA = 97,

        [PermissaoDetalhe("Trocar Certificado Digital PDV", PermissaoGrupo.Pdv, PermissaoSubGrupo.Pdv)]
        PDV_TROCAR_CERTIFICADO_DIGITAL = 98,

        [PermissaoDetalhe("Conexão Direta Impressora PDV", PermissaoGrupo.Pdv, PermissaoSubGrupo.Pdv)]
        PDV_CONEXAO_DIRETA_IMPRESSORA = 99,

        [PermissaoDetalhe("Configura TEF NFC-e", PermissaoGrupo.NFCe, PermissaoSubGrupo.NFCe)]
        PDV_CONFIGURA_TEF_NFCE = 100,

        [PermissaoDetalhe("Trocar Código de Ativação SAT/MF-e", PermissaoGrupo.NFCe, PermissaoSubGrupo.SatMfe)]
        PDV_TROCAR_CODIGO_ATIVACAO_SAT = 101,

        [PermissaoDetalhe("Bloquear SAT/MF-e", PermissaoGrupo.NFCe, PermissaoSubGrupo.SatMfe)]
        PDV_BLOQUEAR_SAT = 102,

        [PermissaoDetalhe("Desbloquear SAT/MF-e", PermissaoGrupo.NFCe, PermissaoSubGrupo.SatMfe)]
        PDV_DESBLOQUEAR_SAT = 103,

        [PermissaoDetalhe("Atualizar SAT/MF-e", PermissaoGrupo.NFCe, PermissaoSubGrupo.SatMfe)]
        PDV_ATUALIZAR_SAT = 104,

        [PermissaoDetalhe("Configuração SAT/MF-e", PermissaoGrupo.NFCe, PermissaoSubGrupo.SatMfe)]
        PDV_CONFIGURACAO_SAT = 105,

        [PermissaoDetalhe("Gerenciar Caixa", PermissaoGrupo.Financeiro, PermissaoSubGrupo.GerenciarCaixa)]
        GERENCIAR_CAIXA = 106,

        [PermissaoDetalhe("Gerenciar MD-E", PermissaoGrupo.Movimentacoes, PermissaoSubGrupo.ManifestadorFiscal)]
        MANIFESTADOR_NFE = 107,

        [PermissaoDetalhe("Preferencias Pedido de Venda/Orçamento", PermissaoGrupo.Movimentacoes, PermissaoSubGrupo.PedidoVendaOrcamento)]
        PEDIDO_VENDA_PREFERENCIAS = 108,

        [PermissaoDetalhe("Gerenciar Aliquota Interna Por Estado (UF)", PermissaoGrupo.Cadastros, PermissaoSubGrupo.AliquotaInternaPorEstadoUf)]
        GERENCIAR_ALIQUOTA_INTERNA_POR_ESTADO_UF = 109,

        [PermissaoDetalhe("Incluir Pagamento MDF-e", PermissaoGrupo.Transportes, PermissaoSubGrupo.MDFe)]
        MDFE_INCLUIR_PAGAMENTO = 110,

        [PermissaoDetalhe("Fazer lançamento direto no caixa loja", PermissaoGrupo.Financeiro, PermissaoSubGrupo.GerenciarCaixa)]
        LANCAMENTO_AVULSO_DIRETO_CAIXA_LOJA = 111,

        [PermissaoDetalhe("Gerenciar NFC-e", PermissaoGrupo.NFCe, PermissaoSubGrupo.NFCe)]
        GERENCIAR_NFCE = 112,

        [PermissaoDetalhe("Cancelar Faturamento", PermissaoGrupo.Movimentacoes, PermissaoSubGrupo.GerenciarFaturamento)]
        CANCELAR_FATURAMENTO = 113,

        [PermissaoDetalhe("Excluir Item Faturamento", PermissaoGrupo.Movimentacoes, PermissaoSubGrupo.GerenciarFaturamento)]
        EXCLUIR_ITEM_FATURAMENTO = 114,

        [PermissaoDetalhe("Excluir Item Nfe", PermissaoGrupo.NFe, PermissaoSubGrupo.NFe)]
        EXCLUIR_ITEM_NFE = 115,

        [PermissaoDetalhe("Cancelar Nfce", PermissaoGrupo.NFCe, PermissaoSubGrupo.NFCe)]
        CANCELAR_NFCE = 116,

        [PermissaoDetalhe("Excluir Item Nfce", PermissaoGrupo.NFCe, PermissaoSubGrupo.NFCe)]
        EXCLUIR_ITEM_NFCE = 117
    }
}