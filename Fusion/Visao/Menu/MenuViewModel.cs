using System;
using System.Reflection;
using Fusion.Sessao;
using FusionCore.FusionAdm.Setup.Empresa;
using FusionCore.Helpers.AssemblyUtils;
using FusionCore.Papeis.Enums;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Menu
{
    public sealed class MenuViewModel : ViewModel
    {
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;
        private bool _isDeveloper;
        private bool _isGerenciarProdutoUnidade;
        private bool _isGerenciarProdutoGrupo;
        private bool _isGerenciarProdutoLocalizacao;
        private bool _isGerencairNcm;
        private bool _isGerenciarVeiculo;
        private bool _isGerenciarTipoDocumento;
        private bool _isGerenciarCfop;
        private bool _isGerenciarRegraSaida;
        private bool _isRegraSaidaListar;
        private bool _isGerenciarPessoa;
        private bool _isGerenciarProduto;
        private bool _isGerarRecibo;
        private bool _isGerenciarCentroLucro;
        private bool _isGerenciarCentroCusto;
        private bool _isGerenciarDocumentoReceber;
        private bool _isGerenciarDocumentoPagar;
        private bool _isGerenciarCompras;
        private bool _isGerenciarFaturamento;
        private bool _isPermissaoPreferenciaFaturamento;
        private bool _isGerenciarConfiguracoes;
        private bool _isGerenciarEmpresa;
        private bool _isGerenciarLicencas;
        private bool _isGerenciarUsuario;
        private bool _isGerenciarTerminalOffline;
        private bool _isGerenciarEmissorFiscalEletronico;
        private bool _isGerenciarEcf;
        private bool _isGerenciarAliquotaInterestadual;
        private bool _isGerenciarImportarTabelaIpbt;
        private bool _isGerenciarCfopBase;
        private bool _isGerenciarTefPos;
        private bool _isGerenciarPedidoOrcamento;
        private bool _isGerenciarMovimentoEstoque;
        private bool _isGerenciarPerfilNFe;
        private bool _isGerenciarNFe;
        private bool _isInutilizarNFe;
        private bool _isExportarXml;
        private bool _isGerenciarPerfilCTe;
        private bool _isGerenciarCTe;
        private bool _isInutilizarCTe;
        private bool _isGerenciarPerfilCteOs;
        private bool _isGerenciarCteOs;
        private bool _isGerenciarMDFe;
        private bool _isPermissaoEncerrarMdfe;
        private bool _isRelatorioListar;
        private bool _isPermissaoGerarSintegra;
        private bool _isPermissaoDashboard;
        private bool _isPermissaoManifestadorNfe;
        private bool _isPermissaoPreferenciaPedidoVenda;
        private bool _isGerenciarAliquotaInterna;
        private bool _isGerenciarNFCe;

        public bool Administrador
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsDeveloper
        {
            get => _isDeveloper;
            set
            {
                if (value == _isDeveloper) return;
                _isDeveloper = value;
                PropriedadeAlterada();
            }
        }

        public string NomeUsuarioLogado
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool EstaLogado
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool PossuiFusionStarter
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool PossuiFusionCte
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                PropriedadeAlterada(nameof(PossuiTransporte));
            }
        }

        public bool PossuiFusionCteOs
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                PropriedadeAlterada(nameof(PossuiTransporte));
            }
        }

        public bool PossuiFusionGestor
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool PossuiFusionMdfe
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                PropriedadeAlterada(nameof(PossuiTransporte));
            }
        }

        public string VersaoSistema
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool TemAcessoAoCaixa
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool PossuiTransporte => PossuiFusionMdfe || PossuiFusionCte || PossuiFusionCteOs;
        public event EventHandler Deslogou;

        public void Inicializa()
        {
            EstaLogado = true;
            Administrador = _sessaoSistema.UsuarioLogado.IsAdmin;
            NomeUsuarioLogado = _sessaoSistema.UsuarioLogado.Login;
            PossuiFusionStarter = _sessaoSistema.AcessoConcedido.PossuiFusionStarter;
            PossuiFusionGestor = _sessaoSistema.AcessoConcedido.PossuiFusionGestor;
            PossuiFusionCte = _sessaoSistema.AcessoConcedido.PossuiFusionCTe;
            PossuiFusionMdfe = _sessaoSistema.AcessoConcedido.PossuiFusionMdfe;
            PossuiFusionCteOs = _sessaoSistema.AcessoConcedido.PossuiFusionCteOs;
            VersaoSistema = AssemblyHelper.LerVersao3Digitos(Assembly.GetExecutingAssembly());

            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;

            IsGerenciarProdutoUnidade = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_PRODUTO_UNIDADE);
            IsGerenciarProdutoGrupo = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_PRODUTO_GRUPO);
            IsGerenciarProdutoLocalizacao = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_PRODUTO_LOCALIZACAO);
            IsGerencairNcm = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_NCM);
            IsGerenciarVeiculo = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_VEICULO);
            IsGerenciarTipoDocumento = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_TIPO_DOCUMENTO);
            IsGerenciarCfop = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_CFOP);
            IsGerenciarRegraSaida = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_REGRA_SAIDA);
            IsRegraSaidaListar = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.REGRA_SAIDA_LISTAR);
            IsGerarRecibo = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.FINANCEIRO_GERAR_RECIBO);
            IsGerenciarCentroLucro = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.FINANCEIRO_GERENCIAR_CENTRO_LUCRO);
            IsGerenciarCentroCusto = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.FINANCEIRO_GERENCIAR_CENTRO_CUSTO);
            IsGerenciarCompras = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_COMPRAS);
            IsGerenciarFaturamento = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_FATURAMENTO);
            IsPermissaoPreferenciaFaturamento = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.FATURAMENTO_PREFERENCIAS);
            IsGerenciarConfiguracoes = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_CONFIGURACOES);
            IsGerenciarEmpresa = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_EMPRESA);
            IsGerenciarLicencas = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_LICENCA);
            IsGerenciarUsuario = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_USUARIO);
            IsGerenciarTerminalOffline = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_TERMINAL_OFFLINE);
            IsGerenciarAliquotaInterna =
                usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_ALIQUOTA_INTERNA_POR_ESTADO_UF);
            IsGerenciarEmissorFiscalEletronico = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_EMISSOR_FISCAL_ELETRONICO);
            IsGerenciarEcf = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_ECF);
            IsGerenciarAliquotaInterestadual = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_ALIQUOTA_INTERESTADUAL);
            IsGerenciarImportarTabelaIpbt = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_IMPORTAR_TABELA_IBPT);
            IsGerenciarCfopBase = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_CFOP_BASE);
            IsGerenciarTefPos = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_TEF_POS);
            IsGerenciarPedidoOrcamento = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_PEDIDO_VENDA_ORÇAMENTO);
            IsPermissaoPreferenciaPedidoVenda =
                usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.PEDIDO_VENDA_PREFERENCIAS);
            IsGerenciarMovimentoEstoque = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_MOVIMENTACAO_ESTOQUE);
            IsGerenciarPerfilNFe = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_PERFIL_NFE);
            IsGerenciarNFe = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_NFE);
            IsGerenciarNFCe = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_NFCE);
            IsInutilizarNFe = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.NFE_INUTILIZAR);
            IsGerenciarPerfilCTe = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_PERFIL_CTE);
            IsGerenciarCTe = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_CTE);
            IsGerenciarPerfilCteOs = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_PERFIL_CTE_OS);
            IsGerenciarCteOs = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_CTE_OS);
            IsGerenciarMDFe = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_MDFE);
            IsPermissaoEncerrarMdfe = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.MDFE_ENCERRAR);
            IsRelatorioListar = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.RELATORIO_LISTAR);
            IsPermissaoGerarSintegra = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.SINTEGRA_GERAR);
            IsPermissaoDashboard = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.DASHBOARD);
            TemAcessoAoCaixa = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_CAIXA);
            IsPermissaoManifestadorNfe = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.MANIFESTADOR_NFE);

            IsExportarXml = usuarioLogado.VerificaPermissao.IsQualquerPermissao(Permissao.NFE_EXPORTAR_XML,
                Permissao.CTE_EXPORTAR_XML,
                Permissao.CTE_OS_EXPORTAR_XML,
                Permissao.MDFE_EXPORTAR_XML);

            IsInutilizarCTe = usuarioLogado.VerificaPermissao.IsQualquerPermissao(Permissao.CTE_INUTILIZAR, Permissao.CTE_OS_INUTILIZAR);

            IsGerenciarPessoa = usuarioLogado.VerificaPermissao.IsQualquerPermissao(
                Permissao.CADASTRO_PESSOA_INSERIR_ALTERAR,
                Permissao.CADASTRO_PESSOA_VISUALIZAR,
                Permissao.CADASTRO_PESSOA_LISTAR);

            IsGerenciarProduto = usuarioLogado.VerificaPermissao.IsQualquerPermissao(
                Permissao.CADASTRO_PRODUTO_INSERIR_ALTERAR,
                Permissao.CADASTRO_PRODUTO_LISTAR,
                Permissao.CADASTRO_PRODUTO_VISUALIZAR,
                Permissao.CADASTRO_PRODUTO_ACRESCENTAR_ESTOQUE_AVULSO,
                Permissao.CADASTRO_PRODUTO_DESCONTAR_ESTOQUE_AVULSO
            );

            IsGerenciarDocumentoReceber = usuarioLogado.VerificaPermissao.IsQualquerPermissao(
                Permissao.FINANCEIRO_DOCUMENTO_RECEBER_GERAR_AVULSO,
                Permissao.FINANCEIRO_DOCUMENTO_RECEBER_ALTERAR,
                Permissao.FINANCEIRO_DOCUMENTO_RECEBER_VISUALIZAR,
                Permissao.FINANCEIRO_DOCUMENTO_RECEBER_LISTAR,
                Permissao.FINANCEIRO_DOCUMENTO_RECEBER_QUITAR,
                Permissao.FINANCEIRO_DOCUMENTO_RECEBER_ADICIONAR_DESCONTO,
                Permissao.FINANCEIRO_DOCUMENTO_RECEBER_ADICIONAR_JUROS,
                Permissao.FINANCEIRO_DOCUMENTO_RECEBER_ESTORNAR_LANCAMENTO,
                Permissao.FINANCEIRO_DOCUMENTO_RECEBER_ESTORNAR
            );

            IsGerenciarDocumentoPagar = usuarioLogado.VerificaPermissao.IsQualquerPermissao(
                Permissao.FINANCEIRO_DOCUMENTO_APAGAR_GERAR_AVULSO,
                Permissao.FINANCEIRO_DOCUMENTO_APAGAR_ALTERAR,
                Permissao.FINANCEIRO_DOCUMENTO_APAGAR_VISUALIZAR,
                Permissao.FINANCEIRO_DOCUMENTO_APAGAR_LISTAR,
                Permissao.FINANCEIRO_DOCUMENTO_APAGAR_QUITAR,
                Permissao.FINANCEIRO_DOCUMENTO_APAGAR_ADICIONAR_DESCONTO,
                Permissao.FINANCEIRO_DOCUMENTO_APAGAR_ADICIONAR_JUROS,
                Permissao.FINANCEIRO_DOCUMENTO_APAGAR_ESTORNAR_LANCAMENTO,
                Permissao.FINANCEIRO_DOCUMENTO_APAGAR_ESTORNAR
            );

#if DEBUG
            IsDeveloper = true;
#endif
        }

        public bool IsGerenciarNFCe
        {
            get => _isGerenciarNFCe;
            set
            {
                if (value == _isGerenciarNFCe) return;
                _isGerenciarNFCe = value;
                PropriedadeAlterada();
            }
        }

        public bool IsPermissaoPreferenciaPedidoVenda
        {
            get => _isPermissaoPreferenciaPedidoVenda;
            set
            {
                if (value == _isPermissaoPreferenciaPedidoVenda) return;
                _isPermissaoPreferenciaPedidoVenda = value;
                PropriedadeAlterada();
            }
        }

        public bool IsPermissaoManifestadorNfe
        {
            get => _isPermissaoManifestadorNfe;
            set
            {
                if (value == _isPermissaoManifestadorNfe) return;
                _isPermissaoManifestadorNfe = value;
                PropriedadeAlterada();
            }
        }

        public bool IsPermissaoEncerrarMdfe
        {
            get => _isPermissaoEncerrarMdfe;
            set
            {
                if (value == _isPermissaoEncerrarMdfe) return;
                _isPermissaoEncerrarMdfe = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarMDFe
        {
            get => _isGerenciarMDFe;
            set
            {
                if (value == _isGerenciarMDFe) return;
                _isGerenciarMDFe = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarCteOs
        {
            get => _isGerenciarCteOs;
            set
            {
                if (value == _isGerenciarCteOs) return;
                _isGerenciarCteOs = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarPerfilCteOs
        {
            get => _isGerenciarPerfilCteOs;
            set
            {
                if (value == _isGerenciarPerfilCteOs) return;
                _isGerenciarPerfilCteOs = value;
                PropriedadeAlterada();
            }
        }

        public bool IsInutilizarCTe
        {
            get => _isInutilizarCTe;
            set
            {
                if (value == _isInutilizarCTe) return;
                _isInutilizarCTe = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarCTe
        {
            get => _isGerenciarCTe;
            set
            {
                if (value == _isGerenciarCTe) return;
                _isGerenciarCTe = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarPerfilCTe
        {
            get => _isGerenciarPerfilCTe;
            set
            {
                if (value == _isGerenciarPerfilCTe) return;
                _isGerenciarPerfilCTe = value;
                PropriedadeAlterada();
            }
        }

        public bool IsExportarXml
        {
            get => _isExportarXml;
            set
            {
                if (value == _isExportarXml) return;
                _isExportarXml = value;
                PropriedadeAlterada();
            }
        }

        public bool IsInutilizarNFe
        {
            get => _isInutilizarNFe;
            set
            {
                if (value == _isInutilizarNFe) return;
                _isInutilizarNFe = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarNFe
        {
            get => _isGerenciarNFe;
            set
            {
                if (value == _isGerenciarNFe) return;
                _isGerenciarNFe = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarPerfilNFe
        {
            get => _isGerenciarPerfilNFe;
            set
            {
                if (value == _isGerenciarPerfilNFe) return;
                _isGerenciarPerfilNFe = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarMovimentoEstoque
        {
            get => _isGerenciarMovimentoEstoque;
            set
            {
                if (value == _isGerenciarMovimentoEstoque) return;
                _isGerenciarMovimentoEstoque = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarPedidoOrcamento
        {
            get => _isGerenciarPedidoOrcamento;
            set
            {
                if (value == _isGerenciarPedidoOrcamento) return;
                _isGerenciarPedidoOrcamento = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarTefPos
        {
            get => _isGerenciarTefPos;
            set
            {
                if (value == _isGerenciarTefPos) return;
                _isGerenciarTefPos = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarCfopBase
        {
            get => _isGerenciarCfopBase;
            set
            {
                if (value == _isGerenciarCfopBase) return;
                _isGerenciarCfopBase = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarImportarTabelaIpbt
        {
            get => _isGerenciarImportarTabelaIpbt;
            set
            {
                if (value == _isGerenciarImportarTabelaIpbt) return;
                _isGerenciarImportarTabelaIpbt = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarAliquotaInterestadual
        {
            get => _isGerenciarAliquotaInterestadual;
            set
            {
                if (value == _isGerenciarAliquotaInterestadual) return;
                _isGerenciarAliquotaInterestadual = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarEcf
        {
            get => _isGerenciarEcf;
            set
            {
                if (value == _isGerenciarEcf) return;
                _isGerenciarEcf = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarEmissorFiscalEletronico
        {
            get => _isGerenciarEmissorFiscalEletronico;
            set
            {
                if (value == _isGerenciarEmissorFiscalEletronico) return;
                _isGerenciarEmissorFiscalEletronico = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarTerminalOffline
        {
            get => _isGerenciarTerminalOffline;
            set
            {
                if (value == _isGerenciarTerminalOffline) return;
                _isGerenciarTerminalOffline = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarAliquotaInterna
        {
            get => _isGerenciarAliquotaInterna;
            set
            {
                if (value == _isGerenciarAliquotaInterna) return;
                _isGerenciarAliquotaInterna = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarUsuario
        {
            get => _isGerenciarUsuario;
            set
            {
                if (value == _isGerenciarUsuario) return;
                _isGerenciarUsuario = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarLicencas
        {
            get => _isGerenciarLicencas;
            set
            {
                if (value == _isGerenciarLicencas) return;
                _isGerenciarLicencas = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarEmpresa
        {
            get => _isGerenciarEmpresa;
            set
            {
                if (value == _isGerenciarEmpresa) return;
                _isGerenciarEmpresa = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarConfiguracoes
        {
            get => _isGerenciarConfiguracoes;
            set
            {
                if (value == _isGerenciarConfiguracoes) return;
                _isGerenciarConfiguracoes = value;
                PropriedadeAlterada();
            }
        }

        public bool IsPermissaoPreferenciaFaturamento
        {
            get => _isPermissaoPreferenciaFaturamento;
            set
            {
                _isPermissaoPreferenciaFaturamento = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarFaturamento
        {
            get => _isGerenciarFaturamento;
            set
            {
                _isGerenciarFaturamento = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarCompras
        {
            get => _isGerenciarCompras;
            set
            {
                if (value == _isGerenciarCompras) return;
                _isGerenciarCompras = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarDocumentoPagar
        {
            get => _isGerenciarDocumentoPagar;
            set
            {
                if (value == _isGerenciarDocumentoPagar) return;
                _isGerenciarDocumentoPagar = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarDocumentoReceber
        {
            get => _isGerenciarDocumentoReceber;
            set
            {
                if (value == _isGerenciarDocumentoReceber) return;
                _isGerenciarDocumentoReceber = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarCentroCusto
        {
            get => _isGerenciarCentroCusto;
            set
            {
                if (value == _isGerenciarCentroCusto) return;
                _isGerenciarCentroCusto = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarCentroLucro
        {
            get => _isGerenciarCentroLucro;
            set
            {
                if (value == _isGerenciarCentroLucro) return;
                _isGerenciarCentroLucro = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerarRecibo
        {
            get => _isGerarRecibo;
            set
            {
                if (value == _isGerarRecibo) return;
                _isGerarRecibo = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarProduto
        {
            get => _isGerenciarProduto;
            set
            {
                if (value == _isGerenciarProduto) return;
                _isGerenciarProduto = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarPessoa
        {
            get => _isGerenciarPessoa;
            set
            {
                if (value == _isGerenciarPessoa) return;
                _isGerenciarPessoa = value;
                PropriedadeAlterada();
            }
        }

        public bool IsRegraSaidaListar
        {
            get => _isRegraSaidaListar;
            set
            {
                if (value == _isRegraSaidaListar) return;
                _isRegraSaidaListar = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarRegraSaida
        {
            get => _isGerenciarRegraSaida;
            set
            {
                if (value == _isGerenciarRegraSaida) return;
                _isGerenciarRegraSaida = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarCfop
        {
            get => _isGerenciarCfop;
            set
            {
                if (value == _isGerenciarCfop) return;
                _isGerenciarCfop = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarTipoDocumento
        {
            get => _isGerenciarTipoDocumento;
            set
            {
                if (value == _isGerenciarTipoDocumento) return;
                _isGerenciarTipoDocumento = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarVeiculo
        {
            get => _isGerenciarVeiculo;
            set
            {
                if (value == _isGerenciarVeiculo) return;
                _isGerenciarVeiculo = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerencairNcm
        {
            get => _isGerencairNcm;
            set
            {
                if (value == _isGerencairNcm) return;
                _isGerencairNcm = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarProdutoLocalizacao
        {
            get => _isGerenciarProdutoLocalizacao;
            set
            {
                if (value == _isGerenciarProdutoLocalizacao) return;
                _isGerenciarProdutoLocalizacao = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarProdutoGrupo
        {
            get => _isGerenciarProdutoGrupo;
            set
            {
                if (value == _isGerenciarProdutoGrupo) return;
                _isGerenciarProdutoGrupo = value;
                PropriedadeAlterada();
            }
        }

        public bool IsGerenciarProdutoUnidade
        {
            get => _isGerenciarProdutoUnidade;
            set
            {
                _isGerenciarProdutoUnidade = value;
                PropriedadeAlterada();
            }
        }

        public bool IsRelatorioListar
        {
            get => _isRelatorioListar;
            set
            {
                if (value == _isRelatorioListar) return;
                _isRelatorioListar = value;
                PropriedadeAlterada();
            }
        }

        public bool IsPermissaoGerarSintegra
        {
            get => _isPermissaoGerarSintegra;
            set
            {
                if (value == _isPermissaoGerarSintegra) return;
                _isPermissaoGerarSintegra = value;
                PropriedadeAlterada();
            }
        }

        public bool IsPermissaoDashboard
        {
            get => _isPermissaoDashboard;
            set
            {
                if (value == _isPermissaoDashboard) return;
                _isPermissaoDashboard = value;
                PropriedadeAlterada();
            }
        }

        public EmpresaSetupHelper ConsultarSituacaoEmpresa()
        {
            try
            {
                var setup = new EmpresaSetupHelper();
                setup.ConsultarSituacaoEmpresa();

                return setup;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        public void Deslogar()
        {
            _sessaoSistema.UsuarioLogado = null;

            OnDeslogou();
        }

        private void OnDeslogou()
        {
            Deslogou?.Invoke(this, EventArgs.Empty);
        }
    }
}