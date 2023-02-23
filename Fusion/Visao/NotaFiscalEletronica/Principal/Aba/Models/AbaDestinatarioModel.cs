using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Fusion.Visao.NotaFiscalEletronica.Principal.Volumes;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.Fiscal.Contratos;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Localidade;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Aba.Models
{
    public class AbaDestintarioEventArgs : EventArgs
    {
        public AbaDestintarioEventArgs(
            DestinatarioModel destinatario,
            TransportadoraModel transportadora,
            ExportacaoModel exportacao,
            VolumesModel volumesModel
        )
        {
            Destinatario = destinatario;
            Transportadora = transportadora;
            Exportacao = exportacao;
            VolumesModel = volumesModel;
        }

        public DestinatarioModel Destinatario { get; }
        public TransportadoraModel Transportadora { get; }
        public VolumesModel VolumesModel { get; }
        public ExportacaoModel Exportacao { get; }
    }

    public class AbaDestinatarioModel : ViewModel
    {
        private Nfeletronica _nfe;

        public AbaDestinatarioModel()
        {
            DestinatarioModel = new DestinatarioModel();
            TransportadoraModel = new TransportadoraModel();

            ExportacaoModel = new ExportacaoModel();
            VolumesModel = new VolumesModel();

            DestinatarioModel.PropertyChanged += PropertyChangedHandler;
        }

        public void UsarNfe(Nfeletronica nfe)
        {
            _nfe = nfe;
            DestinatarioModel.DefinirEmitente(nfe.Emitente);
        }

        public bool Selecionado
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ObservableCollection<EstadoDTO> Estados => (ObservableCollection<EstadoDTO>)
            LocalidadesServico.GetInstancia().GetEstados();

        public ObservableCollection<Pais> Paises => (ObservableCollection<Pais>)
            LocalidadesServico.GetInstancia().GetPaises();

        public DestinatarioModel DestinatarioModel
        {
            get => GetValue<DestinatarioModel>();
            private set => SetValue(value);
        }

        public TransportadoraModel TransportadoraModel
        {
            get => GetValue<TransportadoraModel>();
            private set => SetValue(value);
        }

        public VolumesModel VolumesModel
        {
            get => GetValue<VolumesModel>();
            set => SetValue(value);
        }

        public ExportacaoModel ExportacaoModel
        {
            get => GetValue<ExportacaoModel>();
            private set => SetValue(value);
        }

        public bool SaidaExterior => DestinatarioModel?.IbgeEstado == 99;

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            PropriedadeAlterada(nameof(SaidaExterior));
        }

        public event EventHandler<AbaDestintarioEventArgs> ProximoPassoCalled;
        public event EventHandler<AbaDestintarioEventArgs> PassoAnteriorCalled;

        public void OnPassoAnteriorCalled()
        {
            var args = new AbaDestintarioEventArgs(
                DestinatarioModel,
                TransportadoraModel,
                ExportacaoModel,
                VolumesModel
            );

            PassoAnteriorCalled?.Invoke(this, args);
        }

        public void OnProximoPassoCalled()
        {
            try
            {
                ThrowExceptionSeDestinatarioInvalido();
                ThrowExceptionSeTransportadoraInvalida();

                var args = new AbaDestintarioEventArgs(
                    DestinatarioModel,
                    TransportadoraModel,
                    ExportacaoModel,
                    VolumesModel
                );

                ProximoPassoCalled?.Invoke(this, args);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void ThrowExceptionSeTransportadoraInvalida()
        {
            if (TransportadoraModel.TemTransportadora == false)
            {
                return;
            }

            if (TransportadoraModel.Endereco?.Length > 60)
            {
                throw new InvalidOperationException("O Endereço da transportadora não pode passar de 60 caracteres");
            }

            if (string.IsNullOrWhiteSpace(TransportadoraModel.SiglaEstado))
            {
                throw new InvalidOperationException("Preciso que me informe o Estado (UF) da transportadora");
            }

            if (TransportadoraModel.Cidade == null)
            {
                throw new InvalidOperationException("Preciso que me informe a Cidade da transportadora");
            }
        }

        private void ThrowExceptionSeDestinatarioInvalido()
        {
            if (DestinatarioModel.GetPessoaId() == 0)
            {
                throw new InvalidOperationException("Você precisa informar um destinatário para a NF-e");
            }

            if (string.IsNullOrWhiteSpace(DestinatarioModel.Cep))
            {
                throw new InvalidOperationException("Você precisa informar um CEP para o destinatario da NF-e");
            }

            if (string.IsNullOrWhiteSpace(DestinatarioModel.Logradouro))
            {
                throw new InvalidOperationException("Você precisa informar um Logradouro para o destinatario da NF-e");
            }

            if (string.IsNullOrWhiteSpace(DestinatarioModel.Bairro))
            {
                throw new InvalidOperationException("Você precisa informar um Bairro para o destinatario da NF-e");
            }

            if (DestinatarioModel.Estado == null)
            {
                throw new InvalidOperationException("Você precisa informar um UF para o destinatario da NF-e");
            }

            if (DestinatarioModel.Cidade == null)
            {
                throw new InvalidOperationException("Você precisa informar uma Cidade para o destinatario da NF-e");
            }
        }

        public void AdicionarVolume()
        {
            var model = new VolumeFormModel(_nfe);
            model.VolumeSalvo += VolumeSalvoHandler;

            new VolumeForm(model).ShowDialog();
        }

        private void VolumeSalvoHandler(object sender, VolumeCadastrado e)
        {
            VolumesModel.Adicionar(e.Volume);
        }

        public void RemoverVolume(IVolume volume)
        {
            VolumesModel.Selecionado = volume;
            VolumesModel.RemoverSelecionado();
        }

        public void RemoverTransportadora()
        {
            TransportadoraModel.LimparTransportadora();
        }

        public void Com(Nfeletronica nfe)
        {
            DestinatarioModel.PreecherCom(nfe);
            TransportadoraModel.PreecherCom(nfe);
            VolumesModel.PreecherCom(nfe);
            ExportacaoModel.PreecherCom(nfe);
        }

        public void Com(Cliente clienteDoPerfil)
        {
            DestinatarioModel.PreecherCom(clienteDoPerfil);
        }

        public void Com(Transportadora transportadora)
        {
            TransportadoraModel.PreecherCom(transportadora);
        }

        public void Com(Veiculo veiculo)
        {
            TransportadoraModel.PreecherCom(veiculo);
        }
    }
}