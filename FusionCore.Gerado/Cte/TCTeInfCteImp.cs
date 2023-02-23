namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TCTeInfCteImp {
    
        private TImp iCMSField;
    
        private string vTotTribField;
    
        private string infAdFiscoField;
    
        private TCTeInfCteImpICMSUFFim iCMSUFFimField;
    
        /// <remarks/>
        public TImp ICMS {
            get {
                return this.iCMSField;
            }
            set {
                this.iCMSField = value;
            }
        }
    
        /// <remarks/>
        public string vTotTrib {
            get {
                return this.vTotTribField;
            }
            set {
                this.vTotTribField = value;
            }
        }
    
        /// <remarks/>
        public string infAdFisco {
            get {
                return this.infAdFiscoField;
            }
            set {
                this.infAdFiscoField = value;
            }
        }
    
        /// <remarks/>
        public TCTeInfCteImpICMSUFFim ICMSUFFim {
            get {
                return this.iCMSUFFimField;
            }
            set {
                this.iCMSUFFimField = value;
            }
        }
    }
}