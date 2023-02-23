namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TUnidadeTransp {
    
        private TtipoUnidTransp tpUnidTranspField;
    
        private string idUnidTranspField;
    
        private TUnidadeTranspLacUnidTransp[] lacUnidTranspField;
    
        private TUnidCarga[] infUnidCargaField;
    
        private string qtdRatField;
    
        /// <remarks/>
        public TtipoUnidTransp tpUnidTransp {
            get {
                return this.tpUnidTranspField;
            }
            set {
                this.tpUnidTranspField = value;
            }
        }
    
        /// <remarks/>
        public string idUnidTransp {
            get {
                return this.idUnidTranspField;
            }
            set {
                this.idUnidTranspField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("lacUnidTransp")]
        public TUnidadeTranspLacUnidTransp[] lacUnidTransp {
            get {
                return this.lacUnidTranspField;
            }
            set {
                this.lacUnidTranspField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("infUnidCarga")]
        public TUnidCarga[] infUnidCarga {
            get {
                return this.infUnidCargaField;
            }
            set {
                this.infUnidCargaField = value;
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