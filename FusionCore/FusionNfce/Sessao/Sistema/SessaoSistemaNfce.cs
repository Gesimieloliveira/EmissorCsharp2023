using System.Text;
using DFe.Utils;
using FusionCore.DI;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.CertificadosDigitais;
using FusionCore.FusionNfce.ConfiguracaoBalanca;
using FusionCore.FusionNfce.ConfiguracaoTerminal;
using FusionCore.FusionNfce.Configuracoes;
using FusionCore.FusionNfce.Empresa;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.ImpressaoDireta;
using FusionCore.FusionNfce.Preferencias;
using FusionCore.FusionNfce.Tef;
using FusionCore.FusionNfce.Usuario;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Seguranca.Licenciamento.Dominio;
using FusionCore.Sessao;
using FusionLibrary.Helper.Criptografia;

namespace FusionCore.FusionNfce.Sessao.Sistema
{
    public static class SessaoSistemaNfce
    {
        public static UsuarioNfce Usuario { get; set; }
        public static ConfiguracaoTerminalNfce Configuracao { get; set; }
        public static TipoEmissao TipoEmissao => GetTipoEmissao();
        public static ConfiguracaoFrenteCaixaNfce ConfiguracaoFrenteCaixa { get; set; }
        public static object StatusVenda { get; set; }
        public static AcessoConcedido AcessoConcedido { get; set; }
        public static string MensagemErroAcesso { get; set; }
        public static NfceContingencia Contingencia { get; set; }
        public static ImpressaoDiretaAtiva ImpressaoDireta { get; set; }
        public static BalancaNfce ConfiguracoesBalanca { get; set; }
        public static ConfigTef ConfigTef { get; set; }
        public static ISessaoManager SessaoManager { get; } = new SessaoManagerNfce();
        public static IControleCaixaProvider CaixaProvider { get; set; }
        public static PreferenciaTerminal Preferencia { get; set; }
        public static CertificadoDigitalNfce CertificadoDigital { get; set; }

        public static bool EstaEmContingencia()
        {
            return Contingencia != null && Contingencia.Ativa;
        }

        public static bool IsEmissorNFce()
        {
            if (Configuracao == null) return true;
            return Configuracao.EmissorFiscal.FlagNfce;
        }

        public static bool IsEmissorSat()
        {
            return Configuracao.EmissorFiscal.FlagSat;
        }

        public static bool IsMFe()
        {
            var emissorSat = Configuracao.EmissorFiscal.EmissorFiscalSat;

            if (emissorSat == null) return false;

            return emissorSat.IsMFe;
        }

        private static TipoEmissao GetTipoEmissao()
        {
            return EstaEmContingencia() ? TipoEmissao.ContigenciaOfflineNFCe : TipoEmissao.Normal;
        }

        public static string ChaveAcessoValidadorMFe()
        {
            return Configuracao.EmissorFiscal
                .EmissorFiscalSat.ChaveAcessoValidador;
        }

        public static EmpresaNfce Empresa()
        {
            return Configuracao.EmissorFiscal.Empresa;
        }

        public static StringBuilder ObservacaoPadrao()
        {
            return new StringBuilder(Configuracao.ObservacaoPadrao.TrimOrEmpty()).Append(" ");
        }

        public static bool AmbienteSefazProducao()
        {
            return Configuracao.EmissorFiscal.Ambiente == TipoAmbiente.Producao;
        }

        public static DadosSefazNfce GetDadosSefaz()
        {
            var dados = new DadosSefazNfce(
                modelo: Configuracao.EmissorFiscal.Modelo,
                ambiente: Configuracao.EmissorFiscal.Ambiente,
                ibgeEstadoEmissao: Empresa().Estado.CodigoIbge,
                certificado: new ConfiguracaoCertificado
                {
                    TipoCertificado = CertificadoDigital.TipoToZeus(),
                    Senha = SimetricaCrip.Descomputar(CertificadoDigital.Senha),
                    Arquivo = CertificadoDigital.CaminhoArquivo,
                    CacheId = Configuracao.EmissorFiscal.Id.ToString(),
                    ManterDadosEmCache = true,
                    Serial = SimetricaCrip.Descomputar(CertificadoDigital.SerialRepositorio)
                },
                protocoloSeguranca: Configuracao.EmissorFiscal.ProtocoloSeguranca
            );

            return dados;
        }

        public static TipoAmbiente GetAmbienteSefaz()
        {
            return Configuracao.EmissorFiscal.Ambiente;
        }
    }
}