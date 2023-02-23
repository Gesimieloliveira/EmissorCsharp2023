using System;
using FusionCore.FusionAdm.Emissores.Flags;

namespace FusionNfce.AutorizacaoSatFiscal.Configuracao.Configuracao
{
    [Serializable]
    public class ConfiguracaoDllSat
    {
        public Modelo FabricanteModelo { get; set; }
        public string CaminhoDll { get; set; }
        public ModeloSatFusion ModeloSat { get; set; } 
    }
}