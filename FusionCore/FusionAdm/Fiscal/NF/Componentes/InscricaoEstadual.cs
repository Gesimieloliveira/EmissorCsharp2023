using FusionCore.FusionAdm.Fiscal.Flags;
using static System.String;

namespace FusionCore.FusionAdm.Fiscal.NF.Componentes
{
    public class InscricaoEstadual
    {
        private readonly string _inscricaoEstadual;
        private readonly bool _isIsento;

        public InscricaoEstadual(string inscricaoEstadual)
        {
            _inscricaoEstadual = inscricaoEstadual?.Trim() ?? Empty;
            _isIsento = _inscricaoEstadual.ToUpper() == "ISENTO";
        }

        public IndicadorIE GetIndicador()
        {
            if (_isIsento)
            {
                return IndicadorIE.Isento;
            }

            return IsNullOrWhiteSpace(_inscricaoEstadual)
                ? IndicadorIE.NaoContribuinte
                : IndicadorIE.ContribuinteIcms;
        }

        public string ToNfe()
        {
            return _isIsento ? Empty : _inscricaoEstadual;
        }

        public override string ToString()
        {
            return _inscricaoEstadual;
        }
    }
}
