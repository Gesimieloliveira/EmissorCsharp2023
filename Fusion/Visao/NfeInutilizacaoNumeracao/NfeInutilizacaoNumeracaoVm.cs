using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Fusion.Visao.EmissorFiscalEletronico;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF.InutilizacaoNumero;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;

// ReSharper disable MemberCanBePrivate.Global

namespace Fusion.Visao.NfeInutilizacaoNumeracao
{
    public class NfeInutilizacaoNumeracaoVm : ViewModel
    {
        private ConfiguracaoCertificadoDigital _configuracaoCertificado;
        private NfeInutilizacaoNumeracaoDTO _inutilizacao;

        public NfeInutilizacaoNumeracaoVm(NfeInutilizacaoNumeracaoDTO inutilizacao)
        {
            _inutilizacao = inutilizacao;
        }

        [Required(ErrorMessage = "Preciso que informe este campo")]
        public short? Serie
        {
            get => GetValue<short?>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Preciso que informe este campo")]
        [Range(1, 999999999, ErrorMessage = @"Número inicial precisa ser maior que 0")]
        public int? NumeroInicial
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Preciso que informe este campo")]
        [Range(1, 999999999, ErrorMessage = @"Número final precisa ser maior que 0")]
        public int? NumeroFinal
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Preciso que informe este campo")]
        [MinLength(15, ErrorMessage = @"Justificativa precisa ter 15 ou mais letras")]
        public string Justificativa
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string TipoDocumento
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool Editavel
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ObservableCollection<EmissorFiscalComboBox> Emissores
        {
            get => GetValue<ObservableCollection<EmissorFiscalComboBox>>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Preciso que selecione um emissor")]
        public EmissorFiscalComboBox EmissorSelecionado
        {
            get => GetValue<EmissorFiscalComboBox>();
            set
            {
                SetValue(value);
                TipoDocumento = string.Empty;

                if (value?.IsNfce == true) TipoDocumento = "NFC-E";
                if (value?.IsNfe == true) TipoDocumento = "NF-E";
            }
        }

        public void Inicializar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmissorFiscal(sessao);
                var emissores = repositorio.BuscaTodosParaComboBox();
                var emissoresCompativeis = emissores.Where(c => c.IsNfce || c.IsNfe).ToList();

                Emissores = new ObservableCollection<EmissorFiscalComboBox>(emissoresCompativeis);
            }

            InicializarCampos();
        }

        private void InicializarCampos()
        {
            if (_inutilizacao == null || _inutilizacao.Id == 0)
            {
                Editavel = true;
                Serie = null;
                NumeroInicial = null;
                NumeroFinal = null;
                Justificativa = null;

                return;
            }

            Serie = _inutilizacao.Serie;
            NumeroInicial = _inutilizacao.NumeroInicial;
            NumeroFinal = _inutilizacao.NumeroFinal;
            Justificativa = _inutilizacao.Justificativa;

            if (_inutilizacao.ModeloDocumento == ModeloDocumento.NFe) TipoDocumento = "NF-E";
            if (_inutilizacao.ModeloDocumento == ModeloDocumento.NFCe) TipoDocumento = "NFC-E";
        }

        public void SolicitaInutilizacao(Dispatcher dispatcher)
        {
            try
            {
                ThrowExceptionSeExistirErros();
                SolicitarCertificadoDigital();

                ProgressBarAgil4.ShowProgressBar();

                Task.Run(() =>
                {
                    try
                    {
                        FazerInutilizacao();

                        dispatcher.Invoke(() => { DialogBox.MostraInformacao("Inutilizado com sucesso!"); });
                        InicializarParaNovaInutilizacao();
                    }
                    catch (InvalidOperationException e)
                    {
                        dispatcher.Invoke(() => { DialogBox.MostraAviso("Falha ao inutilizar", e.Message); });
                    }
                    catch (Exception e)
                    {
                        dispatcher.Invoke(() => { DialogBox.MostraErro($"Falha ao inutilizar: {e.Message}", e); });
                    }
                    finally
                    {
                        dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);
                    }
                });
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void SolicitarCertificadoDigital()
        {
            if (!EmissorSelecionado.IsNfce)
            {
                return;
            }

            if (_configuracaoCertificado != null && DesejaUtilizarCertificadoAnterior())
            {
                return;
            }

            _configuracaoCertificado = null;

            var model = new CertificadoDigitalFormModel();
            model.ConfirmarConfiguracaoCertificadoDigital += (s, e) => { _configuracaoCertificado = e; };
            var certificadoDigitalForm = new CertificadoDigitalForm(model);
            certificadoDigitalForm.ShowDialog();

            if (_configuracaoCertificado == null)
            {
                throw new InvalidOperationException("Para NFC-E é preciso selecionar o certificado");
            }
        }

        private bool DesejaUtilizarCertificadoAnterior()
        {
            const string msg = "Deseja utilizar o última certificado para inutilizar essa nuneração da NFC-E?";
            return DialogBox.MostraConfirmacao(msg) == MessageBoxResult.Yes;
        }

        private void FazerInutilizacao()
        {
            var emissor = CarregaEmissor();
            var inutilizacao = CarregaInutilizacao(emissor);
            var sucesso = inutilizacao.EnviarParaSefaz();

            _inutilizacao.ModeloDocumento = sucesso.Inutilizacao.Modelo;
            _inutilizacao.Ano = sucesso.Inutilizacao.Ano;
            _inutilizacao.Serie = sucesso.Inutilizacao.Serie;
            _inutilizacao.Justificativa = sucesso.Inutilizacao.Justificativa;
            _inutilizacao.NumeroInicial = sucesso.Inutilizacao.NumeroInicial;
            _inutilizacao.NumeroFinal = sucesso.Inutilizacao.NumeroFinal;
            _inutilizacao.CnpjEmitente = sucesso.Inutilizacao.CnpjEmitente;
            _inutilizacao.CodigoUfSolicitante = emissor.Empresa.EstadoDTO.CodigoIbge;
            _inutilizacao.Protocolo = sucesso.Protocolo;
            _inutilizacao.InutilizacaoEm = DateTime.Now;
            _inutilizacao.Uuid = GuuidHelper.Computar();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioInutilizacao(sessao);
                repositorio.Salvar(_inutilizacao);
            }
        }

        private NfeInutilizacaoZeus CarregaInutilizacao(EmissorFiscal emissor)
        {
            var ano = byte.Parse(DateTime.Now.Year.ToString().Substring(2, 2));

            var inutilizacao = new NfeInutilizacaoZeus(
                ano,
                (short)Serie,
                (int)NumeroInicial,
                (int)NumeroFinal,
                Justificativa.RemoverAcentos(),
                emissor.Cnpj
            );

            if (emissor.EmissorFiscalNfce != null)
            {
                inutilizacao.DadosServicoSefaz = emissor.EmissorFiscalNfce;
            }

            if (emissor.EmissorFiscalNfe != null)
            {
                inutilizacao.DadosServicoSefaz = emissor.EmissorFiscalNfe;
            }

            return inutilizacao;
        }

        private EmissorFiscal CarregaEmissor()
        {
            EmissorFiscal emissor;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmissorFiscal(sessao);
                emissor = repositorio.GetPeloId(EmissorSelecionado.Id);
            }

            if (EmissorSelecionado.IsNfce)
            {
                switch (_configuracaoCertificado.Tipo)
                {
                    case TipoCertificadoDigital.A1Arquivo:
                        emissor.TipoCertificadoDigital = FusionCore.FusionAdm.Fiscal.Flags.TipoCertificadoDigital.A1Arquivo;
                        break;
                    case TipoCertificadoDigital.A1Repositorio:
                        emissor.TipoCertificadoDigital = FusionCore.FusionAdm.Fiscal.Flags.TipoCertificadoDigital.A1Repositorio;
                        break;
                    case TipoCertificadoDigital.A3:
                        emissor.TipoCertificadoDigital = FusionCore.FusionAdm.Fiscal.Flags.TipoCertificadoDigital.A3;
                        break;
                }

                emissor.SerialNumberCertificado = _configuracaoCertificado.Serial;
                emissor.SenhaCertificado = _configuracaoCertificado.Senha;
                emissor.ArquivoCertificado = _configuracaoCertificado.Arquivo;
            }

            return emissor;
        }

        private void InicializarParaNovaInutilizacao()
        {
            _inutilizacao = new NfeInutilizacaoNumeracaoDTO();
            InicializarCampos();
        }
    }
}