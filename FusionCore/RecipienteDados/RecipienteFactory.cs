using System;
using System.Collections.Generic;

namespace FusionCore.RecipienteDados
{
    public static class RecipienteFactory
    {
        private static readonly Dictionary<string, IRecipiente> Instancias = new Dictionary<string, IRecipiente>();

        public static T Get<T>() where T  : IRecipiente
        {
            var type = typeof(T);
            var fullName = type.FullName;

            if (Instancias.ContainsKey(fullName))
            {
                var recipiente = Instancias[fullName];

                if (!recipiente.ManterCache)
                {
                    recipiente.RecarregaCache();
                }

                return (T) recipiente;
            }

            var instancia = Activator.CreateInstance(type) as IRecipiente;

            if (instancia == null)
            {
                throw new InvalidOperationException("Falha ao instanciar Recipiente: " + fullName);
            }

            instancia.RecarregaCache();

            Instancias.Add(fullName, instancia);
            return (T) Instancias[fullName];
        }
    }
}
