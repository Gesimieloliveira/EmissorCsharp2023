using System;
using System.Linq;
using ACBrFramework;
using FusionLibrary.VisaoModel;
using FusionPdv.Modelos;
using FusionPdv.Servicos.Ecf;
using NHibernate.Util;

namespace FusionPdv.Visao.AdicionarImposto
{
    public class AdicionaImpostoModel : ModelBase
    {

        private string _tipoImposto;

        private decimal _valorAliquota;

        public string TipoImposto
        {
            get { return _tipoImposto; }
            set
            {
                _tipoImposto = value;
                PropriedadeAlterada();
            }
        }

        public decimal ValorAliquota
        {
            get { return _valorAliquota; }
            set
            {
                _valorAliquota = value;
                PropriedadeAlterada();
            }
        }

        public void SalvarAliquotaNaEcf()
        {
            try
            {
                new EcfAdicionaAliquota().Adiciona(new Aliquota
                {
                    Tipo = _tipoImposto,
                    Percentual = _valorAliquota
                });
            }
            catch (ACBrException ex)
            {
                throw new ACBrException(ex.Message);
            }
            catch (Exception ex)
            {
                var mensagem = "Falha ao salvar alíquota na ecf";

                if (ex.Message.Contains("movimento"))
                {
                    mensagem = "Já houve movimento no dia, impossivel adicionar aliquota.";
                }
                throw new InvalidOperationException(mensagem, ex);
            }
        }

        public void ValidaRepetido()
        {
            var lista = new EcfPegarAliquotas().Aliquotas();

            var aliquota = new Aliquota            
            {
                Tipo = _tipoImposto,
                Percentual = _valorAliquota
            };

            var aliquotaRepetida = lista.Where(a => a.Valor.Equals(aliquota.Percentual) && a.Tipo.Equals(aliquota.Tipo)).FirstOrNull();

            if (aliquotaRepetida != null) throw new InvalidOperationException("Já existe a alíquota na ECF");
        }
    }
}
