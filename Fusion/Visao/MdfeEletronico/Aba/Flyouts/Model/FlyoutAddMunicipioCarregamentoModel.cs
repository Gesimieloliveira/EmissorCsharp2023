using System;
using System.Windows.Input;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;
using FusionWPF.FusionAdm.Cidades;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model
{
    public class SalvarMunicipioCarregamentoEventArgs : EventArgs
    {
        public SalvarMunicipioCarregamentoEventArgs(FlyoutAddMunicipioCarregamentoModel model)
        {
            Model = model;
        }

        public FlyoutAddMunicipioCarregamentoModel Model { get; set; }
    }

    public sealed class FlyoutAddMunicipioCarregamentoModel : ViewModel
    {
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (value == _isOpen) return;
                _isOpen = value;
                PropriedadeAlterada();
            }
        }

        private CidadeDTO _cidade;
        private bool _isOpen;

        public ICommand CommandBuscarCidade => GetSimpleCommand(BuscarCidade);

        public event EventHandler<SalvarMunicipioCarregamentoEventArgs> SalvarMunicipioCarregamentoHandler; 

        private void BuscarCidade(object obj)
        {
            var model = new CidadePickerModel();
            model.PickItemEvent += SelecionaCidadeCompleted;
            model.ShowPickerDialog();
        }

        public CidadeDTO Cidade
        {
            get => _cidade;
            set
            {
                if (Equals(value, _cidade)) return;
                _cidade = value;
                PropriedadeAlterada();
            }
        }

        private void SelecionaCidadeCompleted(object sender, GridPickerEventArgs e)
        {
            Cidade = e.GetItem<CidadeDTO>();
        }

        public void SalvarMunicipioCarregamento()
        {
            Validar();
            OnSalvarMunicipioCarregamentoHandler();
            LimpaCampos();
        }

        private void Validar()
        {
            if (Cidade == null) throw new ArgumentException("Adicionar uma cidade");
        }

        public void LimpaCampos()
        {
            Cidade = null;
        }

        private void OnSalvarMunicipioCarregamentoHandler()
        {
            SalvarMunicipioCarregamentoHandler?.Invoke(this, new SalvarMunicipioCarregamentoEventArgs(this));
        }
    }
}