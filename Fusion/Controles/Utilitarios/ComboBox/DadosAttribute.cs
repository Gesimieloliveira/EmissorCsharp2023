using System;
using System.Linq;
using Fusion.Controles.Utilitarios.ComboBox.Dados;

namespace Fusion.Controles.Utilitarios.ComboBox
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    internal class DadosAttribute : Attribute
    {
        private readonly Type _provedor;

        public DadosAttribute(Type provedor)
        {
            if (provedor.GetInterfaces().All(i => i != typeof(IDadosCombobox)))
            {
                throw new ArgumentException($"{GetType().Name} espera um subtipo de {nameof(IDadosCombobox)}");
            }

            _provedor = provedor;
        }

        public IDadosCombobox CriarComportamento()
        {
            return (IDadosCombobox) Activator.CreateInstance(_provedor);
        }
    }
}