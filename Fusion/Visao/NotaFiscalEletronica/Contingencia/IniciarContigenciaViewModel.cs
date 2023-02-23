using System;
using System.Collections.ObjectModel;
using FusionCore.FusionAdm.ContingenciaSefaz;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

// ReSharper disable ClassNeverInstantiated.Global

namespace Fusion.Visao.NotaFiscalEletronica.Contingencia
{
    public class IniciarContigenciaViewModel : ViewModel
    {
        public DateTime IniciadoEm
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public TipoEmissao? ServidorEmissao
        {
            get { return GetValue<TipoEmissao>(); }
            set { SetValue(value); }
        }

        public string Justificativa
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool IsOpen
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public EmissorFiscalComboBox EmissorUtilizar
        {
            get { return GetValue<EmissorFiscalComboBox>(); }
            set { SetValue(value); }
        }

        public ObservableCollection<EmissorFiscalComboBox> EmissoresNfe { get; set; }

        public IniciarContigenciaViewModel(bool isOpen = false)
        {
            IsOpen = isOpen;
            IniciadoEm = DateTime.Now;
            EmissoresNfe = new ObservableCollection<EmissorFiscalComboBox>();
        }

        public event EventHandler<ContingenciaNfe> IniciadaNovaContingencia;

        public void Inicializar()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioEmissorFiscal(sessao);
                var emissores = repositorio.BuscaTodosQueSejamNfeParaComboBox();

                EmissoresNfe.Clear();
                emissores?.ForEach(EmissoresNfe.Add);
            }
        }

        public void IniciarContingencia()
        {
            try
            {
                if (EmissorUtilizar == null)
                    throw new InvalidOperationException("Preciso que informe qual emissor utilizar.");

                if (ServidorEmissao == null)
                    throw new InvalidOperationException("Preciso que informe um servidor de contingência.");

                if (Justificativa?.Length < 15)
                    throw new InvalidOperationException(
                        "Preciso que informe uma justificativa com no mínimo 15 caracteres.");

                ContingenciaNfe contingencia;

                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorio = new RepositorioContingenciaNfe(sessao);
                    var aberta = repositorio.ContingenciaAberta(EmissorUtilizar.Id);

                    if (aberta != null)
                    {
                        throw new InvalidOperationException("Já existe uma aberta para esse emissor");
                    }

                    var repoEmissor = new RepositorioEmissorFiscal(sessao);
                    var emissor = repoEmissor.GetPeloId(EmissorUtilizar.Id);

                    contingencia = new ContingenciaNfe(emissor, (TipoEmissao) ServidorEmissao, Justificativa);
                    repositorio.Persistir(contingencia);
                }

                DialogBox.MostraInformacao("Contingência inicada com sucesso!");
                IniciadaNovaContingencia?.Invoke(this, contingencia);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
            catch (Exception e)
            {
                DialogBox.MostraErro(e.Message, e);
            }
        }
    }
}