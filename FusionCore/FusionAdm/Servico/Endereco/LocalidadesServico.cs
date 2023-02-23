using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.FusionAdm.Localidade;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.FusionAdm.Servico.Endereco
{
    public class LocalidadesServico
    {
        private static LocalidadesServico _instancia;
        private IList<CidadeDTO> _cidades;
        private IList<EstadoDTO> _estados;
        private IList<Pais> _paises;

        private LocalidadesServico()
        {
            //nothing
        }

        public static LocalidadesServico GetInstancia(bool load = true)
        {
            if (_instancia != null)
                return _instancia;

            _instancia = new LocalidadesServico();
            if (load) _instancia.Load();
            return _instancia;
        }

        private void Load()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                Load(sessao);
            }
        }

        private void Load(ISession sessao)
        {
            var repositorioCidade = new RepositorioCidade(sessao);
            var cidades = repositorioCidade.BuscaTodos();

            if (cidades?.Count == 0)
                throw new InvalidOperationException($"{nameof(LocalidadesServico)} não retornou cidades");

            var repositorioEstado = new RepositorioEstado(sessao);
            var estados = repositorioEstado.BuscaTodos();

            if (estados?.Count == 0)
                throw new InvalidOperationException($"{nameof(LocalidadesServico)} não retornou estados");

            var repositorioPais = new RepositorioPais(sessao);
            var paises = repositorioPais.BuscaTodos();

            if (!paises.Any())
                throw new InvalidOperationException($"{nameof(LocalidadesServico)} não retornou paises");

            _cidades = cidades;
            _estados = estados;
            _paises = paises;
        }

        public CidadeDTO GetCidade(Func<CidadeDTO, bool> predicate)
        {
            return (CidadeDTO) _cidades?.Where(predicate).FirstOrNull();
        }

        public ICollection<CidadeDTO> GetCidades()
        {
            return new ObservableCollection<CidadeDTO>(_cidades);
        }

        public ICollection<CidadeDTO> GetCidades(Func<CidadeDTO, bool> predicate)
        {
            return new ObservableCollection<CidadeDTO>(_cidades?.Where(predicate).ToList());
        }

        public EstadoDTO GetEstado(Func<EstadoDTO, bool> predicate)
        {
            return (EstadoDTO) _estados?.Where(predicate).FirstOrNull();
        }

        public ICollection<EstadoDTO> GetEstados()
        {
            return new ObservableCollection<EstadoDTO>(_estados);
        }

        public Pais GetPais(Func<Pais, bool> predicate)
        {
            return (Pais) _paises?.Where(predicate).FirstOrNull();
        }

        public ICollection<Pais> GetPaises()
        {
            return new ObservableCollection<Pais>(_paises);
        }
    }
}