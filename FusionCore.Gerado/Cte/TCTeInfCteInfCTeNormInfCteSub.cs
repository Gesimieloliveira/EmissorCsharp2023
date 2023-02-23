namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TCTeInfCteInfCTeNormInfCteSub {
    
        private string chCteField;
    
        private object itemField;
    
        /// <remarks/>
        public string chCte {
            get {
                return this.chCteField;
            }
            set {
                this.chCteField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("tomaICMS", typeof(TCTeInfCteInfCTeNormInfCteSubTomaICMS))]
        [System.Xml.Serialization.XmlElementAttribute("tomaNaoICMS", typeof(TCTeInfCteInfCTeNormInfCteSubTomaNaoICMS))]
        public object Item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
    }
}