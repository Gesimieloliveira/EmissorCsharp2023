using System;
using FusionCore.FusionPdv.Setup.BD;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv.Visao.ConexaoBancoDados
{
    public class ConexaoPdvModel : ViewModel
    {
        private readonly ConexaoSetup _setup;

        public ConexaoPdvModel()
        {
            _setup = new ConexaoSetup();
        }

        public ConexaoCfg Adm
        {
            get => GetValue<ConexaoCfg>();
            set => SetValue(value);
        }

        public ConexaoCfg Pdv
        {
            get => GetValue<ConexaoCfg>();
            set => SetValue(value);
        }

        public void CarregarDados()
        {
            try
            {
                if (!_setup.ExisteArquivoConexao)
                {
                    _setup.CriarArquivoConexao();
                }

                var container = _setup.LerArquivoConexao();

                Adm = container.ConexaoAdm;
                Pdv = container.ConexaoPdv;
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        public void SalvarConfiguracoes()
        {
            _setup.Armazenar(new ContainerCfg(Adm, Pdv));
        }
    }
}