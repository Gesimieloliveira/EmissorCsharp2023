using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Core.Nfes.Xml.Componentes;
using FusionCore.Core.Nfes.Xml.Componentes.Impl;
using FusionCore.Extencoes;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using NFe.Classes.Informacoes.Transporte;
using NFe.Utils.NFe;
using static System.String;
using ZeusNfe = NFe.Classes.NFe;

namespace FusionCore.Core.Nfes.Xml
{
    public class LeitorImportacaoXml
    {
        private readonly string _conteudoXml;

        public LeitorImportacaoXml(string conteudoXml)
        {
            _conteudoXml = conteudoXml;
        }

        public XmlRoot Ler()
        {
            var znfe = new ZeusNfe().CarregarDeXmlString(_conteudoXml);

            if (znfe.infNFe.ide.tpNF != TipoNFe.tnSaida)
            {
                throw new InvalidOperationException("Consigo importar apenas NF-E de saida");
            }

            var xmlRoot = new XmlRoot(znfe.infNFe)
            {
                Emitente = CriaEmitente(znfe.infNFe.emit),
                Destinatario = CriaDestinatario(znfe.infNFe.dest),
                Transportadora = CriaTransportadora(znfe.infNFe.transp),
                Produtos = CriaListaProdutos(znfe.infNFe.det, znfe.infNFe.emit.CRT),
                Totais = new XmlTotais(znfe.infNFe.total.ICMSTot)
            };

            xmlRoot.Duplicatas = new List<XmlDuplicata>();

            if (!znfe.infNFe.cobr.IsNotNull()) return xmlRoot;

            var dup = znfe.infNFe.cobr.dup;
            if (dup.IsNotNull())
            {
                dup.ForEach(x =>
                {
                    xmlRoot.Duplicatas.Add(new XmlDuplicata(x));
                });
            }

            return xmlRoot;
        }

        private XmlPessoa CriaEmitente(emit emit)
        {
            return new XmlPessoa(emit);
        }

        private XmlPessoa CriaDestinatario(dest dest)
        {
            return new XmlPessoa(dest);
        }

        private XmlPessoa CriaTransportadora(transp transp)
        {
            return IsNullOrWhiteSpace(transp?.transporta?.xNome) 
                ? null 
                : new XmlPessoa(transp);
        }

        private IList<XmlProduto> CriaListaProdutos(IList<det> det, CRT crt)
        {
            return det.Select(p => CriaProduto(p, crt)).ToList();
        }

        private XmlProduto CriaProduto(det det, CRT crt)
        {
            var icms = crt == CRT.SimplesNacional
                ? (IXmlIcms) new XmlCsoson(det.imposto.ICMS)
                : new XmlIcms(det.imposto.ICMS);

            var produto = new XmlProduto(det.prod)
            {
                Icms = icms
            };

            if (det.imposto.IPI != null)
            {
                produto.Ipi = new XmlIpi(det.imposto.IPI);
            }

            if (det.imposto.PIS != null)
            {
                produto.Pis = new XmlPis(det.imposto.PIS);
            }

            if (det.imposto.COFINS != null)
            {
                produto.Cofins = new XmlCofins(det.imposto.COFINS);
            }

            return produto;
        }
    }
}