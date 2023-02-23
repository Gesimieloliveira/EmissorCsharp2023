using System;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Tipos
{
    public static class ConversorTributacaoCofins
    {
        private static readonly Array ZeusTipos;

        static ConversorTributacaoCofins()
        {
            ZeusTipos = Enum.GetValues(typeof(CSTCOFINS));
        }

        public static CSTCOFINS ToZeus(this TributacaoCofins cst)
        {
            foreach (var zeusTipo in ZeusTipos)
            {
                var padraoComparar = $"cofins{cst.Id}";

                if (zeusTipo.ToString() == padraoComparar)
                    return (CSTCOFINS) zeusTipo;
            }

            throw new InvalidCastException("Não foi possível converter o COFINS para um COFINS do Zeus");
        }

        public static CSTCOFINS ToZeus(this NfceCofins pis)
        {
            return ToZeus(new TributacaoCofins { Id = pis.Id });
        }

        public static CSTCOFINS ToZeusCofins(this ProdutoDTO produto)
        {
            return ToZeus(produto.Cofins);
        }
    }
}