using System.Windows.Input;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionLibrary.VisaoModel;
using FusionNfce.AutorizacaoSatFiscal.Configuracao.Configuracao;
using FusionWPF.Base.Utils.Dialogs;
using Microsoft.Win32;

namespace FusionNfce.Visao.Principal.ConfiguracoesSAT
{
    public class ConfiguracoesSATFormModel : ViewModel
    {

        public ConfiguracoesSATFormModel()
        {
            var config = ConfiguracaoDllSatCriador.CriaXmlConfigSeNaoExistir(SessaoSistemaNfce.Configuracao.EmissorFiscal.EmissorFiscalSat);

            ModeloSat = config.ModeloSat;
            CaminhoDll = config.CaminhoDll;
        }

        private ModeloSatFusion _modeloSat;
        private string _caminhoDll;

        public ModeloSatFusion ModeloSat
        {
            get { return _modeloSat; }
            set
            {
                if (value == _modeloSat) return;
                _modeloSat = value;
                PropriedadeAlterada();
            }
        }

        public string CaminhoDll
        {
            get { return _caminhoDll; }
            set
            {
                if (value == _caminhoDll) return;
                _caminhoDll = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandBuscarDll => GetSimpleCommand(BuscarDllAction);

        public ICommand CommandSalvar => GetSimpleCommand(SalvarAction);

        private void SalvarAction(object obj)
        {
            var configuracao = ConfiguracaoDllSatCriador.BuscarConfiguracao();

            configuracao.CaminhoDll = CaminhoDll;
            configuracao.ModeloSat = ModeloSat;

            ConfiguracaoDllSatCriador.Atualizar(configuracao);

            DialogBox.MostraInformacao("Configurações Salva com Sucesso");
        }

        private void BuscarDllAction(object obj)
        {
            var janelaArquivo = new OpenFileDialog
            {
                Filter = "Dll SAT Fiscal (*.dll)|*.dll"
            };

            if (janelaArquivo.ShowDialog() == true)
            {
                CaminhoDll = janelaArquivo.FileName;
            }
        }
    }
}