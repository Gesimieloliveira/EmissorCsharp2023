using System;
using System.IO;
using System.Xml.Serialization;
using FusionCore.FusionPdv.Flags;

namespace FusionCore.FusionPdv.Models
{
    [Serializable]
    [XmlRoot(Namespace = "www.sistemafusion.com.br",
        ElementName = "Fusion")]
    public class EntidadeTef
    {
        [XmlElement(nameof(ArqReq))]
        public string ArqReq { get; set; } = "C:\\TEF_DIAL\\req\\intpos.001";

        [XmlElement(nameof(ArqResp))]
        public string ArqResp { get; set; } = "C:\\TEF_DIAL\\resp\\intpos.001";

        [XmlElement(nameof(ArqSts))]
        public string ArqSts { get; set; } = "C:\\TEF_DIAL\\resp\\intpos.sts";

        [XmlElement(nameof(ArqTemp))]
        public string ArqTemp { get; set; } = "C:\\TEF_DIAL\\req\\intpos.tmp";

        [XmlElement(nameof(GpExeName))]
        public string GpExeName { get; set; } = "C:\\TEF_DIAL\\tef_dial.exe";

        [XmlElement(nameof(Ativo))]
        public bool Ativo { get; set; }

        [XmlElement(nameof(OperadoraTef))]
        public OperadorasTef OperadoraTef { get; set; } = OperadorasTef.Nenhuma;

        // ReSharper disable once EmptyConstructor
        public EntidadeTef()
        {
        }

        public bool ExisteGpExe()
        {
            return File.Exists(GpExeName);
        }
    }
}