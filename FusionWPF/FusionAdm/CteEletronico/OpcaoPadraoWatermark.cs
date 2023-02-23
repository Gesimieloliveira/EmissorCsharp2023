using System.Collections.Generic;
using FusionWPF.Base.GridPicker.OpcoesBuscas;
using NHibernate;

namespace FusionWPF.FusionAdm.CteEletronico
{
    public class OpcaoPadraoWatermark : IOpcaoBusca
    {
        public OpcaoPadraoWatermark(string watermark)
        {
            Watermark = watermark;
        }

        public string Watermark
        {
            get;
        }

        public IList<T> Listar<T>(string input, ISession sessao)
        {
            throw new System.NotImplementedException();
        }
    }
}