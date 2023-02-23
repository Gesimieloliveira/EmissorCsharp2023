namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TCTeInfCteDest {
    
        private string itemField;
    
        private ItemChoiceType4 itemElementNameField;
    
        private string ieField;
    
        private string xNomeField;
    
        private string foneField;
    
        private string iSUFField;
    
        private TEndereco enderDestField;
    
        private string emailField;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CNPJ", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("CPF", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string DocumentoUnico {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType4 ItemElementName {
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
        public string fone {
            get {
                return this.foneField;
            }
            set {
                this.foneField = value;
            }
        }
    
        /// <remarks/>
        public string ISUF {
            get {
                return this.iSUFField;
            }
            set {
                this.iSUFField = value;
            }
        }
    
        /// <remarks/>
        public TEndereco enderDest {
            get {
                return this.enderDestField;
            }
            set {
                this.enderDestField = value;
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