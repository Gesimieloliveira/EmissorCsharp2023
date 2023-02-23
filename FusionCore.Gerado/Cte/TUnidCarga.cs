namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TUnidCarga {
    
        private TtipoUnidCarga tpUnidCargaField;
    
        private string idUnidCargaField;
    
        private TUnidCargaLacUnidCarga[] lacUnidCargaField;
    
        private string qtdRatField;
    
        /// <remarks/>
        public TtipoUnidCarga tpUnidCarga {
            get {
                return this.tpUnidCargaField;
            }
            set {
                this.tpUnidCargaField = value;
            }
        }
    
        /// <remarks/>
        public string idUnidCarga {
            get {
                return this.idUnidCargaField;
            }
            set {
                this.idUnidCargaField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("lacUnidCarga")]
        public TUnidCargaLacUnidCarga[] lacUnidCarga {
            get {
                return this.lacUnidCargaField;
            }
            set {
                this.lacUnidCargaField = value;
            }
        }
    
        /// <remarks/>
        public string qtdRat {
            get {
                return this.qtdRatField;
            }
            set {
                this.qtdRatField = value;
            }
        }
    }
}