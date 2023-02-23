namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TCTeInfCte {
    
        private TCTeInfCteIde ideField;
    
        private TCTeInfCteCompl complField;
    
        private TCTeInfCteEmit emitField;
    
        private TCTeInfCteRem remField;
    
        private TCTeInfCteExped expedField;
    
        private TCTeInfCteReceb recebField;
    
        private TCTeInfCteDest destField;
    
        private TCTeInfCteVPrest vPrestField;
    
        private TCTeInfCteImp impField;
    
        private object itemField;
    
        private TCTeInfCteAutXML[] autXMLField;
    
        private string versaoField;
    
        private string idField;
    
        /// <remarks/>
        public TCTeInfCteIde ide {
            get {
                return this.ideField;
            }
            set {
                this.ideField = value;
            }
        }
    
        /// <remarks/>
        public TCTeInfCteCompl compl {
            get {
                return this.complField;
            }
            set {
                this.complField = value;
            }
        }
    
        /// <remarks/>
        public TCTeInfCteEmit emit {
            get {
                return this.emitField;
            }
            set {
                this.emitField = value;
            }
        }
    
        /// <remarks/>
        public TCTeInfCteRem rem {
            get {
                return this.remField;
            }
            set {
                this.remField = value;
            }
        }
    
        /// <remarks/>
        public TCTeInfCteExped exped {
            get {
                return this.expedField;
            }
            set {
                this.expedField = value;
            }
        }
    
        /// <remarks/>
        public TCTeInfCteReceb receb {
            get {
                return this.recebField;
            }
            set {
                this.recebField = value;
            }
        }
    
        /// <remarks/>
        public TCTeInfCteDest dest {
            get {
                return this.destField;
            }
            set {
                this.destField = value;
            }
        }
    
        /// <remarks/>
        public TCTeInfCteVPrest vPrest {
            get {
                return this.vPrestField;
            }
            set {
                this.vPrestField = value;
            }
        }
    
        /// <remarks/>
        public TCTeInfCteImp imp {
            get {
                return this.impField;
            }
            set {
                this.impField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("infCTeNorm", typeof(TCTeInfCteInfCTeNorm))]
        [System.Xml.Serialization.XmlElementAttribute("infCteAnu", typeof(TCTeInfCteInfCteAnu))]
        [System.Xml.Serialization.XmlElementAttribute("infCteComp", typeof(TCTeInfCteInfCteComp))]
        public object Item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("autXML")]
        public TCTeInfCteAutXML[] autXML {
            get {
                return this.autXMLField;
            }
            set {
                this.autXMLField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string versao {
            get {
                return this.versaoField;
            }
            set {
                this.versaoField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="ID")]
        public string Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
    }
}