using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using System.Xml;
using DFe.Utils;
using Fusion.Visao.CteEletronico.CCe.Entidade;
using Fusion.Visao.CteEletronicoOs.Helpers;
using FusionCore.DFe.XmlCte.XmlCte.RegistroEventos;
using FusionCore.FusionAdm.CteEletronico.Assinatura;
using FusionCore.FusionAdm.CteEletronico.Cancelar;
using FusionCore.FusionAdm.CteEletronico.CCe;
using FusionCore.FusionAdm.CteEletronico.ConsultarProtocolos;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Flags.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Helpers;
using FusionCore.FusionAdm.CteEletronico.Validacoes;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.Fiscal.Fabricas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionAdm.FabricaRepositorio;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;
using CCeCTe = FusionCore.FusionAdm.CteEletronico.CCe.CteCartaCorrecao;

namespace Fusion.Visao.CteEletronico.CCe
{
    public class FalhouCCe : EventArgs
    {
        public Exception Exception { get; private set; }

        public FalhouCCe(Exception exception)
        {
            Exception = exception;
        }
    }

    public class SucessoCCe : EventArgs
    {
        public FusionRetornoRegistroEventoCTe Retorno { get; private set; }
        public FusionRegistroEventoCTe Evento { get; set; }

        public SucessoCCe(FusionRetornoRegistroEventoCTe retorno, FusionRegistroEventoCTe evento)
        {
            Retorno = retorno;
            Evento = evento;
        }
    }

    public class CteCartaCorrecaoModel : ViewModel
    {
        private readonly ICartaCorrecaoCte _cte;
        private readonly IFabricaRepositorioCte _fabricaRepositorio;
        private ObservableCollection<Correcao> _listaCorrecao;
        private Correcao _correcaoSelecionada;
        private string _xmlEnviar;
        private XmlNode _xmlRetorno;
        private ObservableCollection<ICartaCorrecaoCteDTO> _historicoCorrecoes;
        private ICartaCorrecaoCteDTO _historicoItem;
        public FlyoutAddCorrecaoCCeModel FlyoutAddCorrecaoCCeModel { get; set; }

        public ObservableCollection<Correcao> ListaCorrecao
        {
            get { return _listaCorrecao; }
            set
            {
                if (Equals(value, _listaCorrecao)) return;
                _listaCorrecao = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<ICartaCorrecaoCteDTO> HistoricoCorrecoes
        {
            get { return _historicoCorrecoes; }
            set
            {
                if (Equals(value, _historicoCorrecoes)) return;
                _historicoCorrecoes = value;
                PropriedadeAlterada();
            }
        }

        public ICartaCorrecaoCteDTO HistoricoItem
        {
            get { return _historicoItem; }
            set
            {
                if (Equals(value, _historicoItem)) return;
                _historicoItem = value;
                PropriedadeAlterada();
            }
        }

        public Correcao CorrecaoSelecionada
        {
            get { return _correcaoSelecionada; }
            set
            {
                if (Equals(value, _correcaoSelecionada)) return;
                _correcaoSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandNovaAdicionarCorrecao => GetSimpleCommand(NovaCorrecao);

        public CteCartaCorrecaoModel(ICartaCorrecaoCte cte, IFabricaRepositorioCte fabricaRepositorio)
        {
            _cte = cte;
            _fabricaRepositorio = fabricaRepositorio;
            FlyoutAddCorrecaoCCeModel = new FlyoutAddCorrecaoCCeModel();
            ListaCorrecao = new ObservableCollection<Correcao>();
            HistoricoCorrecoes = new ObservableCollection<ICartaCorrecaoCteDTO>(BuscarHistorico());

            FlyoutAddCorrecaoCCeModel.AdicionarCartaCorrecao += AdicionarCartaCorrecao;
        }

        public event EventHandler<FalhouCCe> EnvioFalhou;
        public event EventHandler<SucessoCCe> EnvioSucesso;

        private List<ICartaCorrecaoCteDTO> BuscarHistorico()
        {
            IList<ICartaCorrecaoCteDTO> lista;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = _fabricaRepositorio.CriaRepositorioCartaCorrecao(sessao);

                lista = repositorio.ListarCartaCorrecao(_cte).ToList();
            }

            return lista.ToList();
        }

        public void EnviaCorrecao()
        {
            Envia();
        }

        private void Envia()
        {
            try
            {
                var evento = GeraObjeto();
                ValidaXml(evento);
                _xmlEnviar = FuncoesXml.ClasseParaXmlString(evento);


                var xmlRequest = new XmlDocument();
                xmlRequest.LoadXml(_xmlEnviar);

                _xmlRetorno = new RecepcaoEventoWsdl().Executar(_cte, xmlRequest);

                var retorno = FuncoesXml.XmlStringParaClasse<FusionRetornoRegistroEventoCTe>(_xmlRetorno.OuterXml);

                if (retorno.RetornoInformacaoEvento.CodigoStatus == 631)
                {
                    var retornoConsultaProtocolo = new NegocioConsultaProtocolo((ICancelarCte)_cte).Consultar();

                    foreach (var fusionProcEventoCTe in retornoConsultaProtocolo.FusionProcEventoCTe)
                    {
                        if (fusionProcEventoCTe.FusionRetornoEventoCTe.RetornoInformacaoEvento.TipoEvento == 110110)
                        {
                            retorno.RetornoInformacaoEvento = retornoConsultaProtocolo.FusionProcEventoCTe[0].FusionRetornoEventoCTe.RetornoInformacaoEvento;
                        }
                    }
                }


                OnEnvioSucesso(retorno, evento);
            }
            catch (Exception ex)
            {
                OnEnvioFalhou(ex);
            }
        }

        private void ValidaXml(FusionRegistroEventoCTe evento)
        {
            var xmlEvento = FuncoesXml.ClasseParaXmlString(evento);
            var xmlCCe = FuncoesXml.ClasseParaXmlString(evento.InformacaoEvento.DetalheEvento.EventoCartaCorrecao);

            var validacao = new ValidarSchema();

            validacao.Validar(xmlEvento, ManipulaArquivo.LocalAplicacao() + @"\Assets\Schemas.Cte\eventoCTe_v3.00.xsd");

            validacao.Validar(xmlCCe, ManipulaArquivo.LocalAplicacao() + @"\Assets\Schemas.Cte\evCCeCTe_v3.00.xsd");
        }

        private FusionRegistroEventoCTe GeraObjeto()
        {
            var evento = new FusionRegistroEventoCTe {Versao = "3.00"};
            var informacao = evento.InformacaoEvento;

            const int tipoEvento = 110110;
            var chave = _cte.Chave;
            var sequenciaEvento = ObterSequenciaEvento();

            informacao.Id = "ID" + tipoEvento + chave + sequenciaEvento.ToString("D2");
            informacao.CodigoOrgao = _cte.EmissorFiscal.Empresa.EstadoDTO.CodigoIbge;
            informacao.Ambiente = _cte.TipoAmbiente.ToXml();
            informacao.Cnpj = _cte.CnpjOuCpf.Trim();
            informacao.Chave = chave;
            informacao.HoraEvento = DateTime.Now.ParaDataHoraStringUtc();
            informacao.TipoEvento = tipoEvento;
            informacao.SequencialEvento = sequenciaEvento;

            var detalheEvento = informacao.DetalheEvento;
            detalheEvento.VersaoEvento = "3.00";

            var cartaCorrecao = new FusionEventoCartaCorrecaoCTe();

            ListaCorrecao.ForEach(c =>
            {
                cartaCorrecao.InfoCorrrecoes.Add(new FusionInfoCorrecaoCTe
                {
                    CampoAlterado = c.CampoAlterado,
                    GrupoAlterado = c.GrupoAlterado,
                    ValorAlterado = c.ValorAlterado,
                    NumeroItem = c.NumeroItem
                });
            });

            detalheEvento.EventoCartaCorrecao = cartaCorrecao;

            var certificado = CertificadoDigitalFactory.Cria(_cte.EmissorFiscal, true);
            var xml = FuncoesXml.ClasseParaXmlString(evento).RemoverAcentos();
            var xmlAssinado = AssinaturaDigital.AssinaDocumento(xml, evento.InformacaoEvento.Id, certificado);

            return FuncoesXml.XmlStringParaClasse<FusionRegistroEventoCTe>(xmlAssinado);
        }

        private byte ObterSequenciaEvento()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = _fabricaRepositorio.CriaRepositorioCartaCorrecao(sessao);

                return repositorio.ObterSequenciaCCe(_cte);
            }
        }

        public void DeletaCorrecaoSelecionada()
        {
            ListaCorrecao.Remove(CorrecaoSelecionada);
        }

        private void NovaCorrecao(object obj)
        {
            FlyoutAddCorrecaoCCeModel.IsOpen = true;
        }

        private void AdicionarCartaCorrecao(object sender, FlyoutAddCorrecaoCCeModelEvent e)
        {
            var model = e.Model;

            var correcao = new Correcao
            {
                CampoAlterado = model.CampoAlterado,
                ValorAlterado = model.ValorAlterado.RemoverAcentos(),
                GrupoAlterado = model.GrupoAlterado,
                NumeroItem = model.NumeroItem == 0 ? null : model.NumeroItem.ToString()
            };

            ListaCorrecao.Add(correcao);
        }

        protected virtual void OnEnvioFalhou(Exception e)
        {
            EnvioFalhou?.Invoke(this, new FalhouCCe(e));
        }

        protected virtual void OnEnvioSucesso(FusionRetornoRegistroEventoCTe e, FusionRegistroEventoCTe evento)
        {
            EnvioSucesso?.Invoke(this, new SucessoCCe(e, evento));
        }

        // todo salvar cce
        public void SalvarCCe(FusionRetornoRegistroEventoCTe retorno, FusionRegistroEventoCTe evento)
        {
            // todo ajustar
            if (_cte is Cte cte)
            {
                var cartaCorrecao = new CCeCTe
                {
                    Cte = cte,
                    SequenciaEvento = retorno.RetornoInformacaoEvento.SequencialEvento,
                    OcorreuEm = retorno.RetornoInformacaoEvento.DataEHoraRegistroEvento ?? DateTime.Now,
                    Protocolo = retorno.RetornoInformacaoEvento.NumeroProtocolo,
                    StatusRetorno = retorno.RetornoInformacaoEvento.CodigoStatus,
                    XmlEnvio = _xmlEnviar,
                    XmlRetorno = _xmlRetorno.OuterXml,
                    ChaveId = evento.InformacaoEvento.Id.Substring(2, evento.InformacaoEvento.Id.Length - 2)
                };

                var cartaCorrecaoInformacoes = ObterInformacoesCorrecoes(cartaCorrecao);


                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorioCte = new RepositorioCte(sessao);

                    repositorioCte.SalvarCartaCorrecao(cartaCorrecao);
                    sessao.Flush();

                    cartaCorrecaoInformacoes.ForEach(repositorioCte.SalvarInformacaoCorrecao);

                    transacao.Commit();
                }
                return;
            }

            if (_cte is CteOs cteOs)
            {
                var cartaCorrecao = new CteOsCartaCorrecao
                {
                    CteOs = cteOs,
                    SequenciaEvento = retorno.RetornoInformacaoEvento.SequencialEvento,
                    OcorreuEm = retorno.RetornoInformacaoEvento.DataEHoraRegistroEvento ?? DateTime.Now,
                    Protocolo = retorno.RetornoInformacaoEvento.NumeroProtocolo,
                    StatusRetorno = retorno.RetornoInformacaoEvento.CodigoStatus,
                    XmlEnvio = _xmlEnviar,
                    XmlRetorno = _xmlRetorno.OuterXml,
                    ChaveId = evento.InformacaoEvento.Id.Substring(2, evento.InformacaoEvento.Id.Length - 2)
                };

                var cartaCorrecaoInformacoes = ObterInformacoesCorrecoes(cartaCorrecao);
                cartaCorrecao.CteOsInformacaoCorrecaos = cartaCorrecaoInformacoes;

                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorioCte = new RepositorioCteOs(sessao);
                    repositorioCte.SalvarCartaCorrecao(cartaCorrecao);
                    transacao.Commit();
                }
            }
        }

        private List<CteInformacaoCorrecao> ObterInformacoesCorrecoes(CCeCTe cartaCorrecao)
        {
            var lista = new List<CteInformacaoCorrecao>();

            ListaCorrecao.ForEach(c =>
            {
                lista.Add(new CteInformacaoCorrecao
                {
                    Campo = c.CampoAlterado,
                    Grupo = c.GrupoAlterado,
                    NovoValor = c.ValorAlterado,
                    NumeroItem = c.NumeroItem ?? string.Empty,
                    CteCartaCorrecao = cartaCorrecao
                });
            });

            return lista;
        }

        private List<CteOsInformacaoCorrecao> ObterInformacoesCorrecoes(CteOsCartaCorrecao cartaCorrecao)
        {
            var lista = new List<CteOsInformacaoCorrecao>();

            ListaCorrecao.ForEach(c =>
            {
                lista.Add(new CteOsInformacaoCorrecao
                {
                    Campo = c.CampoAlterado,
                    Grupo = c.GrupoAlterado,
                    NovoValor = c.ValorAlterado,
                    NumeroItem = c.NumeroItem ?? string.Empty,
                    CteOsCartaCorrecao = cartaCorrecao
                });
            });

            return lista;
        }

        // todo imprimir 
        public void ImprimirCorrecao()
        {
            if (_cte is Cte cte)
            {
                try
                {
                    var impressao = new CTeImpressaoHelper();

                    impressao.GeraCCe(cte, HistoricoItem.XmlEnvio, HistoricoItem.XmlRetorno);
                }
                catch (ArgumentException ex)
                {
                    DialogBox.MostraInformacao(ex.Message);
                }
            }

            if (_cte is CteOs cteOs)
            {
                try
                {
                    var impressao = new CTeOsImpressaoHelper();

                    var pdf = impressao.GeraCCe(cteOs, HistoricoItem.XmlEnvio + HistoricoItem.XmlRetorno,
                        HistoricoItem.ChaveId);

                    Process.Start(pdf);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
                
        }
    }
}