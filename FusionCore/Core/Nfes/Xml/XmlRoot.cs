using System;
using System.Collections.Generic;
using FusionCore.Core.Nfes.Xml.Componentes.Impl;
using FusionCore.FusionAdm.Fiscal.Flags;
using NFe.Classes.Informacoes;
using static FusionCore.FusionAdm.Fiscal.Flags.ModalidadeFrete;

namespace FusionCore.Core.Nfes.Xml
{
    public class XmlRoot
    {
        public XmlRoot(infNFe inf)
        {
            NumeroDocumento = inf.ide.nNF;
            Serie = (short) inf.ide.serie;
            EmissaoEm = inf.ide.dhEmi.Date;
            EntradaSaidaEm = inf.ide.dhSaiEnt?.DateTime;
            Chave = inf.Id.Substring(3);
            ModalidadeFrete = inf.transp?.modFrete == null 
                ? SemFrete 
                : (ModalidadeFrete) inf.transp.modFrete;
        }

        public string Chave { get; }
        public long NumeroDocumento { get; }
        public short Serie { get; }
        public DateTime EmissaoEm { get; }
        public DateTime? EntradaSaidaEm { get; }
        public XmlPessoa Emitente { get; set; }
        public XmlPessoa Destinatario { get; set; }
        public XmlPessoa Transportadora { get; set; }
        public XmlTotais Totais { get; set; }
        public IList<XmlProduto> Produtos { get; set; }
        public IList<XmlDuplicata> Duplicatas { get; set; }
        public ModalidadeFrete ModalidadeFrete { get; }
    }
}