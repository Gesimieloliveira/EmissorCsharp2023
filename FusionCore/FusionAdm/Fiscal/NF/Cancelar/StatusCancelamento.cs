using System.Diagnostics.CodeAnalysis;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.FusionAdm.Fiscal.NF.Cancelar
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public sealed class StatusCancelamento
    {
        public int Codigo { get; set; }
        public string Texto { get; private set; }
        public bool EstaCancelado => Codigo == 135 || Codigo == 136 || Codigo == 151 || Codigo == 7000 || Codigo == 573 || Codigo == 155 || Codigo == 101;

        private StatusCancelamento()
        {
            //nhibernate
        }

        public StatusCancelamento(int codigo, string texto) : this()
        {
            Codigo = codigo;
            Texto = texto;
        }
    }
}