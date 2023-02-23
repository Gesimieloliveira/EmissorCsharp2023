using System.IO;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Helpers.Hidratacao;
using FusionNfce.AutorizacaoSatFiscal;
using FusionNfce.AutorizacaoSatFiscal.Criadores;
using FusionNfce.AutorizacaoSatFiscal.Ext;
using FusionNfce.Visao.ConfiguraCertificado;
using FusionWPF.Base.Utils.Dialogs;
using Microsoft.Win32;
using OpenAC.Net.Sat;

namespace FusionNfce.Core
{
    public static class TrocarCertificadoDigital
    {
        public static void Trocar()
        {
            if (SessaoSistemaNfce.IsEmissorSat())
            {
                var arquivoCertificado = string.Empty;

                var janelaArquivo = new OpenFileDialog
                {
                    Filter = "Certificado digital(*.cer)|*.cer"
                };

                var resultadoJanela = janelaArquivo.ShowDialog();

                if (resultadoJanela == false)
                {
                    return;
                }

                if (resultadoJanela == true)
                {
                    arquivoCertificado = janelaArquivo.FileName;
                }


                var acbrSat = CriaAcbrSat.Criar();

                new AtivarSat(acbrSat).Ativar();

                SatResposta resultado;

                using (acbrSat)
                {
                    resultado = acbrSat.ComunicarCertificadoIcpBrasil(File.ReadAllText(arquivoCertificado));
                }

                if (resultado.MensagemSEFAZ.IsNotNullOrEmpty())
                {
                    DialogBox.MostraInformacao("Mensagem Sefaz: " + resultado.MensagemSEFAZ);
                }

                DialogBox.MostraInformacao(resultado.MensagemDoCodigoDeRetorno().Mensagem + "\nObservação: "
                                           + resultado.MensagemDoCodigoDeRetorno().Observacao
                                           + "\n" + resultado.MensagemRetorno);

            }

            if (SessaoSistemaNfce.IsEmissorNFce())
            {
                new CertificadoDigitalForm(new CertificadoDigitalFormModel()).Show();
            }
        }
    }
}