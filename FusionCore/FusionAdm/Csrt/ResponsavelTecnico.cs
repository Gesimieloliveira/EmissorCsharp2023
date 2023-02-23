using System;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.FusionAdm.Csrt
{
    public class ResponsavelTecnico : ISincronizavelAdm
    {
        public Guid Guid { get; set; }

        public string Csrt { get; set; }

        public int CsrtId { get; set; }

        public EstadoDTO Uf { get; set; }

        public bool IsCTe { get; set; }

        public bool IsCTeOs { get; set; }

        public bool IsMDFe { get; set; }

        public bool IsNFCe { get; set; }

        public bool IsNFe { get; set; }

        public static ResponsavelTecnico Instancia(CsrtDTO csrtDTO, ISession sessao)
        {
            return new ResponsavelTecnico
            {
                CsrtId = int.Parse(csrtDTO.CsrtId),
                Guid = csrtDTO.Guid,
                Csrt = csrtDTO.Csrt,
                Uf = new RepositorioEstado(sessao).GetPelaSigla(csrtDTO.SiglaUf),
                IsCTe = csrtDTO.IsCTe,
                IsCTeOs = csrtDTO.IsCTeOs,
                IsMDFe = csrtDTO.IsMDFe,
                IsNFCe = csrtDTO.IsNFCe,
                IsNFe = csrtDTO.IsNFe
            };
        }

        public string Referencia => Guid.ToString();
        public EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.ResponsavelTecnico;
    }
}