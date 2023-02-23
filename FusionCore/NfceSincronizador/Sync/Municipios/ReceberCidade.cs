using System.Collections.Generic;
using FusionCore.FusionNfce.Fiscal.Converter;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.Municipios
{
    public class ReceberCidade : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel { get; } = EntidadeSincronizavel.Cidade;

        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes, ISession sessaoServidor, ISession sessaoNfce)
        {
            var repositorioCidadeServidor = new RepositorioCidade(sessaoServidor);
            var repositorioCidadeNfce = new RepositorioCidadeNfce(sessaoNfce);

            pendentes.ForEach(sp =>
            {
                var cidadeServidor = repositorioCidadeServidor.GetPeloId(int.Parse(sp.Referencia));

                repositorioCidadeNfce.Salvar(new ConverterCidadeAdmParaCidadeNfce(cidadeServidor).Execute());

                SincronizacaoPendentesADeletar.Add(sp);

                ExecutarFlush();
            });
        }
    }
}