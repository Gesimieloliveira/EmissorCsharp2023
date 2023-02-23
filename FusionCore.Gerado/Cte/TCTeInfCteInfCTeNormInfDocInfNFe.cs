namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TCTeInfCteInfCTeNormInfDocInfNFe {
    
        private string chaveField;
    
        private string pINField;
    
        private string dPrevField;
    
        private object[] itemsField;
    
        /// <remarks/>
        public string chave {
            get {
                return this.chaveField;
            }
            set {
                this.chaveField = value;
            }
        }
    
        /// <remarks/>
        public string PIN {
            get {
                return this.pINField;
            }
            set {
                this.pINField = value;
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