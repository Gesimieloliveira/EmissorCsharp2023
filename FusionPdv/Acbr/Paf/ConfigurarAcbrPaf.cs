using System.Collections.Generic;

namespace FusionPdv.Acbr.Paf
{
    public class ConfigurarAcbrPaf
    {
        private readonly IList<IConfiguracaoPaf> _configuracaos;

        public ConfigurarAcbrPaf()
        {
            _configuracaos = new List<IConfiguracaoPaf>();
            AddConfiguracao();
        }

        public void Executa()
        {
            foreach (var configuracao in _configuracaos)
            {
                configuracao.ExecutaConfiguracao();
            }        
        }

        private void AddConfiguracao()
        {
            _configuracaos.Add(new DadosEmpresaDesenvolvedora());
            _configuracaos.Add(new IdentificacaoPaf());
            _configuracaos.Add(new RelacaoArquivosBinarios());
        }
    }
}
