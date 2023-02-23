using System.Collections.Generic;
using FusionCore.FusionAdm.Emissores;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.TerminalOffline
{
    public class TerminalOffline : EntidadeBase<byte>, ISincronizavelAdm
    {
        public byte Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public int IntervaloSync { get; set; }
        public string BindTerminal { get; set; }
        public IList<EmissorFiscal> EmissorFiscalLista { get; set; } = new List<EmissorFiscal>();
        public string Impressora { get; set; }
        public string Observacao { get; set; }


        public string Referencia => Id.ToString();
        public EntidadeSincronizavel EntidadeSincronizavel { get; set; } = EntidadeSincronizavel.TerminalOffline;
        protected override byte ChaveUnica => Id;
    }
}
