using System;
using FusionCore.Excecoes.Sessao;

namespace FusionPdv.Servicos.Ecf
{
    public class EcfConfiguracao
    {
        private readonly ObterEcfEmUso _obterEcfEmUso;

        public EcfConfiguracao()
        {
            _obterEcfEmUso = new ObterEcfEmUso();
        }

        public void ValidarConfiguracaoDoEcf()
        {
            try
            {
                var ecfAtiva = _obterEcfEmUso.Buscar();

                if (ecfAtiva == null)
                {
                    throw new InvalidOperationException("Não encontrei configuração da ECF.");
                }
            }
            catch (ConexaoInvalidaException ex)
            {
                throw new ConexaoInvalidaException(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
