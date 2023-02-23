using System.Collections.Generic;
using System.Linq;
using FusionCore.Sintegra.Dto;
using NHibernate.Util;

namespace FusionCore.FusionAdm.Sintegra
{
    public class Registro53ComprasAgrupamentoQuandoNaoTemIcms
    {
        private readonly IList<Registro53ComprasDto> _registrosNfe;

        public Registro53ComprasAgrupamentoQuandoNaoTemIcms(IList<Registro53ComprasDto> registrosNfe)
        {
            _registrosNfe = registrosNfe;
        }

        public IList<Registro53ComprasDto> Executar()
        {
            var registros53Agrupados = new List<Registro53ComprasDto>();

            _registrosNfe.Where(x => x.IsNaoTemIcms == false).ForEach(reg50 =>
            {
                registros53Agrupados.Add(new Registro53ComprasDto(reg50));
            });

            var queryGroup = from reg50 in _registrosNfe
                             where reg50.IsNaoTemIcms == true
                             group reg50 by new
                             {
                                 reg50.DocumentoUnico,
                                 reg50.InscricaoEstadual,
                                 reg50.LancamentoEm,
                                 reg50.SiglaUf,
                                 reg50.Serie,
                                 reg50.Numero,
                                 reg50.Cfop,
                                 reg50.ValorTotal,
                                 reg50.BaseCalculoIcmsSt,
                                 reg50.ChaveNfe
                             }
                into grp
                             select new Registro53ComprasDto
                             {
                                 DocumentoUnico = grp.Key.DocumentoUnico,
                                 InscricaoEstadual = grp.Key.InscricaoEstadual,
                                 LancamentoEm = grp.Key.LancamentoEm,
                                 SiglaUf = grp.Key.SiglaUf,
                                 Serie = grp.Key.Serie,
                                 Numero = grp.Key.Numero,
                                 Cfop = grp.Key.Cfop,
                                 ValorTotal = grp.Sum(x => x.ValorTotal),
                                 BaseCalculoIcmsSt = grp.Sum(x => x.BaseCalculoIcmsSt),
                                 ChaveNfe = grp.Key.ChaveNfe
                             };


            var registros50ComValorOutros = queryGroup.ToList();
            registros50ComValorOutros.ForEach(registros53Agrupados.Add);

            var listaOrdenada = registros53Agrupados.OrderBy(x => x.Numero).ThenBy(x => x.Serie).ToList();

            return listaOrdenada;
        }
    }
}