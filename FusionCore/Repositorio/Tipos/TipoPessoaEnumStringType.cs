using System;
using FusionCore.FusionAdm.Pessoas;
using NHibernate.Type;

namespace FusionCore.Repositorio.Tipos
{
    public class TipoPessoaEnumStringType : EnumStringType
    {
        private static Array _enumValues;

        public TipoPessoaEnumStringType() : base(typeof (PessoaTipo), 1)
        {
            if (_enumValues == null) _enumValues = Enum.GetValues(typeof (PessoaTipo));
        }

        public override object GetValue(object code)
        {
            return code == null ? null : ((PessoaTipo) code).GetCodigo();
        }

        public override object GetInstance(object code)
        {
            var codeComoString = (string) code;

            foreach (var enumValue in _enumValues)
            {
                var enumInstance = ((PessoaTipo) enumValue);

                var valorEnumComoString = enumInstance.GetCodigo();
                if (codeComoString == valorEnumComoString)
                    return enumInstance;
            }

            return null;
        }
    }
}