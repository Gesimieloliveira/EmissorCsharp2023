namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TCTeInfCteInfCTeNormInfCteSubTomaICMSRefNF {
    
        private string itemField;
    
        private ItemChoiceType6 itemElementNameField;
    
        private TModDoc modField;
    
        private string serieField;
    
        private string subserieField;
    
        private string nroField;
    
        private string valorField;
    
        private string dEmiField;
    
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
        public ItemChoiceType6 ItemElementName {
            get {
                return this.itemElementNameField;
            }
            set {
                this.itemElementNameField = value;
            }
        }
    
        /// <remarks/>
        public TModDoc mod {
            get {
                return this.modField;
            }
            set {
                this.modField = value;
            }
        }
    
        /// <remarks/>
        public string serie {
            get {
                return this.serieField;
            }
            set {
                this.serieField = value;
            }
        }
    
        /// <remarks/>
        public string subserie {
            get {
                return this.subserieField;
            }
            set {
                this.subserieField = value;
            }
        }
    
        /// <remarks/>
        public string nro {
            get {
                return this.nroField;
            }
            set {
                this.nroField = value;
            }
        }
    
        /// <remarks/>
        public string valor {
            get {
                return this.valorField;
            }
            set {
                this.valorField = value;
            }
        }
    
        /// <remarks/>
        public string dEmi {
            get {
                return this.dEmiField;
            }
            set {
                this.dEmiField = value;
            }
        }
    }
}