using System;

namespace FusionCore.FusionAdm.PedidoVenda
{
    public struct Visitante
    {
        private string _nome;

        public static Visitante Empty => new Visitante
        {
            _nome = string.Empty,
        };

        public string Nome
        {
            get => _nome;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InvalidOperationException("Preciso de um nome válido para o visitante");
                }

                _nome = value;
            }
        }
    }
}