using System;
using System.Collections.ObjectModel;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model
{
    public class SalvarPercuroEventArgs : EventArgs
    {
        public SalvarPercuroEventArgs(FlyoutAddPercursoModel model)
        {
            Model = model;
        }

        public FlyoutAddPercursoModel Model { get; set; }
    }

    public class FlyoutAddPercursoModel : ViewModel
    {
        private ObservableCollection<EstadoDTO> _estadosPercurso;
        private EstadoDTO _estadoPercurso;
        private bool _isOpen;

        public event EventHandler<SalvarPercuroEventArgs> SalvarPercursoHandler;

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (value == _isOpen) return;
                _isOpen = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<EstadoDTO> EstadosPercurso
        {
            get { return _estadosPercurso; }
            set
            {
                _estadosPercurso = value;
                PropriedadeAlterada();
            }
        }

        public EstadoDTO EstadoPercurso
        {
            get { return _estadoPercurso; }
            set
            {
                _estadoPercurso = value;
                PropriedadeAlterada();
            }
        }

        public FlyoutAddPercursoModel()
        {
            EstadosPercurso = new ObservableCollection<EstadoDTO>(LocalidadesServico.GetInstancia(false).GetEstados());

        }

        public void LimparCampos()
        {
            EstadoPercurso = null;
        }

        public void SalvarPercurso()
        {
            Validar();

            OnSalvarPercursoHandle();
            LimparCampos();
        }

        private void Validar()
        {
            if (EstadoPercurso == null) throw new ArgumentException("Estado(UF) é obrigatório");
        }

        protected virtual void OnSalvarPercursoHandle()
        {
            SalvarPercursoHandler?.Invoke(this, new SalvarPercuroEventArgs(this));
        }
    }
}