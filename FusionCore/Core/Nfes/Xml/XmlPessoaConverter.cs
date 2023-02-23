using System;
using FusionCore.Core.Nfes.Xml.Componentes.Impl;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.Fiscal.NF.Componentes;
using FusionCore.FusionAdm.Pessoas;

namespace FusionCore.Core.Nfes.Xml
{
    public class XmlPessoaConverter
    {
        private readonly IRepositorioImportacao _repositorio;

        public XmlPessoaConverter(IRepositorioImportacao repositorio)
        {
            _repositorio = repositorio;
        }

        public PessoaEntidade ParaPessoa(XmlPessoa xml)
        {
            var pessoa = new PessoaEntidade(xml.Nome);

            if (!string.IsNullOrWhiteSpace(xml.Cnpj))
            {
                var cnpj = new Cnpj(xml.Cnpj);
                var nomeFantasia = string.IsNullOrWhiteSpace(xml.NomeFatnasia) ? xml.Nome : xml.NomeFatnasia;

                pessoa.ComoPessoaJuridica(nomeFantasia, cnpj, new InscricaoEstadual(xml.InscricaoEstadual).GetIndicador(), xml.InscricaoEstadual, xml.InscricaoMunicipal);
            }

            if (!string.IsNullOrWhiteSpace(xml.Cpf))
            {
                var cpf = new Cpf(xml.Cpf);
                pessoa.ComoPessoaFisica(cpf, DocumentoRg.Vazio, PessoaSexo.SexoNaoInformado, new InscricaoEstadual(xml.InscricaoEstadual).GetIndicador(), xml.InscricaoEstadual);
            }

            AdicionarEndereco(xml, pessoa);
            AdicionarTelefone(xml, pessoa);
            AdicionarEmail(xml, pessoa);

            return pessoa;
        }

        private void AdicionarTelefone(XmlPessoa xml, PessoaEntidade pessoa)
        {
            if (xml.Telefone?.Length > 11)
            {
                return;
            }

            try
            {
                pessoa.AdicionarTelefone(new PessoaTelefone("TELEFONE", xml.Telefone));
            }
            catch (Exception)
            {
                // ignorar
            }
        }

        private void AdicionarEmail(XmlPessoa xml, PessoaEntidade pessoa)
        {
            if (string.IsNullOrWhiteSpace(xml.Email))
            {
                return;
            }

            try
            {
                var email = new Email(xml.Email.ToLower());
                pessoa.AdicionarEmail(new PessoaEmail(email));
            }
            catch (Exception)
            {
                // ignorar
            }
        }

        private void AdicionarEndereco(XmlPessoa xml, PessoaEntidade pessoa)
        {
            if (xml.Endereco == null)
            {
                return;
            }

            var xEnd = xml.Endereco;
            var cidade = _repositorio.GetCidadePeloIbge((int) xEnd.CodigoMunicipio);

            if (cidade == null)
            {
                return;
            }

            var numero = xEnd.Numero.Length > 10 ? "S/N" : xEnd.Numero;

            var endereco = new PessoaEndereco(xEnd.Logradouro, numero, xEnd.Bairro, xEnd.Cep, cidade)
            {
                Complemento = xEnd.Complemento ?? string.Empty
            };

            pessoa.AdicionarEndereco(endereco);
        }
    }
}