using FusionCore.DFe.XmlCte;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.CteEletronico.Flags.Extencoes;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Helpers.Pessoa;

namespace FusionCore.FusionAdm.CteEletronico.Extencoes
{
    public static class ExtCteTomador
    {

        public static FusionIndicadorPapelTomadorCTe TomadorXml(this CteTomador cteTomador, TipoTomador tipoTomador)
        {
            if (tipoTomador == TipoTomador.Outros) return null;

            var tomador = new FusionIndicadorPapelTomadorCTe
            {
                TipoTomador = tipoTomador.ToXml()
            };

            return tomador;
        }

        public static FusionTomadorOutrosCTe OutrosToXml(this CteTomador cteTomador, TipoTomador tipoTomador)
        {
            if (tipoTomador != TipoTomador.Outros) return null;

            var tomador = new FusionTomadorOutrosCTe();

            var documentoUnico = cteTomador.Tomador.GetDocumentoUnico();

            if (documentoUnico.Length == 11) tomador.Cpf = documentoUnico;
            if (documentoUnico.Length == 14) tomador.Cnpj = documentoUnico;

            var ie = cteTomador.Tomador.InscricaoEstadual;

            tomador.IE = ie.IsNullOrEmpty() ? null : ie;

            tomador.Nome = cteTomador.Tomador.Nome;

            ObtemTelefone(cteTomador, tomador);

            ObtemEndereco(cteTomador, tomador);


            return tomador;
        }

        private static void ObtemEndereco(CteTomador cteTomador, FusionTomadorOutrosCTe tomador)
        {
            var endereco = cteTomador.Tomador.GetEnderecoPrincipal();

            var tomadorEndereco = tomador.EnderecoTomador;

            tomadorEndereco.Logradouro = endereco.Logradouro;
            tomadorEndereco.Numero = endereco.Numero;
            tomadorEndereco.Bairro = endereco.Bairro;
            tomadorEndereco.CodigoIbgeMunicipio = endereco.Cidade.CodigoIbge;
            tomadorEndereco.NomeMunicipio = endereco.Cidade.Nome;
            tomadorEndereco.Cep = endereco.Cep;
            tomadorEndereco.SiglaUf = endereco.Cidade.SiglaUf;
        }

        private static void ObtemTelefone(CteTomador cteTomador, FusionTomadorOutrosCTe tomador)
        {
            var telefone = cteTomador.Tomador.GetPrimeiroTelefone();

            if (telefone != null)
            {
                tomador.Telefone = telefone.Numero;
            }
        }
    }
}