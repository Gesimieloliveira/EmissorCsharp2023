using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Volumes
{
    public class VolumeCadastrado : EventArgs
    {
        public VolumeNfe Volume { get; private set; }

        public VolumeCadastrado(VolumeNfe volume)
        {
            Volume = volume;
        }
    }

    public sealed class VolumeFormModel : ModelValidation
    {
        private readonly Nfeletronica _nfe;
        private string _especie;
        private string _numeracao;
        private string _marca;
        public ICommand CommandAdicionarVolume => GetSimpleCommand(AdicionarVolume);
        public ICommand CommandVoltar => GetSimpleCommand(Voltar);

        [Required(ErrorMessage = @"NF-e precisa de uma quantidade")]
        [Range(1, 999999999999, ErrorMessage = @"NF-e precisa de uma quantidade acima de 0")]
        public int Quantidade
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        [Required(ErrorMessage = @"Peso Bruto deve ser um número")]
        [Range(0.0, 99999999999999999.0, ErrorMessage = @"Peso Bruto não pode ser negativo")]
        public decimal PesoBruto
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }

        [Required(ErrorMessage = @"Peso Líquido deve ser um número")]
        [Range(0.0, 99999999999999999.0, ErrorMessage = @"Peso Líquido não pode ser negativo")]
        public decimal PesoLiquido
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }

        public string Especie
        {
            get { return _especie; }
            set
            {
                if (value == _especie) return;
                _especie = value;
                PropriedadeAlterada();
            }
        }

        public string Numeracao
        {
            get { return _numeracao; }
            set
            {
                if (value == _numeracao) return;
                _numeracao = value;
                PropriedadeAlterada();
            }
        }

        public string Marca
        {
            get { return _marca; }
            set
            {
                if (value == _marca) return;
                _marca = value;
                PropriedadeAlterada();
            }
        }

        public VolumeFormModel(Nfeletronica nfe)
        {
            _nfe = nfe;
        }

        public event EventHandler<VolumeCadastrado> VolumeSalvo;

        private static void Voltar(object obj)
        {
            var volumeForm = obj as VolumeForm;
            volumeForm?.Close();
        }

        private void AdicionarVolume(object obj)
        {
            try
            {
                var volume = new VolumeNfe(_nfe)
                {
                    Especie = Especie ?? string.Empty,
                    Marca = Marca ?? string.Empty,
                    Numeracao = Numeracao ?? string.Empty,
                    PesoBruto = PesoBruto,
                    PesoLiquido = PesoLiquido,
                    Quantidade = Quantidade
                };

                OnVolumeSalvar(volume);

                var volumeForm = obj as VolumeForm;
                volumeForm?.Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void OnVolumeSalvar(VolumeNfe e)
        {
            VolumeSalvo?.Invoke(this, new VolumeCadastrado(e));
        }
    }
}