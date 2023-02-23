using DFe.Classes.Flags;

namespace FusionCore.FusionAdm.Fiscal.NF.Cancelar
{
    public class RetornoCancelamento
    {
        public int CStat { get; }
        public string XMotivo { get; }
        public string Protocolo { get; }
        public TipoAmbiente TpAmb { get; set; }
        public string EnvioStr { get; set; }
        public string RetornoCompletoStr { get; set; }

        public RetornoCancelamento(int cStat, string xMotivo, string protocolo)
        {
            CStat = cStat;
            XMotivo = xMotivo;
            Protocolo = protocolo ?? string.Empty;
        }


    }
}