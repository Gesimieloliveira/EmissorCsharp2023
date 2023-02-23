using System;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Contratos;

namespace Fusion.Visao.Pessoa
{
    public sealed class PessoaFormInicioModel : ViewModel, IChildContext
    {
        public string TituloChild => "Pessoa - Opções inicial";
        public event EventHandler SolicitaFechamento;
        public event EventHandler ConsultaCnpj;
        public event EventHandler PessoaFisica;
        public event EventHandler PessoaJuridica;

        private void OnSolicitaFechamento()
        {
            SolicitaFechamento?.Invoke(this, EventArgs.Empty);
        }

        public void ConsultaCnpjHandler()
        {
            ConsultaCnpj?.Invoke(this, EventArgs.Empty);
            OnSolicitaFechamento();
        }

        public void PessoaJuridicaHandler()
        {
            PessoaJuridica?.Invoke(this, EventArgs.Empty);
            OnSolicitaFechamento();
        }

        public void PessoaFisicaHandler()
        {
            PessoaFisica?.Invoke(this, EventArgs.Empty);
            OnSolicitaFechamento();
        }
    }
}