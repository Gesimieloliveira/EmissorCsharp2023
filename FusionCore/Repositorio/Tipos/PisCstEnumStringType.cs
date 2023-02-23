using System;
using System.ComponentModel;
using FusionCore.FusionAdm.Fiscal.Extensoes.TiposImposto;
using FusionCore.FusionAdm.Fiscal.FlagsImposto;
using FusionCore.Repositorio.Exceptions;

namespace FusionCore.Repositorio.Tipos
{
    public class PisCstEnumStringType : NHibernate.Type.EnumStringType
    {
        private static Array _enumValues;

        public PisCstEnumStringType() : base(typeof (PisCst), 2)
        {
            if (_enumValues == null)
                _enumValues = Enum.GetValues(typeof (PisCst));
        }

        public override object GetValue(object code)
        {
            if (code == null)
                throw new InvalidEnumArgumentException(@"Não foi possível obter o CST correspondente ao PisCst");

            var value = ((PisCst) code).GetCodigoCst();
            return value;
        }

        public override object GetInstance(object code)
        {
            var codeComoString = (string) code;

            foreach (var enumValue in _enumValues)
            {
                var enumInstance = ((PisCst) enumValue);

                var valorEnumComoString = enumInstance.GetCodigoCst();
                if (codeComoString == valorEnumComoString)
                    return enumInstance;
            }

            throw new TipoConversaoException($"Codigo PIS não é um código válido. Código: {codeComoString}", GetType());
        }
    }
}