using System;
using FusionCore.FusionAdm.Componentes;

namespace FusionCore.FusionNfce.Fiscal.TyneTypes
{
    public class TentouEm : IComponenteValorUnico<DateTime>
    {
        private readonly DateTime _valor;

        private TentouEm() { }

        public TentouEm(DateTime tentouEm)
        {
            _valor = tentouEm;
        }

        public DateTime Valor => _valor;
    }
}