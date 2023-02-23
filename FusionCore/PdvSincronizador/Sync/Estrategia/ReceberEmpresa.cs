using System;
using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Empresa;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate.Util;

namespace FusionCore.PdvSincronizador.Sync.Estrategia
{
    public class ReceberEmpresa : SincronizacaoBase
    {
        public override string Tag { get; } = "receber-empresa";

        public override void Sincronizar(DateTime ultimaSincronizacao)
        {
            var empresas = ObterEmpresasAlteradas(ultimaSincronizacao);
            if (empresas.Count == 0)
                return;

            var empresasPdv = ConverterAdmParaPdv(empresas);
            Persistir(empresasPdv);
            RegistraEvento = true;
        }

        private IList<EmpresaDTO> ObterEmpresasAlteradas(DateTime ultimaSincronizacao)
        {
            var repositorio = new RepositorioComun<EmpresaDTO>(SessaoAdm);
            var empresas = repositorio.Busca(new EmpresasParaSincronizacao(ultimaSincronizacao));
            return empresas;
        }

        private static IList<EmpresaDt> ConverterAdmParaPdv(IList<EmpresaDTO> empresas)
        {
            if (empresas == null)
                return new List<EmpresaDt>();

            var listaPdv = new List<EmpresaDt>();
            empresas.ForEach(empresa =>
            {
                listaPdv.Add(new EmpresaDt
                {
                    AtividadeIniciadaEm = empresa.AtividadeIniciadaEm,
                    Id = empresa.Id,
                    CadastradoEm = empresa.CadastradoEm,
                    Email = empresa.Email,
                    Cnpj = empresa.Cnpj,
                    InscricaoEstadual = empresa.InscricaoEstadual,
                    InscricaoMunicipal = empresa.InscricaoMunicipal,
                    NomeFantasia = empresa.NomeFantasia,
                    RazaoSocial = empresa.RazaoSocial
                });
            });

            return listaPdv;
        }

        private void Persistir(IList<EmpresaDt> empresas)
        {
            var transacao = SessaoPdv.BeginTransaction();

            try
            {
                var repositorioPdv = new EmpresaRepositorio(SessaoPdv);
                repositorioPdv.SalvarLista(empresas);
                transacao.Commit();
            }
            catch (Exception)
            {
                transacao?.Rollback();
                throw;
            }
        }
    }
}