namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TCTeInfCteComplEntrega {
    
        private object itemField;
    
        private object item1Field;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("comData", typeof(TCTeInfCteComplEntregaComData))]
        [System.Xml.Serialization.XmlElementAttribute("noPeriodo", typeof(TCTeInfCteComplEntregaNoPeriodo))]
        [System.Xml.Serialization.XmlElementAttribute("semData", typeof(TCTeInfCteComplEntregaSemData))]
        public object Item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("comHora", typeof(TCTeInfCteComplEntregaComHora))]
        [System.Xml.Serialization.XmlElementAttribute("noInter", typeof(TCTeInfCteComplEntregaNoInter))]
        [System.Xml.Serialization.XmlElementAttribute("semHora", typeof(TCTeInfCteComplEntregaSemHora))]
        public object Item1 {
            get {
                return this.item1Field;
            }
            set {
                this.item1Field = value;
            }
        }
    }
}