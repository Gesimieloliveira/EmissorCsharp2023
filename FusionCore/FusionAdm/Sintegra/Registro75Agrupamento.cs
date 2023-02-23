using System.Collections.Generic;
using System.Linq;
using SintegraBr.Classes;

namespace FusionCore.FusionAdm.Sintegra
{
    public class Registro75Agrupamento
    {
        private readonly List<Registro75> _registros75A;

        public Registro75Agrupamento(List<Registro75> registros75a)
        {
            _registros75A = registros75a;
        }

        public List<Registro75> Executar()
        {
            var queryGroup = from reg75 in _registros75A
                group reg75 by new
                {
                    reg75.CodItem,
                    reg75.CodNcm,
                    reg75.Descricao,
                    reg75.UnidadeMedida,
                    reg75.AliquotaIpi,
                    reg75.AliquotaIcms,
                    reg75.ReducaoBaseIcms,
                    reg75.BaseCalculoSt,
                    reg75.DataInicial,
                    reg75.DataFinal
                }
                into grp
                select new Registro75
                {
                    CodItem = grp.Key.CodItem,
                    CodNcm = grp.Key.CodNcm,
                    Descricao = grp.Key.Descricao,
                    UnidadeMedida = grp.Key.UnidadeMedida,
                    AliquotaIpi = grp.Key.AliquotaIpi,
                    AliquotaIcms = grp.Key.AliquotaIcms,
                    ReducaoBaseIcms = grp.Key.ReducaoBaseIcms,
                    BaseCalculoSt = grp.Key.BaseCalculoSt,
                    DataInicial = grp.Key.DataInicial,
                    DataFinal = grp.Key.DataFinal
                };

            var registros75Agrupados = queryGroup.ToList();

            return registros75Agrupados;
        }
    }
}