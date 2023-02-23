using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace Fusion.FastReport.DataSources
{
    public struct DsInutilizacao
    {
        public ModeloDocumento Modelo { get; set; }
        public short Serie { get; set; }
        public int NumeroInicial { get; set; }
        public int NumeroFinal { get; set; }
        public string Protocolo { get; set; }
        public DateTime InutilizadoEm { get; set; }
        public string EmpresaRazao { get; set; }
        public string EmpesaCnpj { get; set; }
    }
}