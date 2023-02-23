using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using DFe.Classes.Entidades;
using DFe.Utils;
using Fusion.Visao.Compras.Importacao;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Sessao;
using FusionCore.GerenciarManifestacoesEletronicas;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.SendMail;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;
using Compressao = DFe.Compressoes.Compressao;

namespace Fusion.Visao.GerenciadorManifestacoesDestinatarios
{
    public class ManifestoOpcoesModel : ViewModel
    {
        private readonly IDadosServicoSefaz _emissor;
        private NfeResumidaGrid _nfeResumidaSelecionada;

        public ManifestoOpcoesModel(NfeResumidaGrid nfeResumidaGrid)
        {
            _emissor = nfeResumidaGrid.GetNfeResumida().EmissorFiscal.EmissorFiscalNfe;
            NfeResumidaSelecionada = nfeResumidaGrid;
        }

        public NfeResumidaGrid NfeResumidaSelecionada
        {
            get => _nfeResumidaSelecionada;
            set
            {
                if (Equals(value, _nfeResumidaSelecionada)) return;
                _nfeResumidaSelecionada = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandDanfe => GetSimpleCommand(DanfeAcao);
        public ICommand CommandEmail => GetSimpleCommand(EmailAcao);
        public ICommand ComandoImportarCompra => GetSimpleCommand(ImportarCompraAcao);

        public void DesconhecimentoDaOperacao()
        {
            ManifestacaoDestinatario(NFeTipoEvento.TeMdDesconhecimentoDaOperacao);
        }

        public void OperacaoNaoRealizada(string justificativa)
        {
            ManifestacaoDestinatario(NFeTipoEvento.TeMdOperacaoNaoRealizada, justificativa);
        }

        public void ConfirmacaoOperacao()
        {
            ManifestacaoDestinatario(NFeTipoEvento.TeMdConfirmacaoDaOperacao);
        }

        public void CienciaEmissaoAcao()
        {
            ManifestacaoDestinatario(NFeTipoEvento.TeMdCienciaDaOperacao);
        }

        private void DanfeAcao(object obj)
        {
            try
            {
                var nfeResuminda = BuscarNFeResumida();

                if (!nfeResuminda.IsDownloadDisponivel())
                {
                    throw new InvalidOperationException(
                        "Somente é possível visualizar o danfe quando tiver feito DOWNLOAD XML");
                }

                var xmlAutorizado = nfeResuminda.DownloadXmlString();

                DanfeNfeHelper.GeraArquivoDanfe(xmlAutorizado, NfeResumidaSelecionada.IsCancelada, null);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void EmailAcao(object obj)
        {
            try
            {
                var nfeResuminda = BuscarNFeResumida();

                if (!nfeResuminda.IsDownloadDisponivel())
                {
                    throw new InvalidOperationException(
                        "Somente é possível visualizar o danfe quando tiver feito DOWNLOAD XML");
                }

                var behavior = new EnvioEmailBehavior();

                behavior.DespacharEmails += DespacharEmailsHandler;
                behavior.Assunto = "NOTA FISCAL ELETRONICA";
                behavior.CorpoMensagem = "Segue em anexo o DANFE e o XML";

                new EnvioEmailView(behavior).ShowDialog();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void DespacharEmailsHandler(object sender, IEnumerable<Email> e)
        {
            if (!(sender is EnvioEmailBehavior behavior))
            {
                return;
            }

            var nfeResuminda = BuscarNFeResumida();

            DanfeNfeHelper.EnviaEmail(nfeResuminda.DownloadXmlString(), e, behavior.Assunto, behavior.CorpoMensagem);
        }

        private void ImportarCompraAcao(object obj)
        {
            try
            {
                var nfeResuminda = BuscarNFeResumida();

                if (!nfeResuminda.IsDownloadDisponivel())
                {
                    throw new InvalidOperationException(
                        "Somente é possível visualizar o danfe quando tiver feito DOWNLOAD XML");
                }

                new ImportacaoCompraView(nfeResuminda.DownloadXmlString()).ShowDialog();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        public void DownloadXml(NfeResumida nfeResumida)
        {
            try
            {
                if (nfeResumida.IsPodeEfetuarDownload() == false)
                    throw new InvalidOperationException(
                        "Somente é possível efetuar o download do xml quando tiver feito uma \nCIÊNCIA DA EMISSÃO\nCIÊNCIA DA OPERAÇÃO\nOPERAÇÃO NÃO REALIZADA\nDESCONHECIMENTO DA OPERAÇÃO");

                var servicoNfe = CriaZeusServico();

                const string ultNsu = "0";
                const string nsu = "0";

                var resultado = servicoNfe.NfeDistDFeInteresse(
                    nfeResumida.Empresa.CidadeDTO.SiglaUf,
                    nfeResumida.Empresa.DocumentoUnico,
                    ultNsu,
                    nsu,
                    nfeResumida.Chave);

                if (resultado?.Retorno == null || resultado.Retorno.cStat != 138)
                {
                    throw new InvalidOperationException(resultado?.Retorno?.xMotivo ?? "A Sefaz não disponibilizou o xml para baixar.\nTente novamente mais tarde.");
                }

                if (resultado.Retorno.loteDistDFeInt == null || resultado.Retorno.loteDistDFeInt.Length == 0)
                {
                    throw new InvalidOperationException("A Sefaz não disponibilizou o xml para baixar.\nTente novamente mais tarde.");
                }

                if (resultado.Retorno.loteDistDFeInt[0].NfeProc == null)
                {
                    throw new InvalidOperationException("A Sefaz não disponibilizou o xml para baixar.\nTente novamente mais tarde.");
                }

                var procNfe = resultado.Retorno.loteDistDFeInt[0];

                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorioDistribuicao = new RepositorioDistribuicaoDFe(sessao);
                    var nfeResumidaId = nfeResumida.Id;

                    nfeResumida = repositorioDistribuicao.NfeResumidaPelo(nfeResumidaId);

                    nfeResumida.DownloadXmlStatus();
                    nfeResumida.AddXmlDownload(Compressao.Unzip(procNfe.XmlNfe));

                    repositorioDistribuicao.Salvar(nfeResumida);
                    transacao.Commit();
                }

                DialogBox.MostraInformacao("Download efetuado com sucesso");
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        public void SalvarXmlEmDisco(NfeResumida nfeResuminda)
        {
            var dialog = new SaveFileDialog { Filter = @"Arquivo Xml|*.xml" };
            var showDialog = dialog.ShowDialog();

            if (showDialog == DialogResult.Cancel)
                return;

            if (dialog.FileName.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Não foi selecionado um caminho");
                return;
            }

            var xmlAutorizado = nfeResuminda.DownloadXml.Xml;

            if (xmlAutorizado.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Não encontrei XML autorizado para essa NF-e");
                return;
            }

            FuncoesXml.SalvarStringXmlParaArquivoXml(xmlAutorizado, dialog.FileName);

            DialogBox.MostraInformacao("XML foi salvo com sucesso");
        }

        private void ManifestacaoDestinatario(NFeTipoEvento evento, string justificativa = null)
        {
            try
            {
                var nfeResumida = NfeResumidaSelecionada.GetNfeResumida();
                var servicoNFe = CriaZeusServico();

                const int idLote = 1;
                const int sequenciaEvento = 1; 

                var resposta = servicoNFe.RecepcaoEventoManifestacaoDestinatario(
                    idLote,
                    sequenciaEvento,
                    nfeResumida.Chave,
                    ConverteEvento(evento),
                    nfeResumida.Empresa.DocumentoUnico, 
                    justificativa);

                var cStatLote = resposta.Retorno.cStat;
                var xMotivLote = resposta.Retorno.xMotivo;
                var temEventos = resposta.Retorno.retEvento?.Any() == true;

                if (cStatLote != 128 || !temEventos)
                {
                    throw new InvalidOperationException($"Falha ao Manifestar: {cStatLote} - {xMotivLote}");
                }

                var cStat = resposta.Retorno.retEvento[0].infEvento.cStat;
                var xMotiv = resposta.Retorno.retEvento[0].infEvento.xMotivo;

                if (cStat != 573 && cStat != 135)
                {
                    throw new InvalidOperationException($"Falha ao Manifestar: {cStat} - {xMotiv}");
                }

                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorio = new RepositorioDistribuicaoDFe(sessao);
                    var nfeResumidaId = nfeResumida.Id;

                    nfeResumida = repositorio.NfeResumidaPelo(nfeResumidaId);

                    nfeResumida.EventoManifestacaoLista.Add(new EventoManifestacao
                    {
                        NfeResumida = nfeResumida,
                        Xml = resposta.RetornoCompletoStr,
                        Evento = ConverteEventoParaFusion(evento)
                    });

                    switch (evento)
                    {
                        case NFeTipoEvento.TeMdConfirmacaoDaOperacao:
                            nfeResumida.ConfirmacaoOperacao();
                            break;
                        case NFeTipoEvento.TeMdCienciaDaOperacao:
                            nfeResumida.CienciaEmissao();
                            break;
                        case NFeTipoEvento.TeMdDesconhecimentoDaOperacao:
                            nfeResumida.DesconhecimentoOperacao();
                            break;
                        case NFeTipoEvento.TeMdOperacaoNaoRealizada:
                            nfeResumida.OperacaoNaoRealizada();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(evento), evento, null);
                    }

                    repositorio.Salvar(nfeResumida);
                    transacao.Commit();
                }

                DialogBox.MostraInformacao(xMotiv);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }

        }

        public NfeResumida BuscarNFeResumida()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioDistribuicaoDFe(sessao);
                var nfeResumida = repositorio.BuscarNfeResumidaPela(NfeResumidaSelecionada.GetNfeResumida().Chave);
                return nfeResumida;
            }
        }

        private StatusManifestacao ConverteEventoParaFusion(NFeTipoEvento evento)
        {
            switch (evento)
            {
                case NFeTipoEvento.TeMdConfirmacaoDaOperacao:
                    return StatusManifestacao.ConfirmacaoOperacao;
                case NFeTipoEvento.TeMdCienciaDaOperacao:
                    return StatusManifestacao.CienciaOperacao;
                case NFeTipoEvento.TeMdDesconhecimentoDaOperacao:
                    return StatusManifestacao.DesconhecimentoOperacao;
                case NFeTipoEvento.TeMdOperacaoNaoRealizada:
                    return StatusManifestacao.OperacaoNaoRealizada;
                default:
                    throw new ArgumentOutOfRangeException(nameof(evento), evento, null);
            }
        }

        private ServicosNFe CriaZeusServico()
        {
            var cfgBuilder = new ConfiguracaoZeusBuilder(_emissor, TipoEmissao.Normal);
            var configuracao = cfgBuilder.GetConfiguracao(Estado.AN);

            var servicoNFe = new ServicosNFe(configuracao);
            return servicoNFe;
        }

        private NFeTipoEvento ConverteEvento(NFeTipoEvento evento)
        {
            switch (evento)
            {
                case NFeTipoEvento.TeMdConfirmacaoDaOperacao:
                    return NFeTipoEvento.TeMdConfirmacaoDaOperacao;
                case NFeTipoEvento.TeMdCienciaDaOperacao:
                    return NFeTipoEvento.TeMdCienciaDaOperacao;
                case NFeTipoEvento.TeMdDesconhecimentoDaOperacao:
                    return NFeTipoEvento.TeMdDesconhecimentoDaOperacao;
                case NFeTipoEvento.TeMdOperacaoNaoRealizada:
                    return NFeTipoEvento.TeMdOperacaoNaoRealizada;
                default:
                    throw new ArgumentOutOfRangeException(nameof(evento), evento, null);
            }
        }

        public void IsContemCienciaEmissao()
        {
            if (BuscarNFeResumida().IsContemCienciaEmissaoEvento())
            {
                throw new InvalidOperationException("Já contém Ciência da Emissão");
            }
        }

        public void IsContemConfirmacaoDaOperacao()
        {
            if (BuscarNFeResumida().IsContemConfirmacaoOperacaoEvento())
            {
                throw new InvalidOperationException("Já contém Confirmação da Operação");
            }
        }

        public void IsContemOperacaoNaoRealizada()
        {
            if (BuscarNFeResumida().IsContemOperacaoNaoRealizadaEvento())
            {
                throw new InvalidOperationException("Já contém Operação não Realizada");
            }
        }

        public void IsContemDesconhecimentoDaOperacao()
        {
            if (BuscarNFeResumida().IsContemDesconhecimentoDaOperacaoEvento())
            {
                throw new InvalidOperationException("Já contém Desconhecimento da Operação");
            }
        }

        public void MarcarComoImportada()
        {
            new NfeResumidaImportadaServico(_nfeResumidaSelecionada.Chave).Importada();
        }
    }
}