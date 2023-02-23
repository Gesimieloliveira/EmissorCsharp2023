namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TCTeInfCteIdeToma4 {
    
        private TCTeInfCteIdeToma4Toma tomaField;
    
        private string itemField;
    
        private ItemChoiceType itemElementNameField;
    
        private string ieField;
    
        private string xNomeField;
    
        private string xFantField;
    
        private string foneField;
    
        private TEndereco enderTomaField;
    
        private string emailField;
    
        /// <remarks/>
        public TCTeInfCteIdeToma4Toma toma {
            get {
                return this.tomaField;
            }
            set {
                this.tomaField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CNPJ", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("CPF", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType ItemElementName {
            get {
                return this.itemElementNameField;
            }
            set {
                this.itemElementNameField = value;
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
        public string fone {
            get {
                return this.foneField;
            }
            set {
                this.foneField = value;
            }
        }
    
        /// <remarks/>
        public TEndereco enderToma {
            get {
                return this.enderTomaField;
            }
            set {
                this.enderTomaField = value;
            }
        }
    
        /// <remarks/>
        public string email {
            get {
                return this.emailField;
            }
            set {
                this.emailField = value;
            }
        }
    }
}