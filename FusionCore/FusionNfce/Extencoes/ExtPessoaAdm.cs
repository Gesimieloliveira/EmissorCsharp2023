using System;
using FusionCore.FusionNfce.Cliente;
using FusionCore.FusionNfce.Vendedores;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtPessoaAdm
    {
        public static FusionAdm.Pessoas.Cliente ToAdm(this ClienteNfce cliente)
        {
            if (cliente == null) return null;

            var type = typeof (FusionAdm.Pessoas.Cliente);

            var clienteAdm = (FusionAdm.Pessoas.Cliente) Activator.CreateInstance(type, true);

            type.GetProperty("Id").SetValue(clienteAdm, cliente.Id);

            return clienteAdm;
        }

        public static FusionAdm.Pessoas.Vendedor ToAdm(this VendedorNfce vendedor)
        {
            if (vendedor == null) return null;

            var type = typeof(FusionAdm.Pessoas.Vendedor);

            var vendedorAdm = (FusionAdm.Pessoas.Vendedor)Activator.CreateInstance(type, true);

            type.GetProperty("Id").SetValue(vendedorAdm, vendedor.Id);

            return vendedorAdm;
        }
    }
}