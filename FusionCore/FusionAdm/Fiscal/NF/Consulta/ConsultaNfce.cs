using System;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Fiscal.Flags;
using NFe.Classes.Servicos.Consulta;
using NFe.Classes.Servicos.Recepcao.Retorno;
using NFe.Servicos;

namespace FusionCore.FusionAdm.Fiscal.NF.Consulta
{
    public class ConsultaNfce
    {
        private readonly IDadosServicoSefaz _dadosServico;

        public ConsultaNfce(IDadosServicoSefaz dadosServico)
        {
            _dadosServico = dadosServico;
        }

        public retConsSitNFe ConsultaPelaChave(string chave)
        {
            ValidaChave(chave);

            var cfg = new ConfiguracaoZeusBuilder(_dadosServico, TipoEmissao.Normal).GetConfiguracao();

            var servicoNFe = new ServicosNFe(cfg);
            var retornoConsulta = servicoNFe.NfeConsultaProtocolo(chave);

            return retornoConsulta.Retorno;
        }

        // ReSharper disable once UnusedParameter.Local
        private static void ValidaChave(string chave)
        {
            if (string.IsNullOrEmpty(chave))
                throw new InvalidOperationException(
                    "Porfavor é obrigatorio o uso de uma chave\nA chave deve ter 44 digitos, obrigado");

            if (chave.Length != 44)
                throw new InvalidOperationException("A chave deve ter 44 digitos, obrigado");
        }

        public retConsReciNFe ConsultaPeloRecibo(string recibo)
        {
            var cfg = new ConfiguracaoZeusBuilder(_dadosServico, TipoEmissao.Normal).GetConfiguracao();

            var servicoNFe = new ServicosNFe(cfg);

            var ret = servicoNFe.NFeRetAutorizacao(recibo);

            return ret.Retorno;
        }
    }
}