using System;
using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.FusionAdm.Servico.Endereco;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using NHibernate.Util;

namespace Fusion.Visao.MdfeEletronico.Emissao
{
    public class MdfeUfEMunicipioEncerramentoFormModel : ViewModel
    {
        private ObservableCollection<EstadoDTO> _estados = new ObservableCollection<EstadoDTO>();
        private readonly LocalidadesServico _localidades = LocalidadesServico.GetInstancia();
        private EstadoDTO _estado;
        private ObservableCollection<CidadeDTO> _cidades = new ObservableCollection<CidadeDTO>();
        private CidadeDTO _cidade;
        private EstadoDTO _estadoEncerramentoPadrao;
        private CidadeDTO _cidadeEncerramentoPadrao;

        public event EventHandler<MdfeUfEMunicipioEncerramentoFormModel> EnviarEncerramentoManipulador; 

        public ObservableCollection<EstadoDTO> Estados
        {
            get => _estados;
            set
            {
                if (Equals(value, _estados)) return;
                _estados = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<CidadeDTO> Cidades
        {
            get => _cidades;
            set
            {
                if (Equals(value, _cidades)) return;
                _cidades = value;
                PropriedadeAlterada();
            }
        }

        public EstadoDTO Estado
        {
            get => _estado;
            set
            {
                if (Equals(value, _estado)) return;
                _estado = value;
                PropriedadeAlterada();
            }
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

        public void InicializaModel()
        {
            CarregarEstados();
            Estado = Estados.FirstOrDefault();

            if (_estadoEncerramentoPadrao != null)
            {
                Estado = _estadoEncerramentoPadrao;
            }

            if (_cidadeEncerramentoPadrao != null)
            {
                Cidade = _cidadeEncerramentoPadrao;
            }
        }

        public void CarregarCidadesComBaseNoEstado()
        {
            if (Estado == null)
            {
                Cidades.Clear();
                return;
            }

            Cidades = new ObservableCollection<CidadeDTO>(_localidades.GetCidades(c => c.SiglaUf == Estado.Sigla));
        }

        private void CarregarEstados()
        {
            var estados = _localidades.GetEstados();
            estados.ForEach(Estados.Add);
        }

        public void EnviarEncerramento()
        {
            if (Cidade == null)
            {
                throw new InvalidOperationException("Selecionar a Cidade de Encerramento do Manifesto");
            }

            OnFechar();
            OnEnviarEncerramentoManipulador();
        }

        protected virtual void OnEnviarEncerramentoManipulador()
        {
            EnviarEncerramentoManipulador?.Invoke(this, this);
        }

        public void AdicionarUfEMunicipioEncerramentoPadrao(EstadoDTO estadoEncerramentoPadrao, CidadeDTO cidadeEncerramentoPadrao)
        {
            _estadoEncerramentoPadrao = estadoEncerramentoPadrao;
            _cidadeEncerramentoPadrao = _localidades.GetCidade(x => x.CodigoIbge == cidadeEncerramentoPadrao.CodigoIbge);
        }
    }
}