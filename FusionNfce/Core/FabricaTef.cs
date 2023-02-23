using System.IO;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.FusionNfce.Tef;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.EmpresaDesenvolvedora;
using Tef.Dominio;
using Tef.Infra;

namespace FusionNfce.Core
{
    public static class FabricaTef
    {
        public static ITef ObterTefDial(IAcTefRequisicao acTefRequisicao)
        {
            var configTef = SessaoSistemaNfce.ConfigTef;

            return FabricaOperadora.RetornaOperadora(configTef.Operadora.ToAcTef(),
                acTefRequisicao,
                new ConfigAcTefDial(
                    ResponsavelLegal.NomeAplicacaoNfce,
                    ResponsavelLegal.VersaoSistema,
                    ResponsavelLegal.RazaoSocial,
                    configTef.RegistroCertificado,
                    true,
                    true,
                    true,
                    true
                ));
        }

        public static IAcTefRequisicao ObterRequisicao()
        {
            var configTef = SessaoSistemaNfce.ConfigTef;

            return new AcTefRequisicao(new ConfigRequisicao(7, 250, 
                configTef.ArquivoTemporario,
                configTef.ArquivoRequisicao, 
                configTef.ArquivoResposta, 
                configTef.ArquivoSts,
                Path.Combine(ManipulaPasta.LocalSistema(), "TEF")));
        }
    }
}