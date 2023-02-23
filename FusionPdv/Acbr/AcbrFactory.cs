using System;
using ACBrFramework.ECF;
using ACBrFramework.PAF;
using ACBrFramework.TEFD;
using FusionCore.FusionPdv.Models;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.EmpresaDesenvolvedora;

#pragma warning disable 618

namespace FusionPdv.Acbr
{
    public static class AcbrFactory
    {
        private static ACBrECF _acbrEcf;
        private static ACBrPAF _acbrPaf;
        private static ACBrTEFD _acbrTefd;

        public static ACBrECF ObterAcbrEcf()
        {
            return _acbrEcf ?? (_acbrEcf = new ACBrECF());
        }

        public static ACBrPAF ObterAcbrPaf()
        {
            return _acbrPaf ?? (_acbrPaf = new ACBrPAF());
        }

        public static ACBrTEFD ObterAcbrTefd(EntidadeTef configTef = null)
        {
            if (_acbrTefd != null) return _acbrTefd;

            if (configTef == null) throw new InvalidOperationException("Erro ao ler arquivo de informações do TEF");

            _acbrTefd = new ACBrTEFD
            {
                AutoEfetuarPagamento = false,
                AutoFinalizarCupom = false,
                MultiplosCartoes = true,
                AutoAtivar = true,
                EsperaSleep = 250,
                PathBackup = "C:\\SistemaFusion\\Backup\\Tef"
            };

            new ManipulaPasta(_acbrTefd.PathBackup).CriaPastaSeNaoExistir();

            _acbrTefd.TEFDial.ArqReq = configTef.ArqReq;
            _acbrTefd.TEFDial.ArqResp = configTef.ArqResp;
            _acbrTefd.TEFDial.ArqSTS = configTef.ArqSts;
            _acbrTefd.TEFDial.ArqTemp = configTef.ArqTemp;
            _acbrTefd.TEFDial.GPExeName = configTef.GpExeName;

            _acbrTefd.Identificacao.NomeAplicacao = ResponsavelLegal.NomeAplicacaoPdv;
            _acbrTefd.Identificacao.RazaoSocial = ResponsavelLegal.RazaoSocial;
            _acbrTefd.Identificacao.SoftwareHouse = ResponsavelLegal.RazaoSocial;
            _acbrTefd.Identificacao.VersaoAplicacao = ResponsavelLegal.VersaoAplicacaoPdv;

            return _acbrTefd;
        }

        public static void FecharTodosSemLancarErro()
        {
            try
            {
                FecharAcbrEcf();
            }
            catch (Exception)
            {
                //ignore
            }

            try
            {
                FecharAcbrPaf();
            }
            catch (Exception)
            {
                //ignore
            }

            try
            {
                FecharAcbrTef();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        public static void FecharAcbrEcf()
        {
            _acbrEcf?.Desativar();
        }

        public static void FecharAcbrPaf()
        {
            _acbrPaf?.Dispose();
        }

        public static void FecharAcbrTef()
        {
            _acbrTefd?.DesInicializar(TefTipo.TefDial);
            _acbrTefd?.Dispose();
        }
    }
}