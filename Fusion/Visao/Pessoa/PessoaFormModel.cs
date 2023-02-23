using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Fusion.Sessao;
using Fusion.Visao.Pessoa.Flyouts;
using Fusion.Visao.Pessoa.SubFormularios;
using Fusion.Visao.Validacoes.CteOs;
using FusionCore.Facades;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.ConsultaCnpj;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using ComponenteCpf = FusionCore.FusionAdm.Componentes.Cpf;
using ComponenteCnpj = FusionCore.FusionAdm.Componentes.Cnpj;
using static System.String;

namespace Fusion.Visao.Pessoa
{
    public sealed class PessoaFormModel : ViewModel
    {
        private readonly int _pessoaId;
        private PessoaEntidade _pessoa;
        private string _rntrc;
        private TipoProprietario _tipoProprietario;
        private bool _solicitaPedido;
        private string _taf;
        private string _numeroDoRegistroEstadual;
        private bool _isPessoaVisualizar;
        private bool _isPessoaAlterar;
        private bool _isFornecedorEnable;
        private bool _ativo = true;
        private IndicadorIE _indicadorIEDestinatario = IndicadorIE.ContribuinteIcms;

        public PessoaFormModel()
        {
            IsCliente = true;
            IsFornecedorEnable = true;
            IsTransportadoraEnable = true;
            IsClienteEnable = true;
            IsVendedorEnable = true;

            Telefones = new ObservableCollection<PessoaTelefone>();
            Enderecos = new ObservableCollection<PessoaEndereco>();
            Emails = new ObservableCollection<PessoaEmail>();

            PermissaoUsuarioLogado();
        }

        public PessoaFormModel(int pessoaId) : this()
        {
            _pessoaId = pessoaId;
        }

        public bool IsPessoaAlterar
        {
            get => _isPessoaAlterar;
            set
            {
                if (value == _isPessoaAlterar) return;
                _isPessoaAlterar = value;
                PropriedadeAlterada();
            }
        }

        public bool IsPessoaVisualizar
        {
            get => _isPessoaVisualizar;
            set
            {
                if (value == _isPessoaVisualizar) return;
                _isPessoaVisualizar = value;
                PropriedadeAlterada();
            }
        }

        public string Nome
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public PessoaTipo Tipo
        {
            get => GetValue<PessoaTipo>();
            set => SetValue(value);
        }

        public bool IsCliente
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsTransportadora
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsFornecedor
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsVendedor
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string OrgaoRg
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string Rg
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string Cpf
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string NomePai
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string NomeMae
        {
            get => GetValue();
            set => SetValue(value);
        }

        public PessoaSexo Sexo
        {
            get => GetValue<PessoaSexo>();
            set => SetValue(value);
        }

        public DateTime? DataNascimento
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public string ObservacaoCliente
        {
            get => GetValue();
            set => SetValue(value);
        }

        public decimal LimiteCredito
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public bool AplicaLimiteCredito
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string InscricaoMunicipal
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string InscricaoEstadual
        {
            get => GetValue();
            set => SetValue(value);
        }

        public IndicadorIE IndicadorIEDestinatario
        {
            get => _indicadorIEDestinatario;
            set
            {
                _indicadorIEDestinatario = value;
                PropriedadeAlterada();
            }
        }

        public string NomeFantasia
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string Cnpj
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string DocumentoExterior
        {
            get => GetValue();
            set => SetValue(value);
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

        public bool IsNovo => _pessoaId == 0 && (_pessoa == null || _pessoa.Id == 0);

        public ObservableCollection<PessoaTelefone> Telefones
        {
            get => GetValue<ObservableCollection<PessoaTelefone>>();
            set => SetValue(value);
        }

        public PessoaTelefone TelefoneSelecionado
        {
            get => GetValue<PessoaTelefone>();
            set => SetValue(value);
        }

        public ObservableCollection<PessoaEndereco> Enderecos
        {
            get => GetValue<ObservableCollection<PessoaEndereco>>();
            set => SetValue(value);
        }

        public PessoaEndereco EnderecoSelecionado
        {
            get => GetValue<PessoaEndereco>();
            set => SetValue(value);
        }

        public ObservableCollection<PessoaEmail> Emails
        {
            get => GetValue<ObservableCollection<PessoaEmail>>();
            set => SetValue(value);
        }

        public string Rntrc
        {
            get => _rntrc;
            set
            {
                if (value == _rntrc) return;
                _rntrc = value;
                PropriedadeAlterada();
            }
        }

        public TipoProprietario TipoProprietario
        {
            get => _tipoProprietario;
            set
            {
                if (value == _tipoProprietario) return;
                _tipoProprietario = value;
                PropriedadeAlterada();
            }
        }

        public string Taf
        {
            get => _taf;
            set
            {
                if (value == _taf) return;
                _taf = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroDoRegistroEstadual
        {
            get => _numeroDoRegistroEstadual;
            set
            {
                if (value == _numeroDoRegistroEstadual) return;
                _numeroDoRegistroEstadual = value;
                PropriedadeAlterada();
            }
        }

        public bool SolicitaPedido
        {
            get => _solicitaPedido;
            set
            {
                if (value == _solicitaPedido) return;
                _solicitaPedido = value;
                PropriedadeAlterada();
            }
        }

        public PessoaEmail EmailSelecionado
        {
            get => GetValue<PessoaEmail>();
            set => SetValue(value);
        }

        public PessoaEmailFlyoutModel PessoaEmailModel
        {
            get => GetValue<PessoaEmailFlyoutModel>();
            set => SetValue(value);
        }

        public bool IsClienteEnable
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsFornecedorEnable
        {
            get => _isFornecedorEnable;
            set
            {
                if (value == _isFornecedorEnable) return;
                _isFornecedorEnable = value;
                PropriedadeAlterada();
            }
        }

        public bool IsTransportadoraEnable
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsVendedorEnable
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        internal event EventHandler CloseRequest;
        public event EventHandler<PessoaEntidade> RegistroSalvo;

        public void Inicializar()
        {
            if (_pessoaId == 0)
            {
                return;
            }

            using (var repositorio = new RepositorioPessoa(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                _pessoa = repositorio.GetPeloId(_pessoaId);

                if (_pessoa == null)
                    throw new InvalidOperationException(
                        $"Não foi possivel carregar a pessoa com Id {_pessoaId} do banco de dados.");

                IsCliente = _pessoa.Cliente != null;
                IsTransportadora = _pessoa.Transportadora != null;
                IsFornecedor = _pessoa.Fornecedor != null;
                IsVendedor = _pessoa.Vendedor != null;
                IsClienteEnable = IsCliente == false;
                IsTransportadoraEnable = IsTransportadora == false;
                IsFornecedorEnable = IsFornecedor == false;
                IsVendedorEnable = IsVendedor == false;

                CarregarPessoa();
                CarregarPessoaFisica();
                CarregarPessoaJuridica();
                CarregarPessoaExterior();
                CarregarCliente();
                CarregarTransportadora();
            }
        }

        private void PermissaoUsuarioLogado()
        {
            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;
            IsPessoaVisualizar = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.CADASTRO_PESSOA_VISUALIZAR);
            IsPessoaAlterar = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.CADASTRO_PESSOA_INSERIR_ALTERAR);
        }

        private void CarregarPessoa()
        {
            Nome = _pessoa.Nome;
            Tipo = _pessoa.Tipo;
            Ativo = _pessoa.Ativo;

            Telefones = new ObservableCollection<PessoaTelefone>(_pessoa.Telefones);
            Enderecos = new ObservableCollection<PessoaEndereco>(_pessoa.Enderecos);
            Emails = new ObservableCollection<PessoaEmail>(_pessoa.Emails);
        }

        private void CarregarPessoaFisica()
        {
            Cpf = _pessoa.Cpf.ToString();
            Rg = _pessoa.Rg.Rg;
            OrgaoRg = _pessoa.Rg.OrgaoRg;
            DataNascimento = _pessoa.NascidoEm;
            Sexo = _pessoa.Sexo;
            NomeMae = _pessoa.NomeMae;
            NomePai = _pessoa.NomePai;
            InscricaoEstadual = _pessoa.InscricaoEstadual;
            IndicadorIEDestinatario = _pessoa.IndicadorIEDestinatario;
        }

        private void CarregarPessoaJuridica()
        {
            Cnpj = _pessoa.Cnpj.ToString();
            NomeFantasia = _pessoa.NomeFantasia;
            InscricaoEstadual = _pessoa.InscricaoEstadual;
            IndicadorIEDestinatario = _pessoa.IndicadorIEDestinatario;
            InscricaoMunicipal = _pessoa.InscricaoMunicipal;
        }

        private void CarregarPessoaExterior()
        {
            DocumentoExterior = _pessoa.DocumentoExterior;
        }

        private void CarregarCliente()
        {
            AplicaLimiteCredito = _pessoa.Cliente?.AplicaLimiteCredito ?? false;
            LimiteCredito = _pessoa.Cliente?.LimiteCredito ?? 0M;
            ObservacaoCliente = _pessoa.Cliente?.Observacao ?? Empty;
            SolicitaPedido = _pessoa.Cliente?.SolicitaPedidoNfe ?? false;
        }

        private void CarregarTransportadora()
        {
            Rntrc = _pessoa.Transportadora?.Rntrc;
            TipoProprietario = _pessoa.Transportadora?.TipoProprietario ?? TipoProprietario.Outros;
            Taf = _pessoa.Transportadora?.Taf;
            NumeroDoRegistroEstadual = _pessoa.Transportadora?.NumeroDoRegistroEstadual;
        }

        public void Salvar()
        {
            try
            {
                ValidaInscricaoEstadual();
                ValidaTransportadora();

                if (_pessoa == null)
                    _pessoa = new PessoaEntidade(Nome);

                PreencherObjetoPessoa();
                PessoaFacade.Salvar(_pessoa);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
                return;
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraAviso(e.Message);
                return;
            }

            DialogBox.MostraInformacao("Tudo Ok! Salvei o registro para você.");
            CloseRequest?.Invoke(this, EventArgs.Empty);
            RegistroSalvo?.Invoke(this, _pessoa);
        }

        private void ValidaTransportadora()
        {
            if (IsTransportadora)
                ValidarTafRegistroEstadual.Executar(Taf, NumeroDoRegistroEstadual);
        }

        private void ValidaInscricaoEstadual()
        {
            if (IndicadorIEDestinatario == IndicadorIE.ContribuinteIcms && InscricaoEstadual.IsNullOrEmpty())
                throw new InvalidOperationException("Indicador IE Contribuiente ICMS, logo então deve possuir IE");

            if (IsNullOrWhiteSpace(InscricaoEstadual))
                return;

            if (InscricaoEstadual == "ISENTO")
                return;

            if (Regex.IsMatch(InscricaoEstadual, "[^0-9]"))
                throw new InvalidOperationException("IE deve possuir apenas númerou ou ISENTA");
        }

        private void PreencherObjetoPessoa()
        {
            _pessoa.Nome = Nome;
            _pessoa.Ativo = IsNovo == true ? true : Ativo;

            if (Tipo == PessoaTipo.Fisica)
            {
                var cpf = new ComponenteCpf(Cpf);
                var rg = new DocumentoRg(Rg, OrgaoRg);

                cpf.ThrowExcetionSeInvalido();

                _pessoa.ComoPessoaFisica(cpf, rg, Sexo, IndicadorIEDestinatario, InscricaoEstadual, DataNascimento);
                _pessoa.NomeMae = NomeMae;
                _pessoa.NomePai = NomePai;
            }

            if (Tipo == PessoaTipo.Juridica)
            {
                var cnpj = IsNullOrEmpty(Cnpj) ? ComponenteCnpj.Vazio : new ComponenteCnpj(Cnpj);
                cnpj.ThrowExcetpionSeInvalido();

                _pessoa.ComoPessoaJuridica(NomeFantasia, cnpj, IndicadorIEDestinatario, InscricaoEstadual, InscricaoMunicipal);
            }

            if (Tipo == PessoaTipo.Extrangeiro)
                _pessoa.ComoPessoaExterior(DocumentoExterior);

            _pessoa.ComTelefones(Telefones);
            _pessoa.ComEnderecos(Enderecos);
            _pessoa.ComEmails(Emails);

            PreencherComCliente();
            PreencherComTransportadora();
            PreencherComFornecedor();
            PreencherComVendedor();
        }

        private void PreencherComCliente()
        {
            if (IsCliente == false)
            {
                _pessoa.Cliente = null;
                return;
            }

            if (_pessoa.Cliente == null)
                _pessoa.Cliente = new Cliente(_pessoa);

            _pessoa.Cliente.AplicaLimiteCredito = AplicaLimiteCredito;
            _pessoa.Cliente.LimiteCredito = LimiteCredito;
            _pessoa.Cliente.Observacao = ObservacaoCliente ?? Empty;
            _pessoa.Cliente.SolicitaPedidoNfe = SolicitaPedido;
        }

        private void PreencherComTransportadora()
        {
            if (IsTransportadora == false)
            {
                _pessoa.Transportadora = null;
                return;
            }

            if (_pessoa.Transportadora == null)
                _pessoa.Transportadora = new Transportadora(_pessoa);

            _pessoa.Transportadora.Rntrc = Rntrc ?? Empty;
            _pessoa.Transportadora.TipoProprietario = TipoProprietario;
            _pessoa.Transportadora.Taf = Taf ?? Empty;
            _pessoa.Transportadora.NumeroDoRegistroEstadual = NumeroDoRegistroEstadual ?? Empty;
        }

        private void PreencherComFornecedor()
        {
            if (IsFornecedor == false)
            {
                _pessoa.Fornecedor = null;
                return;
            }

            if (_pessoa.Fornecedor == null)
                _pessoa.Fornecedor = new Fornecedor(_pessoa);
        }

        private void PreencherComVendedor()
        {
            if (IsVendedor == false)
            {
                _pessoa.Vendedor = null;
                return;
            }

            if (_pessoa.Vendedor == null)
            {
                _pessoa.Vendedor = new Vendedor(_pessoa);
            }
        }

        public TelefoneFormModel GetTelefoneModelNovo()
        {
            var model = new TelefoneFormModel();
            model.TelefoneAdicionado += TelefoneAdicionadoHandler;

            return model;
        }

        public TelefoneFormModel GetTelefoneModelEdicao()
        {
            var model = new TelefoneFormModel(TelefoneSelecionado, IsPessoaAlterar);
            model.TelefoneEditado += TelefoneEditadoHandler;
            return model;
        }

        private void TelefoneAdicionadoHandler(object sender, PessoaTelefone e)
        {
            if (_pessoa?.Id > 0)
            {
                e.Contato = _pessoa;
                PessoaFacade.SalvarTelefone(e);
            }

            Telefones.Add(e);
        }

        public void DeletarTelefoneSelecionado()
        {
            if (_pessoa?.Id > 0)
                PessoaFacade.DeletaTelefone(TelefoneSelecionado);

            Telefones.Remove(TelefoneSelecionado);
        }

        private void TelefoneEditadoHandler(object sender, PessoaTelefone e)
        {
            var telefone = Telefones.FirstOrDefault(i => i == e);

            if (telefone == null)
                return;

            telefone.Update(e);

            if (_pessoa?.Id > 0)
                PessoaFacade.SalvarTelefone(telefone);

            Telefones = new ObservableCollection<PessoaTelefone>(Telefones);
        }

        public EnderecoFormModel GetEnderecoModelNovo()
        {
            var model = new EnderecoFormModel();
            model.EnderecoAdicionado += EnderecoAdicionadoHandler;

            return model;
        }

        private void EnderecoAdicionadoHandler(object sender, PessoaEndereco e)
        {
            ValidaEnderecoPessoa(e);

            if (_pessoa?.Id > 0)
            {
                e.Residente = _pessoa;

                PessoaFacade.SalvarEndereco(e);
            }

            Enderecos.Add(e);
        }

        private void ValidaEnderecoPessoa(PessoaEndereco e)
        {
            if (e.Principal && Enderecos.Any(x => x.Principal == e.Principal))
                throw new InvalidOperationException("Já existe um endereço principal.");

            if (e.Entrega && Enderecos.Any(x => x.Entrega == e.Entrega))
                throw new InvalidOperationException("Já existe um endereço de entrega.");
        }

        public EnderecoFormModel GetEnderecoModelEdicao()
        {
            var model = new EnderecoFormModel(EnderecoSelecionado, IsPessoaAlterar);
            model.EnderecoEditado += EnderecoEditadoHandler;
            model.EnderecoDeletado += EnderecoDeletadoHandler;

            return model;
        }

        private void EnderecoEditadoHandler(object sender, PessoaEndereco e)
        {
            var endereco = Enderecos.FirstOrDefault(i => i == e);

            if (endereco == null)
                return;

            ValidaEnderecoPessoa(e);

            endereco.Atualizar(e);

            if (_pessoa?.Id > 0)
            {
                PessoaFacade.SalvarEndereco(e);
            }

            Enderecos = new ObservableCollection<PessoaEndereco>(Enderecos);
        }

        private void EnderecoDeletadoHandler(object sender, PessoaEndereco e)
        {
            if (_pessoa?.Id > 0)
            {
                PessoaFacade.DeletaEndereco(e);
            }

            Enderecos.Remove(e);
        }

        public void CarregaComEmpresaReceita(EmpresaReceitaWs empresa)
        {
            Tipo = PessoaTipo.Juridica;
            Nome = empresa.RazaoSocial;
            NomeFantasia = empresa.NomeFantasia;
            Cnpj = empresa.Cnpj;

            if (empresa.Email != null)
            {
                Emails.Add(new PessoaEmail(empresa.Email));
            }

            foreach (var telefone in empresa.Telefone)
            {
                Telefones.Add(new PessoaTelefone("CONSULTA RECEITA", telefone));
            }

            try
            {
                var endereco = new PessoaEndereco(
                    empresa.Logradouro.TrimSefaz(60),
                    empresa.Numero.TrimSefaz(60),
                    empresa.Bairro.TrimSefaz(60),
                    empresa.Cep,
                    empresa.Municipio
                );

                endereco.Complemento = empresa.Complemento.TrimSefaz(60);
                Enderecos.Add(endereco);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        public void IniciaFlyoutEmail()
        {
            PessoaEmailModel = new PessoaEmailFlyoutModel(_pessoa) {IsOpen = true};
            PessoaEmailModel.EmailAdicionado += EmailAdicionadoHandler;
        }

        private void EmailAdicionadoHandler(object sender, PessoaEmail e)
        {
            if (_pessoa?.Id > 0)
            {
                e.Pessoa = _pessoa;
                PessoaFacade.SalvarEmail(e);
            }

            Emails.Add(e);
            IniciaFlyoutEmail();
        }

        public void DeletarPessoaEmail(PessoaEmail email)
        {
            try
            {
                if (_pessoa?.Id > 0)
                {
                    PessoaFacade.DeletarEmail(email);
                }

                Emails.Remove(email);
            }
            catch (Exception)
            {
                _pessoa?.ComEmails(Emails);
                throw;
            }
        }

        public void DeletarPessoaEndereco(PessoaEndereco endereco)
        {
            if (_pessoa?.Id > 0)
            {
                PessoaFacade.DeletaEndereco(endereco);
            }

            Enderecos.Remove(endereco);
        }
    }
}