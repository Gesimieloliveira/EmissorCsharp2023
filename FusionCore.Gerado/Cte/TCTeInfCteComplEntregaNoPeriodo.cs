namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TCTeInfCteComplEntregaNoPeriodo {
    
        private TCTeInfCteComplEntregaNoPeriodoTpPer tpPerField;
    
        private string dIniField;
    
        private string dFimField;
    
        /// <remarks/>
        public TCTeInfCteComplEntregaNoPeriodoTpPer tpPer {
            get {
                return this.tpPerField;
            }
            set {
                this.tpPerField = value;
            }
        }
    
        /// <remarks/>
        public string dIni {
            get {
                return this.dIniField;
            }
            set {
                this.dIniField = value;
            }
        }
    
        /// <remarks/>
        public string dFim {
            get {
                return this.dFimField;
            }
            set {
                this.dFimField = value;
            }
        }
    }
}