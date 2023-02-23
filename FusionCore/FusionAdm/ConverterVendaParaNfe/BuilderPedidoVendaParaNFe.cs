using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.Pagamentos;
using FusionCore.FusionAdm.Fiscal.NF.Perfil;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Basico;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Helpers.Pessoa;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Estadual;
using FusionCore.Tributacoes.Flags;
using FusionCore.Tributacoes.Regras;
using FusionCore.Tributacoes.Repositorio;

namespace FusionCore.FusionAdm.ConverterVendaParaNfe
{
    public class BuilderPedidoVendaParaNFe
    {
        private readonly PedidoVenda.PedidoVenda _pedidoVenda;

        private readonly Nfeletronica _nfeletronica;
        private readonly EmitenteNfe _emitenteNfe;
        private readonly UsuarioDTO _usuarioCriacao;
        private AbaPerfilNfeDTO _perfilNfe;
        private PedidoDestinatario _destinatarioPv;
        private decimal _totalDescontoFixo;
        private Transportadora _transportadora;
        private Veiculo _veiculo;
        private List<FormaPagamentoNfe> _pagamentos;
        private PerfilNfeSimplesNacional _simplesNacionalPerfil;

        public BuilderPedidoVendaParaNFe(
            PedidoVenda.PedidoVenda pedidoVenda,
            EmitenteNfe emitenteNfe,
            UsuarioDTO usuarioCriacao
        )
        {
            _pedidoVenda = pedidoVenda;
            _usuarioCriacao = usuarioCriacao;
            _emitenteNfe = emitenteNfe;

            _nfeletronica = new Nfeletronica(_emitenteNfe, _usuarioCriacao);
        }

        public BuilderPedidoVendaParaNFe ComPerfilNfe(AbaPerfilNfeDTO perfilNfe)
        {
            _perfilNfe = perfilNfe;
            return this;
        }

        public Nfeletronica Construir()
        {
            _nfeletronica.TabelaPreco = _pedidoVenda.TabelaPreco;
            _nfeletronica.PerfilId = _perfilNfe.Id;
            _nfeletronica.TipoOperacao = _perfilNfe.TipoOperacao;
            _nfeletronica.FinalidadeEmissao = _perfilNfe.FinalidadeEmissao;
            _nfeletronica.InformacaoAdicional = _perfilNfe.Observacao.Trim();
            _nfeletronica.IncluirInformacaoIbpt = true;
            _nfeletronica.NaturezaOperacao = _perfilNfe.NaturezaOperacao;
            _nfeletronica.ValorDescontoFixo = _totalDescontoFixo;
            _nfeletronica.InformacaoAdicional += $";{_pedidoVenda.Observacao.TrimOrEmpty()}";
            _nfeletronica.PedidoInternoSistema = true;

            _nfeletronica.CalcularItens();

            AdicionarCliente();
            AdicionarTransportadora();
            AjustarPartilhaItens();

            return _nfeletronica;
        }

        private void AdicionarTransportadora()
        {
            try
            {
                if (_transportadora == null) return;

                var enderecoPrincipal = _transportadora.GetEnderecoPrincipal();

                if (enderecoPrincipal == null)
                    throw new InvalidOperationException(
                        $"Adicionar endereço na transportadora que esta no perfil atual\nTransportadora: {_transportadora.Nome}");


                if (_veiculo == null)
                {
                    throw new InvalidOperationException("Adicionar veiculo para transportadora");
                }

                _nfeletronica.Transportadora = new TransportadoraNfe(_nfeletronica,
                    _transportadora.GetDocumentoUnico(),
                    _transportadora.Nome)
                {
                    SiglaUf = enderecoPrincipal.Cidade.SiglaUf,
                    InscricaoEstadual = _transportadora.InscricaoEstadual,
                    EnderecoCompleto = enderecoPrincipal.ToString(),
                    NomeMunicipio = enderecoPrincipal.Cidade.Nome,
                    Veiculo = new VeiculoTransporte(_veiculo.Placa, _veiculo.SiglaUf)
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
            ConstruirDestinatarioDoPedidoVenda();
        }

        private void ConstruirDestinatarioDoPedidoVenda()
        {
            var telefone = _destinatarioPv.Cliente.Telefones.FirstOrDefault();

            var ibgeEstado = LocalidadesServico.GetInstancia().GetEstados()
                .First(uf => uf.Sigla == _destinatarioPv.Cidade.SiglaUf).CodigoIbge;

            var localizacaoFiscal = new LocalizacaoFiscal(
                _destinatarioPv.Cidade.Nome,
                _destinatarioPv.Cidade.CodigoIbge,
                ibgeEstado,
                _destinatarioPv.Cidade.SiglaUf
            );

            var endereco = new EnderecoFiscal(
                _destinatarioPv.Cep,
                _destinatarioPv.Logradouro,
                _destinatarioPv.Numero,
                _destinatarioPv.Bairro,
                _destinatarioPv.Complemento,
                localizacaoFiscal,
                telefone != null ? telefone.Numero : string.Empty
            );

            _nfeletronica.Destinatario = new DestinatarioNfe(_nfeletronica, endereco);
            _nfeletronica.Destinatario.InscricaoEstadual = _destinatarioPv.Cliente.InscricaoEstadual;
            _nfeletronica.Destinatario.DocumentoUnico = _destinatarioPv.Cliente.GetDocumentoUnico();
            _nfeletronica.Destinatario.Nome = _destinatarioPv.Cliente.Nome;
            _nfeletronica.Destinatario.IndicadorIE = _destinatarioPv.Cliente.Pessoa.IndicadorIEDestinatario;

            _nfeletronica.Destinatario.ReferenciaUmaPessoaId(_destinatarioPv.Cliente.Id);
            _nfeletronica.Destinatario.AutoDefinirIndicadores();
        }

        private void AjustarPartilhaItens()
        {
            PerfilNfe perfil;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPerfilNfe(sessao);
                perfil = repositorio.GetPeloId(_perfilNfe.Id);

                if (perfil == null) return;
            }

            foreach (var item in _nfeletronica.Itens)
            {
                if (_nfeletronica.Destinatario.ResideForaDoEstado() == false)
                {
                    item.PartilharIcms = false;
                    continue;
                }

                item.PartilharIcms = perfil.AutoAtivarPartilhaIcms;
            }
        }

        public BuilderPedidoVendaParaNFe ComTransportadora(Transportadora transportadora)
        {
            _transportadora = transportadora;
            return this;
        }

        public BuilderPedidoVendaParaNFe ComDestinatario(PedidoDestinatario pedidoVendaDestinatario)
        {
            _destinatarioPv = pedidoVendaDestinatario;

            return this;
        }

        public BuilderPedidoVendaParaNFe ComProdutos(IEnumerable<PedidoVendaProduto> produtos)
        {
            foreach (var pedidoItem in produtos)
            {
                var item = new ItemNfe(_nfeletronica)
                {
                    NumeroPedido = _pedidoVenda.Id.ToString(),
                    NumeroItemPedido = pedidoItem.Numero,
                    Observacao = pedidoItem.Observacao,
                    AutoAjustarImposto = true
                };

                item.ComMercadoria(pedidoItem.Produto,
                    pedidoItem.Quantidade,
                    pedidoItem.PrecoUnitario,
                    pedidoItem.TotalDesconto);

                DefineCodigoDeBarras(pedidoItem, item);

                item.ComCodigoUtilizado(pedidoItem.Produto.Id.ToString());

                SetCfopItem(item, pedidoItem.Produto.RegraTributacaoSaida);
                SetPisCofins(item, pedidoItem.Produto);
                SetIpi(item, pedidoItem);

                item.CodigoBeneficioFiscal = string.Empty;

                DefineIcms(item, pedidoItem);

                item.MovimentaEstoque = false;

                _nfeletronica.AdicionarItem(item);
            }

            _nfeletronica.CalcularItens();
            _nfeletronica.CalcularCreditoItens();

            return this;
        }

        private static void DefineCodigoDeBarras(PedidoVendaProduto itemPedidoDeVenda, ItemNfe item)
        {
            var codigoBarras = itemPedidoDeVenda.Produto.ProdutosAlias.FirstOrDefault(i => i.IsCodigoBarras);

            var barras = codigoBarras != null ? codigoBarras.Alias : "SEM GTIN";
            const string semGtin = "SEM GTIN";

            if (barras != semGtin)
            {
                barras = Gs1GtinHelper.EhUmGtinValido(barras) ? barras : semGtin;
            }

            item.ComBarras(barras);
        }

        private static void SetIpi(ItemNfe item, PedidoVendaProduto itemPedidoDeVenda)
        {
            item.Ipi = new ImpostoIpi(item, itemPedidoDeVenda.Produto.SituacaoTributariaIpi)
            {
                AliquotaIpi = 0.0m, // todo itemPedidoDeVenda.Produto.AliquotaIpi,
                TributacaoIpi = itemPedidoDeVenda.Produto.SituacaoTributariaIpi
            };

            item.Ipi.AjustarIpi();
        }

        private void SetPisCofins(ItemNfe item, ProdutoDTO produto)
        {
            item.Pis = new ImpostoPis(item)
            {
                AliquotaPis = produto.AliquotaPis,
                Cst = produto.Pis
            };

            item.Pis.AjustarPis();

            item.Cofins = new ImpostoCofins(item)
            {
                AliquotaCofins = produto.AliquotaCofins,
                Cst = produto.Cofins
            };

            item.Cofins.AjustarCofins();
        }

        private void SetCfopItem(ItemNfe item, RegraTributacaoSaida produtoRegraTributacaoSaida)
        {
            if (_emitenteNfe.Empresa.CidadeDTO.SiglaUf != _destinatarioPv.Cidade.SiglaUf)
            {
                var cfop = produtoRegraTributacaoSaida.CfopParaNfeFrom(DestinoOperacao.Interestadual);

                item.Cfop = cfop;
            }

            if (_emitenteNfe.Empresa.CidadeDTO.SiglaUf == _destinatarioPv.Cidade.SiglaUf)
            {
                var cfop = produtoRegraTributacaoSaida.CfopParaNfeFrom(DestinoOperacao.Interna);

                item.Cfop = cfop;
            }

            if (_destinatarioPv.Cidade.SiglaUf == "EX")
            {
                var cfop = produtoRegraTributacaoSaida.CfopParaNfeFrom(DestinoOperacao.Exterior);

                item.Cfop = cfop;
            }
        }

        public BuilderPedidoVendaParaNFe ComDescontoFixo(decimal totalDescontoFixo)
        {
            _totalDescontoFixo = totalDescontoFixo;

            return this;
        }

        private void DefineIcms(ItemNfe nfeItem, PedidoVendaProduto pedidoItem)
        {
            if (_simplesNacionalPerfil.Csosn != null &&
                _emitenteNfe.Empresa.RegimeTributario == RegimeTributario.SimplesNacional)
            {
                nfeItem.ImpostoIcms =
                    new ImpostoIcms(nfeItem, ProcuraTributacaoEquivalente(_simplesNacionalPerfil.Csosn))
                    {
                        AliquotaCredito = _simplesNacionalPerfil.Csosn.PermiteCredito()
                            ? _simplesNacionalPerfil.AliquotaCredito
                            : 0.0m
                    };

                return;
            }

            var produto = pedidoItem.Produto;
            var cst = FindTributacaoSt(produto);

            nfeItem.ImpostoIcms = new ImpostoIcms(nfeItem, cst);

            if (cst.PermiteIcms())
            {
                nfeItem.ImpostoIcms.AliquotaIcms = produto.AliquotaIcms;
                nfeItem.ImpostoIcms.ReducaoBcIcms = cst.PermiteReducaoIcms() ? produto.ReducaoIcms : 0;
            }

            if (cst.PermiteSubstituicao())
            {
                nfeItem.ImpostoIcms.AliquotaSt = produto.AliquotaIcms;
                nfeItem.ImpostoIcms.ReducaoBcSt = produto.ReducaoIcms;
                nfeItem.ImpostoIcms.MvaSt = produto.PercentualMva;
            }

            nfeItem.ImpostoIcms.AjustarImposto();
        }

        private TributacaoCst ProcuraTributacaoEquivalente(TributacaoCsosn csosn)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var rcst = new RepositorioTributacaoCst(sessao);
                var cst = rcst.GetPeloId(csosn.Codigo);
                return cst;
            }
        }

        private TributacaoCst FindTributacaoSt(ProdutoDTO produto)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositoriocst = new RepositorioTributacaoCst(sessao);

                if (_emitenteNfe.RegimeTributario != RegimeTributario.SimplesNacional)
                {
                    var cst = produto.RegraTributacaoSaida.Cst;
                    return repositoriocst.GetPeloId(cst.Codigo);
                }

                var csosn = produto.RegraTributacaoSaida.Csosn.Codigo;

                return repositoriocst.GetPeloId(csosn);
            }
        }

        public BuilderPedidoVendaParaNFe ComVeiculoTransportadora(Veiculo veiculo)
        {
            _veiculo = veiculo;
            return this;
        }

        public BuilderPedidoVendaParaNFe ComPagamento(IEnumerable<Negociacao> negociacoes)
        {
            _pagamentos = new List<FormaPagamentoNfe>();

            foreach (var n in negociacoes)
            {
                switch (n.Especie)
                {
                    case ETipoPagamento.Dinheiro:
                    {
                        _pagamentos.Add(new DinheiroNfe(_usuarioCriacao, n.Valor));

                        continue;
                    }

                    case ETipoPagamento.CreditoLoja:
                    {
                        var parcelas = n.Parcelas
                            .Select(pe => new ParcelaNfe((byte) pe.Numero, pe.Vencimento, pe.Valor))
                            .ToList();

                        var prazo = new Aprazo(_usuarioCriacao, n.TipoDocumento, parcelas);
                        _pagamentos.Add(prazo);

                        continue;
                    }

                    case ETipoPagamento.CartaoCredito:
                    {
                        _pagamentos.Add(new CartaoCreditoNfe(_usuarioCriacao, n.Valor));

                        continue;
                    }

                    case ETipoPagamento.CartaoDebito:
                    {
                        _pagamentos.Add(new CartaoDebitoNfe(_usuarioCriacao, n.Valor));

                        continue;
                    }
                }
            }

            return this;
        }

        public void ComPerfilSimplesNacional(PerfilNfeSimplesNacional simplesNacionalPerfil)
        {
            _simplesNacionalPerfil = simplesNacionalPerfil;
        }
    }
}