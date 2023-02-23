using System;
using FusionCore.FusionAdm.Fiscal.Extensoes.TiposImposto;
using FusionCore.FusionAdm.Fiscal.FlagsImposto;
using FusionCore.Repositorio.Exceptions;
using NHibernate.Type;

namespace FusionCore.Repositorio.Tipos
{
    public class CsosnCstEnumStringType : EnumStringType
    {
        private static Array _enumValues;

        public CsosnCstEnumStringType() : base(typeof (CsosnCst), 2)
        {
            if (_enumValues == null)
                _enumValues = Enum.GetValues(typeof (CsosnCst));
        }

        public override object GetValue(object code)
        {
            if (code == null)
                throw new ArgumentException(@"CsosnCst inválido para persistência");

            var value = ((CsosnCst) code).GetCodigoCst();
            return value;
        }

        public override object GetInstance(object code)
        {
            var codeComoString = (string) code;

            foreach (var enumValue in _enumValues)
            {
                var enumInstance = ((CsosnCst) enumValue);

                var valorEnumComoString = enumInstance.GetCodigoCst();
                if (codeComoString == valorEnumComoString)
                    return enumInstance;
            }

            throw new TipoConversaoException($"Codigo CSOSN não é um código válido. Código: {codeComoString}", GetType());
        }
    }
}