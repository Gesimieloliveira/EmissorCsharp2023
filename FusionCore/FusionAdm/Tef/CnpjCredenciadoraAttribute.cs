using System;

namespace FusionCore.FusionAdm.Tef
{
    public class CnpjCredenciadoraAttribute : Attribute
    {
        public string Cnpj { get; }

        public string Codigo { get; }

        public CnpjCredenciadoraAttribute(string cnpj, string codigo)
        {
            Cnpj = cnpj;
            Codigo = codigo;
        }
    }
}