using System.IO;
using System.Reflection;
using DFe.Classes.Flags;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFe.Classes.Servicos.Tipos;
using NFe.Utils.Validacao;

namespace UnitTest.SchemasTest
{
    [TestClass]
    public class SchemasTestes
    {

        // se teste falhar então modificar arquivo leiauteCCe_v1.00.xsd
		// deletar bloco de código 
		/**
         * <xs:simpleType name="TCOrgaoIBGE">
		<xs:annotation>
			<xs:documentation>Tipo Código de orgão (UF da tabela do IBGE + 90 RFB)</xs:documentation>
		</xs:annotation>
		<xs:restriction base="xs:string">
			<xs:whiteSpace value="preserve"/>
			<xs:enumeration value="11"/>
			<xs:enumeration value="12"/>
			<xs:enumeration value="13"/>
			<xs:enumeration value="14"/>
			<xs:enumeration value="15"/>
			<xs:enumeration value="16"/>
			<xs:enumeration value="17"/>
			<xs:enumeration value="21"/>
			<xs:enumeration value="22"/>
			<xs:enumeration value="23"/>
			<xs:enumeration value="24"/>
			<xs:enumeration value="25"/>
			<xs:enumeration value="26"/>
			<xs:enumeration value="27"/>
			<xs:enumeration value="28"/>
			<xs:enumeration value="29"/>
			<xs:enumeration value="31"/>
			<xs:enumeration value="32"/>
			<xs:enumeration value="33"/>
			<xs:enumeration value="35"/>
			<xs:enumeration value="41"/>
			<xs:enumeration value="42"/>
			<xs:enumeration value="43"/>
			<xs:enumeration value="50"/>
			<xs:enumeration value="51"/>
			<xs:enumeration value="52"/>
			<xs:enumeration value="53"/>
			<xs:enumeration value="90"/>
		</xs:restriction>
	</xs:simpleType>
         */
		[TestMethod]
        public void ValidarSchemaCartaCorrecao()
        {
			var _xmlCartaCorrecao =
			"<envEvento xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"1.00\"><idLote>1</idLote><evento versao=\"1.00\"><infEvento Id=\"ID1101105221102102576000012355001000233229110254046301\"><cOrgao>52</cOrgao><tpAmb>2</tpAmb><CNPJ>21025760000123</CNPJ><chNFe>52211021025760000123550010002332291102540463</chNFe><dhEvento>2021-10-26T09:21:24-03:00</dhEvento><tpEvento>110110</tpEvento><nSeqEvento>1</nSeqEvento><verEvento>1.00</verEvento><detEvento versao=\"1.00\"><descEvento>Carta de Correcao</descEvento><xCorrecao>DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD</xCorrecao><xCondUso>A Carta de Correcao e disciplinada pelo paragrafo 1o-A do art. 7o do Convenio S/N, de 15 de dezembro de 1970 e pode ser utilizada para regularizacao de erro ocorrido na emissao de documento fiscal, desde que o erro nao esteja relacionado com: I - as variaveis que determinam o valor do imposto tais como: base de calculo, aliquota, diferenca de preco, quantidade, valor da operacao ou da prestacao; II - a correcao de dados cadastrais que implique mudanca do remetente ou do destinatario; III - a data de emissao ou de saida.</xCondUso></detEvento></infEvento><Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\"><SignedInfo><CanonicalizationMethod Algorithm=\"http://www.w3.org/TR/2001/REC-xml-c14n-20010315\" /><SignatureMethod Algorithm=\"http://www.w3.org/2000/09/xmldsig#rsa-sha1\" /><Reference URI=\"#ID1101105221102102576000012355001000233229110254046301\"><Transforms><Transform Algorithm=\"http://www.w3.org/2000/09/xmldsig#enveloped-signature\" /><Transform Algorithm=\"http://www.w3.org/TR/2001/REC-xml-c14n-20010315\" /></Transforms><DigestMethod Algorithm=\"http://www.w3.org/2000/09/xmldsig#sha1\" /><DigestValue>t/RZeOC17PGdD7ZATyojjdXmjUA=</DigestValue></Reference></SignedInfo><SignatureValue>u6kzCdMuAbF8Jli9IGhn8OU3iTfXotA9x8JkrLkqZCQQLIKVQTJsA85dbap9BEx2P0CjpnGcKfMR1u+jyjFpGQ7cdd/CIGuLVJ7I3yQaJlCf+xQ6KPxND5JbkTqHEemZQyurZshDm5bv5xEnWtbaj0Iy+sjVozF8Neh/XJhmwflzFCP4hnhL7fIpHC1FplVCDTRbfJ343wBqINrhb/D/+gP3SCE3tHZBYLCbvRGp57ehS+5oG43RnVCWH8eD0rPDD+ikmRbTjmYWxYT+QFMlNSXNAZLxxGAeY0Cmy25InXk2IaPIs8JhTd+MaWvdYxo8dKkRXBqL3LIqlI3BXYdf2Q==</SignatureValue><KeyInfo><X509Data><X509Certificate>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</X509Certificate></X509Data></KeyInfo></Signature></evento></envEvento>";
			var assembyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Validador.Valida(ServicoNFe.RecepcaoEventoCartaCorrecao, VersaoServico.Versao400, _xmlCartaCorrecao, true, Path.Combine(assembyPath, "Assets", "Schemas.Nfe"));
        }

		// se teste falhar então modificar arquivo leiauteConfRecebto_v1.00.xsd
		// deletar bloco de código 
		/**
         * <xs:simpleType name="TCOrgaoIBGE">
		<xs:annotation>
			<xs:documentation>Tipo Código de orgão (UF da tabela do IBGE + 90 RFB)</xs:documentation>
		</xs:annotation>
		<xs:restriction base="xs:string">
			<xs:whiteSpace value="preserve"/>
			<xs:enumeration value="11"/>
			<xs:enumeration value="12"/>
			<xs:enumeration value="13"/>
			<xs:enumeration value="14"/>
			<xs:enumeration value="15"/>
			<xs:enumeration value="16"/>
			<xs:enumeration value="17"/>
			<xs:enumeration value="21"/>
			<xs:enumeration value="22"/>
			<xs:enumeration value="23"/>
			<xs:enumeration value="24"/>
			<xs:enumeration value="25"/>
			<xs:enumeration value="26"/>
			<xs:enumeration value="27"/>
			<xs:enumeration value="28"/>
			<xs:enumeration value="29"/>
			<xs:enumeration value="31"/>
			<xs:enumeration value="32"/>
			<xs:enumeration value="33"/>
			<xs:enumeration value="35"/>
			<xs:enumeration value="41"/>
			<xs:enumeration value="42"/>
			<xs:enumeration value="43"/>
			<xs:enumeration value="50"/>
			<xs:enumeration value="51"/>
			<xs:enumeration value="52"/>
			<xs:enumeration value="53"/>
			<xs:enumeration value="90"/>
		</xs:restriction>
	</xs:simpleType>
         */
		[TestMethod]
        public void ValidarSchemaEventoManifestacao()
        {
			var _xmlManifestacaoCienciaOperacao = "<envEvento versao=\"1.00\" xmlns=\"http://www.portalfiscal.inf.br/nfe\"><idLote>1</idLote><evento versao=\"1.00\"><infEvento Id=\"ID2102105221082102576000012355001000233224111292443401\"><cOrgao>91</cOrgao><tpAmb>2</tpAmb><CNPJ>21025760000123</CNPJ><chNFe>52210821025760000123550010002332241112924434</chNFe><dhEvento>2021-11-03T08:55:33-03:00</dhEvento><tpEvento>210210</tpEvento><nSeqEvento>1</nSeqEvento><verEvento>1.00</verEvento><detEvento versao=\"1.00\"><descEvento>Ciencia da Operacao</descEvento></detEvento></infEvento><Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\"><SignedInfo><CanonicalizationMethod Algorithm=\"http://www.w3.org/TR/2001/REC-xml-c14n-20010315\" /><SignatureMethod Algorithm=\"http://www.w3.org/2000/09/xmldsig#rsa-sha1\" /><Reference URI=\"#ID2102105221082102576000012355001000233224111292443401\"><Transforms><Transform Algorithm=\"http://www.w3.org/2000/09/xmldsig#enveloped-signature\" /><Transform Algorithm=\"http://www.w3.org/TR/2001/REC-xml-c14n-20010315\" /></Transforms><DigestMethod Algorithm=\"http://www.w3.org/2000/09/xmldsig#sha1\" /><DigestValue>d9VxykM/hloW4QzYJOzw4L8+M2Y=</DigestValue></Reference></SignedInfo><SignatureValue>f+ELW3gIyoHOvpE2cYuH+iBmK7oSf+81g4n/WwLjoh133bPRAYHkn0JNPDDqQ5OFGqacIxhGSymIe29oiUN7mJ1EhAA4elPArmLKqyerU4+6QIn9bOPBMWuPMmN4N0X2GbZsXl6fGBpZ8inDW1IB5Bvg1g2t9yTBgi2PtgjkqOyXY+whGgLhwxlURGU27Dh3X0eW4oCLjJyKZXt4rx8BBk7uAx17928toeglcIQIuYtNzbkCW0sURZCTHJXNWK8ak900c3s74G9zMqUkrBRWWiBPfAlH3a1I08+NQVOfTNkOA9RCdBYTzBT/K7dqy5UNdk6ynTHic42hV4ybYwrb9Q==</SignatureValue><KeyInfo><X509Data><X509Certificate>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</X509Certificate></X509Data></KeyInfo></Signature></evento></envEvento>";

			var assembyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Validador.Valida(ServicoNFe.RecepcaoEventoManifestacaoDestinatario, VersaoServico.Versao400, _xmlManifestacaoCienciaOperacao, true, Path.Combine(assembyPath, "Assets", "Schemas.Nfe"));
        }
	}
}