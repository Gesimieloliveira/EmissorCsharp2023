namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.portalfiscal.inf.br/cte")]
    [System.Xml.Serialization.XmlRootAttribute("CTe", Namespace="http://www.portalfiscal.inf.br/cte", IsNullable=false)]
    public partial class TCTe {
    
        private TCTeInfCte infCteField;
    
        private SignatureType signatureField;
    
        /// <remarks/>
        public TCTeInfCte infCte {
            get {
                return this.infCteField;
            }
            set {
                this.infCteField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace="http://www.w3.org/2000/09/xmldsig#")]
        public SignatureType Signature {
            get {
                return this.signatureField;
            }
            set {
                this.signatureField = value;
            }
        }
    }
}