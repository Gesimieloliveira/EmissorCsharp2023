namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TCTeInfCteInfCTeNormInfDocInfOutros {
    
        private TCTeInfCteInfCTeNormInfDocInfOutrosTpDoc tpDocField;
    
        private string descOutrosField;
    
        private string nDocField;
    
        private string dEmiField;
    
        private string vDocFiscField;
    
        private string dPrevField;
    
        private object[] itemsField;
    
        /// <remarks/>
        public TCTeInfCteInfCTeNormInfDocInfOutrosTpDoc tpDoc {
            get {
                return this.tpDocField;
            }
            set {
                this.tpDocField = value;
            }
        }
    
        /// <remarks/>
        public string descOutros {
            get {
                return this.descOutrosField;
            }
            set {
                this.descOutrosField = value;
            }
        }
    
        /// <remarks/>
        public string nDoc {
            get {
                return this.nDocField;
            }
            set {
                this.nDocField = value;
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
    
        /// <remarks/>
        public string vDocFisc {
            get {
                return this.vDocFiscField;
            }
            set {
                this.vDocFiscField = value;
            }
        }
    
        /// <remarks/>
        public string dPrev {
            get {
                return this.dPrevField;
            }
            set {
                this.dPrevField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("infUnidCarga", typeof(TUnidCarga))]
        [System.Xml.Serialization.XmlElementAttribute("infUnidTransp", typeof(TUnidadeTransp))]
        public object[] Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
    }
}