using System;

namespace FusionPdv.Ecf
{
    public class CriaEcfFiscal
    {
        public static EcfFiscal ObterEcfFiscal(string objeto)
        {
            try
            {
                if (SessaoEcf.EcfFiscal != null)
                {
                    if (SessaoEcf.EcfFiscal.Ativo)
                    {
                        SessaoEcf.EcfFiscal.Desativar();
                    }
                }

                if (string.IsNullOrEmpty(objeto)) throw new NullReferenceException("Não foi possível criar uma instância da IEcfFiscal, \n" +
                                                               "pois o valor foi null");

                var tipoEcfFiscal = Type.GetType(objeto);

                if (tipoEcfFiscal == null) throw new NullReferenceException("Não foi possível criar uma instância da IEcfFiscal, \n" +
                                                               "pois o valor foi null");
                var iEcfFiscal = (EcfFiscal) Activator.CreateInstance(tipoEcfFiscal);

                iEcfFiscal.Configurar();
                iEcfFiscal.ManipulaArquivo();

                return iEcfFiscal;
            }
            catch (NullReferenceException ex)
            {
                throw new EcfFiscalException(ex.Message, ex);
            }

            
        }
    }
}
