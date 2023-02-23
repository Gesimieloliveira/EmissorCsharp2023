using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using DFe.Utils;
using FusionCore.DFe.RegrasNegocios;
using FusionCore.DFe.XmlCte;
using FusionCore.DFe.XmlCte.XmlCte.Inutilizacao;
using FusionCore.FusionAdm.CteEletronico.Assinatura;
using FusionCore.FusionAdm.CteEletronico.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Flags.Extencoes;
using FusionCore.FusionAdm.CteEletronico.Inutilizacao;
using FusionCore.FusionAdm.CteEletronico.Validacoes;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Fabricas;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.FusionAdm.CteEletronico;

namespace Fusion.Visao.CteEletronico.Inutilizacao
{
    public class CteInutilizacaoFormModel : ViewModel
    {
        private bool _editavel;
        private IInutilizar _inutilizar;
        private string _xmlEnvio;
        private string _xmlRetorno;
        private EmissorFiscal _emissor;
        private string _tipoDocumento;

        public ICommand CommandSelecionaEmissorFiscal => GetSimpleCommand(SelecionaEmissorFiscal);
        public ICommand CommandEnviarSefaz => GetSimpleCommand(EnviarSefaz);
        public ICommand CommandFecharTela => GetSimpleCommand(FecharTela);

        [Required(ErrorMessage = @"Porfavor escolher um emissor")]
        public string CnpjEmissor
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public short Serie
        {
            get { return GetValue<short>(); }
            set { SetValue(value); }
        }

        [Range(1, 999999999, ErrorMessage = @"Porfavor número inicial tem que ser maior que zero(0)")]
        public int NumeroInicial
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        [Range(1, 999999999, ErrorMessage = @"Porfavor número final tem que ser maior que zero(0)")]
        public int NumeroFinal
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        [Required(ErrorMessage = @"Porfavor digitar uma justificativa")]
        [MinLength(15, ErrorMessage = @"Porfavor a justificativa, deve ter no mínimo 15 caracteres.")]
        public string Justificativa
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool Editavel
        {
            get { return _editavel; }
            set
            {
                if (value == _editavel) return;
                _editavel = value;
                PropriedadeAlterada();
            }
        }

        public string TipoDocumento
        {
            get { return _tipoDocumento; }
            set
            {
                if (value == _tipoDocumento) return;
                _tipoDocumento = value;
                PropriedadeAlterada();
            }
        }

        public CteInutilizacaoFormModel()
        {
            Justificativa = string.Empty;
            NumeroFinal = 0;
            NumeroInicial = 0;
            Serie = 0;
            Editavel = true;
            CnpjEmissor = string.Empty;
        }


        private void SelecionaEmissorFiscal(object obj)
        {
            var model = new CteEmissorPickerModel();
            model.PickItemEvent += EmissorSelecionado;
            model.GetPickerView().ShowDialog();
        }

        private void EmissorSelecionado(object sender, GridPickerEventArgs e)
        {
            var emissor = e.GetItem<EmissorFiscal>();
            _emissor = emissor;

            if (emissor.FlagCte)
            {
                _inutilizar = emissor.EmissorFiscalCte;
                TipoDocumento = "CT-e";
            }

            if (emissor.FlagCteOs)
            {
                _inutilizar = emissor.EmissorFiscalCteOs;
                TipoDocumento = "CT-e Os";
            }

            CnpjEmissor = _inutilizar.Cnpj;
            Serie = _inutilizar.SerieInutilizar;
        }

        private async void EnviarSefaz(object obj)
        {
            Application.Current.Dispatcher.Invoke(ProgressBarAgil4.ShowProgressBar);
            await Task.Run(() => Evnia());
        }

        private void Evnia()
        {
            try
            {
                GerarXml();
                ValidarSchemaEnvioXml();

                var inutilizacaoCte = new InutilizacaoCte();


                var xmlRequest = new XmlDocument();
                xmlRequest.LoadXml(_xmlEnvio);

                var retorno = inutilizacaoCte.Executa(xmlRequest, _inutilizar.Estado.ToXml(),
                    CertificadoDigitalFactory.Cria(_emissor, true), _inutilizar.TipoAmbienteSefaz.ToXml(),
                    FusionTipoEmissaoCTe.Normal);

                _xmlRetorno = retorno.OuterXml;

                var retornoObjeto = FuncoesXml.XmlStringParaClasse<FustionRetornoInutilizacaoCTe>(_xmlRetorno);

                if(retornoObjeto.DadosRetorno.CodigoStatus == 102) SalvaInutilizacao(retornoObjeto);
                TrataRetorno(retornoObjeto);
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            finally
            {
                Application.Current.Dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);
                OnFechar();
            }
        }

        private void SalvaInutilizacao(FustionRetornoInutilizacaoCTe retornoObjeto)
        {
            var inutilizacao = new CteInutilizacao
            {
                Ano = byte.Parse(DateTime.Now.ToString("yy")),
                CnpjEmitente = CnpjEmissor,
                CodigoUfSolicitante = _inutilizar.Estado.CodigoIbge,
                Justificativa = Justificativa.Trim(),
                ModeloDocumento = _inutilizar.ModeloDocumento,
                NumeroFinal = NumeroFinal,
                NumeroInicial = NumeroInicial,
                Serie = Serie,
                XmlEnvio = _xmlEnvio,
                XmlRetorno = _xmlRetorno,
                Protocolo = retornoObjeto.DadosRetorno.Protocolo ?? string.Empty
            };

            var sessao = SessaoHelperFactory.AbrirSessaoAdm();
            var transacao = sessao.BeginTransaction();

            using (var repositorio = new RepositorioCte(sessao))
            {
                repositorio.SalvarCteInutilizacao(inutilizacao);

                transacao.Commit();
            }
        }

        private void TrataRetorno(FustionRetornoInutilizacaoCTe retornoObjeto)
        {
            if (retornoObjeto.DadosRetorno.CodigoStatus == 102)
            {
                DialogBox.MostraInformacao("Inutilização realizada com sucesso. \n Retorno Sefaz: " +
                                           retornoObjeto.DadosRetorno.Motivo);
                return;
            }

            DialogBox.MostraInformacao(retornoObjeto.DadosRetorno.Motivo);
        }

        private void GerarXml()
        {
            var fusionInutilizacaoXml = new FusionInutilizacaoCTe {Versao = "3.00"};
            var dadosInutilizacaoXml = fusionInutilizacaoXml.DadosDoPedido;

            var codigoUf = _inutilizar.Estado.CodigoIbge.ToString();
            var cnpj = _inutilizar.Cnpj.Trim();
            var modelo = ((byte) _inutilizar.ModeloDocumento).ToString();
            var serie = _inutilizar.SerieInutilizar.ToString("D3");
            var numeroInicial = NumeroInicial.ToString("D9");
            var numeroFinal = NumeroFinal.ToString("D9");

            var id = new StringBuilder("ID");
            id.Append(codigoUf).Append(cnpj).Append(modelo).Append(serie)
                .Append(numeroInicial).Append(numeroFinal);

            dadosInutilizacaoXml.Id = id.ToString();
            dadosInutilizacaoXml.Ambiente = _inutilizar.TipoAmbienteSefaz.ToXml();
            dadosInutilizacaoXml.Servico = "INUTILIZAR";
            dadosInutilizacaoXml.CodigoEstadoUf = _inutilizar.Estado.CodigoIbge;
            dadosInutilizacaoXml.Ano = byte.Parse(DateTime.Now.ToString("yy"));
            dadosInutilizacaoXml.Cnpj = cnpj;
            dadosInutilizacaoXml.TipoDocumentoFiscal = ConverterParaTipoDocumento(_inutilizar.ModeloDocumento);
            dadosInutilizacaoXml.Serie = _inutilizar.SerieInutilizar;
            dadosInutilizacaoXml.NumeroInicial = NumeroInicial;
            dadosInutilizacaoXml.NumeroFinal = NumeroFinal;
            dadosInutilizacaoXml.Justificativa = Justificativa.Trim().RemoverAcentos();

            var certificado = CertificadoDigitalFactory.Cria(_emissor, true);
            var xml = FuncoesXml.ClasseParaXmlString(fusionInutilizacaoXml).RemoverAcentos();
            var xmlAsssinado = AssinaturaDigital.AssinaDocumento(xml, dadosInutilizacaoXml.Id, certificado);

            _xmlEnvio = xmlAsssinado;
        }

        private FusionTipoDocumentoFiscalCTe ConverterParaTipoDocumento(ModeloDocumento inutilizarModeloDocumento)
        {
            switch (inutilizarModeloDocumento)
            {
                case ModeloDocumento.CTe:
                    return FusionTipoDocumentoFiscalCTe.CTe;
                case ModeloDocumento.CTeOS:
                    return FusionTipoDocumentoFiscalCTe.CTeOs;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inutilizarModeloDocumento), inutilizarModeloDocumento, null);
            }
        }

        private void ValidarSchemaEnvioXml()
        {
            var validar = new ValidarSchema();
            validar.Validar(_xmlEnvio, ManipulaArquivo.LocalAplicacao() + @"\Assets\Schemas.Cte\inutCTe_v3.00.xsd");
        }

        private void FecharTela(object obj)
        {
            OnFechar();
        }
    }
}