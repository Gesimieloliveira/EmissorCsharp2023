using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Fusion.Sessao;
using Fusion.Visao.MdfeEletronico.Emissao;
using FusionCore.ExportacaoPacote.Empacotadores;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using FusionCore.FusionAdm.MdfeEletronico.Helpers;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.Filtros;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.SendMail;
using Microsoft.Win32;
using NHibernate.Util;

// ReSharper disable RedundantBoolCompare

namespace Fusion.Visao.MdfeEletronico
{
    public class GridMdfeModel : ViewModel
    {
        private MdfeGridDto _selecionado;
        private bool _todosSelecionado;
        private bool _isPermissaoEnviarXml;
        private bool _isPermissaoBaixarXml;
        private ObservableCollection<EstadoDTO> _estadosCarregamento = new ObservableCollection<EstadoDTO>();
        private ObservableCollection<EstadoDTO> _estadosDescarregamento = new ObservableCollection<EstadoDTO>();

        public GridMdfeModel()
        {
            ListaMdfe = new List<MdfeGridDto>();
            Filtro = new FiltroMdfe();

            CarregarEstados();
        }

        public FiltroMdfe Filtro
        {
            get => GetValue<FiltroMdfe>();
            set => SetValue(value);
        }

        public IEnumerable<MdfeGridDto> ListaMdfe
        {
            get => GetValue<IEnumerable<MdfeGridDto>>();
            set => SetValue(value);
        }

        public MdfeGridDto Selecionado
        {
            get => _selecionado;
            set
            {
                if (Equals(value, _selecionado)) return;
                _selecionado = value;
                PropriedadeAlterada();
            }
        }

        public ICommand NovoCommand => GetSimpleCommand(NovoAction);
        public ICommand SelecionarTodosCommand => GetSimpleCommand(SelecionarTodosAction);
        public ICommand ExportarXmlCommand => GetSimpleCommand(ExportarXmlAction);
        public ICommand BaixarXmlCommand => GetSimpleCommand(BaixarXmlAction);

        public bool IsPermissaoEnviarXml
        {
            get => _isPermissaoEnviarXml;
            set
            {
                if (value == _isPermissaoEnviarXml) return;
                _isPermissaoEnviarXml = value;
                PropriedadeAlterada();
            }
        }

        public bool IsPermissaoBaixarXml
        {
            get => _isPermissaoBaixarXml;
            set
            {
                if (value == _isPermissaoBaixarXml) return;
                _isPermissaoBaixarXml = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<EstadoDTO> EstadosCarregamento
        {
            get { return _estadosCarregamento; }
            set
            {
                _estadosCarregamento = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<EstadoDTO> EstadosDescarregamento
        {
            get { return _estadosDescarregamento; }
            set
            {
                _estadosDescarregamento = value;
                PropriedadeAlterada();
            }
        }

        private void BaixarXmlAction(object obj)
        {
            SessaoSistema.ObterUsuarioLogado().VerificaPermissao.IsTemPermissaoThrow(Permissao.MDFE_BAIXAR_XML);

            if (ExportarXmlValidacao()) return;

            var destino = DialogSalvar();

            if (destino.IsNullOrEmpty()) return;

            var xmlExpostacao = MontaListaParaExportacao();

            var pacoteGerado = EmpacotaXmls(xmlExpostacao);


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

        private void ExportarXmlAction(object obj)
        {
            SessaoSistema.ObterUsuarioLogado().VerificaPermissao.IsTemPermissaoThrow(Permissao.MDFE_ENVIAR_XML);

            if (ExportarXmlValidacao()) return;

            var behavior = CriaJanelaEnvioEmail();

            new EnvioEmailView(behavior).ShowDialog();
        }

        private bool ExportarXmlValidacao()
        {
            if (ListaMdfe.Count(c => c.IsSelecionado == true) == 0)
            {
                DialogBox.MostraInformacao("Selecione no mínimo uma linha da tabela");
                return true;
            }


            var qtd = ListaMdfe.Count(c => c.IsSelecionado == true && (c.Status == MDFeStatus.ConsultaProcessamento ||
                                                                       c.Status == MDFeStatus.EmDigitacao));

            if (qtd > 0)
            {
                DialogBox.MostraInformacao("Somente MDF-e Autorizados ou Canceladas.");
                return true;
            }
            return false;
        }

        private EnvioEmailBehavior CriaJanelaEnvioEmail()
        {
            var behavior = new EnvioEmailBehavior();

            behavior.DespacharEmails += DespacharEmailsHandler;
            behavior.Assunto = "MANIFESTO ELETRÔNICO DE DOCUMENTOS";
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
            var pacoteGerado = EmpacotaXmls(xmlExpostacao);

            new MDFeImpressaoHelper().EnviarLoteXml(pacoteGerado, e, behavior.Assunto, behavior.CorpoMensagem);
        }

        private static FileInfo EmpacotaXmls(IList<IEnvelope> xmlExpostacao)
        {
            var empacotador = new Empacotador();

            empacotador.ComEnvelopes(xmlExpostacao);
            var pacoteGerado = empacotador.GeraPacote();
            return pacoteGerado;
        }

        private IList<IEnvelope> MontaListaParaExportacao()
        {
            var listaXmlAutorizado = ListaMdfe.Where(c => 
                c.IsSelecionado == true && (
                    c.Status == MDFeStatus.Autorizado || 
                    c.Status == MDFeStatus.Cancelada || 
                    c.Status == MDFeStatus.Encerrada
                )).ToList();

            var xmlExpostacao = new List<IEnvelope>();

            listaXmlAutorizado.ForEach(x =>
            {
                var xml = new XmlExportacaoMDFe
                {
                    Xml = x.XmlAutorizado,
                    Chave = x.Chave,
                    Status = x.Status
                };

                xmlExpostacao.Add(xml);
            });

            return xmlExpostacao;
        }

        private void SelecionarTodosAction(object obj)
        {
            if (_todosSelecionado == false)
            {
                ListaMdfe.ForEach(c =>
                {
                    c.IsSelecionado = true;
                });

                _todosSelecionado = true;
                return;
            }

            if (_todosSelecionado)
            {
                ListaMdfe.ForEach(c =>
                {
                    c.IsSelecionado = false;
                });

                _todosSelecionado = false;
            }
        }

        private void NovoAction(object obj)
        {
            var emitirForm = new MdfeEmitirForm();

            emitirForm.ShowDialog();

            AplicarPesquisa();
        }

        public void AplicarPesquisa()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioMdfe(sessao);
                var resultado = repositorio.BuscarParaGrid(Filtro);

                var lista = new List<MdfeGridDto>();

                resultado.ForEach(x =>
                {
                    if (lista.Any(m => m.Id == x.Id))
                    {
                        var mdfeGrid = lista.SingleOrDefault(mdfeG => mdfeG.Id == x.Id);

                        mdfeGrid.MotoristasNomes.Add(x.NomeMotorista);
                    }

                    if (lista.All(m => m.Id != x.Id))
                    {
                        x.MotoristasNomes.Add(x.NomeMotorista);
                        lista.Add(x);
                    }
                });

                ListaMdfe = lista.OrderByDescending(i => i.Id);
            }
        }

        public void Editar()
        {
            switch (Selecionado.Status)
            {
                case MDFeStatus.Cancelada:
                    throw new ArgumentException("MDF-e está cancelada");

                case MDFeStatus.Encerrada:
                case MDFeStatus.ConsultaProcessamento:
                case MDFeStatus.Autorizado:
                    var opcoes = new MdfeEletronicaOpcoes(Selecionado.Id);
                    opcoes.ShowDialog();
                    AplicarPesquisa();
                    return;

            }

            var emitirForm =  new MdfeEmitirForm(Selecionado.Id);
            emitirForm.ShowDialog();

            AplicarPesquisa();
        }

        public void Opcoes()
        {
            switch (Selecionado.Status)
            {
                case MDFeStatus.Cancelada:
                    throw new ArgumentException("MDF-e está cancelada");
                case MDFeStatus.Encerrada:
                case MDFeStatus.ConsultaProcessamento:
                case MDFeStatus.Autorizado:
                    var opcoes = new MdfeEletronicaOpcoes(Selecionado.Id);
                    opcoes.ShowDialog();
                    AplicarPesquisa();
                    return;
            }

            throw new ArgumentException("MDF-e está em digitação");
        }

        private void CarregarEstados()
        {
            EstadosCarregamento = new ObservableCollection<EstadoDTO>(LocalidadesServico.GetInstancia().GetEstados());
            EstadosDescarregamento =
                new ObservableCollection<EstadoDTO>(LocalidadesServico.GetInstancia(false).GetEstados());
        }
    }
}