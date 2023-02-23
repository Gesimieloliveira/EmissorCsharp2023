using FusionCore.Core.Nfes.Xml.Componentes.Impl;
using FusionCore.FusionAdm.Pessoas;
using FusionLibrary.Validacao.Regras;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Compras.Importacao.Models
{
    public class PessoaVM : ViewModel
    {
        private readonly XmlPessoa _xml;

        public PessoaVM(XmlPessoa xml, PessoaEntidade pessoa = null)
        {
            _xml = xml;

            PessoaVinculada = pessoa;
            Vinculado = pessoa != null;
            Nome = xml.Nome;
            DocumentoUnico = xml.DocumentoUnico;
            Logradouro = xml.Endereco?.Logradouro;
            Bairro = xml.Endereco?.Bairro;
            Municipio = xml.Endereco?.NomeMunicipio;
            Uf = xml.Endereco?.Uf;
        }

        public PessoaEntidade PessoaVinculada { get; set; }

        public bool Vinculado
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string Nome
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string DocumentoUnico
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Logradouro
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Bairro
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Municipio
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Uf
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool DocumentoIsValido => DocumentoUnicoRegra.IsValido(DocumentoUnico);

        public XmlPessoa GetXml()
        {
            return _xml;
        }
    }
}