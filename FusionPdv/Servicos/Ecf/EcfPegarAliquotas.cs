using System;
using System.Collections.Generic;
using FusionCore.FusionPdv.Ecf;
using FusionPdv.Ecf;

namespace FusionPdv.Servicos.Ecf
{
    public class EcfPegarAliquotas
    { 
        public IList<Aliquota> Aliquotas()
        {
            try
            {
                
                IList<Aliquota> aliquotas = new List<Aliquota>(SessaoEcf.EcfFiscal.Aliquotas());

                return aliquotas;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao tentar listar alíquotas da ecf.", ex);
            }           
        }
    }
}
