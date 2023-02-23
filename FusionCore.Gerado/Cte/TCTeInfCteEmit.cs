namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TCTeInfCteEmit {
    
        private string cNPJField;
    
        private string ieField;
    
        private string xNomeField;
    
        private string xFantField;
    
        private TEndeEmi enderEmitField;
    
        /// <remarks/>
        public string CNPJ {
            get {
                return this.cNPJField;
            }
            set {
                this.cNPJField = value;
            }
        }
    
        /// <remarks/>
        public string IE {
            get {
                return this.ieField;
            }
            set {
                this.ieField = value;
            }
        }
    
        /// <remarks/>
        public string xNome {
            get {
                return this.xNomeField;
            }
            set {
                this.xNomeField = value;
            }
        }
    
        /// <remarks/>
        public string xFant {
            get {
                return this.xFantField;
            }
            set {
                this.xFantField = value;
            }
        }
    
        /// <remarks/>
        public TEndeEmi enderEmit {
            get {
                return this.enderEmitField;
            }
            set {
                this.enderEmitField = value;
            }
        }
    }
}