using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FusionCore.FusionAdm.Configuracoes;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.Helper.Conversores;
using FusionLibrary.VisaoModel;
using Microsoft.Win32;

namespace Fusion.Visao.Configuracao.Model
{
    public class ConfiguracaoFrenteCaixaModel : AutoSaveModel
    {
        private ConfiguracaoFrenteCaixa _configuracao;
        private bool _isBloquearVendaParaResolverPendencia;
        private bool _isSegundaViaContingencia;

        public BitmapImage Logo
        {
            get => GetValue<BitmapImage>();
            set => SetValue(value);
        }

        public ICommand SelecionaLogoCommand => GetSimpleCommand(SelecionaLogoAction);

        public bool IsBloquearVendaParaResolverPendencia
        {
            get => _isBloquearVendaParaResolverPendencia;
            set
            {
                if (value == _isBloquearVendaParaResolverPendencia) return;
                _isBloquearVendaParaResolverPendencia = value;
                PropriedadeAlterada();
            }
        }

        public decimal? ValorMinimoParaForcarClienteNaVenda
        {
            get => GetValue<decimal?>();
            set => SetValue(value);
        }

        public bool IsSegundaViaContingencia
        {
            get => _isSegundaViaContingencia;
            set
            {
                _isSegundaViaContingencia = value;
                PropriedadeAlterada();
            }
        }

        private void SelecionaLogoAction(object obj)
        {
            var janelaArquivo = new OpenFileDialog
            {
                Filter = "Logo Frente Caixa(*.png)| *.png"
            };

            if (janelaArquivo.ShowDialog() == true)
            {
                Logo = new BitmapImage(new Uri(janelaArquivo.FileName));
            }
        }

        protected override void OnInicializa()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioConfiguracaoFrenteCaixa(sessao);
                _configuracao = repositorio.BuscarUnica();
            }

            if (_configuracao == null)
            {
                return;
            }

            Logo = ConverteImage.ByteEmImagem(_configuracao.Logo);
            ValorMinimoParaForcarClienteNaVenda = _configuracao.ValorMinimoParaForcarClienteNaVenda;
            IsBloquearVendaParaResolverPendencia = _configuracao.IsBloquearVendaParaResolverPendencia;
            IsSegundaViaContingencia = _configuracao.IsSegundaViaContingencia;
        }

        protected override void OnSalvaAlteracoes()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioConfiguracaoFrenteCaixa(sessao);
                var imagemEmByte = ConverteImage.ImagemEmByte(Logo, new PngBitmapEncoder());

                _configuracao.Logo = imagemEmByte;
                _configuracao.IsBloquearVendaParaResolverPendencia = IsBloquearVendaParaResolverPendencia;
                _configuracao.ValorMinimoParaForcarClienteNaVenda = ValorMinimoParaForcarClienteNaVenda;
                _configuracao.IsSegundaViaContingencia = IsSegundaViaContingencia;

                repositorio.Salvar(_configuracao);
            }
        }
    }
}