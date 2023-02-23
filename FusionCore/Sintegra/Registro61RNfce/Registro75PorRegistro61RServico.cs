using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sintegra.Dto;

namespace FusionCore.Sintegra.Registro61RNfce
{
    public class Registro75PorRegistro61RServico
    {
        private readonly DateTime _filtroDataInicio;
        private readonly DateTime _filtroDataFinal;
        private readonly EmpresaDTO _daEmpresa;

        public Registro75PorRegistro61RServico(DateTime filtroDataInicio, DateTime filtroDataFinal, EmpresaDTO daEmpresa)
        {
            _filtroDataInicio = filtroDataInicio;
            _filtroDataFinal = filtroDataFinal;
            _daEmpresa = daEmpresa;
        }

        public IList<Registro75Dto> ObterRegistros75()
        {
            IList<Registro75Dto> registros75Dto;
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                registros75Dto = new RepositorioNfceAdm(sessao).BuscarRegistro75(_filtroDataInicio, _filtroDataFinal, _daEmpresa);
            }

            return registros75Dto;
        }

    }
}