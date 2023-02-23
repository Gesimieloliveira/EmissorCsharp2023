using System;
using System.Windows.Input;
using Fusion.Visao.Pessoa.Picker;
using Fusion.Visao.Validacoes.CteOs;
using FusionCore.Excecoes;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Helpers.Pessoa;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronicoOs.Emitir.Aba.Model
{
    public class AbaEmitenteTomadorModel : AbaCTeOSViewModel
    {
        private PessoaEntidade _tomador;
        private string _tomadorDocumentoUnico;
        private string _tomadorCep;
        private string _tomadorLogradouro;
        private string _tomadorNumero;
        private string _tomadorBairro;
        private string _tomadorTelefone;
        private string _tomadorSiglaUF;
        private string _tomadorNomeCidade;
        private string _tomadorInscricaoEstadual;
        private EmpresaDTO _emitente;
        public event EventHandler<AbaEmitenteTomadorModel> Anterior;
        public event EventHandler<AbaEmitenteTomadorModel> Proximo;

        protected virtual void OnAnterior()
        {
            Anterior?.Invoke(this, this);
        }

        public ICommand PassoAnteriorCommand => GetSimpleCommand(PassoAnteriorAction);
        public ICommand CommandBuscarTomador => GetSimpleCommand(BuscartomadorAction);
        public ICommand ProximoPassoCommand => GetSimpleCommand(ProximoPassoAction);

        public PessoaEntidade Tomador
        {
            get => _tomador;
            set
            {
                _tomador = value;
                PropriedadeAlterada();

                AtualizarModelTomador();
            }
        }

        private void AtualizarModelTomador()
        {
            TomadorDocumentoUnico = _tomador.GetDocumentoUnico();

            var tomadorEndereco = _tomador.GetEnderecoPrincipal();
 
            TomadorInscricaoEstadual = _tomador.InscricaoEstadual;
            TomadorCep = tomadorEndereco.Cep;
            TomadorLogradouro = tomadorEndereco.Logradouro;
            TomadorNumero = tomadorEndereco.Numero;
            TomadorBairro = tomadorEndereco.Bairro;
            TomadorSiglaUF = tomadorEndereco.Cidade.SiglaUf;
            TomadorNomeCidade = tomadorEndereco.Cidade.Nome;

            var telefone = _tomador.GetPrimeiroTelefone();
            TomadorTelefone = telefone?.Numero;
        }

        public string TomadorDocumentoUnico
        {
            get => _tomadorDocumentoUnico;
            set
            {
                if (value == _tomadorDocumentoUnico) return;
                _tomadorDocumentoUnico = value;
                PropriedadeAlterada();
            }
        }

        public string TomadorCep
        {
            get { return _tomadorCep; }
            set
            {
                if (value == _tomadorCep) return;
                _tomadorCep = value;
                PropriedadeAlterada();
            }
        }

        public string TomadorLogradouro
        {
            get { return _tomadorLogradouro; }
            set
            {
                if (value == _tomadorLogradouro) return;
                _tomadorLogradouro = value;
                PropriedadeAlterada();
            }
        }

        public string TomadorNumero
        {
            get { return _tomadorNumero; }
            set
            {
                if (value == _tomadorNumero) return;
                _tomadorNumero = value;
                PropriedadeAlterada();
            }
        }

        public string TomadorBairro
        {
            get { return _tomadorBairro; }
            set
            {
                if (value == _tomadorBairro) return;
                _tomadorBairro = value;
                PropriedadeAlterada();
            }
        }

        public string TomadorTelefone
        {
            get { return _tomadorTelefone; }
            set
            {
                if (value == _tomadorTelefone) return;
                _tomadorTelefone = value;
                PropriedadeAlterada();
            }
        }

        public string TomadorSiglaUF
        {
            get { return _tomadorSiglaUF; }
            set
            {
                if (value == _tomadorSiglaUF) return;
                _tomadorSiglaUF = value;
                PropriedadeAlterada();
            }
        }

        public string TomadorNomeCidade
        {
            get { return _tomadorNomeCidade; }
            set
            {
                if (value == _tomadorNomeCidade) return;
                _tomadorNomeCidade = value;
                PropriedadeAlterada();
            }
        }

        public string TomadorInscricaoEstadual
        {
            get { return _tomadorInscricaoEstadual; }
            set
            {
                if (value == _tomadorInscricaoEstadual) return;
                _tomadorInscricaoEstadual = value;
                PropriedadeAlterada();
            }
        }

        private void BuscartomadorAction(object obj)
        {
            var pickerModel = new PessoaPickerModel(new PessoaEngine());
            pickerModel.PickItemEvent += SelecionaTomadorPicker;
            pickerModel.GetPickerView().ShowDialog();
        }

        private void SelecionaTomadorPicker(object sender, GridPickerEventArgs e)
        {
            var tomador = e.GetItem<PessoaEntidade>();

            ValidacaoTomadorCTeOs.Executar(tomador);

            Tomador = tomador;
        }

        private void PassoAnteriorAction(object obj)
        {
            OnAnterior();
        }

        public EmpresaDTO Emitente
        {
            get { return _emitente; }
            set
            {
                if (Equals(value, _emitente)) return;
                _emitente = value;
                PropriedadeAlterada();
            }
        }

        public void ComEmitente(EmpresaDTO emitente)
        {
            Emitente = emitente;
        }

        private void ProximoPassoAction(object obj)
        {
            try
            {
                if (Tomador == null)
                    CriaExcecao.OperacaoInvalida("Selecionar um tomador");

                ValidacaoTomadorCTeOs.Executar(Tomador);
                OnProximo();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        protected virtual void OnProximo()
        {
            Proximo?.Invoke(this, this);
        }

        public void ComCteOs(CteOs cteOs)
        {
            Tomador = cteOs.Tomador;
            Emitente = cteOs.Emitente;
        }
    }
}