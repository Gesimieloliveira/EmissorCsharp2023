using System;
using System.Collections.ObjectModel;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Helpers.Pessoa;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using NHibernate.Util;
using static System.String;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Aba.Models
{
    public class TransportadoraModel : ViewModel
    {
        private int _pessoaId;
        private readonly LocalidadesServico _localidadesServico = LocalidadesServico.GetInstancia();

        public string Nome
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Endereco
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string DocumentoUnico
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string SiglaEstado
        {
            get => GetValue<string>();
            set
            {
                SetValue(value);
                CarregaCidades();
            }
        }

        public CidadeDTO Cidade
        {
            get => GetValue<CidadeDTO>();
            set => SetValue(value);
        }

        public string SiglaEstadoVeiculo
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string PlacaVeiculo
        {
            get => GetValue<string>();
            set => SetValue(value ?? Empty);
        }

        public ObservableCollection<CidadeDTO> Cidades { get; } = new ObservableCollection<CidadeDTO>();

        public bool TemTransportadora => !IsNullOrWhiteSpace(Nome);
        public bool TemVeiculo => SiglaEstadoVeiculo.IsNotNullOrEmpty() && PlacaVeiculo.IsNotNullOrEmpty();

        public void Validar()
        {
            if (!TemTransportadora)
            {
                return;
            }

            if (DocumentoUnico.IsNullOrEmpty())
                throw new ArgumentException("Cpf/Cnpj campo obrigatório");

            if (Nome.IsNullOrEmpty())
                throw new ArgumentException("Razação Social/Nome campo obrigatório");

            if (Endereco.IsNullOrEmpty())
                throw new ArgumentException("Endereço campo obrigatório");

            if (Cidade == null)
                throw new ArgumentException("Cidade obrigatório");

            if (SiglaEstado.IsNullOrEmpty())
                throw new ArgumentException("UF obrigatório");
        }

        public void LimparTransportadora()
        {
            _pessoaId = 0;

            Nome = Empty;
            DocumentoUnico = Empty;
            Endereco = Empty;
            SiglaEstado = null;
            Cidade = null;

            PropriedadeAlterada(nameof(TemTransportadora));
        }

        private void CarregaCidades()
        {
            Cidades.Clear();

            if (IsNullOrWhiteSpace(SiglaEstado)) return;

            var cidades = _localidadesServico.GetCidades(c => c.SiglaUf.ToLower() == SiglaEstado.ToLower());
            cidades?.ForEach(Cidades.Add);
        }

        public void PreecherCom(Nfeletronica nfe)
        {
            if (nfe.Transportadora == null) return;

            _pessoaId = nfe.Transportadora.GetPessoaId();
            Nome = nfe.Transportadora.Nome;
            DocumentoUnico = nfe.Transportadora.DocumentoUnico;
            SiglaEstado = nfe.Transportadora.SiglaUf;
            Endereco = nfe.Transportadora.EnderecoCompleto;
            SiglaEstadoVeiculo = nfe.Transportadora.Veiculo.SiglaUF;
            PlacaVeiculo = nfe.Transportadora.Veiculo.Placa;
            Cidade = GetCidadePeloNome(nfe.Transportadora.NomeMunicipio);
        }

        private CidadeDTO GetCidadePeloNome(string nomeMunicipio)
        {
            return !IsNullOrWhiteSpace(nomeMunicipio)
                ? _localidadesServico.GetCidade(c => c.Nome.ToLower() == nomeMunicipio.ToLower())
                : null;
        }

        public void PreecherCom(Transportadora transportadora)
        {
            _pessoaId = transportadora.Id;

            Nome = transportadora.Nome.TrimSefaz(60);
            DocumentoUnico = transportadora.Pessoa.GetDocumentoUnico();
            InscricaoEstadual = transportadora.InscricaoEstadual;

            var enderecoPrincipal = transportadora.Pessoa.GetEnderecoPrincipal();
            Endereco = GetStringEndereco(enderecoPrincipal);
            SiglaEstado = enderecoPrincipal?.Cidade.SiglaUf;
            Cidade = enderecoPrincipal?.Cidade;

            PropriedadeAlterada(nameof(TemTransportadora));
        }

        public string InscricaoEstadual { get; set; }

        public void PreecherCom(Veiculo veiculo)
        {
            SiglaEstadoVeiculo = veiculo?.SiglaUf;
            PlacaVeiculo = veiculo?.Placa;
        }

        private static string GetStringEndereco(PessoaEndereco endereco)
        {
            return endereco == null
                ? Empty
                : $"{endereco.Logradouro}, {endereco.Numero}, {endereco.Bairro}";
        }

        public int GetPessoaId()
        {
            return _pessoaId;
        }

        public VeiculoTransporte GetVeiculoFiscal()
        {
            return new VeiculoTransporte(PlacaVeiculo, SiglaEstadoVeiculo);
        }
    }
}