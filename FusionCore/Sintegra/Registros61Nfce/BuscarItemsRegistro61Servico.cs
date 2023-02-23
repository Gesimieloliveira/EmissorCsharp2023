using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sintegra.Dto;

namespace FusionCore.Sintegra.Registros61Nfce
{
    public class BuscarItemsRegistro61Servico
    {
        private readonly DateTime _filtroDataInicio;
        private readonly DateTime _filtroDataFinal;
        private readonly EmpresaDTO _daEmpresa;

        public BuscarItemsRegistro61Servico(DateTime filtroDataInicio, DateTime filtroDataFinal, EmpresaDTO daEmpresa)
        {
            _filtroDataInicio = filtroDataInicio;
            _filtroDataFinal = filtroDataFinal;
            _daEmpresa = daEmpresa;
        }

        public IList<ItemRegistro61Nfce> Busca()
        {
            IList<ItemRegistro61Nfce> itemRegistro61;
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                itemRegistro61 =
                    new RepositorioNfceAdm(sessao).BuscarRegistro61(_filtroDataInicio, _filtroDataFinal, _daEmpresa);
            }

            var informacaoRegistro61 = AgruparPorId(itemRegistro61);

            itemRegistro61 = informacaoRegistro61;

            foreach (var itemRegistro61Nfce in itemRegistro61)
            {
                itemRegistro61Nfce.AtualizarCst();
            }

            return itemRegistro61;
        }

        private static List<ItemRegistro61Nfce> AgruparPorId(IList<ItemRegistro61Nfce> itemRegistro61)
        {
            var informacaoRegistro61 = itemRegistro61.GroupBy(x => new
            {
                x.Id
            }).Select(x =>
            {
                var item = x.First();

                return new ItemRegistro61Nfce
                {
                    ValorTotal = item.ValorTotal,
                    Cst = item.Cst,
                    EmissaoNoDia = item.EmissaoNoDia,
                    NumeroFiscal = item.NumeroFiscal,
                    Serie = item.Serie
                };
            }).ToList();
            return informacaoRegistro61;
        }
    }
}