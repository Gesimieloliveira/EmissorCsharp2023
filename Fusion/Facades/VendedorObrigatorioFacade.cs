using Fusion.Sessao;
using FusionCore.Preferencias;
using System;

namespace Fusion.Facades
{
    public static class VendedorObrigatorioFacade
    {
        public const string MensagemVendedorObrigatorio = "Obrigatório informar vendedor";

        public static void ThrowExceptionSeVendedorObrigatorio()
        {
            if (VendedorEhObrigatorio())
            {
                throw new InvalidOperationException(MensagemVendedorObrigatorio);
            }

        }

        public static bool VendedorEhObrigatorio()
        {
            return SessaoSistema.Instancia.Preferencias.Obter(Preferencias.Vendedor.ObrigarUsoVendedor, false);

        }

    }
}
