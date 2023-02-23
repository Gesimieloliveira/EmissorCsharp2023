namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TCTeInfCteComplFluxo {
    
        private string xOrigField;
    
        private TCTeInfCteComplFluxoPass[] passField;
    
        private string xDestField;
    
        private string xRotaField;
    
        /// <remarks/>
        public string xOrig {
            get {
                return this.xOrigField;
            }
            set {
                this.xOrigField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("pass")]
        public TCTeInfCteComplFluxoPass[] pass {
            get {
                return this.passField;
            }
            set {
                this.passField = value;
            }
        }
    
        /// <remarks/>
        public string xDest {
            get {
                return this.xDestField;
            }
            set {
                this.xDestField = value;
            }
        }
    
        /// <remarks/>
        public string xRota {
            get {
                return this.xRotaField;
            }
            set {
                this.xRotaField = value;
            }
        }
    }
}