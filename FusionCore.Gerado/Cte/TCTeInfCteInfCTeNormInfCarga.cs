namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TCTeInfCteInfCTeNormInfCarga {
    
        private string vCargaField;
    
        private string proPredField;
    
        private string xOutCatField;
    
        private TCTeInfCteInfCTeNormInfCargaInfQ[] infQField;
    
        /// <remarks/>
        public string vCarga {
            get {
                return this.vCargaField;
            }
            set {
                this.vCargaField = value;
            }
        }
    
        /// <remarks/>
        public string proPred {
            get {
                return this.proPredField;
            }
            set {
                this.proPredField = value;
            }
        }
    
        /// <remarks/>
        public string xOutCat {
            get {
                return this.xOutCatField;
            }
            set {
                this.xOutCatField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("infQ")]
        public TCTeInfCteInfCTeNormInfCargaInfQ[] infQ {
            get {
                return this.infQField;
            }
            set {
                this.infQField = value;
            }
        }
    }
}