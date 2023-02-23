using System.Collections.Generic;
using FusionCore.FusionNfce.Cidade;
using FusionCore.FusionNfce.Empresa;
using FusionCore.FusionNfce.Uf;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.NfceSincronizador.Sync.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;
using NHibernate.Util;

namespace FusionCore.NfceSincronizador.Sync.Empresas
{
    public class ReceberEmpresa : SincronizavelPadraoAdm
    {
        protected override EntidadeSincronizavel EntidadeSincronizavel { get; } = EntidadeSincronizavel.Empresa;

        protected override void Sincroniza(IList<SincronizacaoPendente> pendentes, ISession sessaoServidor, ISession sessaoNfce)
        {
            var repositorioEmpresaServidor = new RepositorioEmpresa(sessaoServidor);
            var repositorioEmpresaNfce = new RepositorioEmpresaNfce(sessaoNfce);

            pendentes.ForEach(sp =>
            {
                var empresaServidor = repositorioEmpresaServidor.GetPeloId(int.Parse(sp.Referencia));

                repositorioEmpresaNfce.Salvar(new EmpresaNfce
                {
                    AlteradoEm = empresaServidor.AlteradoEm,
                    AtividadeIniciadaEm = empresaServidor.AtividadeIniciadaEm,
                    Bairro = empresaServidor.Bairro,
                    CadastradoEm = empresaServidor.CadastradoEm,
                    Cep = empresaServidor.Cep,
                    Cidade = new CidadeNfce
                    {
                        CodigoIbge = empresaServidor.CidadeDTO.CodigoIbge,
                        Id = empresaServidor.CidadeDTO.Id,
                        Nome = empresaServidor.CidadeDTO.Nome,
                        SiglaUf = empresaServidor.CidadeDTO.SiglaUf
                    },
                    Cnpj = empresaServidor.Cnpj,
                    Complemento = empresaServidor.Complemento,
                    Email = empresaServidor.Email,
                    Estado = new UfNfce
                    {
                        CodigoIbge = empresaServidor.EstadoDTO.CodigoIbge,
                        Id = (byte) empresaServidor.EstadoDTO.Id,
                        Nome = empresaServidor.EstadoDTO.Nome,
                        Sigla = empresaServidor.EstadoDTO.Sigla
                    },
                    Id = empresaServidor.Id,
                    RegimeTributario = empresaServidor.RegimeTributario,
                    Fone1 = empresaServidor.Fone1,
                    Fone2 = empresaServidor.Fone2,
                    InscricaoEstadual = empresaServidor.InscricaoEstadual,
                    InscricaoMunicipal = empresaServidor.InscricaoMunicipal,
                    Logradouro = empresaServidor.Logradouro,
                    NomeFantasia = empresaServidor.NomeFantasia,
                    Numero = empresaServidor.Numero,
                    RazaoSocial = empresaServidor.RazaoSocial,
                    LogoMarca = empresaServidor.LogoMarcaNfce
                });

                SincronizacaoPendentesADeletar.Add(sp);

                ExecutarFlush();
            });
        }
    }
}