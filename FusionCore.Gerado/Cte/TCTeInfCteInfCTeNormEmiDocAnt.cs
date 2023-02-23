namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TCTeInfCteInfCTeNormEmiDocAnt {
    
        private string itemField;
    
        private ItemChoiceType5 itemElementNameField;
    
        private string ieField;
    
        private TUf ufField;
    
        private string xNomeField;
    
        private TCTeInfCteInfCTeNormEmiDocAntIdDocAnt[] idDocAntField;
    
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
        public ItemChoiceType5 ItemElementName {
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
        public TUf UF {
            get {
                return this.ufField;
            }
            set {
                this.ufField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("idDocAnt")]
        public TCTeInfCteInfCTeNormEmiDocAntIdDocAnt[] idDocAnt {
            get {
                return this.idDocAntField;
            }
            set {
                this.idDocAntField = value;
            }
        }
    }
}