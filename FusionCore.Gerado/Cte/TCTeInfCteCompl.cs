namespace FusionCore.Gerado.Cte
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.portalfiscal.inf.br/cte")]
    public partial class TCTeInfCteCompl {
    
        private string xCaracAdField;
    
        private string xCaracSerField;
    
        private string xEmiField;
    
        private TCTeInfCteComplFluxo fluxoField;
    
        private TCTeInfCteComplEntrega entregaField;
    
        private string origCalcField;
    
        private string destCalcField;
    
        private string xObsField;
    
        private TCTeInfCteComplObsCont[] obsContField;
    
        private TCTeInfCteComplObsFisco[] obsFiscoField;
    
        /// <remarks/>
        public string xCaracAd {
            get {
                return this.xCaracAdField;
            }
            set {
                this.xCaracAdField = value;
            }
        }
    
        /// <remarks/>
        public string xCaracSer {
            get {
                return this.xCaracSerField;
            }
            set {
                this.xCaracSerField = value;
            }
        }
    
        /// <remarks/>
        public string xEmi {
            get {
                return this.xEmiField;
            }
            set {
                this.xEmiField = value;
            }
        }
    
        /// <remarks/>
        public TCTeInfCteComplFluxo fluxo {
            get {
                return this.fluxoField;
            }
            set {
                this.fluxoField = value;
            }
        }
    
        /// <remarks/>
        public TCTeInfCteComplEntrega Entrega {
            get {
                return this.entregaField;
            }
            set {
                this.entregaField = value;
            }
        }
    
        /// <remarks/>
        public string origCalc {
            get {
                return this.origCalcField;
            }
            set {
                this.origCalcField = value;
            }
        }
    
        /// <remarks/>
        public string destCalc {
            get {
                return this.destCalcField;
            }
            set {
                this.destCalcField = value;
            }
        }
    
        /// <remarks/>
        public string xObs {
            get {
                return this.xObsField;
            }
            set {
                this.xObsField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ObsCont")]
        public TCTeInfCteComplObsCont[] ObsCont {
            get {
                return this.obsContField;
            }
            set {
                this.obsContField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ObsFisco")]
        public TCTeInfCteComplObsFisco[] ObsFisco {
            get {
                return this.obsFiscoField;
            }
            set {
                this.obsFiscoField = value;
            }
        }
    }
}