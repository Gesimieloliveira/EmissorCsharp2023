namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TCTeInfCteInfCTeNormCobr {
    
        private TCTeInfCteInfCTeNormCobrFat fatField;
    
        private TCTeInfCteInfCTeNormCobrDup[] dupField;
    
        /// <remarks/>
        public TCTeInfCteInfCTeNormCobrFat fat {
            get {
                return this.fatField;
            }
            set {
                this.fatField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("dup")]
        public TCTeInfCteInfCTeNormCobrDup[] dup {
            get {
                return this.dupField;
            }
            set {
                this.dupField = value;
            }
        }
    }
}