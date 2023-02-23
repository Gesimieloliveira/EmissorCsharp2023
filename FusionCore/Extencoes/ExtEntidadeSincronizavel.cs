using FusionCore.NfceSincronizador;
using FusionCore.NfceSincronizador.Financeiro;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.Sync;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.NfceSincronizador.Sync.Clientes;
using FusionCore.NfceSincronizador.Sync.Configuracoes;
using FusionCore.NfceSincronizador.Sync.EmissoresFiscais;
using FusionCore.NfceSincronizador.Sync.Municipios;
using FusionCore.NfceSincronizador.Sync.Produtos;
using FusionCore.NfceSincronizador.Sync.TabelasPrecos;
using FusionCore.NfceSincronizador.Sync.TefsPos;
using FusionCore.NfceSincronizador.Sync.TerminaisOffline;
using FusionCore.NfceSincronizador.Sync.Usuarios;
using ReceberConfiguracaoBalanca = FusionCore.NfceSincronizador.Sync.Configuracoes.ReceberConfiguracaoBalanca;
using ReceberConfiguracaoEstoque = FusionCore.NfceSincronizador.Sync.Configuracoes.ReceberConfiguracaoEstoque;
using ReceberEmpresa = FusionCore.NfceSincronizador.Sync.Empresas.ReceberEmpresa;
using ReceberProduto = FusionCore.NfceSincronizador.Sync.Produtos.ReceberProduto;
using ReceberUsuario = FusionCore.NfceSincronizador.Sync.Usuarios.ReceberUsuario;

namespace FusionCore.Extencoes
{
    public static class ExtEntidadeSincronizavel
    {
        public static void Sincronizar(this EntidadeSincronizavel entidadeSincronizavel)
        {
            ISincronizavelPadrao sincronizar = null;

            switch (entidadeSincronizavel)
            {
                case EntidadeSincronizavel.Usuario:
                    sincronizar = new ReceberUsuario();
                    break;

                case EntidadeSincronizavel.EstadoUf:
                    sincronizar = new ReceberEstadoUf();
                    break;

                case EntidadeSincronizavel.Cidade:
                    sincronizar = new ReceberCidade();
                    break;

                case EntidadeSincronizavel.Empresa:
                    sincronizar = new ReceberEmpresa();
                    break;

                case EntidadeSincronizavel.Cfop:
                    sincronizar = new ReceberCfop();
                    break;

                case EntidadeSincronizavel.EmissorFiscal:
                    sincronizar = new ReceberEmissorFiscal();
                    break;

                case EntidadeSincronizavel.ProdutoUnidade:
                    sincronizar = new ReceberProdutoUnidade();
                    break;

                case EntidadeSincronizavel.Produto:
                    sincronizar = new ReceberProduto();
                    break;

                case EntidadeSincronizavel.Pessoa:
                    sincronizar = new ReceberPessoa();
                    break;

                case EntidadeSincronizavel.ConfiguracaoEmail:
                    sincronizar = new ReceberConfiguracaoEmail();
                    break;

                case EntidadeSincronizavel.Nfce:
                    sincronizar = new EnviarNfce();
                    break;

                case EntidadeSincronizavel.EmissorFiscalNfce:
                    sincronizar = new EnviarNfceEmissorFiscalNfce();
                    break;

                case EntidadeSincronizavel.TerminalOffline:
                    sincronizar = new SincronizarTerminalOffilineConfiguracaoAtualizar();
                    break;

                case EntidadeSincronizavel.ProdutoEstoqueEvento:
                    sincronizar = new EnviarEventoEstoque();
                    break;

                case EntidadeSincronizavel.Ibpt:
                    sincronizar = new ReceberIbpt();
                    break;

                case EntidadeSincronizavel.ConfiguracaoFinanceiro:
                    sincronizar = new ReceberConfiguracaoFinanceiroNfce();
                    break;

                case EntidadeSincronizavel.ConfiguracaoFrenteCaixa:
                    sincronizar = new ReceberconfiguracaoFrenteCaixa();
                    break;

                case EntidadeSincronizavel.ConfiguracaoEstoque:
                    sincronizar = new ReceberConfiguracaoEstoque();
                    break;

                case EntidadeSincronizavel.Pos:
                    sincronizar = new ReceberPos();
                    break;

                case EntidadeSincronizavel.Inutilizacao:
                    sincronizar = new EnviarInutilizacao();
                    break;

                case EntidadeSincronizavel.RegraTributacaoSaida:
                    sincronizar = new ReceberRegraTributacaoSaida();
                    break;

                case EntidadeSincronizavel.Balanca:
                    sincronizar = new ReceberConfiguracaoBalanca();
                    break;

                case EntidadeSincronizavel.ResponsavelTecnico:
                    sincronizar = new ReceberResponsavelTecnico();
                    break;

                case EntidadeSincronizavel.Papel:
                    sincronizar = new ReceberPapel();
                    break;

                case EntidadeSincronizavel.TipoDocumento:
                    var servico = new ServicoReceberTipoDocumento(new SessaoSyncFactory());
                    servico.ReceberDados();
                    return;

                case EntidadeSincronizavel.TabelaPreco:
                    sincronizar = new ReceberTabelaPreco();
                    break;
            }

            sincronizar?.RealizarSincronizacao();
        }
    }
}