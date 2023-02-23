using System;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedAutoPropertyAccessor.
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace FusionCore.FusionAdm.ContingenciaSefaz
{
    // Nhibernate: ContigenciaNfe.hbm.xml
    public class ContingenciaNfe
    {
        public int Id { get; private set; }
        public EmissorFiscal EmissorFiscal { get; private set; }
        public TipoEmissao TipoEmissao { get; private set; }
        public string Justificativa { get; private set; }
        public DateTime IniciadaEm { get; private set; }
        public DateTime? FinalizadaEm { get; set; }
        public bool EstaAberta => FinalizadaEm == null;

        private ContingenciaNfe()
        {
            //Nhibernate need this
        }

        public ContingenciaNfe(
            EmissorFiscal emissorFiscal,
            TipoEmissao tipoEmissao,
            string justificativa)
        {
            EmissorFiscal = emissorFiscal;
            TipoEmissao = tipoEmissao;
            Justificativa = justificativa;
            IniciadaEm = DateTime.Now;
        }

        public void FinalizarContigencia()
        {
            FinalizadaEm = DateTime.Now;
        }
    }
}