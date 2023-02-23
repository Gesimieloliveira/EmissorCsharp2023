using System;
using System.Xml.Serialization;
using NFe.Classes;

namespace FusionCore.DFe.XmlCte
{
    [Serializable]
    public class FusionImpostoCTe
    {
        private decimal? _totalTributosFederais;

        [XmlElement(ElementName = "ICMS")]
        public FusionIcmsCTe FusionIcmsCTe { get; set; }

        [XmlElement(ElementName = "vTotTrib")]
        public decimal? TotalTributosFederais
        {
            get { return _totalTributosFederais; }
            set { _totalTributosFederais = value; }
        }

        public bool TotalTributosFederaisSpecified => _totalTributosFederais.HasValue;

        [XmlElement(ElementName = "ICMSUFFim")]
        public FusionIcmsUfFimCTe FusionIcmsUfFimCTe { get; set; }

        public FusionImpostoCTe()
        {
            FusionIcmsCTe = new FusionIcmsCTe();
        }
    }

    [Serializable]
    public class FusionIcmsCTe
    {
        [XmlElement("ICMS00", typeof(FusionIcms00CTe))]
        [XmlElement("ICMS20", typeof(FusionIcms20CTe))]
        [XmlElement("ICMS45", typeof(FusionIcms45CTe))]
        [XmlElement("ICMS60", typeof(FusionIcms60CTe))]
        [XmlElement("ICMS90", typeof(FusionIcms90CTe))]
        [XmlElement("ICMSSN", typeof(FusionIcmsSimplesNacionalCTe))]
        public FusionIcmsBasicoCTe Icms { get; set; }
    }


    [Serializable]
    public class FusionIcms90CTe : FusionIcmsBasicoCTe
    {
        [XmlElement(ElementName = "CST")]
        public short CST { get; set; }

        private decimal _vBc;
        private decimal _pIcms;
        private decimal _vIcms;
        private decimal _pRedBc;
        private decimal? _vCred;

        public decimal pRedBC
        {
            get { return _pRedBc.Arredondar(2); }
            set { _pRedBc = value.Arredondar(2); }
        }

        public decimal vBC
        {
            get { return _vBc.Arredondar(2); }
            set { _vBc = value.Arredondar(2); }
        }

        public decimal pICMS
        {
            get { return _pIcms.Arredondar(2); }
            set { _pIcms = value.Arredondar(2); }
        }

        public decimal vICMS
        {
            get { return _vIcms.Arredondar(2); }
            set { _vIcms = value.Arredondar(2); }
        }

        public decimal? vCred
        {
            get { return _vCred.Arredondar(2); }
            set { _vCred = value.Arredondar(2); }
        }

        public bool vCredSpecified => vCred.HasValue;

        public FusionIcms90CTe()
        {
            CST = 90;
        }
    }

    [Serializable]
    public class FusionIcms60CTe : FusionIcmsBasicoCTe
    {
        [XmlElement(ElementName = "CST")]
        public short CST { get; set; }

        private decimal _vICMSSTRet;
        private decimal _pICMSSTRet;
        private decimal? _vCred;
        private decimal _vBCSTRet;

        public decimal vBCSTRet
        {
            get { return _vBCSTRet.Arredondar(2); }
            set { _vBCSTRet = value.Arredondar(2); }
        }

        public decimal vICMSSTRet
        {
            get { return _vICMSSTRet.Arredondar(2); }
            set { _vICMSSTRet = value.Arredondar(2); }
        }

        public decimal pICMSSTRet
        {
            get { return _pICMSSTRet.Arredondar(2); }
            set { _pICMSSTRet = value.Arredondar(2); }
        }

        public decimal? vCred
        {
            get { return _vCred.Arredondar(2); }
            set { _vCred = value.Arredondar(2); }
        }

        public bool vCredSpecified => vCred.HasValue;

        public FusionIcms60CTe()
        {
            CST = 60;
        }
    }

    [Serializable]
    public class FusionIcms45CTe : FusionIcmsBasicoCTe
    {
        [XmlElement(ElementName = "CST")]
        public short CST { get; set; }
    }

    [Serializable]
    public class FusionIcms20CTe : FusionIcmsBasicoCTe
    {
        [XmlElement(ElementName = "CST")]
        public short CST { get; set; }

        private decimal _vBc;
        private decimal _pIcms;
        private decimal _vIcms;
        private decimal _pRedBc;

        public decimal pRedBC
        {
            get { return _pRedBc.Arredondar(2); }
            set { _pRedBc = value.Arredondar(2); }
        }

        public decimal vBC
        {
            get { return _vBc.Arredondar(2); }
            set { _vBc = value.Arredondar(2); }
        }

        public decimal pICMS
        {
            get { return _pIcms.Arredondar(2); }
            set { _pIcms = value.Arredondar(2); }
        }

        public decimal vICMS
        {
            get { return _vIcms.Arredondar(2); }
            set { _vIcms = value.Arredondar(2); }
        }

        public FusionIcms20CTe()
        {
            CST = 20;
        }
    }

    [Serializable]
    public class FusionIcms00CTe : FusionIcmsBasicoCTe
    {
        [XmlElement(ElementName = "CST")]
        public string CST { get; set; }

        private decimal _vBc;
        private decimal _pIcms;
        private decimal _vIcms;

        public decimal vBC
        {
            get { return _vBc.Arredondar(2); }
            set { _vBc = value.Arredondar(2); }
        }

        public decimal pICMS
        {
            get { return _pIcms.Arredondar(2); }
            set { _pIcms = value.Arredondar(2); }
        }

        public decimal vICMS
        {
            get { return _vIcms.Arredondar(2); }
            set { _vIcms = value.Arredondar(2); }
        }

        public FusionIcms00CTe()
        {
            CST = "00";
        }
    }


    [Serializable]
    public class FusionIcmsSimplesNacionalCTe : FusionIcmsBasicoCTe
    {
        [XmlElement(ElementName = "CST")]
        public short CST { get; set; }

        [XmlElement(ElementName = "indSN")]
        public int IndicadorSimplesNacional { get; set; }

        public FusionIcmsSimplesNacionalCTe()
        {
            IndicadorSimplesNacional = 1;
            CST = 90;
        }
    }

    [Serializable]
    public class FusionIcmsUfFimCTe
    {
        public decimal vBCUFFim { get; set; }
        public decimal pFCPUFFim { get; set; }
        public decimal pICMSUFFim { get; set; }
        public decimal pICMSInter { get; set; }
        public decimal vFCPUFFim { get; set; }
        public decimal vICMSUFFim { get; set; }
        public decimal vICMSUFIni { get; set; }
    }
}