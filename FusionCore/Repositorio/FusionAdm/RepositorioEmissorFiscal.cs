using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Emissores;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Vendas.Autorizadores.Nfce;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

// ReSharper disable RedundantBoolCompare

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioEmissorFiscal : Repositorio<EmissorFiscal, byte>
    {
        private readonly EmissorFiscal _tbEmissor = null;
        private readonly EmissorFiscalNFE _aliasEmissorNfe = null;
        private readonly EmissorFiscalNFCE _aliasEmissorNfce = null;
        private readonly EmissorFiscalCTE _aliasEmissorCte = null;
        private readonly EmissorFiscalCTeOS _aliasEmissorCteOs = null;
        private readonly EmissorFiscalMDFE _aliasEmissorMdfe = null;
        private readonly EmissorFiscalVo _emissorVo;

        public RepositorioEmissorFiscal(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(EmissorFiscal emissorFiscal)
        {
            if (emissorFiscal.Id == 0)
            {
                Sessao.Save(emissorFiscal);
                return;
            }

            Sessao.Merge(emissorFiscal);
        }

        public void Altera(EmissorFiscalNFE emissorNfe)
        {
            Sessao.Update(emissorNfe);
        }

        public void SalvarEmissorFiscalNfce(EmissorFiscalNFCE emissorFiscalNfce)
        {
            Sessao.SaveOrUpdate(emissorFiscalNfce);
        }

        public void SalvarEmissorFiscalSat(EmissorFiscalSAT emissorFiscalSat)
        {
            Sessao.SaveOrUpdate(emissorFiscalSat);
        }

        public void SalvarEmissorFiscalCte(EmissorFiscalCTE emissorFiscalCte)
        {
            Sessao.SaveOrUpdate(emissorFiscalCte);
        }

        public IEnumerable<EmissorFiscalComboBox> BuscaTodosParaComboBox()
        {
            var query = CriaQueryOverEmissorComboBox();
            return query.List<EmissorFiscalComboBox>();
        }

        public IEnumerable<EmissorFiscalVo> BuscaVo()
        {
            var query = Sessao.QueryOver(() => _tbEmissor)
                .SelectList(list => list
                    .Select(() => _tbEmissor.Id).WithAlias(() => _emissorVo.Id)
                    .Select(() => _tbEmissor.Descricao).WithAlias(() => _emissorVo.Descricao)
                );

            query.TransformUsing(Transformers.AliasToBean<EmissorFiscalVo>());

            return query.List<EmissorFiscalVo>();
        }

        private IQueryOver<EmissorFiscal, EmissorFiscal> CriaQueryOverEmissorComboBox()
        {
            EmissorFiscalComboBox result = null;

            var isNfe = Projections.Conditional(
                Restrictions.IsNotNull(Projections.Property(() => _aliasEmissorNfe.EmissorFiscalId)),
                Projections.Constant(true, NHibernateUtil.Boolean),
                Projections.Constant(false, NHibernateUtil.Boolean));

            var isNfce = Projections.Conditional(
                Restrictions.IsNotNull(Projections.Property(() => _aliasEmissorNfce.EmissorFiscalId)),
                Projections.Constant(true, NHibernateUtil.Boolean),
                Projections.Constant(false, NHibernateUtil.Boolean));

            var query = Sessao.QueryOver(() => _tbEmissor)
                .JoinAlias(() => _tbEmissor.EmissorFiscalNfe, () => _aliasEmissorNfe, JoinType.LeftOuterJoin)
                .JoinAlias(() => _tbEmissor.EmissorFiscalNfce, () => _aliasEmissorNfce, JoinType.LeftOuterJoin)
                .JoinAlias(() => _tbEmissor.EmissorFiscalCte, () => _aliasEmissorCte, JoinType.LeftOuterJoin)
                .JoinAlias(() => _tbEmissor.EmissorFiscalCteOs, () => _aliasEmissorCteOs, JoinType.LeftOuterJoin)
                .JoinAlias(() => _tbEmissor.EmissorFiscalMdfe, () => _aliasEmissorMdfe, JoinType.LeftOuterJoin)
                .SelectList(list => list
                    .Select(isNfe).WithAlias(() => result.IsNfe)
                    .Select(isNfce).WithAlias(() => result.IsNfce)
                    .Select(() => _tbEmissor.Id).WithAlias(() => result.Id)
                    .Select(() => _tbEmissor.Descricao).WithAlias(() => result.Descricao));

            query.TransformUsing(Transformers.AliasToBean(typeof(EmissorFiscalComboBox)));

            return query;
        }

        public IEnumerable<EmissorFiscalComboBox> BuscaTodosQueSejamNfeParaComboBox()
        {
            var query = CriaQueryOverEmissorComboBox()
                .Where(Restrictions.IsNotNull(Projections.Property(() => _aliasEmissorNfe.EmissorFiscalId)));

            return query.List<EmissorFiscalComboBox>();
        }

        public IEnumerable<EmissorFiscalComboBox> BuscaTodosQueSejamCteOsParaComboBox()
        {
            var query = CriaQueryOverEmissorComboBox()
                .Where(Restrictions.IsNotNull(Projections.Property(() => _aliasEmissorCteOs.EmissorFiscalId)));

            return query.List<EmissorFiscalComboBox>();
        }

        public IEnumerable<EmissorFiscalComboBox> BuscaTodosQueSejamCteParaComboBox()
        {
            var query = CriaQueryOverEmissorComboBox()
                .Where(Restrictions.IsNotNull(Projections.Property(() => _aliasEmissorCte.EmissorFiscalId)));

            return query.List<EmissorFiscalComboBox>();
        }

        public IEnumerable<EmissorFiscalComboBox> BuscaTodosQueSejamMdfeParaComboBox()
        {
            var query = CriaQueryOverEmissorComboBox()
                .Where(Restrictions.IsNotNull(Projections.Property(() => _aliasEmissorMdfe.EmissorFiscalId)));

            return query.List<EmissorFiscalComboBox>();
        }

        public IList<EmissorFiscal> BuscaEmissorCTe()
        {
            var query = Sessao.QueryOver<EmissorFiscal>().Where(e => e.FlagCte == true || e.FlagCteOs == true);

            var lista = query.List<EmissorFiscal>();

            return lista;
        }

        public IList<EmissorFiscal> BuscaEmissorMDFe()
        {
            var query = Sessao.QueryOver<EmissorFiscal>().Where(e => e.FlagMdfe == true);

            var lista = query.List<EmissorFiscal>();

            return lista;
        }

        public void SalvarEmissorFiscalMdfe(EmissorFiscalMDFE emissorFiscalMdfe)
        {
            Sessao.SaveOrUpdate(emissorFiscalMdfe);
        }

        public IList<EmissorFiscal> BuscaTodosNfe()
        {
            var query = Sessao.QueryOver<EmissorFiscal>().Where(e => e.FlagNfe == true);
            var lista = query.List<EmissorFiscal>();

            return lista;
        }

        public void DeletarEmissorNfceEEmissorSat()
        {
            var queyDeleteSat = Sessao.CreateQuery($"delete from {nameof(EmissorFiscalSAT)}");
            var queryDeleteNfce = Sessao.CreateQuery($"delete from {nameof(EmissorFiscalNFCE)}");

            queyDeleteSat.ExecuteUpdate();
            queryDeleteNfce.ExecuteUpdate();
        }

        public EmissorFiscal BuscarEmissorSat()
        {
            var query = Sessao.QueryOver<EmissorFiscal>().Where(e => e.FlagSat == true);
            var lista = query.List<EmissorFiscal>();

            return lista.FirstOrDefault();
        }

        public EmissorFiscal BuscarEmissorFaturamentoNfcePorEmpresa(EmpresaDTO empresa)
        {
            var query = Sessao.QueryOver<EmissorFiscal>().Where(e => e.IsFaturamento == true);
            query = query.Where(e => e.Empresa == empresa);

            return query.SingleOrDefault();
        }

        public TokenNfce BuscarTokenNfceFaturamento(byte emissorFiscalId)
        {
            EmissorFiscal tabelaEmissorFiscal = null;
            EmissorFiscalNFCE tabelaEmissorFiscalNFCE = null;

            TokenNfce tokenNfce = null;

            var consulta = Sessao.QueryOver(() => tabelaEmissorFiscal)
                .JoinAlias(() => tabelaEmissorFiscal.EmissorFiscalNfce, () => tabelaEmissorFiscalNFCE)
                .SelectList(lista => lista
                    .Select(() => tabelaEmissorFiscalNFCE.IdToken).WithAlias(() => tokenNfce.Token)
                    .Select(() => tabelaEmissorFiscalNFCE.Csc).WithAlias(() => tokenNfce.Csc));

            consulta.TransformUsing(Transformers.AliasToBeanConstructor(typeof(TokenNfce).GetConstructors().First()));

            consulta.Where(emissor => emissor.IsFaturamento == true);
            consulta.Where(emissor => emissor.Id == emissorFiscalId);

            return consulta.SingleOrDefault<TokenNfce>();
        }

        public EmissorFiscal BuscarEmissorDadosCertificado(byte emissorFiscalId)
        {
            EmissorFiscal tabelaEmissorFiscal = null;
            EmissorFiscal resposta = null;

            var consulta = Sessao.QueryOver(() => tabelaEmissorFiscal)
                .SelectList(lista =>
                    lista
                        .Select(() => tabelaEmissorFiscal.TipoCertificadoDigital).WithAlias(() => resposta.TipoCertificadoDigital)
                        .Select(() => tabelaEmissorFiscal.Id).WithAlias(() => resposta.Id)
                        .Select(() => tabelaEmissorFiscal.ArquivoCertificado).WithAlias(() => resposta.ArquivoCertificado)
                        .Select(() => tabelaEmissorFiscal.SenhaCertificado).WithAlias(() => resposta.SenhaCertificado)
                        .Select(() => tabelaEmissorFiscal.SerialNumberCertificado).WithAlias(() => resposta.SerialNumberCertificado)
                        .Select(() => tabelaEmissorFiscal.ProtocoloSeguranca).WithAlias(() => resposta.ProtocoloSeguranca)
                );

            consulta.TransformUsing(Transformers.AliasToBean<EmissorFiscal>());

            consulta.Where(e => e.Id == emissorFiscalId);

            return consulta.SingleOrDefault();
        }

        public bool EmpresaJaEstaVinculada(int empresaId)
        {
            EmissorFiscal emissorFiscalTabela = null;
            EmpresaDTO empresaDTOTabela = null;

            var qtdEmissor = Sessao.QueryOver(() => emissorFiscalTabela)
                .JoinAlias(() => emissorFiscalTabela.Empresa, () => empresaDTOTabela, JoinType.InnerJoin)
                .Where(() => emissorFiscalTabela.IsFaturamento == true)
                .Where(() => empresaDTOTabela.Id == empresaId).RowCount();

            return qtdEmissor != 0;
        }
    }
}