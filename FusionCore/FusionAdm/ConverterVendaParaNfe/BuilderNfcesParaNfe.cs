using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.Perfil;
using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.Helpers.Pessoa;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Vendas.Faturamentos;

namespace FusionCore.FusionAdm.ConverterVendaParaNfe
{
    public class BuilderNfcesParaNfe
    {
        private readonly Nfeletronica _nfeletronica;
        private readonly EmitenteNfe _emitenteNfe;
        private readonly UsuarioDTO _usuarioCriacao;
        private readonly Cliente _cliente;
        private PerfilNfe _perfilNfe;
        private decimal _totalDescontoFixo;

        public BuilderNfcesParaNfe(EmitenteNfe emitenteNfe, UsuarioDTO usuarioCriacao, Cliente cliente)
        {
            _emitenteNfe = emitenteNfe;
            _usuarioCriacao = usuarioCriacao;
            _cliente = cliente;

            _nfeletronica = new Nfeletronica(_emitenteNfe, _usuarioCriacao);
        }

        public BuilderNfcesParaNfe ComPerfilNfe(PerfilNfe perfilNfe)
        {
            _perfilNfe = perfilNfe;
            return this;
        }

        public Nfeletronica Construir()
        {
            _nfeletronica.PerfilId = _perfilNfe.Id;
            _nfeletronica.TipoOperacao = _perfilNfe.TipoOperacao;
            _nfeletronica.FinalidadeEmissao = _perfilNfe.FinalidadeEmissao;
            _nfeletronica.InformacaoAdicional = _perfilNfe.Observacao.Trim();
            _nfeletronica.IncluirInformacaoIbpt = true;
            _nfeletronica.NaturezaOperacao = _perfilNfe.NaturezaOperacao;
            _nfeletronica.ValorDescontoFixo = _totalDescontoFixo;
            _nfeletronica.PedidoInternoSistema = true;

            _nfeletronica.CalcularItens();

            AdicionarCliente();
            AdicionarTransportadora();

            return _nfeletronica;
        }

        public BuilderNfcesParaNfe ComProdutos(IEnumerable<NfceItemAdm> produtos)
        {
            foreach (var nfceItem in produtos)
            {
                new NfceAdmItemParaItemNfe().Cria(nfceItem, _nfeletronica, _cliente, _perfilNfe, _emitenteNfe);
            }

            _nfeletronica.CalcularItens();

            return this;
        }

        public BuilderNfcesParaNfe ComProdutos(IEnumerable<FaturamentoProduto> produtos)
        {
            foreach (var faturamentoProduto in produtos)
            {
                new FaturamentoProdutoParaItemNfe().Cria(faturamentoProduto, _nfeletronica, _cliente, _perfilNfe, _emitenteNfe);
            }

            _nfeletronica.CalcularItens();

            return this;
        }

        public BuilderNfcesParaNfe ComDescontoFixo(decimal totalDescontoFixo)
        {
            _totalDescontoFixo = totalDescontoFixo;

            return this;
        }

        private void AdicionarTransportadora()
        {
            try
            {
                if (_perfilNfe.Transportadora == null) return;

                var transportadora = _perfilNfe.Transportadora.Transportadora;
                var veiculo = _perfilNfe.Transportadora.Veiculo;

                if (transportadora == null) return;

                var enderecoPrincipal = transportadora.GetEnderecoPrincipal();

                if (enderecoPrincipal == null)
                    throw new InvalidOperationException(
                        $"Adicionar endereço na transportadora que esta no perfil atual\nTransportadora: {transportadora.Nome}");


                if (veiculo == null)
                {
                    throw new InvalidOperationException("Adicionar veiculo para transportadora");
                }

                _nfeletronica.Transportadora = new TransportadoraNfe(_nfeletronica,
                    transportadora.GetDocumentoUnico(),
                    transportadora.Nome)
                {
                    SiglaUf = enderecoPrincipal.Cidade.SiglaUf,
                    InscricaoEstadual = transportadora.InscricaoEstadual,
                    EnderecoCompleto = enderecoPrincipal.ToString(),
                    NomeMunicipio = enderecoPrincipal.Cidade.Nome,
                    Veiculo = new VeiculoTransporte(veiculo.Placa, veiculo.SiglaUf)
                };

                _nfeletronica.ModalidadeFrete = ModalidadeFrete.PorContaDestintario;
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        private void AdicionarCliente()
        {
            var telefone = _cliente.Telefones.FirstOrDefault();
            var endereco = _cliente.GetEnderecoPrincipal();

            _nfeletronica.Destinatario = new DestinatarioNfe(
                _nfeletronica,
                new EnderecoFiscal(
                    endereco.Cep,
                    endereco.Logradouro,
                    endereco.Numero,
                    endereco.Bairro,
                    endereco.Complemento,

                    new LocalizacaoFiscal(endereco.Cidade.Nome,
                        endereco.Cidade.CodigoIbge,
                        LocalidadesServico.GetInstancia().GetEstados()
                            .First(uf => uf.Sigla == endereco.Cidade.SiglaUf)
                            .CodigoIbge,
                        endereco.Cidade.SiglaUf),
                    telefone != null ? telefone.Numero : string.Empty
                ));

            _nfeletronica.Destinatario.InscricaoEstadual = _cliente.InscricaoEstadual;
            _nfeletronica.Destinatario.DocumentoUnico = _cliente.GetDocumentoUnico();
            _nfeletronica.Destinatario.Nome = _cliente.Nome;

            _nfeletronica.Destinatario.ReferenciaUmaPessoaId(_cliente.Id);
            _nfeletronica.Destinatario.AutoDefinirIndicadores();
        }

        public void ComChave(string chaveNfce)
        {
            _nfeletronica.AdicionaReferencia(new ReferenciaNfe(_nfeletronica, new ChaveSefaz(chaveNfce)));
        }
    }
}