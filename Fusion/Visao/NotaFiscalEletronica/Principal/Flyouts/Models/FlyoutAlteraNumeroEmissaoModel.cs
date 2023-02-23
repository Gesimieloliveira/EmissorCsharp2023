using System;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Flyouts.Models
{
    public class FlyoutAlteraNumeroEmissaoModel : ViewModel
    {
        private readonly Nfeletronica _nfe;
        private readonly ISessaoManager _sessaoManager;

        public int NumeroDocumento
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string TextoAviso
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool IsOpen
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public FlyoutAlteraNumeroEmissaoModel(Nfeletronica nfe, ISessaoManager sessaoManager)
        {
            _nfe = nfe;
            _sessaoManager = sessaoManager;
            TextoAviso = "Alterar o número do documento manualmente não implicará " +
                         "na alteração do número sequencial do emissor.";
        }

        public event EventHandler FlyoutClosed;

        public void Inicializar()
        {
            NumeroDocumento = _nfe.NumeroEmissao;
        }

        public void SalvarAlteracao()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                _nfe.NumeroEmissao = NumeroDocumento;

                var repositorio = new RepositorioNfe(sessao);
                repositorio.SalvarAlteracoes(_nfe);
            }

            DialogBox.MostraInformacao("Numero alterado com sucesso");
            IsOpen = false;
            FlyoutClosed?.Invoke(this, EventArgs.Empty);
        }
    }
}