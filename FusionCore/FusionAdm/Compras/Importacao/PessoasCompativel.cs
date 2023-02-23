using System.Linq;
using FusionCore.Core.Nfes.Xml;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;

namespace FusionCore.FusionAdm.Compras.Importacao
{
    public static class PessoasCompativel
    {
        public static CadastroCompativel Find(XmlRoot xml)
        {
            var cadastro = new CadastroCompativel();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioEmpresa = new RepositorioEmpresa(sessao);
                var repositorioPessoa = new RepositorioPessoa(sessao);

                cadastro.Empresa = repositorioEmpresa.PeloCnpj(xml.Destinatario?.Cnpj);

                var listaFornecedores = repositorioPessoa.PeloDocumentoUnico(xml.Emitente?.DocumentoUnico);
                var listaTransportadora = repositorioPessoa.PeloDocumentoUnico(xml.Transportadora?.DocumentoUnico);

                cadastro.Fornecedor = listaFornecedores.FirstOrDefault(i => i.Ativo && i.Fornecedor != null);
                if (cadastro.Fornecedor == null)
                {
                    cadastro.Fornecedor = listaFornecedores.FirstOrDefault(i => i.Fornecedor != null);
                }

                cadastro.Transportadora = listaTransportadora.FirstOrDefault(t => t.Ativo && t.Transportadora != null);
                if (cadastro.Transportadora == null)
                {
                    cadastro.Transportadora = listaTransportadora.FirstOrDefault(t => t.Transportadora != null);
                }
            }

            return cadastro;
        }
    }
}