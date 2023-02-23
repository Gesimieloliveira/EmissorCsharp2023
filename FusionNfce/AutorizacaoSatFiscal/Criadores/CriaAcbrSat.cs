using System.Text;
using FusionCore.Debug;
using FusionCore.FusionAdm.Emissores.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Helpers.EmpresaDesenvolvedora;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Tributacoes.Flags;
using FusionNfce.AutorizacaoSatFiscal.Configuracao.Configuracao;
using OpenAC.Net.DFe.Core.Common;
using OpenAC.Net.Sat;

namespace FusionNfce.AutorizacaoSatFiscal.Criadores
{
    public static class CriaAcbrSat
    {
        public static OpenSat Criar()
        {
            var emissorSat = SessaoSistemaNfce.Configuracao.EmissorFiscal.EmissorFiscalSat;
            var empresaSat = SessaoSistemaNfce.Configuracao.EmissorFiscal.Empresa;

            var configuracaoDllSat = ConfiguracaoDllSatCriador.CriaXmlConfigSeNaoExistir(emissorSat);

            var cnpjEmitente = empresaSat.Cnpj;
            var ieEmitente = empresaSat.InscricaoEstadual;
            var cnpjDesenvolvedora = ResponsavelLegal.Cnpj;

            if (SessaoSistemaNfce.AmbienteSefazProducao() == false && BuildMode.IsHomologacao)
            {
                const string cnpjTanca = "08723218000186";
                const string cnpjElgin = "14200166000166";

                switch (emissorSat.Fabricante)
                {
                    case Modelo.Emulador:
                        cnpjEmitente = "11111111111111";
                        ieEmitente = "111111111111";
                        break;
                    case Modelo.Tanca:
                    case Modelo.ControlId:
                        cnpjDesenvolvedora = "16716114000172";
                        break;
                }

                if (SessaoSistemaNfce.IsMFe())
                {
                    if (cnpjEmitente == cnpjTanca)
                        cnpjDesenvolvedora = "16716114000172";

                    if (cnpjEmitente == cnpjElgin)
                        cnpjDesenvolvedora = "10615281000140";
                }
            }

            var acbrSat = new OpenSat
            { 
                Arquivos =
                {
                    SalvarEnvio = true,
                    SalvarCFe = true,
                    SalvarCFeCanc = true,
                    SepararPorMes = true,
                    SepararPorCNPJ = true
                },
                Configuracoes =
                {
                    RemoverAcentos = true,
                    EmitCNPJ = cnpjEmitente.TrimSefaz(14),
                    EmitCRegTrib = empresaSat.RegimeTributario == RegimeTributario.RegimeNormal ? RegTrib.Normal : RegTrib.SimplesNacional,
                    EmitCRegTribISSQN = RegTribIssqn.Nenhum,
                    EmitIE = ieEmitente.PadRight(12, ' '),
                    EmitIM = empresaSat.InscricaoMunicipal.TrimOrNull(),
                    EmitIndRatISSQN = RatIssqn.Nao,
                    IdeCNPJ = cnpjDesenvolvedora,
                    IdeNumeroCaixa = emissorSat.NumeroCaixa,
                    IdeTpAmb = emissorSat.Ambiente == TipoAmbiente.Homologacao ? DFeTipoAmbiente.Homologacao : DFeTipoAmbiente.Producao,
                    InfCFeVersaoDadosEnt = (int)emissorSat.VersaoLayoutSat/100m,
                    IsUtf8 = Equals(Encoding.GetEncoding((int)emissorSat.CodificacaoArquivoXml), Encoding.UTF8)
                },
                CodigoAtivacao = emissorSat.CodigoAtivacao,
                Modelo = configuracaoDllSat.ModeloSat.ToSat(),
                Encoding = Encoding.GetEncoding((int)emissorSat.CodificacaoArquivoXml),
                PathDll = configuracaoDllSat.CaminhoDll,
                SignAC = emissorSat.CodigoAcossiacao
            };

            return acbrSat;
        }
    }
}