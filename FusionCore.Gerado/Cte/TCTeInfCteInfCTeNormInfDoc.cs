namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TCTeInfCteInfCTeNormInfDoc {
    
        private object[] itemsField;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("infNF", typeof(TCTeInfCteInfCTeNormInfDocInfNF))]
        [System.Xml.Serialization.XmlElementAttribute("infNFe", typeof(TCTeInfCteInfCTeNormInfDocInfNFe))]
        [System.Xml.Serialization.XmlElementAttribute("infOutros", typeof(TCTeInfCteInfCTeNormInfDocInfOutros))]
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