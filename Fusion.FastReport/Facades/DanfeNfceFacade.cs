using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Fusion.FastReport.Relatorios.Sistema;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionNfce.Preferencias;
using FusionCore.Helpers.Basico;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace Fusion.FastReport.Facades
{
    public class DanfeNfceFacade
    {
        public void Imprimir(DanfeNfceConfiguracaoDto danfeConfiguracaoDto, IDadosParaImpressaoNfce dadosParaImpressaoNfce
        , IServicoObterXml servicoObterXml)
        {
            using (var report = danfeConfiguracaoDto.PlanoDeImpressao.DanfeNfce())
            {
                var xmlstring = servicoObterXml.ObterXml(dadosParaImpressaoNfce.Id);

                report.SegundaViaContingencia(danfeConfiguracaoDto.IsSegundaViaContingencia);
                report.ForcarSegundaVia(danfeConfiguracaoDto.SegundaViaForcada);
                report.ComXml(xmlstring, dadosParaImpressaoNfce.Cancelada, dadosParaImpressaoNfce.Logo, danfeConfiguracaoDto.NomeFantasiaCustomizado);

                if (danfeConfiguracaoDto.ImprimirFinalizacao && danfeConfiguracaoDto.NaoImprimir == false)
                    report.Imprimir(danfeConfiguracaoDto.Impressora);

                if (danfeConfiguracaoDto.PreVisualizar)
                    report.Visualizar();
            }
        }

        public void EnviarEmail(IDadosParaEnvioEmailNfce dadosParaEnvioEmailNfce
            , IEnumerable<Email> emails
            , EscreverMensagem escreverMensagem
            , IRDanfeNfce relatorio
            , IServicoObterXml obterXml
            , IConfiguracaoEmail configuracaoEmail)
        {
            var destinos = emails.Select(email => email.Valor).ToList();
            
            using (relatorio)
            using (var builder = new EmailBuilder(configuracaoEmail))
            {
                var xmlstring = obterXml.ObterXml(dadosParaEnvioEmailNfce.Id);
                var xmlstream = new MemoryStream(Encoding.UTF8.GetBytes(xmlstring));
                var pdf = new MemoryStream();

                relatorio.ComXml(xmlstring, dadosParaEnvioEmailNfce.Cancelada, dadosParaEnvioEmailNfce.Logo, escreverMensagem.NomeFantasiaCustomizado);
                relatorio.ExportarPdf(pdf);

                builder.AddDestinatarios(destinos)
                    .Assunto(escreverMensagem.Assunto)
                    .Mensagem(escreverMensagem.Mensagem)
                    .AddAnexo(xmlstream, $"xml-{dadosParaEnvioEmailNfce.NumeroChave}.xml")
                    .AddAnexo(pdf, $"pdf-{dadosParaEnvioEmailNfce.NumeroChave}.pdf");

                builder.Enviar();
            }
        }
    }
}