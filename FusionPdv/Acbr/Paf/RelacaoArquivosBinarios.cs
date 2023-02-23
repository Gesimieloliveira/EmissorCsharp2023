// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ACBrFramework.PAF;

namespace FusionPdv.Acbr.Paf
{
    public class RelacaoArquivosBinarios : IConfiguracaoPaf
    {
        private readonly ACBrPAF _acbrPaf;
        private readonly IList<ACBrPAFRegistroN3> _registroN3S;
        private readonly Md5 _md5;

        public RelacaoArquivosBinarios()
        {
            _acbrPaf = AcbrFactory.ObterAcbrPaf();
            _registroN3S = new List<ACBrPAFRegistroN3>();
            _md5 = new Md5();

            try
            {
                InicializaRegistrosN3();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao gerar o arquivo MD5\n" + ex.Message, ex);
            }
            
        }

        private void InicializaRegistrosN3()
        {
#if DEBUG == false
            AddBinario(_md5.GerarMd5(Application.StartupPath + "\\FusionPdv.exe"), "FusionPdv.exe");
            AddBinario(_md5.GerarMd5(Application.StartupPath + "\\FusionCore.dll"), "FusionCore.dll");
#else
            AddBinario(_md5.GerarMd5(Application.StartupPath + "\\FusionPdv.exe"), "FusionPdv.exe");
#endif
        }

        public void ExecutaConfiguracao()
        {
            _acbrPaf.PafN.RegistroN3.Clear();
            _acbrPaf.PafN.RegistroN3.AddRange(_registroN3S.ToArray());
        }

        private void AddBinario(string md5, string nomeArquivo)
        {
            _registroN3S.Add(new ACBrPAFRegistroN3 {MD5 = md5, NOME_ARQUIVO = nomeArquivo});
        }
    }
}
