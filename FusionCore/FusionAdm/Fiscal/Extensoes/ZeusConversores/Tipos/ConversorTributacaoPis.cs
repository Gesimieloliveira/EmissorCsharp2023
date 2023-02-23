using System;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Tipos
{
    public static class ConversorTributacaoPis
    {
        private static readonly Array ZeusTipos;

        static ConversorTributacaoPis()
        {
            ZeusTipos = Enum.GetValues(typeof(CSTPIS));
        }

        public static CSTPIS ToZeus(this TributacaoPis cst)
        {
            foreach (var zeusTipo in ZeusTipos)
            {
                var padraoComparar = $"pis{cst.Id}";

                if (zeusTipo.ToString() == padraoComparar)
                {
                    return (CSTPIS) zeusTipo;
                }
            }

            throw new InvalidCastException("Não foi possível converter o CST do PIS para Zeus");
        }

        public static CSTPIS ToZeus(this NfcePis pis)
        {
            return ToZeus(new TributacaoPis {Id = pis.Id});
        }

        public static CSTPIS ToZeus(this ProdutoDTO produto)
        {
            return ToZeus(produto.Pis);
        }
    }
}