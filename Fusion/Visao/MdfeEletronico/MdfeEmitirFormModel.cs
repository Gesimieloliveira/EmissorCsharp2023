using System;
using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Entidades;
using Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model;
using Fusion.Visao.MdfeEletronico.Aba.IncluirPagamento;
using Fusion.Visao.MdfeEletronico.Aba.Model;
using Fusion.Visao.MdfeEletronico.Builder;
using Fusion.Visao.MdfeEletronico.Emissao;
using FusionCore.Excecoes.RegraNegocio;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.FusionAdm.MdfeEletronico.Autorizador;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Pessoa;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.MdfeEletronico
{
    public sealed class MdfeEmitirFormModel : ViewModel
    {
        private AbaCabecalhoMdfeModel _abaCabecalhoMdfeModel;
        private MDFeBuilder _mdfeBuilder;
        private AbaMdfeCarregamentoModel _abaMdfeCarregamentoModel;
        private FlyoutAddLacreModel _flyoutAddLacreModel;
        private FlyoutAddPercursoModel _flyoutAddPercursoModel;
        private FlyoutAddVeiculoTracaoModel _flyoutAddVeiculoTracaoModel;
        private FlyoutAddCondutorModel _flyoutAddCondutorModel;
        private FlyoutAddVeiculoReboqueModel _flyoutAddVeiculoReboqueModel;
        private FlyoutAddValePedagioModel _flyoutAddValePedagioModel;
        private FlyoutAddMunicipioCarregamentoModel _flyoutAddMunicipioCarregamentoModel;
        private FlyoutAddSeguroModel _flyoutAddSeguroModel;
        private FlyoutAddContratanteModel _flyoutAddContratanteModel;
        private FlyoutAddCiotModel _flyoutAddCiotModel;
        private bool _isNaoExisteHistoricoPendente;

        public FlyoutAddLacreModel FlyoutAddLacreModel
        {
            get { return _flyoutAddLacreModel; }
            set
            {
                _flyoutAddLacreModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAddSeguroModel FlyoutAddSeguroModel
        {
            get { return _flyoutAddSeguroModel; }
            set
            {
                if (Equals(value, _flyoutAddSeguroModel)) return;
                _flyoutAddSeguroModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAddPercursoModel FlyoutAddPercursoModel
        {
            get { return _flyoutAddPercursoModel; }
            set
            {
                if (Equals(value, _flyoutAddPercursoModel)) return;
                _flyoutAddPercursoModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAddVeiculoTracaoModel FlyoutAddVeiculoTracaoModel
        {
            get { return _flyoutAddVeiculoTracaoModel; }
            set
            {
                _flyoutAddVeiculoTracaoModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAddCondutorModel FlyoutAddCondutorModel
        {
            get { return _flyoutAddCondutorModel; }
            set
            {
                if (Equals(value, _flyoutAddCondutorModel)) return;
                _flyoutAddCondutorModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAddVeiculoReboqueModel FlyoutAddVeiculoReboqueModel
        {
            get { return _flyoutAddVeiculoReboqueModel; }
            set
            {
                if (Equals(value, _flyoutAddVeiculoReboqueModel)) return;
                _flyoutAddVeiculoReboqueModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAddValePedagioModel FlyoutAddValePedagioModel
        {
            get { return _flyoutAddValePedagioModel; }
            set
            {
                if (Equals(value, _flyoutAddValePedagioModel)) return;
                _flyoutAddValePedagioModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAddMunicipioCarregamentoModel FlyoutAddMunicipioCarregamentoModel
        {
            get { return _flyoutAddMunicipioCarregamentoModel; }
            set
            {
                if (Equals(value, _flyoutAddMunicipioCarregamentoModel)) return;
                _flyoutAddMunicipioCarregamentoModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAddContratanteModel FlyoutAddContratanteModel
        {
            get { return _flyoutAddContratanteModel; }
            set
            {
                if (Equals(value, _flyoutAddContratanteModel)) return;
                _flyoutAddContratanteModel = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAddCiotModel FlyoutAddCiotModel
        {
            get { return _flyoutAddCiotModel; }
            set
            {
                if (Equals(value, _flyoutAddCiotModel)) return;
                _flyoutAddCiotModel = value;
                PropriedadeAlterada();
            }
        }

        public AbaCabecalhoMdfeModel AbaCabecalhoMdfeModel
        {
            get { return _abaCabecalhoMdfeModel; }
            set
            {
                _abaCabecalhoMdfeModel = value;
                PropriedadeAlterada();
            }
        }

        public AbaMdfeCarregamentoModel AbaMdfeCarregamentoModel
        {
            get { return _abaMdfeCarregamentoModel; }
            set
            {
                if (Equals(value, _abaMdfeCarregamentoModel)) return;
                _abaMdfeCarregamentoModel = value;
                PropriedadeAlterada();
            }
        }

        public AbaRodoviarioMdfeModel AbaRodoviarioMdfeModel { get; set; }

        public bool IsNaoExisteHistoricoPendente
        {
            get => _isNaoExisteHistoricoPendente;
            set
            {
                if (value == _isNaoExisteHistoricoPendente) return;
                _isNaoExisteHistoricoPendente = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler FecharTela;

        public void InicializarModel()
        {
            AbaCabecalhoMdfeModel = new AbaCabecalhoMdfeModel();
            AbaMdfeCarregamentoModel = new AbaMdfeCarregamentoModel();
            AbaRodoviarioMdfeModel = new AbaRodoviarioMdfeModel();
            FlyoutAddLacreModel = new FlyoutAddLacreModel();
            FlyoutAddSeguroModel = new FlyoutAddSeguroModel();
            FlyoutAddPercursoModel = new FlyoutAddPercursoModel();
            FlyoutAddVeiculoTracaoModel = new FlyoutAddVeiculoTracaoModel();
            FlyoutAddCondutorModel = new FlyoutAddCondutorModel();
            FlyoutAddVeiculoReboqueModel = new FlyoutAddVeiculoReboqueModel();
            FlyoutAddValePedagioModel = new FlyoutAddValePedagioModel();
            FlyoutAddMunicipioCarregamentoModel = new FlyoutAddMunicipioCarregamentoModel();
            FlyoutAddContratanteModel = new FlyoutAddContratanteModel();
            FlyoutAddCiotModel = new FlyoutAddCiotModel();


            AbaCabecalhoMdfeModel.ProximoHandler += ProximoAbaCarregamentoDescarregamentoCompleted;
            AbaCabecalhoMdfeModel.EventoAlocarNumeracao += AlocarNumeracao;

            AbaMdfeCarregamentoModel.AnteriorHandler += AnteriorAbaCabecalhoCompleted;
            AbaMdfeCarregamentoModel.ProximoHandler += ProximoAbaRodoviarioCompleted;
            AbaMdfeCarregamentoModel.AbrirFlyoutAddSeguroHandler += AbrirFlyoutAddSeguroCompleted;
            AbaMdfeCarregamentoModel.AbrirFlyoutAddLacreHandler += AbrirFlyoutLacreCompleted;
            AbaMdfeCarregamentoModel.AbrirFlyoutAddPercursoHandler += AbrirFlyoutPercursoCompleted;
            AbaMdfeCarregamentoModel.AbrirFlyoutAddMunicipioCarregamentoHandler +=
                AbrirFlyoutMunicipioCarregamentoCompleted;

            AbaRodoviarioMdfeModel.AnteriorHandler += AnteriorAbaCarregamentoDescarregamentoCompleted;
            AbaRodoviarioMdfeModel.AbrirFlyoutAddVeiculoTracaoHandler += AbrirFlyoutAddVeiculoTracaoCompleted;
            AbaRodoviarioMdfeModel.AbrirFlyoutAddCondutorHandler += AbrirFlyoutAddCondutorCompleted;
            AbaRodoviarioMdfeModel.AbrirFlyoutAddVeiculoReboqueHandler += AbrirFlyoutAddVeiculoReboqueCompleted;
            AbaRodoviarioMdfeModel.AbrirFlyoutAddValePedagioHandler += AbrirFlyoutAddValePedagioCompleted;
            AbaRodoviarioMdfeModel.AbrirFlyoutAddContratanteHandler += AbrirFlyoutAddContratanteCompleted;
            AbaRodoviarioMdfeModel.AbrirFlyoutAddCiotHandler += AbrirFlyoutAddCiotCompleted;
            AbaRodoviarioMdfeModel.AbrirDialogInformacaoPagamento += AdicionarInformacaoPagamentoCompleted;
            AbaRodoviarioMdfeModel.EmitirMdfe += EmitirMdfeInitialize;

            FlyoutAddLacreModel.SalvarLacreHandler += SalvarLacreCompleted;
            FlyoutAddPercursoModel.SalvarPercursoHandler += SalvarPercursoCompleted;
            FlyoutAddVeiculoTracaoModel.SalvarVeiculoTracaoHandler += SalvarVeiculoTracaoCompleted;
            FlyoutAddSeguroModel.SalvarSeguroHandler += SalvarSeguroCompleted;
            FlyoutAddCondutorModel.SalvarCondutorHandler += SalvarCondutorCompleted;
            FlyoutAddCiotModel.SalvarCiotHandler += SalvarCiotCompleted;
            FlyoutAddContratanteModel.SalvarContratanteHandler += SalvarContratanteCompleted;
            FlyoutAddVeiculoReboqueModel.SalvarVeiculoReboqueHandler += SalvarVeiculoReboqueCompleted;
            FlyoutAddValePedagioModel.SalvarValePedagioHandler += SalvarValePedagioCompleted;
            FlyoutAddMunicipioCarregamentoModel.SalvarMunicipioCarregamentoHandler += SalvarMunicipioCarregamentoCompleted;
        }

        public void CarregarMdfeParaEdicao()
        {
            var mdfe = _mdfeBuilder.Construir();

            if (mdfe.Id != 0)
            {
                AbaCabecalhoMdfeModel.ComMdfe(mdfe);
                AbaMdfeCarregamentoModel.ComMdfe(mdfe);
                AbaRodoviarioMdfeModel.ComMdfe(mdfe);
            }
        }

        private void AlocarNumeracao(object sender, AbaCabecalhoMdfeModel e)
        {
            new AlocarNumeroFiscalMDFe().Alocar(_mdfeBuilder.Construir());
            AlocarNumeracaoUpdate(_mdfeBuilder.Construir());
        }

        private void SalvarSeguroCompleted(object sender, EventSeguro e)
        {
            var mdfeSeguroCarga = new MDFeSeguroCarga
            {
                CnpjSeguradora = e.Model.CnpjSeguradora,
                MDFeEletronico = _mdfeBuilder.Construir(),
                NomeSeguradora = e.Model.NomeSeguradora,
                NumeroApolice = e.Model.NumeroApolice,
                Responsavel = e.Model.ResponsavelSeguro
            };

            e.Model.Averbacoes.ForEach(averbacao =>
            {
                averbacao.SeguroCarga = mdfeSeguroCarga;
                mdfeSeguroCarga.Averbacoes.Add(averbacao);
            });

            if (e.Model.DocumentoResponsavel?.Length == 14)
            {
                mdfeSeguroCarga.CnpjResponsavel = e.Model.DocumentoResponsavel;
            }

            if (e.Model.DocumentoResponsavel?.Length == 11)
            {
                mdfeSeguroCarga.CpfResponsavel = e.Model.DocumentoResponsavel;
            }

            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);

                repositorio.SalvarSeguro(mdfeSeguroCarga);

                transacao.Commit();
            }

            var seguroGrid = new GridSeguroCarga
            {
                MDFeSeguroCarga = mdfeSeguroCarga,
                CpfResponsavel = mdfeSeguroCarga.CpfResponsavel,
                NomeSeguradora = mdfeSeguroCarga.NomeSeguradora,
                CnpjSeguradora = mdfeSeguroCarga.CnpjSeguradora,
                NumeroApolice = mdfeSeguroCarga.NumeroApolice,
                CnpjResponsavel = mdfeSeguroCarga.CnpjResponsavel,
                ResponsavelSeguro = mdfeSeguroCarga.Responsavel
            };

            mdfeSeguroCarga.Averbacoes.ForEach(averbacao =>
            {
                seguroGrid.Averbacoes.Add(averbacao);
            });

            _mdfeBuilder.ComSeguro(mdfeSeguroCarga);
            AbaMdfeCarregamentoModel.ListaSeguroCarga.Add(seguroGrid);
        }

        private void SalvarMunicipioCarregamentoCompleted(object sender, SalvarMunicipioCarregamentoEventArgs e)
        {
            var municipioCarregamento = new GridMunicipioCarregamento
            {
                Cidade = e.Model.Cidade
            };


            var carregamentoMdfe = new MDFeMunicipioCarregamento
            {
                Cidade = e.Model.Cidade,
                MDFeEletronico = _mdfeBuilder.Construir()
            };

            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);
                repositorio.SalvarMunicipioCarregamento(carregamentoMdfe);
                municipioCarregamento.MunicipioCarregamento = carregamentoMdfe;
                transacao.Commit();
            }


            _mdfeBuilder.ComMunicipioCarregamento(carregamentoMdfe);
            AbaMdfeCarregamentoModel.AddMunicipioCarregamento(municipioCarregamento);
        }

        private void AbrirFlyoutMunicipioCarregamentoCompleted(object sender, EventArgs e)
        {
            FlyoutAddMunicipioCarregamentoModel.LimpaCampos();
            FlyoutAddMunicipioCarregamentoModel.IsOpen = true;
        }

        private async void EmitirMdfeInitialize(object sender, EmitirMdfeEventArgs e)
        {
            try
            {
                if (e.Model.ListaVeiculoTracao.Count != 1)
                {
                    throw new ArgumentException("Deve ter um veículo tração");
                }

                if (e.Model.ListaCondutor.Count == 0)
                {
                    throw new ArgumentException("Deve ter no mínimo um condutor");
                }

                if (e.Model.ListaCondutor.Count >= 11)
                {
                    throw new ArgumentException("Deve ter no máximo 10 condutores");
                }

                _mdfeBuilder.ComRodoviario(e.Model);
                SalvarRodoviarioValores();

                if (_mdfeBuilder.Construir().EmissaoEm <= DateTime.Now.AddMonths(-12)) throw new ArgumentException("Emissão Em muito antigo não e permitido");

                if (_mdfeBuilder.Construir().CargaFechada &&
                    _mdfeBuilder.Construir().ProdutoPredominante.TipoCarga == TipoCarga.Nenhuma)
                    throw new ArgumentException("Quando o frete é lacrado (carga fechada) deve ter um produto predominante");

                var model = CriaSefazMdfeViewModel();

                new EmissaoSefazMdfeView(model, _mdfeBuilder.Construir()).ShowDialog();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (RegraNegocioException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private EmissaoSefazMdfeViewModel CriaSefazMdfeViewModel()
        {
            return new EmissaoSefazMdfeViewModel(delegate (object o, MDFeEletronico eletronico)
                {
                    Application.Current.Dispatcher.Invoke(() => { AlocarNumeracaoUpdate(eletronico); });
                },
                delegate (object o, EmissaoSefazMdfeViewModel viewModel)
                {
                    Application.Current.Dispatcher?.Invoke(() =>
                    {
                        var opcoes = new MdfeEletronicaOpcoes(viewModel.ObterIdMdfe());
                        opcoes.ShowDialog();
                        viewModel.OnFechar();
                        OnFecharTela();
                    });
                });
        }

        private void AlocarNumeracaoUpdate(MDFeEletronico mdfe)
        {
            AbaCabecalhoMdfeModel.AlocouNumeracao(mdfe);
        }

        private void SalvarRodoviarioValores()
        {
            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);

                repositorio.Salvar(_mdfeBuilder.Construir());
                repositorio.SalvarRodoviario(_mdfeBuilder.Construir().Rodoviario);

                transacao.Commit();
            }
        }

        private void SalvarValePedagioCompleted(object sender, SalvarValePedagioEventArgs e)
        {
            if (AbaRodoviarioMdfeModel.ListaValePedagio.Count > 0 &&
                AbaRodoviarioMdfeModel.ListaValePedagio.Count + 1 != 1)
                throw new ArgumentException("Somente pode ter 1 vale pedágio");

            var mdfeValePedagio = new MDFeValePedagio
            {
                CnpjEmpresaFornecedora = e.Model.CnpjEmpresaFornecedora,
                CnpjResponsavelPagamento = e.Model.CnpjResposavelPagamento,
                NumeroComprovante = e.Model.NumeroComprovante,
                Rodoviario = _mdfeBuilder.Construir().Rodoviario,
                Valor = e.Model.Valor,
                CpfResponsavel = e.Model.CpfResposavelPagamento
            };

            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);

                repositorio.SalvarValePedagio(mdfeValePedagio);
                transacao.Commit();
            }


            var gridValePedagio = new GirdValePedagio
            {
                CnpjEmpresaFornecedora = e.Model.CnpjEmpresaFornecedora,
                CnpjResponsavel = e.Model.CnpjResposavelPagamento,
                NumeroCompra = e.Model.NumeroComprovante,
                MDFeValePedagio = mdfeValePedagio,
                Valor = e.Model.Valor,
                CpfResponsavel = e.Model.CpfResposavelPagamento
            };

            AbaRodoviarioMdfeModel.AdicionarValePedagio(gridValePedagio);
        }

        private void SalvarVeiculoReboqueCompleted(object sender, SalvarVeiculoReboqueEventArgs e)
        {
            if (AbaRodoviarioMdfeModel.ListaVeiculoReboque.Count >= 3)
                throw new ArgumentException("Não é permitido ter mais que 3 veículos reboque");


            var mdfeVeiculoReboque = new MDFeVeiculoReboque
            {
                Rodoviario = _mdfeBuilder.Construir().Rodoviario,
                Veiculo = e.Model.Veiculo
            };


            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);
                repositorio.SalvarVeiculoReboque(mdfeVeiculoReboque);
                transacao.Commit();
            }

            var gridVeiculoTracao = new GridVeiculoReboque
            {
                Veiculo = e.Model.Veiculo,
                SiglaUf = e.Model.SiglaUf,
                TipoCarroceria = e.Model.TipoCarroceria,
                TipoRodado = e.Model.TipoRodado,
                TipoPropriedadeVeiculo = e.Model.TipoPropriedadeVeiculo,
                Renavam = e.Model.Renavam,
                TipoVeiculo = e.Model.TipoVeiculo,
                CapacidadeEmM3 = e.Model.CapacidadeEmM3,
                CapacidadeEmKg = e.Model.CapacidadeEmKg,
                Placa = e.Model.Placa,
                CodigoInterno = e.Model.CodigoInterno,
                Tara = e.Model.TaraKg,
                MFDeVeiculoReboque = mdfeVeiculoReboque
            };


            AbaRodoviarioMdfeModel.AdicionarVeiculoReboque(gridVeiculoTracao);
        }

        private void SalvarContratanteCompleted(object sender, SalvarContratanteEvent e)
        {
            var mdfeContratante = new MDFeContratante
            {
                PessoaEntidade = e.Model.PessoaEntidade,
                Rodoviario = _mdfeBuilder.Construir().Rodoviario
            };

            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);

                repositorio.SalvarContratante(mdfeContratante);

                transacao.Commit();
            }

            var contratante = new GridContratante
            {
                Nome = e.Model.Nome,
                DocumentoUnico = mdfeContratante.PessoaEntidade.GetDocumentoUnico(),
                Contratante = mdfeContratante
            };


            AbaRodoviarioMdfeModel.AdicionarContratante(contratante);
        }

        private void SalvarCiotCompleted(object sender, SalvarCiotEvent e)
        {
            var mdfeCiot = new MDFeCiot
            {
                Ciot = e.Model.Ciot,
                DocumentoUnico = e.Model.DocumentoUnico,
                Rodoviario = _mdfeBuilder.Construir().Rodoviario
            };

            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);

                repositorio.SalvarCiot(mdfeCiot);

                transacao.Commit();
            }

            var ciot = new GridCiot
            {
                DocumentoUnico = mdfeCiot.DocumentoUnico,
                Ciot = mdfeCiot.Ciot,
                MDFeCiot = mdfeCiot
            };


            AbaRodoviarioMdfeModel.AdicionarCiot(ciot);
        }

        private void AdicionarInformacaoPagamentoCompleted(object sender, EditarInformacaoPagamento editarInformacaoPagamento)
        {
            var model = new IncluirPagamentoFormModel(editarInformacaoPagamento.ObterInformacaoPagamento());

            model.IncluidoPagamento += delegate (object o, IncluirPagamentoFormModel formModel)
            {
                var incluirPagamentoRetorno = formModel.IncluirPagamentoRetorno();

                var deletarInformacaoPagamento = incluirPagamentoRetorno.Remover;

                var informacaoPagamento = incluirPagamentoRetorno.Novo;
                informacaoPagamento.Mdfe = _mdfeBuilder.Construir();

                var sessao = SessaoHelperFactory.AbrirSessaoAdm();
                var transacao = sessao.BeginTransaction();

                using (sessao)
                using (transacao)
                {
                    var repositorio = new RepositorioMdfe(sessao);

                    if (deletarInformacaoPagamento != null)
                    {
                        deletarInformacaoPagamento.Parcelas.ForEach(repositorio.Deletar);
                        deletarInformacaoPagamento.ComponentePagamentoFrete.ForEach(repositorio.Deletar);
                        repositorio.Deletar(deletarInformacaoPagamento);
                    }

                    repositorio.Salvar(informacaoPagamento);
                    informacaoPagamento.ComponentePagamentoFrete.ForEach(repositorio.Salvar);
                    informacaoPagamento.Parcelas.ForEach(repositorio.Salvar);

                    transacao.Commit();
                }

                AbaRodoviarioMdfeModel.IncluirPagamento(incluirPagamentoRetorno);
            };
            new IncluirPagamentoForm(model).ShowDialog();
        }

        private void SalvarCondutorCompleted(object sender, SalvarCondutorEventArgs e)
        {
            if (AbaRodoviarioMdfeModel.ListaCondutor.Count > 10)
                throw new ArgumentException("É permitido o máximo de 10 condutores");


            var mdfeCondutor = new MDFeCondutor
            {
                Condutor = e.Model.PessoaSelecionada,
                VeiculoTracao = _mdfeBuilder.Construir().Rodoviario.VeiculoTracao
            };

            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);

                repositorio.SalvarCondutor(mdfeCondutor);

                transacao.Commit();
            }


            var condutor = new GridCondutor
            {
                Nome = e.Model.Nome,
                Cpf = e.Model.Cpf,
                Condutor = e.Model.PessoaSelecionada,
                MDFeCondutor = mdfeCondutor
            };


            AbaRodoviarioMdfeModel.AdicionarCondutor(condutor);
        }

        private bool NaoExisteVeiculoTracao()
        {
            return AbaRodoviarioMdfeModel.ListaVeiculoTracao.Count == 0;
        }

        private void SalvarVeiculoTracaoCompleted(object sender, SalvarVeiculoTracaoEventArgs e)
        {
            if (AbaRodoviarioMdfeModel.ListaVeiculoTracao.Count == 1)
                throw new ArgumentException("Não é permitido ter mais de um veículo tração");

            if (e.Model.TipoPropriedadeVeiculo == TipoPropriedadeVeiculo.Terceiro)
            {
                var proprietario = e.Model.Veiculo.CarregaProprietario();

                if (string.IsNullOrWhiteSpace(proprietario.Rntrc))
                {
                    throw new ArgumentException("Quando o veículo tração é de terceiro e obrigatorio ter o RNTRC");
                }
            }

            var mdfeVeiculoTracao = new MDFeVeiculoTracao
            {
                Veiculo = e.Model.Veiculo,
                Rodoviario = _mdfeBuilder.Construir().Rodoviario
            };

            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);

                repositorio.SalvarVeiculoTracao(mdfeVeiculoTracao);

                transacao.Commit();
            }

            var gridVeiculoTracao = new GridVeiculoTracao
            {
                Veiculo = e.Model.Veiculo,
                SiglaUf = e.Model.SiglaUf,
                TipoCarroceria = e.Model.TipoCarroceria,
                TipoRodado = e.Model.TipoRodado,
                TipoPropriedadeVeiculo = e.Model.TipoPropriedadeVeiculo,
                Renavam = e.Model.Renavam,
                TipoVeiculo = e.Model.TipoVeiculo,
                CapacidadeEmM3 = e.Model.CapacidadeEmM3,
                CapacidadeEmKg = e.Model.CapacidadeEmKg,
                Placa = e.Model.Placa,
                CodigoInterno = e.Model.CodigoInterno,
                Tara = e.Model.TaraKg,
                MDFeVeiculoTracao = mdfeVeiculoTracao
            };

            _mdfeBuilder.ComVeiculoTracao(mdfeVeiculoTracao);

            AbaRodoviarioMdfeModel.AdicionarVeiculoTracao(gridVeiculoTracao);
        }

        private void AbrirFlyoutAddValePedagioCompleted(object sender, EventArgs e)
        {
            FlyoutAddValePedagioModel.LimpaCampos();
            FlyoutAddValePedagioModel.IsOpen = true;
        }

        private void AbrirFlyoutAddContratanteCompleted(object sender, EventArgs e)
        {
            FlyoutAddContratanteModel.LimpaCampos();
            FlyoutAddContratanteModel.IsOpen = true;
        }

        private void AbrirFlyoutAddCiotCompleted(object sender, EventArgs e)
        {
            FlyoutAddCiotModel.LimpaCampos();
            FlyoutAddCiotModel.IsOpen = true;
        }

        private void AbrirFlyoutAddVeiculoReboqueCompleted(object sender, EventArgs e)
        {
            FlyoutAddVeiculoReboqueModel.LimparCampos();
            FlyoutAddVeiculoReboqueModel.IsOpen = true;
        }

        private void AbrirFlyoutAddCondutorCompleted(object sender, EventArgs e)
        {
            if (NaoExisteVeiculoTracao())
                throw new ArgumentException("Adicione um veículo tração para depois adicionar condutores");
            FlyoutAddCondutorModel.LimpaCampos();
            FlyoutAddCondutorModel.IsOpen = true;
        }

        private void AbrirFlyoutAddVeiculoTracaoCompleted(object sender, EventArgs e)
        {
            FlyoutAddVeiculoTracaoModel.LimpaCampos();
            FlyoutAddVeiculoTracaoModel.IsOpen = true;
        }

        private void AnteriorAbaCarregamentoDescarregamentoCompleted(object sender, EventArgs e)
        {
            AbaMdfeCarregamentoModel.Selecionado = true;
        }

        private void ProximoAbaRodoviarioCompleted(object sender, RetornoModelEventArgs e)
        {
            try
            {
                AbaMdfeCarregamentoModel.ThrowExceptionSeInvalido();

                _mdfeBuilder.ComCarregamentoDescarregamento(e.Model);

                SalvarRodoviario();

                AbaRodoviarioMdfeModel.Selecionado = true;
                AbaRodoviarioMdfeModel.Inicializa(_mdfeBuilder.Construir());
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void SalvarPercursoCompleted(object sender, SalvarPercuroEventArgs e)
        {
            try
            {
                var gridPercurso = new GridPercurso
                {
                    EstadoUf = e.Model.EstadoPercurso
                };

                var mdfePercurso = new MDFePercurso
                {
                    Estado = e.Model.EstadoPercurso,
                    MDFeEletronico = _mdfeBuilder.Construir()
                };

                var sessao = SessaoHelperFactory.AbrirSessaoAdm();
                var transacao = sessao.BeginTransaction();

                using (sessao)
                using (transacao)
                {
                    var repositorio = new RepositorioMdfe(sessao);
                    repositorio.SalvarPercurso(mdfePercurso);

                    gridPercurso.Percurso = mdfePercurso;

                    transacao.Commit();
                }

                _mdfeBuilder.ComPercurso(mdfePercurso);
                AbaMdfeCarregamentoModel.AdicionarPercurso(gridPercurso);
            }
            catch (Exception ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void AbrirFlyoutPercursoCompleted(object sender, EventArgs e)
        {
            FlyoutAddPercursoModel.LimparCampos();
            FlyoutAddPercursoModel.IsOpen = true;
        }

        private void SalvarLacreCompleted(object sender, SalvarLacreEventArgs e)
        {
            var gridLacre = new GridLacre
            {
                Numero = e.Model.NumeroLacre
            };

            var lacre = new MDFeLacre
            {
                MDFeEletronico = _mdfeBuilder.Construir(),
                Numero = e.Model.NumeroLacre
            };

            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                var repositorio = new RepositorioMdfe(sessao);
                repositorio.SalvarLacre(lacre);
                gridLacre.Lacre = lacre;
                transacao.Commit();
            }

            _mdfeBuilder.ComLacre(lacre);
            AbaMdfeCarregamentoModel.AdicionarLacre(gridLacre);
        }

        private void AbrirFlyoutLacreCompleted(object sender, EventArgs e)
        {
            FlyoutAddLacreModel.LimpaCampos();
            FlyoutAddLacreModel.IsOpen = true;
        }

        private void AbrirFlyoutAddSeguroCompleted(object sender, EventArgs e)
        {
            FlyoutAddSeguroModel.LimparCampos();
            FlyoutAddSeguroModel.IsOpen = true;
        }

        private void AnteriorAbaCabecalhoCompleted(object sender, EventArgs e)
        {
            AbaCabecalhoMdfeModel.Selecionado = true;
        }

        private void ProximoAbaCarregamentoDescarregamentoCompleted(object sender, RetornoCabecalhoMDFe e)
        {
            _mdfeBuilder.ComCabecalho(e.Model);

            SalvarAbaCabecalho();

            AbaMdfeCarregamentoModel.ComMdfe(_mdfeBuilder.Construir());
            AbaMdfeCarregamentoModel.Selecionado = true;
        }

        private void SalvarAbaCabecalho()
        {
            try
            {
                var sessao = SessaoHelperFactory.AbrirSessaoAdm();
                var transacao = sessao.BeginTransaction();

                using (sessao)
                using (transacao)
                {
                    var repositorio = new RepositorioMdfe(sessao);
                    repositorio.Salvar(_mdfeBuilder.Construir());
                    repositorio.SalvarEmitente(_mdfeBuilder.Construir().Emitente);
                    transacao.Commit();
                }
            }
            catch (Exception ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void SalvarRodoviario()
        {
            if (_mdfeBuilder.Construir().Rodoviario.MDFeId != 0)
            {
                var sessao = SessaoHelperFactory.AbrirSessaoAdm();
                var transacao = sessao.BeginTransaction();

                using (sessao)
                using (transacao)
                {
                    var repositorio = new RepositorioMdfe(sessao);
                    var mdfe = _mdfeBuilder.Construir();
                    repositorio.Salvar(mdfe);

                    transacao.Commit();
                }

                return;
            }

            try
            {
                var sessao = SessaoHelperFactory.AbrirSessaoAdm();
                var transacao = sessao.BeginTransaction();

                using (sessao)
                using (transacao)
                {
                    var repositorio = new RepositorioMdfe(sessao);

                    var mdfe = _mdfeBuilder.Construir();

                    mdfe.Rodoviario.Rntrc = string.Empty;
                    mdfe.Rodoviario.CodigoAgendamentoPorto = string.Empty;
                    mdfe.Rodoviario.MDFeEletronico = _mdfeBuilder.Construir();

                    repositorio.Salvar(mdfe);
                    repositorio.SalvarRodoviario(mdfe.Rodoviario);
                    transacao.Commit();
                }
            }
            catch (Exception ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void OnFecharTela()
        {
            FecharTela?.Invoke(this, EventArgs.Empty);
        }

        public void VerificarHistoricoPendente()
        {
            try
            {
                if (_mdfeBuilder.Construir().Id == 0)
                {
                    IsNaoExisteHistoricoPendente = true;
                    return;
                }

                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorioMdfe = new RepositorioMdfe(sessao);
                    var emissaoHistorico = repositorioMdfe.BuscaUltimaEmissaoHistorico(_mdfeBuilder.Construir());

                    if (emissaoHistorico == null || emissaoHistorico.Finalizada)
                    {
                        IsNaoExisteHistoricoPendente = true;
                        return;
                    }
                }

                AbaCabecalhoMdfeModel.Selecionado = false;
                AbaRodoviarioMdfeModel.Selecionado = true;

                var model = CriaSefazMdfeViewModel();
                var telaEmissao = new EmissaoSefazMdfeView(model, _mdfeBuilder.Construir());

                telaEmissao.Closing += delegate
                {
                    if (model.IsAutorizado)
                    {
                        OnFecharTela();
                    }

                    IsNaoExisteHistoricoPendente = true;
                };

                model.Fechar += delegate
                {
                    Application.Current.Dispatcher.Invoke(telaEmissao.Close);
                };

                telaEmissao.ShowDialog();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        public void CarregarMdfe(int idMdfe)
        {
            if (idMdfe == 0)
                _mdfeBuilder = new MDFeBuilder(new MDFeEletronico());

            if (idMdfe != 0)
            {
                _mdfeBuilder = new MDFeBuilder(BuscarMdfe(idMdfe));
            }
        }

        private MDFeEletronico BuscarMdfe(int idMdfe)
        {
            MDFeEletronico mdfe;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioMdfe(sessao);

                mdfe = repositorio.GetPeloId(idMdfe);
            }

            return mdfe;
        }
    }
}