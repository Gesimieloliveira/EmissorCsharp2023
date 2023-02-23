using System;
using System.Windows.Input;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Cfop
{
    public class CfopFormModel : ViewModel
    {
        private readonly CfopDTO _cfop;

        public string Codigo
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Descricao
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool ElegivelNfce
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public string SaveButtonContent
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public ICommand SalvarCommand => GetSimpleCommand(SalvarCommandHandler);

        public CfopFormModel(CfopDTO cfop)
        {
            _cfop = cfop;
        }

        public CfopForm GetView()
        {
            return new CfopForm(this);
        }

        public void Inicializar()
        {
            SaveButtonContent = "Salvar as alterações";

            Codigo = _cfop.Id;
            Descricao = _cfop.Descricao;
            ElegivelNfce = _cfop.ElegivelNfce;
        }

        private void SalvarCommandHandler(object obj)
        {
            try
            {
                _cfop.ElegivelNfce = ElegivelNfce;

                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorio = new RepositorioCfop(sessao);
                    repositorio.Alterar(_cfop);
                }

                DialogBox.MostraMensagemSalvouComSucesso();
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro("Erro ao salvar registro: " + ex.Message, ex);
            }
        }
    }
}