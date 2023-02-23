using System.Collections.ObjectModel;
using System.Windows.Input;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.FusionAdm.Fiscal.Contratos.Componentes;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.Componentes;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Helpers.Pessoa;
using FusionCore.MigracaoFluente.Migracoes;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Aba.Models
{
    public class DestinatarioModel : ViewModel
    {
        private int _pessoaId;
        private readonly LocalidadesServico _localidadesServico;
        private EmitenteNfe _emitente;

        public DestinatarioModel()
        {
            Cidades = new ObservableCollection<CidadeDTO>();
            LocaisEntregas = new ObservableCollection<PessoaEndereco>();
            IndicadorIE = IndicadorIE.NaoContribuinte;
            IndicadorPresencaComprador = IndicadorComprador.NaoSeAplica;
            IndicadorOperacaoFinal = IndicadorOperacaoFinal.ConsumidorFinal;
            IndicadorDestinoOperacao = DestinoOperacao.Interna;

            _localidadesServico = LocalidadesServico.GetInstancia();
        }

        public string Logradouro
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Numero
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Nome
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Cep
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string DocumentoUnico
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string InscricaoEstadual
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Bairro
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public CidadeDTO Cidade
        {
            get => GetValue<CidadeDTO>();
            set => SetValue(value);
        }

        public EstadoDTO Estado
        {
            get => GetValue<EstadoDTO>();
            set
            {
                SetValue(value);
                CarregaCidades();
            }
        }

        public string Telefone
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int IbgeCidade
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public byte IbgeEstado
        {
            get => GetValue<byte>();
            set => SetValue(value);
        }

        public IndicadorIE IndicadorIE
        {
            get => GetValue<IndicadorIE>();
            set => SetValue(value);
        }

        public DestinoOperacao IndicadorDestinoOperacao
        {
            get => GetValue<DestinoOperacao>();
            set => SetValue(value);
        }

        public IndicadorComprador IndicadorPresencaComprador
        {
            get => GetValue<IndicadorComprador>();
            set => SetValue(value);
        }

        public IndicadorOperacaoFinal IndicadorOperacaoFinal
        {
            get => GetValue<IndicadorOperacaoFinal>();
            set => SetValue(value);
        }

        public ObservableCollection<CidadeDTO> Cidades
        {
            get => GetValue<ObservableCollection<CidadeDTO>>();
            set => SetValue(value);
        }

        public string Complemento
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool SolicitaPedido
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ObservableCollection<PessoaEndereco> LocaisEntregas
        {
            get => GetValue<ObservableCollection<PessoaEndereco>>();
            set => SetValue(value);
        }

        public PessoaEndereco LocalEntregaSelecionada
        {
            get => GetValue<PessoaEndereco>();
            set => SetValue(value);
        }

        public ICommand SearchDestinatarioCommand => GetSimpleCommand(SearchDestinatarioHandler);

        private void CarregaCidades()
        {
            if (Estado == null) return;
            var cidades = _localidadesServico.GetCidades(c => c.SiglaUf == Estado.Sigla);
            Cidades = new ObservableCollection<CidadeDTO>(cidades);
        }

        private void SearchDestinatarioHandler(object obj)
        {
            var pickerModel = new PessoaPickerModel(new ClienteEngine());
            pickerModel.PickItemEvent += (sender, e) => PreecherCom(e.GetItem<Cliente>());
            pickerModel.GetPickerView().ShowDialog();
        }

        public void PreecherCom(Nfeletronica nfe)
        {
            _pessoaId = nfe.Destinatario.GetPessoaId();

            Nome = nfe.Destinatario.Nome;
            InscricaoEstadual = nfe.Destinatario.InscricaoEstadual.TrimOrEmpty();
            IndicadorIE = nfe.Destinatario.IndicadorIE;
            DocumentoUnico = nfe.Destinatario.DocumentoUnico;
            Cep = nfe.Destinatario.Endereco.Cep;
            Bairro = nfe.Destinatario.Endereco.Bairro;
            Logradouro = nfe.Destinatario.Endereco.Logradouro;
            Numero = nfe.Destinatario.Endereco.Numero;
            IbgeEstado = nfe.Destinatario.Endereco.Localizacao.CodigoUF;
            IbgeCidade = nfe.Destinatario.Endereco.Localizacao.CodigoMunicipio;
            Telefone = nfe.Destinatario.Endereco.Telefone;
            Complemento = nfe.Destinatario.Endereco.Complemento;
            IndicadorPresencaComprador = nfe.Destinatario.IndicadorPresenca.GetValueOrDefault(IndicadorComprador.NaoSeAplica);
            IndicadorDestinoOperacao = nfe.Destinatario.IndicadorDestinoOperacao.GetValueOrDefault(DestinoOperacao.Interna);
            IndicadorOperacaoFinal = nfe.Destinatario.IndicadorOperacaoFinal.GetValueOrDefault(IndicadorOperacaoFinal.ConsumidorFinal);


            LocaisEntregas = new ObservableCollection<PessoaEndereco>(nfe.Destinatario.FindCliente().Enderecos);
            LocalEntregaSelecionada = nfe.LocalEntrega?.Endereco;
        }

        public void PreecherCom(Cliente cliente)
        {
            _pessoaId = cliente?.Id ?? 0;

            Nome = cliente?.Pessoa.Nome;

            InscricaoEstadual = cliente?.Pessoa.InscricaoEstadual;
            IndicadorIE = new InscricaoEstadual(InscricaoEstadual).GetIndicador();
            DocumentoUnico = cliente?.Pessoa.GetDocumentoUnico();
            SolicitaPedido = cliente?.SolicitaPedidoNfe ?? false;

            var endereco = cliente?.Pessoa.GetEnderecoPrincipal();

            Cep = endereco?.Cep;
            Bairro = endereco?.Bairro.TrimSefaz(60);
            Logradouro = endereco?.Logradouro.TrimSefaz(60);
            Numero = endereco?.Numero.TrimSefaz(60);
            IbgeEstado = GetIbgeEstado(endereco?.Cidade);
            IbgeCidade = endereco?.Cidade.CodigoIbge ?? default(int);
            Complemento = endereco?.Complemento.TrimSefaz(60);

            var telefone = cliente?.Pessoa.GetPrimeiroTelefone();
            Telefone = telefone?.Numero ?? string.Empty;

            if (cliente?.Enderecos == null || cliente.Enderecos.Count == 0) return;
            LocaisEntregas = new ObservableCollection<PessoaEndereco>(cliente.Enderecos);

            IndicadorOperacaoFinal = IndicadorIE == IndicadorIE.NaoContribuinte
                ? IndicadorOperacaoFinal.ConsumidorFinal
                : IndicadorOperacaoFinal.Normal;

            IndicadorPresencaComprador = IndicadorComprador.NaoSeAplica;
            DefinirIndicadorDestinoOperacao();
        }

        private void DefinirIndicadorDestinoOperacao()
        {
            IndicadorDestinoOperacao = DestinoOperacao.Interna;

            if (IbgeEstado == 99) // IBGE 99 == Exterior
            {
                IndicadorDestinoOperacao = DestinoOperacao.Exterior;
            }
            else if (_emitente != null && _emitente.Empresa.EstadoDTO.CodigoIbge != IbgeEstado)
            {
                IndicadorDestinoOperacao = DestinoOperacao.Interestadual;
            }
        }

        private byte GetIbgeEstado(CidadeDTO cidade)
        {
            if (cidade == null) return default(byte);

            var estado = _localidadesServico.GetEstado(e => e.Sigla == cidade.SiglaUf);
            return estado?.CodigoIbge ?? 0;
        }

        public int GetPessoaId()
        {
            return _pessoaId;
        }

        public IEnderecoFiscal GetEnderecoFiscal()
        {
            var localizacao = new LocalizacaoFiscal(Cidade.Nome, Cidade.CodigoIbge, Estado.CodigoIbge, Estado.Sigla);
            var enderecoFiscal = new EnderecoFiscal(Cep, Logradouro, Numero, Bairro, Complemento, localizacao, Telefone);

            return enderecoFiscal;
        }

        public void DefinirEmitente(EmitenteNfe emitente)
        {
            _emitente = emitente;
        }
    }
}