namespace FusionCore.Tributacoes.Estadual
{
    public class PermissaoCst
    {
        private readonly string _codigoCst;

        public PermissaoCst(string codigoCst)
        {
            _codigoCst = codigoCst;
        }

        public bool TemIcms()
        {
            return _codigoCst == "00" ||
                   _codigoCst == "10" ||
                   _codigoCst == "20" ||
                   _codigoCst == "70" ||
                   _codigoCst == "90";
        }

        public bool TemReducao()
        {
            return _codigoCst == "20" || _codigoCst == "70" || _codigoCst == "90";
        }

        public bool TemIcmsSt()
        {
            return _codigoCst == "10" ||
                   _codigoCst == "70" ||
                   _codigoCst == "90" ||
                   _codigoCst == "30";
        }

        public bool TemReducaoSt()
        {
            return _codigoCst == "10" ||
                   _codigoCst == "70" ||
                   _codigoCst == "90" ||
                   _codigoCst == "30";
        }

        public bool TemMva()
        {
            return _codigoCst == "10" ||
                   _codigoCst == "70" ||
                   _codigoCst == "90" ||
                   _codigoCst == "30";
        }
    }
}