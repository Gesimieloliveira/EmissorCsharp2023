using System;

namespace FusionCore.FusionAdm.Csrt
{
    public class CsrtDTO
    {
        public Guid Guid { get; set; }

        public string Csrt { get; set; }

        public string CsrtId { get; set; }

        public string SiglaUf { get; set; }

        public bool IsCTe { get; set; }

        public bool IsCTeOs { get; set; }

        public bool IsMDFe { get; set; }

        public bool IsNFCe { get; set; }

        public bool IsNFe { get; set; }
    }
}