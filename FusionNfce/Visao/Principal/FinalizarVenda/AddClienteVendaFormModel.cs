using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using FusionCore.FusionNfce.Cidade;
using FusionCore.FusionNfce.Cliente;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Uf;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionNfce;
using FusionLibrary.Validacao;
using FusionLibrary.Validacao.Regras;
using FusionLibrary.VisaoModel;
using FusionNfce.Visao.Clientes;
using FusionNfce.Visao.Clientes.Model;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace FusionNfce.Visao.Principal.FinalizarVenda
{
    public sealed class AddClienteVendaFormModel : ViewModel
    {
        private readonly decimal _nfceTotal;
        private readonly NfceDestinatario _destinatario;
        public event EventHandler<NfceDestinatario> SalvarHandler;
        public event EventHandler LimparDestinatario;

        private string _documentoUnicoCliente;
        private string _nomeCliente;
        private string _cep;
        private string _logradouro;
        private string _numero;
        private string _bairro;
        private string _complemento;
        private ObservableCollection<UfNfce> _estados;
        private UfNfce _ufSelecionado;
        private ObservableCollection<CidadeNfce> _cidades;
        private CidadeNfce _cidadeSelacionada;
        private ObservableCollection<ClienteEnderecoNfce> _enderecos;
        private ClienteEnderecoNfce _enderecoSelecionado;
        private string _inscricaoEstadual;

        public AddClienteVendaFormModel(decimal nfceTotal, NfceDestinatario destinatario)
        {
            _nfceTotal = nfceTotal;
            _destinatario = destinatario;
            Cliente = _destinatario?.Cliente;
        }

        public bool ExibirMensagemCliente
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        private void PreencherView()
        {
            if (_destinatario == null) return;

            DocumentoUnicoCliente = _destinatario.DocumentoUnico;
            NomeCliente = _destinatario.Nome;
            InscricaoEstadual = _destinatario.InscricaoEstadual;
            Cep = _destinatario.Cep;
            Logradouro = _destinatario.Logradouro;
            Numero = _destinatario.Numero;
            Bairro = _destinatario.Bairro;

            if (_destinatario.Cidade == null) return;

            UfSelecionado = (UfNfce) Estados.Where(e => e.Sigla == _destinatario.Cidade.SiglaUf).FirstOrNull();
            CidadeSelacionada = _destinatario.Cidade;
            Complemento = _destinatario.Complemento;
        }

        private ClienteNfce Cliente { get; set; }

        public string DocumentoUnicoCliente
        {
            get => _documentoUnicoCliente;
            set
            {
                if (value == _documentoUnicoCliente) return;
                _documentoUnicoCliente = value;

                if (_documentoUnicoCliente.IsNotNullOrEmpty() && (_documentoUnicoCliente.Length == 11 || _documentoUnicoCliente.Length == 14))
                    PreencherCliente();

                PropriedadeAlterada();
            }
        }

        public string NomeCliente
        {
            get => _nomeCliente;
            set
            {
                if (value == _nomeCliente) return;
                _nomeCliente = value;
                PropriedadeAlterada();
            }
        }

        public string Cep
        {
            get => _cep;
            set
            {
                if (value == _cep) return;
                _cep = value;
                PropriedadeAlterada();
            }
        }

        public string Logradouro
        {
            get => _logradouro;
            set
            {
                if (value == _logradouro) return;
                _logradouro = value;
                PropriedadeAlterada();
            }
        }

        public string Numero
        {
            get => _numero;
            set
            {
                if (value == _numero) return;
                _numero = value;
                PropriedadeAlterada();
            }
        }

        public string Bairro
        {
            get => _bairro;
            set
            {
                if (value == _bairro) return;
                _bairro = value;
                PropriedadeAlterada();
            }
        }

        public string Complemento
        {
            get => _complemento;
            set
            {
                if (value == _complemento) return;
                _complemento = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<UfNfce> Estados
        {
            get => _estados;
            set
            {
                if (Equals(value, _estados)) return;
                _estados = value;
                PropriedadeAlterada();
            }
        }

        public UfNfce UfSelecionado
        {
            get => _ufSelecionado;
            set
            {
                if (Equals(value, _ufSelecionado)) return;
                _ufSelecionado = value;
                PropriedadeAlterada();
                SelecionaCidades(value);
            }
        }

        public ObservableCollection<CidadeNfce> Cidades
        {
            get => _cidades;
            set
            {
                if (Equals(value, _cidades)) return;
                _cidades = value;
                PropriedadeAlterada();
            }
        }

        public CidadeNfce CidadeSelacionada
        {
            get => _cidadeSelacionada;
            set
            {
                if (Equals(value, _cidadeSelacionada)) return;
                _cidadeSelacionada = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<ClienteEnderecoNfce> Enderecos
        {
            get => _enderecos;
            set
            {
                if (Equals(value, _enderecos)) return;
                _enderecos = value;
                PropriedadeAlterada();
            }
        }

        public ClienteEnderecoNfce EnderecoSelecionado
        {
            get => _enderecoSelecionado;
            set
            {
                if (Equals(value, _enderecoSelecionado)) return;
                _enderecoSelecionado = value;
                PropriedadeAlterada();
                PreencherEndereco(value);
            }
        }

        public string InscricaoEstadual
        {
            get => _inscricaoEstadual;
            set
            {
                if (value == _inscricaoEstadual) return;
                _inscricaoEstadual = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandBuscaCliente => GetSimpleCommand(BuscaClienteAction);

        public ICommand CommandRemoverCliente => GetSimpleCommand(LimpaClienteAction);

        public ICommand CommandSalvarCliente => GetSimpleCommand(SalvarClienteAction);

        private void SalvarClienteAction(object obj)
        {
            try
            {
                Hidratacao();
                Validacao();

                var email = Cliente != null && Cliente.Emails.Count > 0 ? Cliente.Emails[0].Email : string.Empty;

                var destinatario = new NfceDestinatario
                {
                    Logradouro = Logradouro,
                    Numero = Numero,
                    Cep = Cep,
                    Complemento = Complemento,
                    Bairro = Bairro,
                    Cidade = CidadeSelacionada,
                    DocumentoUnico = DocumentoUnicoCliente,
                    Nome = NomeCliente,
                    Email = email,
                    Cliente = Cliente,
                    InscricaoEstadual = InscricaoEstadual
                };

                OnSalvarHandler(destinatario);
                OnFechar();
            }
            catch (Exception e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void Validacao()
        {
            if (DocumentoUnicoCliente.IsNullOrEmpty())
            {
                throw new ArgumentException("CPF/CNPJ Obrigatório");
            }

            if (new DocumentoUnicoRegra().NaoValido(DocumentoUnicoCliente))
            {
                throw new ArgumentException("CPF/CNPJ não é válido");
            }

            if (_nfceTotal <= 10000 && Logradouro.IsNullOrEmpty() && Numero.IsNullOrEmpty() && Bairro.IsNullOrEmpty() && Cep.IsNullOrEmpty() && Complemento.IsNullOrEmpty() && CidadeSelacionada == null)
            {
                return;
            }

            ValidacaoEndereco();
        }

        private void ValidacaoEndereco()
        {
            if (Logradouro.IsNullOrEmpty())
                throw new ArgumentException("Logradouro obrigatório");

            if (Numero.IsNullOrEmpty())
                throw new ArgumentException("Número obrigatório");

            if (Bairro.IsNullOrEmpty())
                throw new ArgumentException("Bairro obrigatório");

            if (CidadeSelacionada == null)
                throw new ArgumentException("Cidade obrigatório");

            if (Logradouro.Length < 3)
                throw new ArgumentException("Logradouro deve ser maior que 3 digitos");

            if (Numero.Length < 2)
                throw new ArgumentException("Número deve ser maior que 2 digitos");

            if (Bairro.Length < 2) 
                throw new ArgumentException("Bairro deve ser maior que 2 digitos");
        }

        private void Hidratacao()
        {
            DocumentoUnicoCliente = DocumentoUnicoCliente.TrimOrEmpty();
            NomeCliente = NomeCliente.TrimOrEmpty();
            Cep = Cep.TrimOrEmpty();
            Logradouro = Logradouro.TrimOrEmpty();
            Numero = Numero.TrimOrEmpty();
            Bairro = Bairro.TrimOrEmpty();
            Complemento = Complemento.TrimOrEmpty();
            InscricaoEstadual = InscricaoEstadual.TrimOrEmpty();
        }

        private void RecebeCliente(object sender, ClienteEvent e)
        {
            Cliente = e.Cliente;

            PreencherViewComCliente();
        }

        private void PreencherViewComCliente()
        {
            DocumentoUnicoCliente = Cliente.DocumentoUnico;

            var nome = string.Empty;

            if (Cliente.Nome.Length < 60)
                nome = Cliente.Nome;

            if (Cliente.Nome.Length >= 60)
                nome = Cliente.Nome.Trim().Substring(0, 60);

            NomeCliente = nome;

            InscricaoEstadual = Cliente.InscricaoEstadual.TrimOrEmpty();

            PreencherEnderecosSeNaoExistir(Cliente);
        }

        private void PreencherEnderecosSeNaoExistir(ClienteNfce e)
        {
            Enderecos.Clear();
            e.Enderecos.ForEach(end => { Enderecos.Add(end); });

            if (Enderecos.Count <= 0) return;

            var endereco = Enderecos[0];
            EnderecoSelecionado = endereco;
        }

        private void PreencherEndereco(ClienteEnderecoNfce endereco)
        {
            if (endereco == null) return;

            Cep = endereco.Cep;
            Logradouro = endereco.Logradouro;
            Numero = endereco.Numero;
            Bairro = endereco.Bairro;
            UfSelecionado = Estados.SingleOrDefault(e => e.Sigla == endereco.Cidade.SiglaUf);
            CidadeSelacionada = endereco.Cidade;
            Complemento = endereco.Complemento;
        }

        public void OnRendered()
        {
            var ufs = BuscarUfs();
            PreencherUf(ufs);

            Cidades = new ObservableCollection<CidadeNfce>();
            Enderecos = new ObservableCollection<ClienteEnderecoNfce>();

            PreencherView();
        }

        private void PreencherUf(IEnumerable<UfNfce> ufs)
        {
            Estados = new ObservableCollection<UfNfce>(ufs);
        }

        private IEnumerable<UfNfce> BuscarUfs()
        {
            using (var repositorio = new RepositorioUfNfce(GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao()))
            {
                return repositorio.BuscaTodos();
            }
        }


        private void SelecionaCidades(UfNfce uf)
        {
            Cidades.Clear();

            if (uf == null) return;

            IEnumerable<CidadeNfce> cidades;

            using (var repositorio =
                new RepositorioCidadeNfce(GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao()))
            {
                cidades = repositorio.BuscarCidadePorUf(uf);
            }

            PreencherCidades(cidades);
        }

        private void PreencherCidades(IEnumerable<CidadeNfce> cidades)
        {
            cidades.ForEach(c =>
            {
                Cidades.Add(c);
            });
        }

        public void BuscaClienteAction(object obj)
        {
            var clienteModel = new ClientesFormModel();
            clienteModel.RetornaItem += RecebeCliente;
            new ClientesForm(clienteModel).ShowDialog();
        }

        private void LimpaClienteAction(object obj)
        {
            try
            {
                DocumentoUnicoCliente = string.Empty;
                NomeCliente = string.Empty;
                InscricaoEstadual = string.Empty;
                Cep = string.Empty;
                Logradouro = string.Empty;
                Numero = string.Empty;
                Bairro = string.Empty;
                Complemento = string.Empty;
                CidadeSelacionada = null;
                UfSelecionado = null;
                Enderecos.Clear();
                Cliente = null;

                OnLimparDestinatario();
                DialogBox.MostraInformacao("Cliente removido");
            }
            catch (Exception e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void OnSalvarHandler(NfceDestinatario destinatario)
        {
            SalvarHandler?.Invoke(this, destinatario);
        }

        private void PreencherCliente()
        {
            if (BuscarCliente()) return;

            ClienteNfceDto clienteDto;

            using (var repositorio = new RepositorioNfce(GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao()))
            {
                clienteDto = repositorio.BuscarDestinatarioPorDocumento(DocumentoUnicoCliente);
            }

            PreencherviewComClienteDto(clienteDto);
        }

        private void PreencherviewComClienteDto(ClienteNfceDto destinatario)
        {
            if (destinatario == null) return;

            DocumentoUnicoCliente = destinatario.DocumentoUnico;

            var nome = string.Empty;

            if (destinatario.Nome.Length < 60)
                nome = destinatario.Nome;

            if (destinatario.Nome.Length >= 60)
                nome = destinatario.Nome.Trim().Substring(0, 60);

            NomeCliente = nome;

            InscricaoEstadual = destinatario.InscricaoEstadual.TrimOrEmpty();

            PreencherEnderecoClienteDto(destinatario);
        }

        private void PreencherEnderecoClienteDto(ClienteNfceDto destinatario)
        {
            Cep = destinatario.Cep;
            Logradouro = destinatario.Logradouro;
            Numero = destinatario.Numero;
            Bairro = destinatario.Bairro;
            Complemento = destinatario.Complemento;

            if (destinatario.Cidade == null) return;

            UfSelecionado = Estados.SingleOrDefault(e => e.Sigla == destinatario.Cidade.SiglaUf);
            CidadeSelacionada = destinatario.Cidade;
        }

        private bool BuscarCliente()
        {
            using (var repositorio =
                new RepositorioPessoaNfce(GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao()))
            {
                Cliente = repositorio.BuscarClientePorDocumento(DocumentoUnicoCliente);
            }

            if (Cliente == null) return false;

            PreencherViewComCliente();
            return true;
        }

        private void OnLimparDestinatario()
        {
            LimparDestinatario?.Invoke(this, EventArgs.Empty);
        }
    }
}