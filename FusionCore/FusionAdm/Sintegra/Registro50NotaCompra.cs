using System.Collections.Generic;
using System.Linq;
using FusionCore.Sintegra.Dto;
using NHibernate.Util;

namespace FusionCore.FusionAdm.Sintegra
{
    public class Registro50NotaCompra
    {
        private readonly IList<Registro50ComprasDto> _registrosNfe;

        public Registro50NotaCompra(IList<Registro50ComprasDto> registrosNfe)
        {
            _registrosNfe = registrosNfe;
        }

        public IList<Registro50ComprasDto> Executar()
        {
            var registros50Agrupados = new List<Registro50ComprasDto>();

            _registrosNfe.Where(x => x.IsTemValorOutros == false).ForEach(reg50 =>
            {
                registros50Agrupados.Add(new Registro50ComprasDto(reg50));
            });

            var queryGroup = from reg50 in _registrosNfe
                where reg50.IsTemValorOutros == true
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
                    reg50.BaseCalculoIcms,
                    reg50.ValorIcms,
                    reg50.Aliquota,
                    reg50.ChaveNfe
                }
                into grp
                select new Registro50ComprasDto
                {
                    DocumentoUnico = grp.Key.DocumentoUnico,
                    InscricaoEstadual = grp.Key.InscricaoEstadual,
                    LancamentoEm = grp.Key.LancamentoEm,
                    SiglaUf = grp.Key.SiglaUf,
                    Serie = grp.Key.Serie,
                    Numero = grp.Key.Numero,
                    Cfop = grp.Key.Cfop,
                    ValorTotal = grp.Sum(x => x.ValorTotal),
                    BaseCalculoIcms = grp.Sum(x => x.BaseCalculoIcms),
                    ValorIcms = grp.Sum(x => x.ValorIcms),
                    Aliquota = grp.Key.Aliquota,
                    ChaveNfe = grp.Key.ChaveNfe
                };


            var registros50ComValorOutros = queryGroup.ToList();
            registros50ComValorOutros.ForEach(registros50Agrupados.Add);

            var listaOrdenada = registros50Agrupados.OrderBy(x => x.Numero).ThenBy(x => x.Serie).ToList();

            return listaOrdenada;
        }
    }
}