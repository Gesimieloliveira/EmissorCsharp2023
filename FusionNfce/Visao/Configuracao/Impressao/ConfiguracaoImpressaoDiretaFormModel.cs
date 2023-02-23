using System.Net.Sockets;
using System.Threading;
using System.Windows.Input;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.Acbr;
using FusionCore.FusionAdm.Emissores.Flags;
using FusionCore.FusionNfce.ImpressaoDireta;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using Ini.Net;

namespace FusionNfce.Visao.Configuracao.Impressao
{
    public class ConfiguracaoImpressaoDiretaFormModel : ViewModel
    {
        private const string CaminhoAcbrIni = @"C:\SistemaFusion\FusionAcbr\ACBrMonitor.ini";
        private const string SessaoAcbrIni = "PosPrinter";
        private const string ModeloAcbrIni = "Modelo";
        private const string PortaAcbrIni = "Porta";
        private const string PaginaDeCodigoAcbrIni = "PaginaDeCodigo";

        public ConfiguracaoImpressaoDiretaFormModel()
        {
            var iniAcbr = new IniFile(CaminhoAcbrIni);
            
            ObtemCodigficacaoXml(iniAcbr);

            ObtemModeloMiniImpressora(iniAcbr);

            ObtemPorta(iniAcbr);

            VerificaSeEstaAtivo();
        }

        private void VerificaSeEstaAtivo()
        {
            var impressaoDiretaAtiva = new SalvarImpressaoDireta().Ler();

            Ativo = impressaoDiretaAtiva.Ativa;
        }

        private void ObtemModeloMiniImpressora(IniFile iniAcbr)
        {
            var modeloImpressora = iniAcbr.ReadInteger(SessaoAcbrIni, ModeloAcbrIni);
            ModeloMiniImpressora = (ModeloMiniImpressora) modeloImpressora;
        }

        private void ObtemPorta(IniFile iniAcbr)
        {
            var porta = iniAcbr.ReadString(SessaoAcbrIni, PortaAcbrIni);
            NomePorta = porta.IsNullOrEmpty() ? "COM1" : porta;
        }

        private void ObtemCodigficacaoXml(IniFile iniAcbr)
        {
            var codificacaoArquivoXml = iniAcbr.ReadInteger(SessaoAcbrIni, PaginaDeCodigoAcbrIni);

            switch (codificacaoArquivoXml)
            {
                case -1:
                case 5:
                    CodificacaoArquivoXml = CodificacaoArquivoXml.UTF8;
                    break;
                case 6:
                    CodificacaoArquivoXml = CodificacaoArquivoXml.Windows1252;
                    break;
                default:
                    CodificacaoArquivoXml = CodificacaoArquivoXml.UTF8;
                    break;
            }
        }

        private ModeloMiniImpressora _modeloMiniImpressora;
        private string _nomePorta;
        private CodificacaoArquivoXml _codificacaoArquivoXml;
        private bool _ativo;

        public ModeloMiniImpressora ModeloMiniImpressora
        {
            get => _modeloMiniImpressora;
            set
            {
                if (value == _modeloMiniImpressora) return;
                _modeloMiniImpressora = value;
                PropriedadeAlterada();
            }
        }

        public string NomePorta
        {
            get => _nomePorta;
            set
            {
                if (value == _nomePorta) return;
                _nomePorta = value;
                PropriedadeAlterada();
            }
        }

        public CodificacaoArquivoXml CodificacaoArquivoXml
        {
            get => _codificacaoArquivoXml;
            set
            {
                if (value == _codificacaoArquivoXml) return;
                _codificacaoArquivoXml = value;
                PropriedadeAlterada();
            }
        }

        public bool Ativo
        {
            get => _ativo;
            set
            {
                _ativo = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandSalvar => GetSimpleCommand(SalvarAction);

        private void SalvarAction(object obj)
        {
            if (Ativo == false)
            {
                ModeloMiniImpressora = ModeloMiniImpressora.Nenhuma;
            }

            try
            {
                EnviaComandoAcbrMonitor(AcbrMonitorPlusComando.ACBr_DataHora);
                SalvarIni();
                Thread.Sleep(1000);
                ExecutaRotinaVerificarAtivacaoAcbr();

                var impressoraAtiva = new ImpressaoDiretaAtiva {Ativa = Ativo};
                new SalvarImpressaoDireta(impressoraAtiva).Salvar();

                SessaoSistemaNfce.ImpressaoDireta = impressoraAtiva;

                DialogBox.MostraInformacao("Impressão direta configurada com sucesso");
                OnFechar();
            }
            catch (SocketException)
            {
                DialogBox.MostraInformacao("Verifique se o ACBrMonitor está ativo e configurado");
            }
        }

        private void ExecutaRotinaVerificarAtivacaoAcbr()
        {
            using (var acbr = new AcbrMonitorPlus())
            {
                acbr.EnviarComando(AcbrMonitorPlusComando.ACBr_LerIni);
                acbr.EnviarComando(AcbrMonitorPlusComando.ESCPOS_Ativar);
                acbr.EnviarComando(AcbrMonitorPlusComando.ESCPOS_ImprimirLinha, "");
                acbr.EnviarComando(AcbrMonitorPlusComando.ESCPOS_Desativar);
            }
        }

        private void EnviaComandoAcbrMonitor(AcbrMonitorPlusComando comando, params string[] parametros)
        {
            using (var acbr = new AcbrMonitorPlus())
            {
                acbr.EnviarComando(comando, parametros);
            }
        }

        private void SalvarIni()
        {
            var iniAcbr = new IniFile(CaminhoAcbrIni);

            iniAcbr.WriteInteger(SessaoAcbrIni, ModeloAcbrIni, (int) ModeloMiniImpressora);
            iniAcbr.WriteString(SessaoAcbrIni, PortaAcbrIni, NomePorta);

            switch (CodificacaoArquivoXml)
            {
                case CodificacaoArquivoXml.Windows1252:
                    AlteraCodificacao(6, iniAcbr);
                    break;
                case CodificacaoArquivoXml.UTF8:
                    AlteraCodificacao(5, iniAcbr);
                    break;
            }
        }

        private void AlteraCodificacao(int codificacao, IniFile iniAcbr)
        {
            iniAcbr.WriteInteger(SessaoAcbrIni, PaginaDeCodigoAcbrIni, codificacao);
        }
    }
}