namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TImp {
    
        private object itemField;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ICMS00", typeof(TImpICMS00))]
        [System.Xml.Serialization.XmlElementAttribute("ICMS20", typeof(TImpICMS20))]
        [System.Xml.Serialization.XmlElementAttribute("ICMS45", typeof(TImpICMS45))]
        [System.Xml.Serialization.XmlElementAttribute("ICMS60", typeof(TImpICMS60))]
        [System.Xml.Serialization.XmlElementAttribute("ICMS90", typeof(TImpICMS90))]
        [System.Xml.Serialization.XmlElementAttribute("ICMSOutraUF", typeof(TImpICMSOutraUF))]
        [System.Xml.Serialization.XmlElementAttribute("ICMSSN", typeof(TImpICMSSN))]
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