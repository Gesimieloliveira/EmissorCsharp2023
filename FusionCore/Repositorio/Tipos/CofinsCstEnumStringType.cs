using System;
using System.ComponentModel;
using FusionCore.FusionAdm.Fiscal.Extensoes.TiposImposto;
using FusionCore.FusionAdm.Fiscal.FlagsImposto;
using FusionCore.Repositorio.Exceptions;

namespace FusionCore.Repositorio.Tipos
{
    public class CofinsCstEnumStringType : NHibernate.Type.EnumStringType
    {
        private static Array _enumValues;

        public CofinsCstEnumStringType() : base(typeof (CofinsCst), 2)
        {
            if (_enumValues == null)
                _enumValues = Enum.GetValues(typeof (CofinsCst));
        }

        public override object GetValue(object code)
        {
            if (code == null)
                throw new InvalidEnumArgumentException(@"Não foi possível obter o CST correspondente ao CofinsCst");

            var value = ((CofinsCst) code).GetCodigoCst();
            return value;
        }

        public override object GetInstance(object code)
        {
            var codeComoString = (string) code;

            foreach (var enumValue in _enumValues)
            {
                var enumInstance = ((CofinsCst) enumValue);

                var valorEnumComoString = enumInstance.GetCodigoCst();
                if (codeComoString == valorEnumComoString)
                    return enumInstance;
            }

            throw new TipoConversaoException($"Codigo COFINS não é um código válido. Código: {codeComoString}", GetType());
        }

    }
}