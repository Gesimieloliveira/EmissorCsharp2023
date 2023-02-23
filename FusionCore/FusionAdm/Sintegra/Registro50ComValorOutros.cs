using System.Collections.Generic;
using System.Linq;
using FusionCore.Sintegra.Dto;
using NHibernate.Util;

namespace FusionCore.FusionAdm.Sintegra
{
    public class Registro50ComValorOutros
    {
        private readonly IList<Registro50NfeDto> _registrosNfe;

        public Registro50ComValorOutros(IList<Registro50NfeDto> registrosNfe)
        {
            _registrosNfe = registrosNfe;
        }

        public IList<Registro50NfeDto> Executar()
        {
            var registros50Agrupados = new List<Registro50NfeDto>();

            _registrosNfe.Where(x => x.IsTemValorOutros == false).ForEach(reg50 =>
            {
                registros50Agrupados.Add(new Registro50NfeDto(reg50));
            });

            var queryGroup = from reg50 in _registrosNfe
                             where reg50.IsTemValorOutros == true
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
                    reg50.BaseCalculo,
                    reg50.ValorIcms,
                    reg50.AliquotaIcms,
                    reg50.Cancelamento
                }
                into grp
                select new Registro50NfeDto
                {
                    DocumentoUnico = grp.Key.DocumentoUnico,
                    InscricaoEstadual = grp.Key.InscricaoEstadual,
                    EmitidaEm = grp.Key.EmitidaEm,
                    SiglaUf = grp.Key.SiglaUf,
                    Serie = grp.Key.Serie,
                    Numero = grp.Key.Numero,
                    Cfop = grp.Key.Cfop,
                    ValorTotal = grp.Sum(x => x.ValorTotal),
                    BaseCalculo = grp.Sum(x => x.BaseCalculo),
                    ValorIcms = grp.Sum(x => x.ValorIcms),
                    AliquotaIcms = grp.Key.AliquotaIcms,
                    Cancelamento = grp.Key.Cancelamento
                };


            var registros50ComValorOutros = queryGroup.ToList();
            registros50ComValorOutros.ForEach(registros50Agrupados.Add);

            var listaOrdenada = registros50Agrupados.OrderBy(x => x.Numero).ThenBy(x => x.Serie).ToList();

            return listaOrdenada;
        }
    }
}