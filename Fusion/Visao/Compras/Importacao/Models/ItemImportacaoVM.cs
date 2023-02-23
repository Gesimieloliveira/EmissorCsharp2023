using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Fusion.Visao.Cfop;
using Fusion.Visao.Produto;
using FusionCore.Core.Flags;
using FusionCore.Core.Nfes.Xml.Componentes.Impl;
using FusionCore.Core.Tributario;
using FusionCore.FusionAdm.Compras;
using FusionCore.FusionAdm.Produtos;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.RecipienteDados;
using FusionCore.RecipienteDados.Adm.Impl;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Federal;
using FusionCore.Tributacoes.Flags;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.Compras.Importacao.Models
{
    public class ItemImportacaoVM : ViewModel
    {
        private ProdutoVinculoCompra _vinculo;
        private readonly XmlProduto _xml;
        private PessoaVM _fornecedorVinculo;
        private bool _ativo;

        public ItemImportacaoVM(XmlProduto xml, CfopDTO cfop)
        {
            _xml = xml;
            NomeProduto = xml.Nome;
            Cean = xml.Cean;
            Unidade = xml.Unidade;
            Quantidade = xml.Quantidade;
            ValorUnitario = xml.ValorUnitario;
            ValorDesconto = xml.ValorDesconto;
            ValorTotal = xml.ValorTotalComDesconto;
            NomeVinculado = "NÃO VINCULADO";
            CfopOrigem = xml.Cfop;
            CodigoCfopDestino = cfop.Id;
            CfopDestino = cfop;

        }

        private ProdutoDTO Produto
        {
            get => GetValue<ProdutoDTO>();
            set
            {
                SetValue(value);

                UnidadeConversao = value?.ProdutoUnidadeDTO.Sigla ?? string.Empty;
                NomeVinculado = value?.Nome ?? "NÃO VINCULADO";
                Vinculado = value != null;
                Ativo = value == null || value.Ativo;

            }
                
        }

        public bool Ativo
        {
            get => _ativo;
            set
            {
                if (value == _ativo) return;
                _ativo = value;
                PropriedadeAlterada();
            }
        }

        public short CfopOrigem
        {
            get => GetValue<short>();
            private set => SetValue(value);
        }

        public string CodigoCfopDestino
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public CfopDTO CfopDestino
        {
            get => GetValue<CfopDTO>();
            private set
            {
                SetValue(value);
                CodigoCfopDestino = value?.Id;
            }
        }

        public string NomeProduto
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string NomeVinculado
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Cean
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Unidade
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public decimal Quantidade
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorUnitario
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorDesconto
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal ValorTotal
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal FatorConversao
        {
            get => GetValue<decimal>();
            set
            {
                SetValue(value);
                CalculaConversao();
            }
        }

        public decimal QuantidadeConversao
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public string UnidadeConversao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool Vinculado
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ICommand CommandVincular => GetSimpleCommand(o =>
        {
            try
            {
                var vm = new ProdutoGridPickerModel();
                vm.UsaProdutoBase(CriaProdutoBase());
                vm.Titulo = $"VINCULAR: {_xml.Nome}";
                vm.InicializarComPesquisa(ConvertNomeToPipeSearch(_xml.Nome));

                vm.PickItemEvent += (s, e) =>
                {
                    Produto = e.GetItem<ProdutoDTO>();
                    ArmazenaVinculo();
                };

                vm.GetPickerView().ShowDialog();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        });

        public ICommand ComandoAtivarProduto => GetSimpleCommand(o =>
        {
            try
            {
                if (!DialogBox.MostraConfirmacao("Deseja realmente ativar este produto novamente?",
                    MessageBoxImage.Question)) return;

                Produto.Ativo = true;

                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                using (var transacao = sessao.BeginTransaction())
                {
                    new RepositorioProduto(sessao).Salvar(Produto);

                    Ativo = true;
                    transacao.Commit();
                }
                DialogBox.MostraInformacao("Produto ativado com sucesso ;)");
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        });

        public ICommand CfopDestinoCommand => GetSimpleCommand(o =>
        {
            var view = new CfopPickerView();

            view.Contexto.MostrarApenasOsDeOperacao(TipoOperacao.Entrada);
            view.Contexto.MostrarApenasOsDeOrigem(CodigoCfopHelper.ObtemOrigem(_xml.Cfop.ToString()));

            view.Contexto.CfopFoiSelecionado += (sender, cfop) =>
            {
                CfopDestino = cfop;
                view.Close();
            };

            view.ShowDialog();
        });

        private void CalculaConversao()
        {
            QuantidadeConversao = decimal.Round(Quantidade * FatorConversao, 4);
        }

        public void FazVinculo(PessoaVM fornecedor)
        {
            _fornecedorVinculo = fornecedor;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProduto(sessao);

                _vinculo = repositorio.BuscaVinculo(_xml.Codigo, _fornecedorVinculo.DocumentoUnico, _xml.Unidade);

                if (_vinculo != null)
                    Produto = _vinculo.Produto;

                if (_vinculo == null)
                    Produto = repositorio.BuscaPeloAlias(Cean);

                FatorConversao = _vinculo?.FatorUtilizado ?? 1.00M;
            }
        }

        private string ConvertNomeToPipeSearch(string xmlNome)
        {
            var pesquisa = new List<string>();

            xmlNome.Split(' ').ForEach(s =>
            {
                var parte = s.Trim();

                if (parte.Length > 2)
                {
                    pesquisa.Add(parte);
                }
            });

            return string.Join("|", pesquisa);
        }

        private ProdutoDTO CriaProdutoBase()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var unidades = new RepositorioProdutoUnidade(sessao);
                var grupos = new RepositorioGrupoProduto(sessao);

                var unidadeComprativel = unidades.GetUnidadesPelaSigla(_xml.Unidade).FirstOrDefault();
                var precoCompra = decimal.Round(_xml.ValorTotalComDesconto / QuantidadeConversao, 4);

                var produto = new ProdutoDTO
                {
                    Nome = _xml.Nome,
                    Cest = _xml.Cest,
                    Ncm = _xml.Ncm,
                    PrecoCompra = precoCompra,
                    PrecoCusto = precoCompra,
                    ProdutoUnidadeDTO = unidadeComprativel,
                    ProdutoGrupoDTO = grupos.GetFirstGrupo(),
                    CodigoAnp = _xml.CodigoAnp,
                    PercentualGlpPetroleo = _xml.PercentualGlp,
                    PercentualGasImportador = _xml.PercentualGasNaturalImportado,
                    PercentualGasNacional = _xml.PercentualGasNatural,
                    ValorDePartida = _xml.ValorPartida
                    
                };

                if (!_xml.Cean.IsNotNullOrEmpty()) return produto;

                try
                {
                    produto.AddAlias(_xml.Cean);
                }
                catch
                {
                    // ignored
                }


                return produto;
            }
        }

        public void ArmazenaVinculo()
        {
            if (Produto == null || FatorConversao <= 0)
                return;

            _vinculo = _vinculo ?? new ProdutoVinculoCompra();

            _vinculo.Produto = Produto;
            _vinculo.DocumentoFornecedor = _fornecedorVinculo.DocumentoUnico;
            _vinculo.Codigo = _xml.Codigo;
            _vinculo.FatorUtilizado = FatorConversao;
            _vinculo.UnidadeCompra = _xml.Unidade;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProduto(sessao);
                repositorio.Salvar(_vinculo);
            }
        }

        public ItemCompra CriaItemCompra(NotaFiscalCompra nf)
        {
            var item = new ItemCompra(nf)
            {
                ValorUnitario = _xml.ValorUnitario,
                Unidade = ResolveUnidadeMedida(),
                Quantidade = _xml.Quantidade,
                ValorTotal = ValorTotal,
                Produto = Produto,
                Cest = _xml.Cest ?? string.Empty,
                Ncm = ResolveNcm(),
                ValorFreteRateio = _xml.ValorFrete,
                ValorSeguroRateio = _xml.ValorSeguro,
                ValorDespesasRateio = _xml.ValorOutros,
                ValorDescontoTotal = _xml.ValorDesconto,
                ImportadoDeXml = true,
                ImpostoManual = true
            };

            item.SetCfop(CfopDestino);
            item.SetConversao(new ConversaoUnidade(FatorConversao, QuantidadeConversao, UnidadeConversao));

            item.Icms = CriaIcms(item);
            item.Ipi = CriaIpi(item);
            item.Pis = CriaPis(item);
            item.Cofins = CriaCofins(item);

            return item;
        }

        private ProdutoUnidadeDTO ResolveUnidadeMedida()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProdutoUnidade(sessao);
                var unidade = repositorio.PrimeiraUnidadePelaSigla(_xml.Unidade);

                if (unidade != null)
                    return unidade;

                unidade = new ProdutoUnidadeDTO
                {
                    Sigla = _xml.Unidade,
                    Nome = _xml.Unidade
                };

                repositorio.Salva(unidade);

                return unidade;
            }
        }

        private NcmDTO ResolveNcm()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioNcm(sessao);
                var ncm = repositorio.GetPeloId(_xml.Ncm);

                if (ncm != null)
                    return ncm;

                ncm = new NcmDTO
                {
                    Cest = _xml.Cest,
                    Descricao = $"Cadastrado pela Importação de XML {_xml.Ncm}",
                    Id = _xml.Ncm
                };

                repositorio.Salva(ncm);

                return ncm;
            }
        }

        private IcmsCompra CriaIcms(ItemCompra item)
        {
            var cst = _xml.Icms.Regime == RegimeTributario.RegimeNormal ? _xml.Icms.Cst : "00";
            var icms = RecipienteFactory.Get<RecipienteTributacaoCst>().Get(cst);

            return new IcmsCompra(item)
            {
                Icms = icms,
                Aliquota = _xml.Icms.Aliquota,
                BaseCalculo = _xml.Icms.ValorBc,
                ValorIcms = _xml.Icms.Valor,
                AliquotaSt = _xml.Icms.AliquotaSt,
                ValorSt = _xml.Icms.ValorSt,
                BaseCalculoSt = _xml.Icms.ValorBcSt,
                Mva = _xml.Icms.Mva,
                Reducao = _xml.Icms.Reducao,
                ReducaoSt = _xml.Icms.ReducaoSt,
                ValorFcpSt = _xml.Icms.ValorFcpSt,
                PercentualFcpSt = _xml.Icms.AliquotaFcpSt,
                BaseCalculoFcpSt = _xml.Icms.ValorBcFcpSt
            };
        }

        private IpiCompra CriaIpi(ItemCompra item)
        {
            var recipiente = RecipienteFactory.Get<RecipienteIpi>();
            var ipi = recipiente.Get(_xml.PossuiIpi ? _xml.Ipi.Cst : "49");

            if (!_xml.PossuiIpi)
                return new IpiCompra(item, ipi);

            return new IpiCompra(item, ipi.ToEntrada())
            {
                ValorIpi = _xml.Ipi.Valor,
                Aliquota = _xml.Ipi.Aliquota,
                BaseCalculo = _xml.Ipi.ValorBc
            };
        }

        private PisCompra CriaPis(ItemCompra item)
        {
            var recipiente = RecipienteFactory.Get<RecipientePis>();
            var pis = recipiente.Get("98");

            if (!_xml.PossuiPis)
                return new PisCompra(item, pis);

            return new PisCompra(item, pis)
            {
                Aliquota = _xml.Pis.Aliquota,
                BaseCalculo = _xml.Pis.ValorBc,
                ValorPis = _xml.Pis.Valor
            };
        }

        private CofinsCompra CriaCofins(ItemCompra item)
        {
            var recipiente = RecipienteFactory.Get<RecipienteCofins>();
            var cofins = recipiente.Get("98");

            if (!_xml.PossuiCofins)
                return new CofinsCompra(item, cofins);

            return new CofinsCompra(item, cofins)
            {
                Aliquota = _xml.Cofins.Aliquota,
                BaseCalculo = _xml.Cofins.ValorBc,
                ValorCofins = _xml.Cofins.Valor
            };
        }

        public void AtualizarAnp()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProduto(sessao);
                var produto = repositorio.GetPeloId(Produto.Id);

                if (!string.IsNullOrWhiteSpace(produto.CodigoAnp))
                {
                    return;
                }

                produto.CodigoAnp = _xml.CodigoAnp;
                produto.PercentualGasImportador = _xml.PercentualGasNaturalImportado;
                produto.PercentualGasNacional = _xml.PercentualGasNatural;
                produto.PercentualGlpPetroleo = _xml.PercentualGlp;
                produto.ValorDePartida = _xml.ValorPartida;

                using (var transacao = sessao.BeginTransaction())
                {
                    repositorio.Salvar(produto);
                    transacao.Commit();
                }
            }
        }
    }
}