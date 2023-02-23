using System;
using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using SintegraBr.Classes;

namespace FusionCore.Sintegra.Registros61Nfce
{
    public class Registro61Servico
    {
        private readonly DateTime _filtroDataInicio;
        private readonly DateTime _filtroDataFinal;
        private readonly EmpresaDTO _daEmpresa;

        public Registro61Servico(DateTime filtroDataInicio, DateTime filtroDataFinal, EmpresaDTO daEmpresa)
        {
            _filtroDataInicio = filtroDataInicio;
            _filtroDataFinal = filtroDataFinal;
            _daEmpresa = daEmpresa;
        }

        public IEnumerable<Registro61> ObterRegistros61()
        {
            var itemRegistro61 = new BuscarItemsRegistro61Servico(_filtroDataInicio, _filtroDataFinal, _daEmpresa).Busca();

            var informacaoRegistro61 = new AgruparPreRegistros61PorDiaESerie(itemRegistro61).Agrupar();

            var lista = new GerarRegistros61Servico(itemRegistro61, informacaoRegistro61).Gerar();

            return lista;
        }
    }
}