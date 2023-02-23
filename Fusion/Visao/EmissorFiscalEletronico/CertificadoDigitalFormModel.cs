using System;
using System.Windows.Input;
using DFe.Utils;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.Command;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using Microsoft.Win32;

namespace Fusion.Visao.EmissorFiscalEletronico
{
    public class CertificadoDigitalFormModel : ViewModel
    {
        private TipoCertificadoDigital _tipoCertificadoDigital = TipoCertificadoDigital.A1Arquivo;
        private bool _isCertificadoA3;
        private bool _isCertificadoA1Arquivo;
        private bool _isCertificadoSenha;
        private ICommand _commandBuscaCertificado;
        private string _arquivoCertificado;
        private ICommand _commandLimpaArquivoCertificado;
        private ICommand _commandBuscaCertificadoRepositorio;
        private string _serialNumberCertificado;
        private ICommand _commandLimpaSerialNumberCertificado;
        private string _senhaCertificado;

        public TipoCertificadoDigital TipoCertificadoDigital
        {
            get => _tipoCertificadoDigital;
            set
            {
                if (value == _tipoCertificadoDigital) return;
                _tipoCertificadoDigital = value;
                PropriedadeAlterada();

                SelecionaCamposApartirDoTipoCertificado(value);
            }
        }

        public bool IsCertificadoA3
        {
            get => _isCertificadoA3;
            set
            {
                if (value == _isCertificadoA3) return;
                _isCertificadoA3 = value;
                PropriedadeAlterada();
            }
        }

        public bool IsCertificadoA1Arquivo
        {
            get => _isCertificadoA1Arquivo;
            set
            {
                if (value == _isCertificadoA1Arquivo) return;
                _isCertificadoA1Arquivo = value;
                PropriedadeAlterada();
            }
        }

        public bool IsCertificadoSenha
        {
            get => _isCertificadoSenha;
            set
            {
                if (value == _isCertificadoSenha) return;
                _isCertificadoSenha = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandConfirmar => GetSimpleCommand(ConfirmarAction);

        public ICommand CommandBuscaCertificado
        {
            get
            {
                return _commandBuscaCertificado ??
                       (_commandBuscaCertificado = new SimpleCommand
                       {
                           CanExecuteDelegate = x => true,
                           ExecuteDelegate = x =>
                           {
                               var janelaArquivo = new OpenFileDialog
                               {
                                   Filter = "Certificado digital(*.pfx)|*.pfx"
                               };
                               if (janelaArquivo.ShowDialog() == true)
                               {
                                   ArquivoCertificado = janelaArquivo.FileName;
                               }
                           }
                       });
            }
        }

        public ICommand CommandLimpaArquivoCertificado
        {
            get
            {
                return _commandLimpaArquivoCertificado ??
                       (_commandLimpaArquivoCertificado = new SimpleCommand
                       {
                           CanExecuteDelegate = x => true,
                           ExecuteDelegate = x => { ArquivoCertificado = null; }
                       });
            }
        }

        public string ArquivoCertificado
        {
            get => _arquivoCertificado;
            set
            {
                if (value == _arquivoCertificado) return;
                _arquivoCertificado = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandBuscaCertificadoRepositorio
        {
            get
            {
                return _commandBuscaCertificadoRepositorio ??
                       (_commandBuscaCertificadoRepositorio = new SimpleCommand
                       {
                           CanExecuteDelegate = x => true,
                           ExecuteDelegate = x =>
                           {
                               try
                               {
                                   var cert = CertificadoDigitalUtils.ListareObterDoRepositorio();
                                   SerialNumberCertificado = cert.SerialNumber?.Trim();
                               }
                               catch (Exception ex)
                               {
                                   DialogBox.MostraInformacao(ex.Message);
                               }
                           }
                       });
            }
        }

        public string SerialNumberCertificado
        {
            get => _serialNumberCertificado;
            set
            {
                if (value == _serialNumberCertificado) return;
                _serialNumberCertificado = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandLimpaSerialNumberCertificado
        {
            get
            {
                return _commandLimpaSerialNumberCertificado ??
                       (_commandLimpaSerialNumberCertificado = new SimpleCommand
                       {
                           CanExecuteDelegate = x => true,
                           ExecuteDelegate = x => { SerialNumberCertificado = null; }
                       });
            }
        }

        public string SenhaCertificado
        {
            get => _senhaCertificado;
            set
            {
                if (value == _senhaCertificado) return;
                _senhaCertificado = value;
                PropriedadeAlterada();
            }
        }

        public CertificadoDigitalFormModel()
        {
            SelecionaCamposApartirDoTipoCertificado(TipoCertificadoDigital);
        }


        public event EventHandler FecharTela;

        public event EventHandler<ConfiguracaoCertificadoDigital> ConfirmarConfiguracaoCertificadoDigital;

        public void SelecionaCamposApartirDoTipoCertificado(TipoCertificadoDigital value)
        {
            switch (value)
            {
                case TipoCertificadoDigital.A1Arquivo:
                    IsCertificadoA1Arquivo = true;
                    IsCertificadoSenha = true;
                    IsCertificadoA3 = false;
                    LimpaCampos();
                    break;

                case TipoCertificadoDigital.A1Repositorio:
                    IsCertificadoA1Arquivo = false;
                    IsCertificadoSenha = false;
                    IsCertificadoA3 = true;
                    LimpaCampos();
                    break;

                case TipoCertificadoDigital.A3:
                    IsCertificadoA1Arquivo = false;
                    IsCertificadoSenha = true;
                    IsCertificadoA3 = true;
                    LimpaCampos();
                    break;
            }
        }

        private void LimpaCampos()
        {
            ArquivoCertificado = string.Empty;
            SerialNumberCertificado = string.Empty;
            SenhaCertificado = string.Empty;
        }

        private void ConfirmarAction(object obj)
        {
            try
            {
                Validacoes();

                OnConfirmarConfiguracaoCertificadoDigital(new ConfiguracaoCertificadoDigital
                {
                    Arquivo = ArquivoCertificado,
                    Senha = SimetricaCrip.Computar(SenhaCertificado),
                    Serial = SimetricaCrip.Computar(SerialNumberCertificado),
                    Tipo = TipoCertificadoDigital
                });

                OnFecharTela();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void Validacoes()
        {
            if (TipoCertificadoDigital == TipoCertificadoDigital.A1Arquivo)
            {
                if (ArquivoCertificado.IsNullOrEmpty())
                    throw new InvalidOperationException("Selecionar um arquivo de certificado digital");

                if (SenhaCertificado.IsNullOrEmpty())
                    throw new InvalidOperationException("Digitar a senha do certificado digital");
            }

            if (TipoCertificadoDigital == TipoCertificadoDigital.A1Repositorio)
            {
                if (SerialNumberCertificado.IsNullOrEmpty())
                    throw new InvalidOperationException("Selecionar um certificado digital no repositório");
            }

            if (TipoCertificadoDigital == TipoCertificadoDigital.A3)
            {
                if (SerialNumberCertificado.IsNullOrEmpty())
                    throw new InvalidOperationException("Selecionar um certificado digital no repositório");
            }
        }

        protected virtual void OnFecharTela()
        {
            FecharTela?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnConfirmarConfiguracaoCertificadoDigital(ConfiguracaoCertificadoDigital e)
        {
            ConfirmarConfiguracaoCertificadoDigital?.Invoke(this, e);
        }
    }
}