using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DFe.Utils;
using Fusion.Visao.Compras.Importacao.Models;
using Fusion.Visao.GerenciadorManifestacoesDestinatarios;
using FusionCore.Core.Nfes.Xml;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Compras;
using FusionCore.FusionAdm.Compras.Importacao;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.ManifestoSefaz;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.Command;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NFe.Servicos;
using NHibernate;
using NHibernate.Util;

namespace Fusion.Visao.Compras.Importacao
{
    public class ImportacaoCompraViewModel : ViewModel
    {
        private XmlRoot _xmlAnalise;
        private CadastroCompativel _cadastros;
        private string _documentoXml;

        public string DocumentoImportar
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool PassoDefinirRegras
        {
            get => GetValue<bool>();
            set
            {
                SetValue(false, nameof(PassoFinalizar));
                SetValue(false, nameof(PassoManifestoDocumento));
                SetValue(value);
            }
        }

        public bool PassoFinalizar
        {
            get => GetValue<bool>();
            set
            {
                SetValue(false, nameof(PassoDefinirRegras));
                SetValue(false, nameof(PassoManifestoDocumento));
                SetValue(value);
            }
        }

        public bool PassoManifestoDocumento
        {
            get => GetValue<bool>();
            set
            {
                SetValue(false, nameof(PassoDefinirRegras));
                SetValue(false, nameof(PassoFinalizar));
                SetValue(value);
            }
        }

        public PessoaVM Empresa
        {
            get => GetValue<PessoaVM>();
            set => SetValue(value);
        }

        public PessoaVM Fornecedor
        {
            get => GetValue<PessoaVM>();
            set
            {
                SetValue(value);
                AvisoFornecedorInativo = !value?.PessoaVinculada?.Ativo ?? false;
            }
        }

        public bool AvisoFornecedorInativo
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public PessoaVM Transportadora
        {
            get => GetValue<PessoaVM>();
            set
            {
                SetValue(value);
                AvisoTransportadoraInativa = !value?.PessoaVinculada?.Ativo ?? false;
            }
        }

        public bool AvisoTransportadoraInativa
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public short Serie
        {
            get => GetValue<short>();
            set => SetValue(value);
        }

        public string Chave
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public long NumeroDocumento
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public DateTime EmissaoEm
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public DateTime? EntradaSaidaEm
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public bool EmProgresso
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool EmAnalise
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ObservableCollection<RegraCfopVM> RegrasCfop
        {
            get => GetValue<ObservableCollection<RegraCfopVM>>();
            set => SetValue(value);
        }

        public ObservableCollection<ItemImportacaoVM> Itens
        {
            get => GetValue<ObservableCollection<ItemImportacaoVM>>();
            private set => SetValue(value);
        }

        public ObservableCollection<EmissorFiscal> Emissores
        {
            get => GetValue<ObservableCollection<EmissorFiscal>>();
            set => SetValue(value);
        }

        public EmissorFiscal EmissorManifesto
        {
            get => GetValue<EmissorFiscal>();
            set => SetValue(value);
        }

        public ICommand AnalisarCommand => AsyncCommand(AnalisaDocumento);
        public ICommand ConfirmarRegrasCommand => AsyncCommand(ConfirmaRegras);
        public ICommand ImportarCommand => AsyncCommand(ImportaDocumento);
        public ICommand ConfirmarImportacaoChaveCommand => AsyncCommand(ImportaPelaChave);

        public event EventHandler<NotaFiscalCompra> ImportacaoCompleta;

        private ICommand AsyncCommand(Action<object, Dispatcher> action)
        {
            var asyncAction = new Action<object>(o =>
            {
                var dispatcher = Application.Current.Dispatcher;

                Task.Run(() =>
                {
                    EmProgresso = true;

                    try
                    {
                        action.Invoke(o, dispatcher);
                    }
                    catch (InvalidOperationException e)
                    {
                        dispatcher.Invoke(() => { DialogBox.MostraAviso(e.Message); });
                    }
                    catch (Exception e)
                    {
                        dispatcher.Invoke(() => { DialogBox.MostraErro(e.Message, e); });
                    }
                    finally
                    {
                        EmProgresso = false;
                    }
                });
            });

            return new SimpleCommand
            {
                CanExecuteDelegate = p => true,
                ExecuteDelegate = asyncAction
            };
        }

        private void AnalisaDocumento(object obj, Dispatcher dispatcher)
        {
            try
            {
                if (File.Exists(DocumentoImportar))
                {
                    AnalisarArquivo();
                    EmAnalise = true;
                    return;
                }

                if (ChaveSefaz.Valida(DocumentoImportar))
                {
                    AnalisarChave();
                    EmAnalise = true;
                    return;
                }

                throw new InvalidOperationException(
                    "Para fazer a importação preciso de um caminho para o XML ou a CHAVE da NF-e");
            }
            catch
            {
                EmAnalise = false;
                throw;
            }
        }

        private void ConfirmaRegras(object obj, Dispatcher dispatcher)
        {
            RegrasCfop.ForEach(r => r.CarregarCfop());

            if (RegrasCfop.Any(r => r.CodigoCfop == null))
            {
                throw new InvalidOperationException("Todos os CFOP's precisam estar configurados");
            }

            Serie = _xmlAnalise.Serie;
            Chave = _xmlAnalise.Chave;
            NumeroDocumento = _xmlAnalise.NumeroDocumento;
            EmissaoEm = _xmlAnalise.EmissaoEm;
            EntradaSaidaEm = _xmlAnalise.EntradaSaidaEm;

            Empresa = new PessoaVM(_xmlAnalise.Destinatario);
            Fornecedor = new PessoaVM(_xmlAnalise.Emitente, _cadastros.Fornecedor);

            if (_xmlAnalise.Transportadora != null)
            {
                Transportadora = new PessoaVM(_xmlAnalise.Transportadora, _cadastros.Transportadora);
            }

            ListarItens(dispatcher);
            PassoFinalizar = true;
        }

        private void ListarItens(Dispatcher dispatcher)
        {
            if (_xmlAnalise.Produtos.Count > 50)
            {
                dispatcher.Invoke(() =>
                {
                    DialogBox.MostraInformacao(
                        "Nota contém muitos itens, irei demorar um pouco para te mostra-los");
                });
            }

            var itens = new ObservableCollection<ItemImportacaoVM>();

            _xmlAnalise.Produtos.ForEach(p =>
            {
                var regra = RegrasCfop.SingleOrDefault(s => s.Origem == p.Cfop);

                if (regra == null)
                {
                    throw new InvalidOperationException($"Não foi possível localizar regra para o cfop {p.Cfop.ToString()}");
                }

                var currentItem = new ItemImportacaoVM(p, regra.CodigoCfop);
                currentItem.FazVinculo(Fornecedor);

                itens.Add(currentItem);
            });

            Itens = itens;
        }

        private void AnalisarArquivo()
        {
            _documentoXml = File.ReadAllText(DocumentoImportar);
            AnalisarDocumentoXml();
        }

        public void AnalisarDocumentoXmlImportadoPelaManifestacao(string xml)
        {
            if (xml == null) return;

            _documentoXml = xml;
            AnalisarDocumentoXml();
        }

        private void AnalisarDocumentoXml()
        {
            var leitor = new LeitorImportacaoXml(_documentoXml);

            _xmlAnalise = leitor.Ler();
            _cadastros = PessoasCompativel.Find(_xmlAnalise);

            if (_cadastros.Empresa == null)
            {
                throw new InvalidOperationException(
                    "Não encontrei a empresa compativel com o destinatário da nota.");
            }

            NotaFiscalCompraValidator.ValidaExistenciaPelaChave(_xmlAnalise.Chave);

            var regras = new ObservableCollection<RegraCfopVM>();

            _xmlAnalise.Produtos.ForEach(p =>
            {
                if (regras.All(i => i.Origem != p.Cfop))
                {
                    regras.Add(new RegraCfopVM(p.Cfop));
                }
            });

            RegrasCfop = regras;
            PassoDefinirRegras = true;
        }

        private void ImportaDocumento(object obj, Dispatcher dispatcher)
        {
            if (Itens.Any(i => i.Vinculado == false))
            {
                throw new InvalidOperationException("Preciso que vincule todos os produtos");
            }

            if (Fornecedor.Vinculado && Fornecedor.PessoaVinculada.Ativo == false)
            {
                throw new InvalidOperationException("Preciso que resolva o Fornecedor inativo");
            }

            if (Transportadora != null && Transportadora.Vinculado && Transportadora.PessoaVinculada.Ativo == false)
            {
                throw new InvalidOperationException("Preciso que resolva a Transportadora inativa");
            }

            if (Itens.Any(i => i.Ativo == false))
            {
                throw new InvalidOperationException("Preciso que resolva todos os produtos que estão inativos");
            }

            Itens.ForEach(i => i.ArmazenaVinculo());

            NotaFiscalCompra nf;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                AutoCadastraPessoas(sessao);

                transacao.Commit();
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transaction = sessao.BeginTransaction())
            {
                nf = GeraNotaDeCompra(sessao);
                transaction.Commit();
            }

            if (nf.PossuiChave)
            {
                new NfeResumidaImportadaServico(nf.Chave.Chave).Importada();
            }

            dispatcher.Invoke(() => ImportacaoCompleta?.Invoke(this, nf));
        }

        private void AutoCadastraPessoas(ISession sessao)
        {
            var repositorio = new RepositorioPessoa(sessao);
            var converter = new XmlPessoaConverter(new RepositorioImportacao(sessao));

            CadastraFornecedor(repositorio, converter);
            CadastraTransportadora(repositorio, converter);
        }

        private void CadastraFornecedor(RepositorioPessoa repositorio, XmlPessoaConverter converter)
        {
            if (Fornecedor.Vinculado == false)
            {
                var pessoa = converter.ParaPessoa(Fornecedor.GetXml());

                pessoa.Fornecedor = new Fornecedor(pessoa);
                repositorio.SalvaAlteracoes(pessoa);

                Fornecedor.PessoaVinculada = pessoa;
                Fornecedor.Vinculado = true;
                return;
            }

            if (Fornecedor.PessoaVinculada.Fornecedor == null)
            {
                Fornecedor.PessoaVinculada.Fornecedor = new Fornecedor(Fornecedor.PessoaVinculada);
                repositorio.SalvaAlteracoes(Fornecedor.PessoaVinculada);
            }
        }

        private void CadastraTransportadora(RepositorioPessoa repositorio, XmlPessoaConverter converter)
        {
            if (Transportadora == null)
            {
                return;
            }

            var mesmoFornecedor = Fornecedor.DocumentoUnico == Transportadora.DocumentoUnico;

            if (Transportadora.DocumentoIsValido == false)
            {
                return;
            }

            if (Transportadora.Vinculado == false && mesmoFornecedor)
            {
                var pessoa = Fornecedor.PessoaVinculada;
                pessoa.Transportadora = new Transportadora(pessoa);

                repositorio.SalvaAlteracoes(pessoa);
                repositorio.Evict(pessoa);

                Transportadora.PessoaVinculada = pessoa;
                Transportadora.Vinculado = true;
                return;
            }

            if (Transportadora.Vinculado == false)
            {
                var pessoa = converter.ParaPessoa(Transportadora.GetXml());

                pessoa.Transportadora = new Transportadora(pessoa);
                repositorio.SalvaAlteracoes(pessoa);

                Transportadora.PessoaVinculada = pessoa;
                Transportadora.Vinculado = true;
                return;
            }

            if (Transportadora.PessoaVinculada.Transportadora == null)
            {
                Transportadora.PessoaVinculada.Transportadora = new Transportadora(Transportadora.PessoaVinculada);
                repositorio.SalvaAlteracoes(Transportadora.PessoaVinculada);
            }
        }

        private NotaFiscalCompra GeraNotaDeCompra(ISession sessao)
        {
            var nf = new NotaFiscalCompra(_cadastros.Empresa)
            {
                Fornecedor = Fornecedor.PessoaVinculada.Fornecedor,
                Transportadora = Transportadora?.PessoaVinculada?.Transportadora,
                Chave = new ChaveSefaz(Chave),
                EmitidaEm = EmissaoEm,
                EntradaSaidaEm = EntradaSaidaEm ?? EmissaoEm,
                Serie = Serie,
                NumeroDocumento = (int)NumeroDocumento,
                ModalidadeFrete = _xmlAnalise.ModalidadeFrete,
                TotalBcIcms = _xmlAnalise.Totais.BcIcms,
                TotalBcIcmsSt = _xmlAnalise.Totais.BcIcmsSt,
                ValorTotalDesconto = _xmlAnalise.Totais.ValorDesconto,
                ValorTotalFrete = _xmlAnalise.Totais.ValorFrete,
                ValorTotalIcms = _xmlAnalise.Totais.ValorIcms,
                ValorTotalIcmsSt = _xmlAnalise.Totais.ValorIcmsSt,
                ValorTotalIpi = _xmlAnalise.Totais.ValorIpi,
                ValorTotalItens = _xmlAnalise.Totais.ValorProdutos,
                ValorTotalOutros = _xmlAnalise.Totais.ValorOutros,
                ValorTotalSeguro = _xmlAnalise.Totais.ValorSeguro,
                ValorTotal = _xmlAnalise.Totais.ValorNf
            };

            nf.InformarImportacaoXml(_documentoXml);

            Itens.ForEach(i =>
            {
                i.AtualizarAnp();
                nf.Adicionar(i.CriaItemCompra(nf));
            });


            AdicionaDuplicatasSeTiver(nf);

            nf.CalculaPrecoCustoDosItens();

            NotaFiscalCompraValidator.ValidaOsDados(nf);
            NotaFiscalCompraValidator.ValidaExistencia(nf);

            var repositorio = new RepositorioNotaFiscalCompra(sessao);
            repositorio.Salvar(nf);

            return nf;
        }

        private void AdicionaDuplicatasSeTiver(NotaFiscalCompra nf)
        {
            _xmlAnalise.Duplicatas?.ForEach(dup =>
            {
                nf.Adicionar(new DuplicataCompra
                {
                    NfCompra = nf,
                    Numero = dup.Numero,
                    Valor = dup.Valor,
                    Vencimento = (DateTime)dup.DataVencimento
                });
            });
        }

        private void AnalisarChave()
        {
            NotaFiscalCompraValidator.ValidaExistenciaPelaChave(DocumentoImportar);

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmissorFiscal(sessao);
                var emissores = repositorio.BuscaTodosNfe();

                Emissores = new ObservableCollection<EmissorFiscal>(emissores);
            }

            if (Emissores.Count == 1)
            {
                EmissorManifesto = Emissores.FirstOrDefault();
            }

            PassoManifestoDocumento = true;
        }

        private void ImportaPelaChave(object arg1, Dispatcher arg2)
        {
            if (EmissorManifesto == null)
            {
                throw new InvalidOperationException(
                    "Preciso que cadastre um emissor de NF-e para fazer o download e manifesto do documento");
            }

            FazManifestoDestinatario();
            FazDownloadDoXml();
            AnalisarDocumentoXml();
        }

        private void FazManifestoDestinatario()
        {
            try
            {
                ManifestoFacade.FazCienciaOperacao(EmissorManifesto, DocumentoImportar);
                Thread.Sleep(5000);
            }
            catch (JaManifestadoException)
            {
                // ignore
            }
        }

        private void FazDownloadDoXml()
        {
            var zeusBuilder = new ConfiguracaoZeusBuilder(EmissorManifesto.EmissorFiscalNfe, TipoEmissao.Normal);
            var cfg = zeusBuilder.GetConfiguracao();
            var servico = new ServicosNFe(cfg);

            var resposta = servico.NfeDistDFeInteresse(
                EmissorManifesto.Uf,
                EmissorManifesto.Cnpj,
                chNFE: DocumentoImportar);

            var cStat = resposta.Retorno.cStat;
            var xMotiv = resposta.Retorno.xMotivo;
            var docs = resposta.Retorno.loteDistDFeInt;

            if (cStat != 138)
                throw new InvalidOperationException($"Falha: {cStat} - {xMotiv}");

            if (!docs.Any())
                throw new InvalidOperationException("Falha: Nenhum documento foi retornado pela distribuição NF-E");

            _documentoXml = Compressao.Unzip(docs[0].XmlNfe);
        }
    }
}