using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Fusion.Sessao;
using Fusion.Visao.CteEletronico.Emitir;
using Fusion.Visao.CteEletronico.Emitir.Emissao;
using FusionCore.ExportacaoPacote.Empacotadores;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.CteEletronico.Helpers;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.SendMail;
using Microsoft.Win32;
using NHibernate.Util;

namespace Fusion.Visao.CteEletronico.Grid
{
    public class GridCTeModel : ViewModel
    {

        private ObservableCollection<CteGridDto> _listaCte;
        private CteGridDto _selecionado;
        private DateTime? _dataEmissaoInicial;
        private DateTime? _dataEmissaoFinal;
        private string _pesqusiaTexto;
        private CteStatus? _status;
        private string _numeroDocumento;
        private bool _todosSelecionado;
        private bool _isPermiteEnviarXml;
        private bool _isPermiteBaixarXml;

        public CteGridDto Selecionado
        {
            get => _selecionado;
            set
            {
                if (Equals(value, _selecionado)) return;
                _selecionado = value;
                PropriedadeAlterada();
            }
        }


        public ObservableCollection<CteGridDto> ListaCte
        {
            get => _listaCte;
            set
            {
                if (Equals(value, _listaCte)) return;
                _listaCte = value;
                PropriedadeAlterada();
            }
        }

        public ICommand NovoCommand => GetSimpleCommand(NovoAction);

        public ICommand AplicarPesquisaCommand => GetSimpleCommand(AplicarFiltroAction);

        public DateTime? DataEmissaoInicial
        {
            get => _dataEmissaoInicial;
            set
            {
                if (value.Equals(_dataEmissaoInicial)) return;
                _dataEmissaoInicial = value;
                PropriedadeAlterada();
            }
        }

        public DateTime? DataEmissaoFinal
        {
            get => _dataEmissaoFinal;
            set
            {
                if (value.Equals(_dataEmissaoFinal)) return;
                _dataEmissaoFinal = value;
                PropriedadeAlterada();
            }
        }

        public string PesqusiaTexto
        {
            get => _pesqusiaTexto;
            set
            {
                if (value == _pesqusiaTexto) return;
                _pesqusiaTexto = value;
                PropriedadeAlterada();
            }
        }

        public CteStatus? Status
        {
            get => _status;
            set
            {
                if (value == _status) return;
                _status = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroDocumento
        {
            get => _numeroDocumento;
            set
            {
                if (value == _numeroDocumento) return;
                _numeroDocumento = value;
                PropriedadeAlterada();
            }
        }

        public ICommand ExportarXmlCommand => GetSimpleCommand(ExportarXmlAction);

        public ICommand SelecionarTodosCommand => GetSimpleCommand(SelecionarTodosAction);

        public ICommand BaixarXmlCommand => GetSimpleCommand(BaixarXmlAction);

        public bool IsPermiteEnviarXml
        {
            get => _isPermiteEnviarXml;
            set
            {
                if (value == _isPermiteEnviarXml) return;
                _isPermiteEnviarXml = value;
                PropriedadeAlterada();
            }
        }

        private void BaixarXmlAction(object obj)
        {
            if (ExportarXmlValidacao()) return;

            var destino = DialogSalvar();

            if (destino.IsNullOrEmpty()) return;

            var xmlExpostacao = MontaListaParaExportacao();

            var pacoteGerado = EcmpacotaXmls(xmlExpostacao);


            if (File.Exists(destino))
                File.Delete(destino);

            pacoteGerado.CopyTo(destino);

            DialogBox.MostraInformacao("Xml baixados com sucesso");
        }

        private static string DialogSalvar()
        {
            var dialog = new SaveFileDialog {Filter = @"Arquivo Zipado (.zip)|*.zip"};
            dialog.ShowDialog();

            var destino = dialog.FileName;
            return destino;
        }

        private void SelecionarTodosAction(object obj)
        {

            if (_todosSelecionado == false)
            {
                ListaCte.ForEach(c => { c.IsSelecionado = true; });

                _todosSelecionado = true;
                return;
            }

            if (_todosSelecionado)
            {
                ListaCte.ForEach(c => { c.IsSelecionado = false; });

                _todosSelecionado = false;
            }
        }

        private void ExportarXmlAction(object obj)
        {

            if (ExportarXmlValidacao()) return;

            var behavior = CriaJanelaEnvioEmail();

            new EnvioEmailView(behavior).ShowDialog();
        }

        private bool ExportarXmlValidacao()
        {
            if (ListaCte.Count(c => c.IsSelecionado) == 0)
            {
                DialogBox.MostraInformacao("Selecione no mínimo uma linha da tabela");
                return true;
            }

            if (ListaCte.Any(c => c.IsSelecionado && c.PermiteExportacao() == false))
            {
                DialogBox.MostraInformacao("Selecione CTe apenas Autorizados ou Cancelados.");
                return true;
            }

            return false;
        }

        private EnvioEmailBehavior CriaJanelaEnvioEmail()
        {
            var behavior = new EnvioEmailBehavior();

            behavior.DespacharEmails += DespacharEmailsHandler;
            behavior.Assunto = "CONHECIMENTO TRANSPORTE ELETRÔNICO";
            behavior.CorpoMensagem = "Segue em anexo os XMLs";

            behavior.Emails = new ObservableCollection<Email>();

            return behavior;
        }

        private void DespacharEmailsHandler(object sender, IEnumerable<Email> e)
        {
            if (!(sender is EnvioEmailBehavior behavior))
            {
                return;
            }

            var xmlExpostacao = MontaListaParaExportacao();
            var pacoteGerado = EcmpacotaXmls(xmlExpostacao);

            new CTeImpressaoHelper().EnviarLoteXmlAutorizado(pacoteGerado, e, behavior.Assunto, behavior.CorpoMensagem);
        }

        private static FileInfo EcmpacotaXmls(IList<IEnvelope> xmlExpostacao)
        {
            var empacotador = new Empacotador();
            empacotador.ComEnvelopes(xmlExpostacao);

            return empacotador.GeraPacote();
        }

        private IList<IEnvelope> MontaListaParaExportacao()
        {
            var listaXmlAutorizado = ListaCte.Where(
                c => c.IsSelecionado && (
                         c.Status == CteStatus.Autorizado || 
                         c.Status == CteStatus.Cancelada
                )).ToList();

            var xmlExpostacao = new List<IEnvelope>();

            listaXmlAutorizado.ForEach(x =>
            {
                var xml = new XmlExportacao
                {
                    Xml = x.XmlAutorizado,
                    Chave = x.Chave,
                    StatusRetorno = x.StatusCancelamento,
                    XmlCancelamento = x.XmlCancelamento
                };

                xmlExpostacao.Add(xml);
            });

            return xmlExpostacao;
        }

        public void AplicarFiltroAction(object obj)
        {
            AplicaFiltro();
        }

        private void AplicaFiltro()
        {
            var filtros = new CteFiltroGridDto
            {
                NumeroDocumento = NumeroDocumento,
                TextoPesquisado = PesqusiaTexto,
                DataEmissaoFinal = DataEmissaoFinal,
                DataEmissaoInicial = DataEmissaoInicial,
                Status = Status
            };

            BuscarCtesGrid(filtros);
        }

        private void NovoAction(object obj)
        {
            var vm = new CteEmitirFormModel(new Cte {CalcularTotalCargaAutomatico = true});
            var cteEmitirForm = new CteEmitirForm(vm);

            cteEmitirForm.ShowDialog();
            AplicaFiltro();
        }

        public void Opcoes()
        {
            if (Selecionado.IsOpcoes())
            {
                var janela = new CteEletronicaOpcoes(BuscarCte());
                janela.ShowDialog();

                AplicaFiltro();
                return;
            }

            DialogBox.MostraInformacao("CT-e não está autorizado");
        }

        private Cte BuscarCte()
        {
            Cte cte;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioCte(sessao);
                cte = repositorio.GetPeloId(Selecionado.Id);
            }

            return cte;
        }

        public void Editar()
        {
            if (Selecionado.Inutilizado)
            {
                throw new InvalidOperationException("CT-e está inutilizado!");
            }

            if (Selecionado.PodeEditar())
            {
                var vm = new CteEmitirFormModel(BuscarCte());
                var cteEmitirForm = new CteEmitirForm(vm);
                cteEmitirForm.ShowDialog();

                AplicaFiltro();
                return;
            }

            var janela = new CteEletronicaOpcoes(BuscarCte());
            janela.ShowDialog();

            AplicaFiltro();
        }

        public void BuscarCtesGrid(CteFiltroGridDto filtros)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioCte(sessao);
                ListaCte = new ObservableCollection<CteGridDto>(repositorio.BuscarTodosParaGrid(filtros));
            }
        }

        public void InicializarPermissoes()
        {
            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;

            IsPermiteEnviarXml = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.CTE_ENVIAR_XML);
            IsPermiteBaixarXml = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.CTE_BAIXAR_XML);
        }

        public bool IsPermiteBaixarXml
        {
            get => _isPermiteBaixarXml;
            set
            {
                _isPermiteBaixarXml = value;
                PropriedadeAlterada();
            }
        }
    }
}