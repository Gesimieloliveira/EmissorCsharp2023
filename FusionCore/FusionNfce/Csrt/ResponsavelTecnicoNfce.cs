using System;
using FusionCore.FusionAdm.Csrt;
using FusionCore.FusionNfce.Uf;

namespace FusionCore.FusionNfce.Csrt
{
    public class ResponsavelTecnicoNfce
    {
        public ResponsavelTecnicoNfce()
        {
        }

        public Guid Guid { get; set; }

        public string Csrt { get; set; }

        public int CsrtId { get; set; }

        public UfNfce Uf { get; set; }

        public static ResponsavelTecnicoNfce Instancia(ResponsavelTecnico responsavelTecnico)
        {
            return new ResponsavelTecnicoNfce
            {
                Guid = responsavelTecnico.Guid,
                CsrtId = responsavelTecnico.CsrtId,
                Uf = new UfNfce { Id = Convert.ToByte(responsavelTecnico.Uf.Id) },
                Csrt = responsavelTecnico.Csrt
            };
        }
    }
}