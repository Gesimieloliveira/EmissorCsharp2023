using System.Collections.Generic;
using System.Linq;
using FusionCore.Sintegra.Dto;
using NHibernate.Util;

namespace FusionCore.FusionAdm.Sintegra
{
    public class Registro53NfeAgrupamentoQuandoNaoTemIcms
    {
        private readonly IList<Registro53NfeDto> _registrosNfe;

        public Registro53NfeAgrupamentoQuandoNaoTemIcms(IList<Registro53NfeDto> registrosNfe)
        {
            _registrosNfe = registrosNfe;
        }

        public IList<Registro53NfeDto> Executar()
        {
            var registros53Agrupados = new List<Registro53NfeDto>();

            _registrosNfe.Where(x => x.IsNaoTemIcms == false).ForEach(reg50 =>
            {
                registros53Agrupados.Add(new Registro53NfeDto(reg50));
            });

            var queryGroup = from reg50 in _registrosNfe
                             where reg50.IsNaoTemIcms == true
                             group reg50 by new
                             {
                                 reg50.DocumentoUnico,
                                 reg50.InscricaoEstadual,
                                 reg50.EmitidaEm,
                                 reg50.SiglaUf,
                                 reg50.Serie,
                                 reg50.Numero,
                                 reg50.Cfop,
                                 reg50.ValorTotal,
                                 reg50.BaseCalculoSt,
                                 reg50.Cancelamento
                             }
                into grp
                             select new Registro53NfeDto
                             {
                                 DocumentoUnico = grp.Key.DocumentoUnico,
                                 InscricaoEstadual = grp.Key.InscricaoEstadual,
                                 EmitidaEm = grp.Key.EmitidaEm,
                                 SiglaUf = grp.Key.SiglaUf,
                                 Serie = grp.Key.Serie,
                                 Numero = grp.Key.Numero,
                                 Cfop = grp.Key.Cfop,
                                 ValorTotal = grp.Sum(x => x.ValorTotal),
                                 BaseCalculoSt = grp.Sum(x => x.BaseCalculoSt),
                                 Cancelamento = grp.Key.Cancelamento
                             };


            var registros50ComValorOutros = queryGroup.ToList();
            registros50ComValorOutros.ForEach(registros53Agrupados.Add);

            var listaOrdenada = registros53Agrupados.OrderBy(x => x.Numero).ThenBy(x => x.Serie).ToList();

            return listaOrdenada;
        }
    }
}