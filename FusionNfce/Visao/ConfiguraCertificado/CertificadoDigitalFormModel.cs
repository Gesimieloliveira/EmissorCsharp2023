using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DFe.Utils;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.CertificadosDigitais;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Sessao;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using Microsoft.Win32;

namespace FusionNfce.Visao.ConfiguraCertificado
{
    public class CertificadoDigitalFormModel : ViewModel
    {
        private ObservableCollection<EmpresaComboBoxDTO> _empresas;
        private TipoCertificadoDigital _tipoCertificadoDigital;
        private bool _isCertificadoA1Arquivo;
        private bool _isCertificadoA3;
        private bool _isCertificadoSenha;
        private EmpresaComboBoxDTO _empresaSelecionada;
        private string _arquivoCertificado;
        private string _serialNumberCertificado;
        private string _senhaCertificado;
        private CertificadoDigitalNfceFacade _facade;
        private CertificadoDigitalNfce _certificado;

        public ICommand CommandBuscaCertificado => GetSimpleCommand(BuscaCertificadoDigitalAcao);
        public ICommand CommandLimpaArquivoCertificado => GetSimpleCommand(LimpaArquivoCertificadoAcao);
        public ICommand CommandBuscaCertificadoRepositorio => GetSimpleCommand(BuscaCertificadoRepositorioAcao);
        public ICommand CommandLimpaSerialNumberCertificado => GetSimpleCommand(LimpaSerialNumberCertificadoAcao);
        public ICommand CommandConfirmar => GetSimpleCommand(ConfirmarAcao);


        public ObservableCollection<EmpresaComboBoxDTO> Empresas
        {
            get => _empresas;
            set
            {
                if (Equals(value, _empresas)) return;
                _empresas = value;
                PropriedadeAlterada();
            }
        }

        public EmpresaComboBoxDTO EmpresaSelecionada
        {
            get => _empresaSelecionada;
            set
            {
                _empresaSelecionada = value;
                PropriedadeAlterada();
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

        public TipoCertificadoDigital TipoCertificadoDigital
        {
            get => _tipoCertificadoDigital;
            set
            {
                _tipoCertificadoDigital = value;

                IsCertificadoA1Arquivo = TipoCertificadoDigital == TipoCertificadoDigital.A1Arquivo;
                IsCertificadoA3 = TipoCertificadoDigital == TipoCertificadoDigital.A1Repositorio ||
                                  TipoCertificadoDigital == TipoCertificadoDigital.A3;
                IsCertificadoSenha = TipoCertificadoDigital == TipoCertificadoDigital.A1Arquivo ||
                                     TipoCertificadoDigital == TipoCertificadoDigital.A3;

                LimpaCampos();

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

        public void Inicializa()
        {
            using (var sessao = new SessaoManagerNfce().CriaSessao())
            {
                Empresas = new ObservableCollection<EmpresaComboBoxDTO>(new RepositorioEmpresaNfce(sessao).BuscarParaComboBox());
            }

            TipoCertificadoDigital = TipoCertificadoDigital.A1Repositorio;
            EmpresaSelecionada = Empresas.FirstOrDefault(x => x.Id == SessaoSistemaNfce.Empresa().Id) ?? Empresas.FirstOrDefault();

            _facade = new CertificadoDigitalNfceFacade();
            _certificado = _facade.CarregarPorEmpresa(SessaoSistemaNfce.Empresa()) ?? new CertificadoDigitalNfce
            {
                Empresa = SessaoSistemaNfce.Empresa()
            };

            TipoCertificadoDigital = _certificado.Tipo;
            ArquivoCertificado = _certificado.CaminhoArquivo;
            SerialNumberCertificado = SimetricaCrip.Descomputar(_certificado.SerialRepositorio);
            SenhaCertificado = SimetricaCrip.Descomputar(_certificado.Senha);
        }

        private void BuscaCertificadoDigitalAcao(object obj)
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

        private void LimpaArquivoCertificadoAcao(object obj)
        {
            ArquivoCertificado = string.Empty;
        }

        private void BuscaCertificadoRepositorioAcao(object obj)
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

        private void LimpaSerialNumberCertificadoAcao(object obj)
        {
            SerialNumberCertificado = string.Empty;
        }

        private void LimpaCampos()
        {
            ArquivoCertificado = string.Empty;
            SerialNumberCertificado = string.Empty;
            SenhaCertificado = string.Empty;
        }

        private void ConfirmarAcao(object obj)
        {
            try
            {
                if (EmpresaSelecionada == null)
                    throw new InvalidOperationException("Preciso que selecione uma empresa");

                ValidaCertificadoArquivo();
                ValidaCertificadoRepositorio();
                ValidaCertificadoA3();

                _certificado.CaminhoArquivo = ArquivoCertificado.TrimOrEmpty();
                _certificado.SerialRepositorio = SerialNumberCertificado.TrimOrEmpty();
                _certificado.Senha = SenhaCertificado.TrimOrEmpty();
                _certificado.Tipo = TipoCertificadoDigital;

                if (_certificado.SerialRepositorio.IsNotNullOrEmpty())
                {
                    _certificado.SerialRepositorio = SimetricaCrip.Computar(_certificado.SerialRepositorio);
                }

                if (_certificado.Senha.IsNotNullOrEmpty())
                {
                    _certificado.Senha = SimetricaCrip.Computar(_certificado.Senha);
                }

                _facade.Salva(_certificado);

                SessaoSistemaNfce.CertificadoDigital = _certificado;

                OnFechar();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void ValidaCertificadoA3()
        {
            if (TipoCertificadoDigital == TipoCertificadoDigital.A3)
            {
                if (SerialNumberCertificado.IsNullOrEmpty())
                    throw new InvalidOperationException("Preciso que selecione um certificado digital");
            }
        }

        private void ValidaCertificadoRepositorio()
        {
            if (TipoCertificadoDigital == TipoCertificadoDigital.A1Repositorio)
                if (SerialNumberCertificado.IsNullOrEmpty())
                    throw new InvalidOperationException("Preciso que selecione um certificado digital");
        }

        private void ValidaCertificadoArquivo()
        {
            if (TipoCertificadoDigital == TipoCertificadoDigital.A1Arquivo)
            {
                if (ArquivoCertificado.IsNullOrEmpty())
                    throw new InvalidOperationException("Preciso que selecione um certificado digital (arquivo)");

                if (SenhaCertificado.IsNullOrEmpty())
                    throw new InvalidOperationException("Preciso de uma senha");
            }
        }
    }
}