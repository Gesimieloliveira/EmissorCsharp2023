using System;
using FusionCore.FusionAdm.Fiscal.FlagsImposto;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class MotivoDesoneracaoIcmsExt
    {
        public static NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.MotivoDesoneracaoIcms ToZeus(this MotivoDesoneracaoIcms motivo)
        {
            switch (motivo)
            {
                case MotivoDesoneracaoIcms.Outros:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.MotivoDesoneracaoIcms.MdiOutros;
                case MotivoDesoneracaoIcms.DeficienteCondutor:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.MotivoDesoneracaoIcms.MdiDeficienteCondutor;
                case MotivoDesoneracaoIcms.DeficienteFisico:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.MotivoDesoneracaoIcms.MdiDeficienteFisico;
                case MotivoDesoneracaoIcms.DeficienteNaoCondutor:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.MotivoDesoneracaoIcms.MdiDeficienteNaoCondutor;
                case MotivoDesoneracaoIcms.DiplomaticoOuConsultar:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.MotivoDesoneracaoIcms.MdiDiplomaticoConsular;
                case MotivoDesoneracaoIcms.FrotistaOuLocadora:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.MotivoDesoneracaoIcms.MdiFrotistaLocadora;
                case MotivoDesoneracaoIcms.Suframa:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.MotivoDesoneracaoIcms.MdiSuframa;
                case MotivoDesoneracaoIcms.Taxi:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.MotivoDesoneracaoIcms.MdiTaxi;
                case MotivoDesoneracaoIcms.UsoNaAgropecuaria:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.MotivoDesoneracaoIcms.MdiProdutorAgropecuario;
                case MotivoDesoneracaoIcms.UtilitariosDaAmazoniaOcidental:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.MotivoDesoneracaoIcms.MdiAmazoniaLivreComercio;
                case MotivoDesoneracaoIcms.VendaAOrgaoPublico:
                    return NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos.MotivoDesoneracaoIcms.MdiVendaOrgaosPublicos;
            }

            throw new InvalidCastException("Conversão de tipo para Zeus Inválida");
        }
    }
}