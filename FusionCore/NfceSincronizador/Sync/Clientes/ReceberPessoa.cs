using System.Collections.Generic;
using FusionCore.FusionNfce.Fiscal.Converter;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.Clientes
{
    public class ReceberPessoa : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel { get; } = EntidadeSincronizavel.Pessoa;

        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes,
            ISession sessaoServidor,
            ISession sessaoNfce)
        {
            var repositorioPessoaServidor = new RepositorioPessoa(sessaoServidor);
            var repositorioClienteNfce = new RepositorioPessoaNfce(sessaoNfce);

            pendentes.ForEach(sp =>
            {
                var pessoa = repositorioPessoaServidor.GetPeloId(int.Parse(sp.Referencia));

                if (pessoa.Cliente != null)
                {
                    var cliente = new ConverterClienteAdmParaClienteNFCe(pessoa).Executar();

                    repositorioClienteNfce.Salvar(cliente);
                }

                if (pessoa.Vendedor != null)
                {
                    var vendedor = new ConverterVendedorAdmParaNfce(pessoa).Converter();

                    repositorioClienteNfce.Salvar(vendedor);
                }

                SincronizacaoPendentesADeletar.Add(sp);

                ExecutarFlush();
            });
        }
    }
}