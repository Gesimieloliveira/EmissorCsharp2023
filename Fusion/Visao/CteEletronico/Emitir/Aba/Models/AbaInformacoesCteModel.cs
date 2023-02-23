using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using Fusion.Visao.CteEletronico.Emitir.Flyouts.Models;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.CteEletronico.Emitir.Aba.Models
{
    public class RetornoAbaInformacoes : EventArgs
    {
        public AbaInformacoesCteModel AbaInformacoesCteModel { get; set; }
    }

    public class AbaInformacoesCteModel : ViewModel
    {
        private readonly Cte _cte;
        private bool _selecionado;
        private string _remetenteDocumentoUnico;
        private string _remetenteNome;
        private string _remetenteInscricaoEstadual;
        private CidadeDTO _remetenteCidade;
        private EstadoDTO _remetenteUF;
        private string _remetenteBairro;
        private string _remetenteNumero;
        private string _remetenteLogradouro;
        private string _remetenteCep;
        private string _remetenteTelefone;
        private string _destinatarioDocumentoUnico;
        private string _destinatarioNome;
        private string _destinatarioInscricaoEstadual;
        private CidadeDTO _destinatarioCidade;
        private EstadoDTO _destinatarioUF;
        private string _destinatarioBairro;
        private string _destinatarioNumero;
        private string _destinatarioLogradouro;
        private string _destinatarioCep;
        private string _destinatarioTelefone;
        private string _expedidorNome;
        private string _expedidorDocumentoUnico;
        private string _expedidorInscricaoEstadual;
        private string _expedidorCep;
        private string _expedidorLogradouro;
        private string _expedidorNumero;
        private string _expedidorBairro;
        private string _expedidorTelefone;
        private EstadoDTO _expedidorUF;
        private CidadeDTO _expedidorCidade;
        private string _recebedorNome;
        private string _recebedorDocumentoUnico;
        private string _recebedorInscricaoEstadual;
        private string _recebedorCep;
        private string _recebedorLogradouro;
        private string _recebedorNumero;
        private string _recebedorBairro;
        private string _recebedorTelefone;
        private EstadoDTO _recebedorUF;
        private CidadeDTO _recebedorCidade;
        private CidadeDTO _tomadorCidade;
        private string _tomadorNome;
        private string _tomadorDocumentoUnico;
        private string _tomadorInscricaoEstadual;
        private string _tomadorCep;
        private string _tomadorLogradouro;
        private string _tomadorNumero;
        private string _tomadorBairro;
        private string _tomadorTelefone;
        private EstadoDTO _tomadorUF;
        private TipoTomador _tipoTomador = TipoTomador.Outros;
        private bool _tomadorOutro;
        private bool _habilitado;

        public bool Habilitado
        {
            get { return _habilitado; }
            set
            {
                if (value == _habilitado) return;
                _habilitado = value;
                PropriedadeAlterada();
            }
        }

        public PessoaEntidade Remetente
        {
            get { return GetValue<PessoaEntidade>(); }
            set { SetValue(value); }
        }

        public string RemetenteNome
        {
            get { return _remetenteNome; }
            set
            {
                if (value == _remetenteNome) return;
                _remetenteNome = value;
                PropriedadeAlterada();
            }
        }

        public string RemetenteDocumentoUnico
        {
            get { return _remetenteDocumentoUnico; }
            set
            {
                if (value == _remetenteDocumentoUnico) return;
                _remetenteDocumentoUnico = value;
                PropriedadeAlterada();
            }
        }

        public string RemetenteInscricaoEstadual
        {
            get { return _remetenteInscricaoEstadual; }
            set
            {
                if (value == _remetenteInscricaoEstadual) return;
                _remetenteInscricaoEstadual = value;
                PropriedadeAlterada();
            }
        }

        public string RemetenteCep
        {
            get { return _remetenteCep; }
            set
            {
                if (value == _remetenteCep) return;
                _remetenteCep = value;
                PropriedadeAlterada();
            }
        }

        public string RemetenteLogradouro
        {
            get { return _remetenteLogradouro; }
            set
            {
                if (value == _remetenteLogradouro) return;
                _remetenteLogradouro = value;
                PropriedadeAlterada();
            }
        }

        public string RemetenteNumero
        {
            get { return _remetenteNumero; }
            set
            {
                if (value == _remetenteNumero) return;
                _remetenteNumero = value;
                PropriedadeAlterada();
            }
        }

        public string RemetenteBairro
        {
            get { return _remetenteBairro; }
            set
            {
                if (value == _remetenteBairro) return;
                _remetenteBairro = value;
                PropriedadeAlterada();
            }
        }

        public string RemetenteTelefone
        {
            get { return _remetenteTelefone; }
            set
            {
                if (value == _remetenteTelefone) return;
                _remetenteTelefone = value;
                PropriedadeAlterada();
            }
        }

        public EstadoDTO RemetenteUF
        {
            get { return _remetenteUF; }
            set
            {
                if (Equals(value, _remetenteUF)) return;
                _remetenteUF = value;
                PropriedadeAlterada();
            }
        }

        public CidadeDTO RemetenteCidade
        {
            get { return _remetenteCidade; }
            set
            {
                if (Equals(value, _remetenteCidade)) return;
                _remetenteCidade = value;
                PropriedadeAlterada();
            }
        }

        public PessoaEntidade Destinatario
        {
            get { return GetValue<PessoaEntidade>(); }
            set { SetValue(value); }
        }

        public string DestinatarioNome
        {
            get { return _destinatarioNome; }
            set
            {
                if (value == _destinatarioNome) return;
                _destinatarioNome = value;
                PropriedadeAlterada();
            }
        }

        public string DestinatarioDocumentoUnico
        {
            get { return _destinatarioDocumentoUnico; }
            set
            {
                if (value == _destinatarioDocumentoUnico) return;
                _destinatarioDocumentoUnico = value;
                PropriedadeAlterada();
            }
        }

        public string DestinatarioInscricaoEstadual
        {
            get { return _destinatarioInscricaoEstadual; }
            set
            {
                if (value == _destinatarioInscricaoEstadual) return;
                _destinatarioInscricaoEstadual = value;
                PropriedadeAlterada();
            }
        }

        public string DestinatarioCep
        {
            get { return _destinatarioCep; }
            set
            {
                if (value == _destinatarioCep) return;
                _destinatarioCep = value;
                PropriedadeAlterada();
            }
        }

        public string DestinatarioLogradouro
        {
            get { return _destinatarioLogradouro; }
            set
            {
                if (value == _destinatarioLogradouro) return;
                _destinatarioLogradouro = value;
                PropriedadeAlterada();
            }
        }

        public string DestinatarioNumero
        {
            get { return _destinatarioNumero; }
            set
            {
                if (value == _destinatarioNumero) return;
                _destinatarioNumero = value;
                PropriedadeAlterada();
            }
        }

        public string DestinatarioBairro
        {
            get { return _destinatarioBairro; }
            set
            {
                if (value == _destinatarioBairro) return;
                _destinatarioBairro = value;
                PropriedadeAlterada();
            }
        }

        public string DestinatarioTelefone
        {
            get { return _destinatarioTelefone; }
            set
            {
                if (value == _destinatarioTelefone) return;
                _destinatarioTelefone = value;
                PropriedadeAlterada();
            }
        }

        public EstadoDTO DestinatarioUF
        {
            get { return _destinatarioUF; }
            set
            {
                if (Equals(value, _destinatarioUF)) return;
                _destinatarioUF = value;
                PropriedadeAlterada();
            }
        }

        public CidadeDTO DestinatarioCidade
        {
            get { return _destinatarioCidade; }
            set
            {
                if (Equals(value, _destinatarioCidade)) return;
                _destinatarioCidade = value;
                PropriedadeAlterada();
            }
        }

        public PessoaEntidade Expedidor
        {
            get { return GetValue<PessoaEntidade>(); }
            set { SetValue(value); }
        }

        public string ExpedidorNome
        {
            get { return _expedidorNome; }
            set
            {
                if (value == _expedidorNome) return;
                _expedidorNome = value;
                PropriedadeAlterada();
            }
        }

        public string ExpedidorDocumentoUnico
        {
            get { return _expedidorDocumentoUnico; }
            set
            {
                if (value == _expedidorDocumentoUnico) return;
                _expedidorDocumentoUnico = value;
                PropriedadeAlterada();
            }
        }

        public string ExpedidorInscricaoEstadual
        {
            get { return _expedidorInscricaoEstadual; }
            set
            {
                if (value == _expedidorInscricaoEstadual) return;
                _expedidorInscricaoEstadual = value;
                PropriedadeAlterada();
            }
        }

        public string ExpedidorCep
        {
            get { return _expedidorCep; }
            set
            {
                if (value == _expedidorCep) return;
                _expedidorCep = value;
                PropriedadeAlterada();
            }
        }

        public string ExpedidorLogradouro
        {
            get { return _expedidorLogradouro; }
            set
            {
                if (value == _expedidorLogradouro) return;
                _expedidorLogradouro = value;
                PropriedadeAlterada();
            }
        }

        public string ExpedidorNumero
        {
            get { return _expedidorNumero; }
            set
            {
                if (value == _expedidorNumero) return;
                _expedidorNumero = value;
                PropriedadeAlterada();
            }
        }

        public string ExpedidorBairro
        {
            get { return _expedidorBairro; }
            set
            {
                if (value == _expedidorBairro) return;
                _expedidorBairro = value;
                PropriedadeAlterada();
            }
        }

        public string ExpedidorTelefone
        {
            get { return _expedidorTelefone; }
            set
            {
                if (value == _expedidorTelefone) return;
                _expedidorTelefone = value;
                PropriedadeAlterada();
            }
        }

        public EstadoDTO ExpedidorUF
        {
            get { return _expedidorUF; }
            set
            {
                if (Equals(value, _expedidorUF)) return;
                _expedidorUF = value;
                PropriedadeAlterada();
            }
        }

        public CidadeDTO ExpedidorCidade
        {
            get { return _expedidorCidade; }
            set
            {
                if (Equals(value, _expedidorCidade)) return;
                _expedidorCidade = value;
                PropriedadeAlterada();
            }
        }

        public PessoaEntidade Recebedor
        {
            get { return GetValue<PessoaEntidade>(); }
            set { SetValue(value); }
        }

        public string RecebedorNome
        {
            get { return _recebedorNome; }
            set
            {
                if (value == _recebedorNome) return;
                _recebedorNome = value;
                PropriedadeAlterada();
            }
        }

        public string RecebedorDocumentoUnico
        {
            get { return _recebedorDocumentoUnico; }
            set
            {
                if (value == _recebedorDocumentoUnico) return;
                _recebedorDocumentoUnico = value;
                PropriedadeAlterada();
            }
        }

        public string RecebedorInscricaoEstadual
        {
            get { return _recebedorInscricaoEstadual; }
            set
            {
                if (value == _recebedorInscricaoEstadual) return;
                _recebedorInscricaoEstadual = value;
                PropriedadeAlterada();
            }
        }

        public string RecebedorCep
        {
            get { return _recebedorCep; }
            set
            {
                if (value == _recebedorCep) return;
                _recebedorCep = value;
                PropriedadeAlterada();
            }
        }

        public string RecebedorLogradouro
        {
            get { return _recebedorLogradouro; }
            set
            {
                if (value == _recebedorLogradouro) return;
                _recebedorLogradouro = value;
                PropriedadeAlterada();
            }
        }

        public string RecebedorNumero
        {
            get { return _recebedorNumero; }
            set
            {
                if (value == _recebedorNumero) return;
                _recebedorNumero = value;
                PropriedadeAlterada();
            }
        }

        public string RecebedorBairro
        {
            get { return _recebedorBairro; }
            set
            {
                if (value == _recebedorBairro) return;
                _recebedorBairro = value;
                PropriedadeAlterada();
            }
        }

        public string RecebedorTelefone
        {
            get { return _recebedorTelefone; }
            set
            {
                if (value == _recebedorTelefone) return;
                _recebedorTelefone = value;
                PropriedadeAlterada();
            }
        }

        public EstadoDTO RecebedorUF
        {
            get { return _recebedorUF; }
            set
            {
                if (Equals(value, _recebedorUF)) return;
                _recebedorUF = value;
                PropriedadeAlterada();
            }
        }

        public CidadeDTO RecebedorCidade
        {
            get { return _recebedorCidade; }
            set
            {
                if (Equals(value, _recebedorCidade)) return;
                _recebedorCidade = value;
                PropriedadeAlterada();
            }
        }

        public PessoaEntidade Tomador
        {
            get { return GetValue<PessoaEntidade>(); }
            set { SetValue(value); }
        }

        public TipoTomador TipoTomador
        {
            get { return _tipoTomador; }
            set
            {
                if (value == _tipoTomador)
                {
                    PreparaTomador(value);
                    return;
                }

                var valorAntigo = _tipoTomador;
                _tipoTomador = value;

                try
                {
                    PreparaTomador(value);
                    PropriedadeAlterada();
                }
                catch (InvalidOperationException ex)
                {
                    DialogBox.MostraInformacao(ex.Message);
                    Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => TipoTomador = valorAntigo), DispatcherPriority.ApplicationIdle);
                }
            }
        }

        public string TomadorNome
        {
            get { return _tomadorNome; }
            set
            {
                if (value == _tomadorNome) return;
                _tomadorNome = value;
                PropriedadeAlterada();
            }
        }

        public string TomadorDocumentoUnico
        {
            get { return _tomadorDocumentoUnico; }
            set
            {
                if (value == _tomadorDocumentoUnico) return;
                _tomadorDocumentoUnico = value;
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

        public EstadoDTO TomadorUF
        {
            get { return _tomadorUF; }
            set
            {
                if (Equals(value, _tomadorUF)) return;
                _tomadorUF = value;
                PropriedadeAlterada();
            }
        }

        public CidadeDTO TomadorCidade
        {
            get { return _tomadorCidade; }
            set
            {
                if (Equals(value, _tomadorCidade)) return;
                _tomadorCidade = value;
                PropriedadeAlterada();
            }
        }

        public bool Selecionado
        {
            get { return _selecionado; }
            set
            {
                if (value == _selecionado) return;
                _selecionado = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandBuscarRemetente => GetSimpleCommand(BuscarRemente);
        public ICommand CommandBuscarDestinatario => GetSimpleCommand(BuscarDestinatario);
        public ICommand CommandBuscarExpedidor => GetSimpleCommand(BuscarExpedidor);
        public ICommand CommandBuscarRecebedor => GetSimpleCommand(BuscarRecebedor);
        public ICommand CommandBuscarTomador => GetSimpleCommand(BuscarTomador);

        public bool TomadorOutro
        {
            get { return _tomadorOutro; }
            set
            {
                if (value == _tomadorOutro) return;
                _tomadorOutro = value;
                PropriedadeAlterada();
            }
        }

        public bool IsUsarRemetenteComoDefault { get; set; }

        public AbaInformacoesCteModel(Cte cte)
        {
            _cte = cte;

            if (cte.PerfilCte != null)
            {
                IsUsarRemetenteComoDefault = cte.PerfilCte.RemetentePadrao;
            }
        }

        public event EventHandler<RetornoAbaInformacoes> ProximoPasso;
        public event EventHandler PassoAnterior;

        public void Anterior()
        {
            OnPassoAnterior();
        }

        public void Proximo()
        {
            OnProximoPasso();
        }

        protected virtual void OnProximoPasso()
        {
            Validacao();

            var retorno = new RetornoAbaInformacoes
            {
                AbaInformacoesCteModel = this
            };

            ProximoPasso?.Invoke(this, retorno);
        }

        protected virtual void OnPassoAnterior()
        {
            PassoAnterior?.Invoke(this, EventArgs.Empty);
        }

        private void BuscarRemente(object obj)
        {
            var pickerModel = new PessoaPickerModel(new PessoaEngine());
            pickerModel.PickItemEvent += SelecionaRemetentePicker;
            pickerModel.GetPickerView().ShowDialog();
        }

        private void BuscarDestinatario(object obj)
        {
            var pickerModel = new PessoaPickerModel(new PessoaEngine());
            pickerModel.PickItemEvent += SelecionaDestinatarioPicker;
            pickerModel.GetPickerView().ShowDialog();
        }

        private void BuscarExpedidor(object obj)
        {
            if (Expedidor != null)
            {
                RemoverExpedidor();
                return;
            }

            var pickerModel = new PessoaPickerModel(new PessoaEngine());
            pickerModel.PickItemEvent += SelecionaExpedidorPicker;
            pickerModel.GetPickerView().ShowDialog();
        }

        private void RemoverExpedidor()
        {
            if (_cte.Id == 0)
            {
                LimparInputExepdidor();
                return;
            }

            try
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorio = new RepositorioCte(sessao);
                    repositorio.DeletaExpedidor(_cte);
                }

                LimparInputExepdidor();
            }
            catch (Exception e)
            {    
                DialogBox.MostraErro("Não consegui remover o Expedidor", e);
            }
        }

        private void LimparInputExepdidor()
        {
            Expedidor = null;
            ExpedidorDocumentoUnico = string.Empty;
            ExpedidorNome = string.Empty;
            ExpedidorInscricaoEstadual = string.Empty;
            ExpedidorCep = string.Empty;
            ExpedidorLogradouro = string.Empty;
            ExpedidorNumero = string.Empty;
            ExpedidorBairro = string.Empty;
            ExpedidorUF = null;
            ExpedidorCidade = null;
            ExpedidorTelefone = string.Empty;
        }

        private void BuscarRecebedor(object obj)
        {
            if (Recebedor != null)
            {
                RemoverRecebedor();
                return;
            }

            var pickerModel = new PessoaPickerModel(new PessoaEngine());
            pickerModel.PickItemEvent += SelecionaRecebedorPicker;
            pickerModel.GetPickerView().ShowDialog();
        }

        private void RemoverRecebedor()
        {
            if (_cte.Id == 0)
            {
                LimparInputsRecebedor();
                return;
            }

            try
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorio = new RepositorioCte(sessao);
                    repositorio.DeletaRecebedor(_cte);
                }

                LimparInputsRecebedor();
            }
            catch (Exception e)
            {
                DialogBox.MostraErro("Não consegui Remover o recebedor", e);
            }
        }

        private void LimparInputsRecebedor()
        {
            Recebedor = null;
            RecebedorDocumentoUnico = string.Empty;
            RecebedorNome = string.Empty;
            RecebedorInscricaoEstadual = string.Empty;
            RecebedorCep = string.Empty;
            RecebedorLogradouro = string.Empty;
            RecebedorNumero = string.Empty;
            RecebedorBairro = string.Empty;
            RecebedorUF = null;
            RecebedorCidade = null;
            RecebedorTelefone = string.Empty;
        }

        private void BuscarTomador(object obj)
        {
            var pickerModel = new PessoaPickerModel(new PessoaEngine());
            pickerModel.PickItemEvent += SelecionaTomadorPicker;
            pickerModel.GetPickerView().ShowDialog();
        }

        private void SelecionaRemetentePicker(object sender, GridPickerEventArgs e)
        {
            var remetente = e.GetItem<PessoaEntidade>();

            CarregaRemetenteCom(remetente);

            if (IsUsarRemetenteComoDefault)
            {
                CarregaDestinatarioCom(remetente);
                TipoTomador = TipoTomador.Remetente;
            }
        }

        private void SelecionaDestinatarioPicker(object sender, GridPickerEventArgs e)
        {
            CarregaDestinatarioCom(e.GetItem<PessoaEntidade>());
        }

        private void SelecionaExpedidorPicker(object sender, GridPickerEventArgs e)
        {
            CarregaExpedidorCom(e.GetItem<PessoaEntidade>());
        }

        private void SelecionaRecebedorPicker(object sender, GridPickerEventArgs e)
        {
            CarregaRecebedorCom(e.GetItem<PessoaEntidade>());
        }

        private void SelecionaTomadorPicker(object sender, GridPickerEventArgs e)
        {
            CarregaTomadorCom(e.GetItem<PessoaEntidade>());
        }

        public void CarregaRemetenteCom(PessoaEntidade pessoa)
        {
            Remetente = pessoa;

            var documentoUnico = pessoa.Cpf.Valor.Trim();

            if (!pessoa.Cnpj.Valor.IsNullOrEmpty())
            {
                documentoUnico = pessoa.Cnpj.Valor.Trim();
            }

            RemetenteDocumentoUnico = documentoUnico;
            RemetenteNome = Remetente.Nome ?? Remetente.NomeFantasia;
            RemetenteInscricaoEstadual = Remetente.InscricaoEstadual ?? string.Empty;

            PreencherEnderecoRemetente();
            PreencherTelefoneRemetente();
        }

        public void CarregaDestinatarioCom(PessoaEntidade pessoa)
        {
            Destinatario = pessoa;

            var documentoUnico = pessoa.Cpf.Valor.Trim();

            if (!pessoa.Cnpj.Valor.IsNullOrEmpty())
            {
                documentoUnico = pessoa.Cnpj.Valor.Trim();
            }

            DestinatarioDocumentoUnico = documentoUnico;

            DestinatarioNome = Destinatario.Nome ?? Destinatario.NomeFantasia;
            DestinatarioInscricaoEstadual = Destinatario.InscricaoEstadual ?? string.Empty;

            PreencherEnderecoDestinatario();
            PreencherTelefoneDestinatario();
        }

        public void CarregaExpedidorCom(PessoaEntidade pessoa)
        {
            Expedidor = pessoa;

            var documentoUnico = pessoa.Cpf.Valor.Trim();

            if (!pessoa.Cnpj.Valor.IsNullOrEmpty())
            {
                documentoUnico = pessoa.Cnpj.Valor.Trim();
            }

            ExpedidorDocumentoUnico = documentoUnico;
            ExpedidorNome = Expedidor.Nome ?? Expedidor.NomeFantasia;
            ExpedidorInscricaoEstadual = Expedidor.InscricaoEstadual ?? string.Empty;

            PreencherEnderecoExpedidor();
            PreencherTelefoneExpedidor();
        }

        public void CarregaRecebedorCom(PessoaEntidade pessoa)
        {
            Recebedor = pessoa;

            var documentoUnico = pessoa.Cpf.Valor.Trim();

            if (!pessoa.Cnpj.Valor.IsNullOrEmpty())
            {
                documentoUnico = pessoa.Cnpj.Valor.Trim();
            }

            RecebedorDocumentoUnico = documentoUnico;

            RecebedorNome = Recebedor.Nome ?? Recebedor.NomeFantasia;
            RecebedorInscricaoEstadual = Recebedor.InscricaoEstadual ?? string.Empty;

            PreencherEnderecoRecebedor();
            PreencherTelefoneRecebedor();
        }

        public void CarregaTomadorCom(PessoaEntidade pessoa)
        {
            Tomador = pessoa;

            var documentoUnico = pessoa.Cpf.Valor.Trim();

            if (!pessoa.Cnpj.Valor.IsNullOrEmpty())
            {
                documentoUnico = pessoa.Cnpj.Valor.Trim();
            }

            TomadorDocumentoUnico = documentoUnico;

            TomadorNome = Tomador.Nome ?? Tomador.NomeFantasia;
            TomadorInscricaoEstadual = Tomador.InscricaoEstadual ?? string.Empty;

            PreencherEnderecoTomador();
            PreencherTelefoneTomador();
        }

        private void PreencherTelefoneRemetente()
        {
            if (Remetente.Telefones.IsNullOrEmpty()) return;

            AtualizaRemetenteTelefoneModel(Remetente.Telefones[0]);
        }

        private void PreencherTelefoneDestinatario()
        {
            if (Destinatario.Telefones.IsNullOrEmpty()) return;

            AtualizaDestinatarioTelefoneModel(Destinatario.Telefones[0]);
        }

        private void PreencherTelefoneExpedidor()
        {
            if (Expedidor.Telefones.IsNullOrEmpty()) return;

            AtualizaExpedidorTelefoneModel(Expedidor.Telefones[0]);
        }

        private void PreencherTelefoneRecebedor()
        {
            if (Recebedor.Telefones.IsNullOrEmpty()) return;

            AtualizaRecebedorTelefoneModel(Recebedor.Telefones[0]);
        }

        private void PreencherTelefoneTomador()
        {
            if (Tomador.Telefones.IsNullOrEmpty()) return;

            AtualizaTomadorTelefoneModel(Tomador.Telefones[0]);
        }

        private void PreencherEnderecoRemetente()
        {
            if (Remetente.Enderecos.IsNullOrEmpty())
            {


                return;
            }

            var enderecoPrincipal = (PessoaEndereco) Remetente.Enderecos?.FirstOrNull();

            if (enderecoPrincipal != null)
            {
                AtualizaRemetenteEnderecoModel(enderecoPrincipal);
                return;
            }

            // ReSharper disable once PossibleNullReferenceException
            AtualizaRemetenteEnderecoModel(Remetente.Enderecos[0]);
        }

        private void PreencherEnderecoDestinatario()
        {
            if (Destinatario.Enderecos.IsNullOrEmpty()) return;

            var enderecoPrincipal = (PessoaEndereco) Destinatario.Enderecos?.FirstOrNull();

            if (enderecoPrincipal != null)
            {
                AtualizaDestinatarioEnderecoModel(enderecoPrincipal);
                return;
            }

            // ReSharper disable once PossibleNullReferenceException
            AtualizaDestinatarioEnderecoModel(Destinatario.Enderecos[0]);
        }

        private void PreencherEnderecoExpedidor()
        {
            if (Expedidor.Enderecos.IsNullOrEmpty()) return;

            var enderecoPrincipal = (PessoaEndereco) Expedidor.Enderecos?.FirstOrNull();

            if (enderecoPrincipal != null)
            {
                AtualizaExpedidorEnderecoModel(enderecoPrincipal);
                return;
            }

            // ReSharper disable once PossibleNullReferenceException
            AtualizaExpedidorEnderecoModel(Expedidor.Enderecos[0]);
        }

        private void PreencherEnderecoRecebedor()
        {
            if (Recebedor.Enderecos.IsNullOrEmpty()) return;

            var enderecoPrincipal = (PessoaEndereco) Recebedor.Enderecos?.FirstOrNull();

            if (enderecoPrincipal != null)
            {
                AtualizaRecebedorEnderecoModel(enderecoPrincipal);
                return;
            }

            // ReSharper disable once PossibleNullReferenceException
            AtualizaRecebedorEnderecoModel(Recebedor.Enderecos[0]);
        }

        private void PreencherEnderecoTomador()
        {
            if (Tomador.Enderecos.IsNullOrEmpty()) return;

            var enderecoPrincipal = (PessoaEndereco) Tomador.Enderecos?.FirstOrNull();

            if (enderecoPrincipal != null)
            {
                AtualizaTomadorEnderecoModel(enderecoPrincipal);
                return;
            }

            // ReSharper disable once PossibleNullReferenceException
            AtualizaTomadorEnderecoModel(Tomador.Enderecos[0]);
        }

        private void AtualizaRemetenteTelefoneModel(PessoaTelefone pessoaTelefone)
        {
            RemetenteTelefone = pessoaTelefone.Numero;
        }

        private void AtualizaDestinatarioTelefoneModel(PessoaTelefone pessoaTelefone)
        {
            DestinatarioTelefone = pessoaTelefone.Numero;
        }

        private void AtualizaExpedidorTelefoneModel(PessoaTelefone pessoaTelefone)
        {
            ExpedidorTelefone = pessoaTelefone.Numero;
        }

        private void AtualizaRecebedorTelefoneModel(PessoaTelefone pessoaTelefone)
        {
            RecebedorTelefone = pessoaTelefone.Numero;
        }

        private void AtualizaTomadorTelefoneModel(PessoaTelefone pessoaTelefone)
        {
            TomadorTelefone = pessoaTelefone.Numero;
        }

        private void AtualizaDestinatarioEnderecoModel(PessoaEndereco pessoaEndereco)
        {
            DestinatarioCep = pessoaEndereco.Cep;
            DestinatarioLogradouro = pessoaEndereco.Logradouro;
            DestinatarioNumero = pessoaEndereco.Numero;
            DestinatarioBairro = pessoaEndereco.Bairro;
            DestinatarioUF =
                (EstadoDTO)
                    LocalidadesServico.GetInstancia(false)
                        .GetEstados()
                        .Where(e => e.Sigla == pessoaEndereco.Cidade?.SiglaUf)
                        .FirstOrNull();
            DestinatarioCidade = pessoaEndereco.Cidade;
        }

        private void AtualizaRemetenteEnderecoModel(PessoaEndereco pessoaEndereco)
        {
            RemetenteCep = pessoaEndereco.Cep;
            RemetenteLogradouro = pessoaEndereco.Logradouro;
            RemetenteNumero = pessoaEndereco.Numero;
            RemetenteBairro = pessoaEndereco.Bairro;
            RemetenteUF =
                (EstadoDTO)
                    LocalidadesServico.GetInstancia(false)
                        .GetEstados()
                        .Where(e => e.Sigla == pessoaEndereco.Cidade?.SiglaUf)
                        .FirstOrNull();
            RemetenteCidade = pessoaEndereco.Cidade;
        }

        private void AtualizaExpedidorEnderecoModel(PessoaEndereco pessoaEndereco)
        {
            ExpedidorCep = pessoaEndereco.Cep;
            ExpedidorLogradouro = pessoaEndereco.Logradouro;
            ExpedidorNumero = pessoaEndereco.Numero;
            ExpedidorBairro = pessoaEndereco.Bairro;
            ExpedidorUF =
                (EstadoDTO)
                    LocalidadesServico.GetInstancia(false)
                        .GetEstados()
                        .Where(e => e.Sigla == pessoaEndereco.Cidade?.SiglaUf)
                        .FirstOrNull();
            ExpedidorCidade = pessoaEndereco.Cidade;
        }

        private void AtualizaRecebedorEnderecoModel(PessoaEndereco pessoaEndereco)
        {
            RecebedorCep = pessoaEndereco.Cep;
            RecebedorLogradouro = pessoaEndereco.Logradouro;
            RecebedorNumero = pessoaEndereco.Numero;
            RecebedorBairro = pessoaEndereco.Bairro;
            RecebedorUF =
                (EstadoDTO)
                    LocalidadesServico.GetInstancia(false)
                        .GetEstados()
                        .Where(e => e.Sigla == pessoaEndereco.Cidade?.SiglaUf)
                        .FirstOrNull();
            RecebedorCidade = pessoaEndereco.Cidade;
        }

        private void AtualizaTomadorEnderecoModel(PessoaEndereco pessoaEndereco)
        {
            TomadorCep = pessoaEndereco.Cep;
            TomadorLogradouro = pessoaEndereco.Logradouro;
            TomadorNumero = pessoaEndereco.Numero;
            TomadorBairro = pessoaEndereco.Bairro;
            TomadorUF =
                (EstadoDTO)
                    LocalidadesServico.GetInstancia(false)
                        .GetEstados()
                        .Where(e => e.Sigla == pessoaEndereco.Cidade?.SiglaUf)
                        .FirstOrNull();
            TomadorCidade = pessoaEndereco.Cidade;
        }

        private void PreparaTomador(TipoTomador tipoTomador)
        {
            switch (tipoTomador)
            {
                case TipoTomador.Remetente:
                    if (Remetente == null) throw new InvalidOperationException("Selecione um Remetente");
                    TomadorOutro = true;
                    Tomador = (PessoaEntidade) Remetente.Clone();
                    AtualizaInformacaoTomador();
                    PreencherEnderecoTomador();
                    PreencherTelefoneTomador();
                    break;

                case TipoTomador.Expedidor:
                    if (Expedidor == null) throw new InvalidOperationException("Selecione um Expedidor");
                    TomadorOutro = true;
                    Tomador = (PessoaEntidade) Expedidor.Clone();
                    AtualizaInformacaoTomador();
                    PreencherEnderecoTomador();
                    PreencherTelefoneTomador();
                    break;

                case TipoTomador.Recebedor:
                    if (Recebedor == null) throw new InvalidOperationException("Selecione um Recebedor");
                    TomadorOutro = true;
                    Tomador = (PessoaEntidade) Recebedor.Clone();
                    AtualizaInformacaoTomador();
                    PreencherEnderecoTomador();
                    PreencherTelefoneTomador();
                    break;

                case TipoTomador.Destinatario:
                    if (Destinatario == null) throw new InvalidOperationException("Selecione um Destinatario");
                    TomadorOutro = true;
                    Tomador = (PessoaEntidade) Destinatario.Clone();
                    AtualizaInformacaoTomador();
                    PreencherEnderecoTomador();
                    PreencherTelefoneTomador();
                    break;

                case TipoTomador.Outros:
                    TomadorOutro = false;
                    Tomador = null;
                    TomadorNome = string.Empty;
                    TomadorDocumentoUnico = string.Empty;
                    TomadorInscricaoEstadual = string.Empty;
                    TomadorCep = string.Empty;
                    TomadorLogradouro = string.Empty;
                    TomadorNumero = string.Empty;
                    TomadorBairro = string.Empty;
                    TomadorTelefone = string.Empty;
                    TomadorUF = null;
                    TomadorCidade = null;
                    break;
            }
        }

        private void AtualizaInformacaoTomador()
        {
            var documentoUnico = Tomador.Cpf.Valor.Trim();

            if (!Tomador.Cnpj.Valor.IsNullOrEmpty())
            {
                documentoUnico = Tomador.Cnpj.Valor.Trim();
            }

            TomadorDocumentoUnico = documentoUnico;

            TomadorNome = Tomador.Nome ?? Tomador.NomeFantasia;
            TomadorInscricaoEstadual = Tomador.InscricaoEstadual ?? string.Empty;
        }

        public void Validacao()
        {
            if (Remetente == null) throw new ArgumentException("Selecione um remetente");
            if (Destinatario == null) throw new ArgumentException("Selecione um destinatário");
            if (Tomador == null) throw new ArgumentException("Selecione um tomador");

            ValidaRemetente();
            ValidaDestinatario();
            ValidaExpedidor();
            ValidaRecebedor();
            ValidaTomador();
        }

        private void ValidaRemetente()
        {
            if (string.IsNullOrEmpty(RemetenteDocumentoUnico))
                throw new ArgumentException("Remetente não tem cnpj/cpf");
            if (string.IsNullOrEmpty(RemetenteNome)) throw new ArgumentException("Remetente não tem nome");
            if (string.IsNullOrEmpty(RemetenteLogradouro)) throw new ArgumentException("Remetente não tem logradouro");
            if (string.IsNullOrEmpty(RemetenteNumero)) throw new ArgumentException("Remetente não tem número");
            if (string.IsNullOrEmpty(RemetenteNumero)) throw new ArgumentException("Remetente não tem bairro");
            if (RemetenteUF == null) throw new ArgumentException("Remetente não tem estado(uf)");
            if (RemetenteCidade == null) throw new ArgumentException("Remetente não tem cidade");
        }

        private void ValidaDestinatario()
        {
            if (string.IsNullOrEmpty(DestinatarioDocumentoUnico))
                throw new ArgumentException("Destinatario não tem cnpj/cpf");
            if (string.IsNullOrEmpty(DestinatarioNome)) throw new ArgumentException("Destinatario não tem nome");
            if (string.IsNullOrEmpty(DestinatarioLogradouro))
                throw new ArgumentException("Destinatario não tem logradouro");
            if (string.IsNullOrEmpty(DestinatarioNumero)) throw new ArgumentException("Destinatario não tem número");
            if (string.IsNullOrEmpty(DestinatarioBairro)) throw new ArgumentException("Destinatario não tem bairro");
            if (DestinatarioUF == null) throw new ArgumentException("Destinatario não tem estado(uf)");
            if (DestinatarioCidade == null) throw new ArgumentException("Destinatario não tem cidade");
        }

        private void ValidaExpedidor()
        {
            if (Expedidor == null) return;

            if (string.IsNullOrEmpty(ExpedidorDocumentoUnico))
                throw new ArgumentException("Expedidor não tem cnpj/cpf");
            if (string.IsNullOrEmpty(ExpedidorNome)) throw new ArgumentException("Expedidor não tem nome");
            if (string.IsNullOrEmpty(ExpedidorLogradouro)) throw new ArgumentException("Expedidor não tem logradouro");
            if (string.IsNullOrEmpty(ExpedidorNumero)) throw new ArgumentException("Expedidor não tem número");
            if (string.IsNullOrEmpty(ExpedidorNumero)) throw new ArgumentException("Expedidor não tem bairro");
            if (ExpedidorUF == null) throw new ArgumentException("Expedidor não tem estado(uf)");
            if (ExpedidorCidade == null) throw new ArgumentException("Expedidor não tem cidade");
        }

        private void ValidaRecebedor()
        {
            if (Recebedor == null) return;

            if (string.IsNullOrEmpty(RecebedorDocumentoUnico))
                throw new ArgumentException("Recebedor não tem cnpj/cpf");
            if (string.IsNullOrEmpty(RecebedorNome)) throw new ArgumentException("Recebedor não tem nome");
            if (string.IsNullOrEmpty(RecebedorLogradouro)) throw new ArgumentException("Recebedor não tem logradouro");
            if (string.IsNullOrEmpty(RecebedorNumero)) throw new ArgumentException("Recebedor não tem número");
            if (string.IsNullOrEmpty(RecebedorNumero)) throw new ArgumentException("Recebedor não tem bairro");
            if (RecebedorUF == null) throw new ArgumentException("Recebedor não tem estado(uf)");
            if (RecebedorCidade == null) throw new ArgumentException("Recebedor não tem cidade");
        }

        private void ValidaTomador()
        {
            if (string.IsNullOrEmpty(TomadorDocumentoUnico)) throw new ArgumentException("Tomador não tem cnpj/cpf");
            if (string.IsNullOrEmpty(TomadorNome)) throw new ArgumentException("Tomador não tem nome");
            if (string.IsNullOrEmpty(TomadorLogradouro)) throw new ArgumentException("Tomador não tem logradouro");
            if (string.IsNullOrEmpty(TomadorNumero)) throw new ArgumentException("Tomador não tem número");
            if (string.IsNullOrEmpty(TomadorNumero)) throw new ArgumentException("Tomador não tem bairro");
            if (TomadorUF == null) throw new ArgumentException("Tomador não tem estado(uf)");
            if (TomadorCidade == null) throw new ArgumentException("Tomador não tem cidade");
        }

        public void PreencerCom(Cte cte)
        {
            Remetente = cte.CteRemetente.Remetente;
            Destinatario = cte.CteDestinatario.Destinatario;


            CarregaRemetenteCom(Remetente);
            CarregaDestinatarioCom(Destinatario);


            var expedidor = cte.CteExpedidor?.Expedidor;
            var recebedor = cte.CteRecebedor?.Recebedor;

            if (expedidor != null)
            {
                Expedidor = expedidor;
                CarregaExpedidorCom(Expedidor);
            }

            if (recebedor != null)
            {
                Recebedor = recebedor;
                CarregaRecebedorCom(Recebedor);
            }

            Tomador = cte.CteTomador.Tomador;
            TipoTomador = cte.TipoTomador;

            if (TipoTomador == TipoTomador.Outros)
            {
                Tomador = cte.CteTomador.Tomador;
            }
            CarregaTomadorCom(Tomador);
        }

        public void ImportacaoXml(RetornoImportacaoXmlCteEventArgs retorno)
        {
            DestinatarioImportacao(retorno);
            EmitenteImportacao(retorno);
        }

        private void DestinatarioImportacao(RetornoImportacaoXmlCteEventArgs retorno)
        {
            if (retorno.IsDestinatarioDestinatario)
            {
                CarregaDestinatarioCom(retorno.PessoaDestinatario);
            }

            if (retorno.IsDestinatarioExpedidor)
            {
                CarregaExpedidorCom(retorno.PessoaDestinatario);
            }

            if (retorno.IsDestinatarioRecebedor)
            {
                CarregaRecebedorCom(retorno.PessoaDestinatario);
            }

            if (retorno.IsDestinatarioRemetente)
            {
                CarregaRemetenteCom(retorno.PessoaDestinatario);
            }

            if (retorno.IsDestinatarioTomador)
            {
                if (retorno.IsDestinatarioRemetente)
                {
                    TipoTomador = TipoTomador.Remetente;
                    return;
                }

                if (retorno.IsDestinatarioExpedidor)
                {
                    TipoTomador = TipoTomador.Expedidor;
                    return;
                }

                if (retorno.IsDestinatarioRecebedor)
                {
                    TipoTomador = TipoTomador.Recebedor;
                    return;
                }

                if (retorno.IsDestinatarioDestinatario)
                {
                    TipoTomador = TipoTomador.Destinatario;
                    return;
                }

                CarregaTomadorCom(retorno.PessoaDestinatario);
            }
        }

        private void EmitenteImportacao(RetornoImportacaoXmlCteEventArgs retorno)
        {
            if (retorno.IsEmitenteDestinatario)
            {
                CarregaDestinatarioCom(retorno.PessoaEmitente);
            }

            if (retorno.IsEmitenteExpedidor)
            {
                CarregaExpedidorCom(retorno.PessoaEmitente);
            }

            if (retorno.IsEmitenteRecebedor)
            {
                CarregaRecebedorCom(retorno.PessoaEmitente);
            }

            if (retorno.IsEmitenteRemetente)
            {
                CarregaRemetenteCom(retorno.PessoaEmitente);
            }

            if (retorno.IsEmitenteTomador)
            {
                if (retorno.IsEmitenteRemetente)
                {
                    TipoTomador = TipoTomador.Remetente;
                    return;
                }

                if (retorno.IsEmitenteExpedidor)
                {
                    TipoTomador = TipoTomador.Expedidor;
                    return;
                }

                if (retorno.IsEmitenteRecebedor)
                {
                    TipoTomador = TipoTomador.Recebedor;
                    return;
                }

                if (retorno.IsEmitenteDestinatario)
                {
                    TipoTomador = TipoTomador.Destinatario;
                    return;
                }

                CarregaTomadorCom(retorno.PessoaEmitente);
            }
        }
    }
}