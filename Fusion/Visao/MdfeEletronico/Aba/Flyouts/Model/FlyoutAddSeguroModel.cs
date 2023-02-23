using System;
using System.Collections.ObjectModel;
using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model
{
    public class EventSeguro : EventArgs
    {
        public EventSeguro(FlyoutAddSeguroModel model)
        {
            Model = model;
        }

        public FlyoutAddSeguroModel Model { get; set; }
    }

    public class FlyoutAddSeguroModel : ViewModel
    {
        private bool _isOpen;
        private MDFeResponsavelSeguro _responsavelSeguro;
        private string _nomeSeguradora;
        private string _cnpjSeguradora;
        private string _numeroApolice;
        private string _numeroAverbacao;
        private ObservableCollection<MdfeSeguroAverbacao> _averbacoes = new ObservableCollection<MdfeSeguroAverbacao>();
        private MdfeSeguroAverbacao _averbacaoSelecionada;

        public event EventHandler<EventSeguro> SalvarSeguroHandler;  

        public MDFeResponsavelSeguro ResponsavelSeguro
        {
            get => _responsavelSeguro;
            set
            {
                if (value == _responsavelSeguro) return;
                _responsavelSeguro = value;
                PropriedadeAlterada();
            }
        }

        public string DocumentoResponsavel
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (value == _isOpen) return;
                _isOpen = value;
                PropriedadeAlterada();
            }
        }

        public string NomeSeguradora
        {
            get => _nomeSeguradora;
            set
            {
                if (value == _nomeSeguradora) return;
                _nomeSeguradora = value;
                PropriedadeAlterada();
            }
        }

        public string CnpjSeguradora
        {
            get => _cnpjSeguradora;
            set
            {
                if (value == _cnpjSeguradora) return;
                _cnpjSeguradora = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroApolice
        {
            get => _numeroApolice;
            set
            {
                if (value == _numeroApolice) return;
                _numeroApolice = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<MdfeSeguroAverbacao> Averbacoes
        {
            get => _averbacoes;
            set
            {
                _averbacoes = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroAverbacao
        {
            get => _numeroAverbacao;
            set
            {
                if (value == _numeroAverbacao) return;
                _numeroAverbacao = value;
                PropriedadeAlterada();
            }
        }

        public MdfeSeguroAverbacao AverbacaoSelecionada
        {
            get => _averbacaoSelecionada;
            set
            {
                if (Equals(value, _averbacaoSelecionada)) return;
                _averbacaoSelecionada = value;
                PropriedadeAlterada();
            }
        }


        public void LimparCampos()
        {
            ResponsavelSeguro = MDFeResponsavelSeguro.ContratanteServicoTransporte;
            DocumentoResponsavel = string.Empty;
            NomeSeguradora = string.Empty;
            CnpjSeguradora = string.Empty;
            NumeroApolice = string.Empty;
            NumeroAverbacao = string.Empty;
            Averbacoes = new ObservableCollection<MdfeSeguroAverbacao>();
            AverbacaoSelecionada = null;
        }

        public void SalvarSeguro()
        {
            NomeSeguradora = NomeSeguradora.TrimOrEmpty();
            CnpjSeguradora = CnpjSeguradora.TrimOrEmpty();
            NumeroApolice = NumeroApolice.TrimOrEmpty();
            NumeroAverbacao = NumeroAverbacao.TrimOrEmpty();

            OnSalvarSeguroHandler(this);
            LimparCampos();
        }

        protected virtual void OnSalvarSeguroHandler(FlyoutAddSeguroModel model)
        {
            SalvarSeguroHandler?.Invoke(this, new EventSeguro(model));
        }

        public void DeletarAverbacao()
        {
            Averbacoes.Remove(AverbacaoSelecionada);
        }

        public void IncluirAverbacao()
        {
            if (NumeroAverbacao.IsNullOrEmpty())
                throw new InvalidOperationException("Averbação digitada inválida, tenta novamente.");

            if (Averbacoes.Count >= 40)
            {
                throw new InvalidOperationException("É permitido apenas 40 averbações.");
            }

            Averbacoes.Add(new MdfeSeguroAverbacao
            {
                Averbacao = NumeroAverbacao
            });

            NumeroAverbacao = string.Empty;
        }
    }
}