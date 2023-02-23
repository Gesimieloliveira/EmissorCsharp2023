using System;
using FusionCore.FusionAdm.EntradaOutras;

namespace FusionCore.Sintegra.Dto
{
    public class NfOutroDto
    {
        public int Id { get; set; }
        public short Serie { get; set; }
        public int Numero { get; set; }
        public string NomeFornecedor { get; set; }
        public DateTime EmissaoEm { get; set; }
        public DateTime RecebimentoEm { get; set; }
        public ModeloDocumentoOutro ModeloDocumentoOutro { get; set; }
    }
}