using DFe.Classes.Extensoes;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Transporte;

namespace FusionCore.Core.Nfes.Xml.Componentes.Impl
{
    public class XmlPessoa
    {
        public XmlPessoa(emit emit)
        {
            InscricaoEstadual = emit.IE;
            InscricaoMunicipal = emit.IM;
            Cnpj = emit.CNPJ;
            Nome = emit.xNome?.ToUpper();
            NomeFatnasia = emit.xFant?.ToUpper();
            Cpf = emit.CPF;
            Telefone = emit.enderEmit.fone?.ToString();
            Endereco = CriaEnderecoEmitente(emit.enderEmit);
        }

        public XmlPessoa(dest dest)
        {
            InscricaoEstadual = dest.IE;
            InscricaoMunicipal = dest.IM;
            Cnpj = dest.CNPJ;
            Nome = dest.xNome?.ToUpper();
            Cpf = dest.CPF;
            Email = dest.email;
            Telefone = dest.enderDest.fone?.ToString();
            Endereco = CriaEnderecoDestinatario(dest.enderDest);
        }

        public XmlPessoa(transp transp)
        {
            Cnpj = transp.transporta.CNPJ;
            Cpf = transp.transporta.CPF;
            InscricaoEstadual = transp.transporta.IE;
            Nome = transp.transporta.xNome?.ToUpper();
        }

        public XmlPessoa()
        {
        }

        public string Cnpj { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string NomeFatnasia { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public XmlEndereco Endereco { get; set; }
        public string DocumentoUnico => !string.IsNullOrWhiteSpace(Cnpj) ? Cnpj : Cpf;

        private XmlEndereco CriaEnderecoDestinatario(enderDest endereco)
        {
            return new XmlEndereco
            {
                Bairro = endereco.xBairro?.ToUpper(),
                CodigoMunicipio = endereco.cMun,
                Logradouro = endereco.xLgr?.ToUpper(),
                NomeMunicipio = endereco.xMun?.ToUpper(),
                Complemento = endereco.xCpl?.ToUpper(),
                Numero = endereco.nro,
                Uf = endereco.UF,
                Cep = endereco.CEP
            };
        }

        private XmlEndereco CriaEnderecoEmitente(enderEmit endereco)
        {
            return new XmlEndereco
            {
                Bairro = endereco.xBairro?.ToUpper(),
                Cep = endereco.CEP,
                CodigoMunicipio = endereco.cMun,
                Logradouro = endereco.xLgr?.ToUpper(),
                NomeMunicipio = endereco.xMun?.ToUpper(),
                Numero = endereco.nro,
                Uf = endereco.UF.GetSiglaUfString(),
                Complemento = endereco.xCpl?.ToUpper()
            };
        }
    }
}