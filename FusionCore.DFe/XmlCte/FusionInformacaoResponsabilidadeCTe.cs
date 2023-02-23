using System;

namespace FusionCore.DFe.XmlCte
{
    [Serializable]
    public class FusionInformacaoResponsabilidadeCTe
    {
        public string CNPJ { get; set; }

        public string xContato { get; set; }

        public string email { get; set; }

        public string fone { get; set; }

        public string hashCSRT { get; set; }

        public string idCSRT { get; set; }
    }
}