using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Debug;
using FusionCore.ExportacaoPacote.Empacotadores;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.FusionAdm.MdfeEletronico.Autorizador;
using FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Contratos;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.Filtros;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using MDFeEvento = FusionCore.FusionAdm.MdfeEletronico.MDFeEvento;

// ReSharper disable HeuristicUnreachableCode
#pragma warning disable 162

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioMdfe : Repositorio<MDFeEletronico, int>, IRepositorioMdfe, IRepositorioExportacaoXml
    {
        public RepositorioMdfe(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(MDFeEletronico mdfe)
        {
            if (mdfe.Id == 0)
            {
                Sessao.Persist(mdfe);
                Sessao.Flush();
                return;
            }

            Sessao.Update(mdfe);
            Sessao.Flush();
        }

        public void Salvar(MDFeEventoPagamento eventoPagamento)
        {
            Sessao.SaveOrUpdate(eventoPagamento);
        }

        public void Salvar(InformacaoPagamento informacaoPagamento)
        {
            Sessao.SaveOrUpdate(informacaoPagamento);
        }

        public void Salvar(ComponentePagamentoFrete componentePagamentoFrete)
        {
            Sessao.SaveOrUpdate(componentePagamentoFrete);
        }

        public void Salvar(MdfeParcela parcela)
        {
            Sessao.SaveOrUpdate(parcela);
        }

        public void Deletar(ComponentePagamentoFrete componentePagamentoFrete)
        {
            Sessao.Delete(componentePagamentoFrete);
        }

        public void Deletar(MDFeEventoPagamento eventoPagamento)
        {
            Sessao.Delete(eventoPagamento);
        }

        public void Deletar(MdfeParcela parcela)
        {
            Sessao.Delete(parcela);
        }

        public void Deletar(InformacaoPagamento informacaoPagamento)
        {
            Sessao.Delete(informacaoPagamento);
        }

        public void SalvarEmitente(MDFeEmitente emitente)
        {
            Sessao.SaveOrUpdate(emitente);
            Sessao.Flush();
        }

        public void SalvarMunicipioCarregamento(MDFeMunicipioCarregamento municipioCarregamento)
        {
            Sessao.SaveOrUpdate(municipioCarregamento);
            Sessao.Flush();
        }

        public void SalvarPercurso(MDFePercurso percurso)
        {
            Sessao.SaveOrUpdate(percurso);
            Sessao.Flush();
        }

        public void SalvarLacre(MDFeLacre lacres)
        {
            Sessao.SaveOrUpdate(lacres);
            Sessao.Flush();
        }

        public void DeletarMunicipioCarregamento(MDFeMunicipioCarregamento municipioCarregamento)
        {
            Sessao.Delete(municipioCarregamento);
            Sessao.Flush();
        }

        public void DeletarPercurso(MDFePercurso percurso)
        {
            Sessao.Delete(percurso);
            Sessao.Flush();
        }

        public void DeletarLacre(MDFeLacre lacre)
        {
            Sessao.Delete(lacre);
            Sessao.Flush();
        }

        public void SalvarRodoviario(MDFeRodoviario rodoviario)
        {
            Sessao.SaveOrUpdate(rodoviario);
            Sessao.Flush();
        }

        public void SalvarVeiculoTracao(MDFeVeiculoTracao mdfeVeiculoTracao)
        {
            Sessao.SaveOrUpdate(mdfeVeiculoTracao);
            Sessao.Flush();
        }

        public List<MDFeCondutor> BuscarCondutoresPorVeiculoTracao(MDFeVeiculoTracao mdfeVeiculoTracao)
        {
            MDFeCondutor condutorAlias = null;
            MDFeVeiculoTracao veiculoTracaoAlias = null;
            MDFeCondutor resultadoAlias = null;


            var query = Sessao.QueryOver(() => condutorAlias)
                .Inner.JoinAlias(() => condutorAlias.VeiculoTracao, () => veiculoTracaoAlias)
                .SelectList(list => list.Select(() => condutorAlias.Id).WithAlias(() => resultadoAlias.Id))
                .Where(() => veiculoTracaoAlias.RodoviarioId == mdfeVeiculoTracao.RodoviarioId);

            query.TransformUsing(Transformers.AliasToBean<MDFeCondutor>());

            var lista = query.List<MDFeCondutor>();

            return lista.ToList();
        }

        public void DeletarVeiculoTracao(MDFeVeiculoTracao mdfeVeiculoTracao)
        {
            Sessao.Delete(mdfeVeiculoTracao);
            Sessao.Flush();
        }

        public void SalvarCondutor(MDFeCondutor condutor)
        {
            Sessao.SaveOrUpdate(condutor);
            Sessao.Flush();
        }

        public void DeletarCondutor(MDFeCondutor condutor)
        {
            Sessao.Delete(condutor);
            Sessao.Flush();
        }

        public void SalvarVeiculoReboque(MDFeVeiculoReboque mdfeVeiculoReboque)
        {
            Sessao.SaveOrUpdate(mdfeVeiculoReboque);
            Sessao.Flush();
        }

        public void DeletarVeiculoReboque(MDFeVeiculoReboque veiculoReboque)
        {
            Sessao.Delete(veiculoReboque);
            Sessao.Flush();
        }

        public void SalvarValePedagio(MDFeValePedagio valePedagio)
        {
            Sessao.SaveOrUpdate(valePedagio);
            Sessao.Flush();
        }

        public void DeletarValePedagio(MDFeValePedagio valePedagio)
        {
            Sessao.Delete(valePedagio);
            Sessao.Flush();
        }

        public IList<MdfeGridDto> BuscarParaGrid(IFiltro filtro = null)
        {
            MDFeEletronico tbMdfe = null;
            MDFeRodoviario rodoviario = null;
            MDFeVeiculoTracao veiculoTracao = null;
            Veiculo veiculo = null;
            MDFeCondutor condutor = null;
            PessoaEntidade motorista = null;
            EstadoDTO tbEstadoCarreg = null;
            EstadoDTO tbEstadoDesca = null;
            MDFeEmissao tbEmissao = null;
            MDFeEvento tbEvento = null;
            MDFeEmitente tbEmitente = null;
            EmpresaDTO tbEmpresa = null;
            MdfeGridDto alias = null;

            var query = Sessao.QueryOver(() => tbMdfe)
                .JoinAlias(() => tbMdfe.EstadoCarregamento, () => tbEstadoCarreg, JoinType.LeftOuterJoin)
                .JoinAlias(() => tbMdfe.EstadoDescarregamento, () => tbEstadoDesca, JoinType.LeftOuterJoin)
                .JoinAlias(() => tbMdfe.Emissao, () => tbEmissao, JoinType.LeftOuterJoin)
                .JoinAlias(() => tbMdfe.Eventos, () => tbEvento, JoinType.LeftOuterJoin)
                .JoinAlias(() => tbMdfe.Emitente, () => tbEmitente, JoinType.LeftOuterJoin)
                .JoinAlias(() => tbEmitente.Empresa, () => tbEmpresa, JoinType.LeftOuterJoin)
                .JoinAlias(() => tbMdfe.Rodoviario, () => rodoviario, JoinType.LeftOuterJoin)
                .JoinAlias(() => rodoviario.VeiculoTracao, () => veiculoTracao, JoinType.LeftOuterJoin)
                .JoinAlias(() => veiculoTracao.Veiculo, () => veiculo, JoinType.LeftOuterJoin)
                .JoinAlias(() => veiculoTracao.Condutores, () => condutor, JoinType.LeftOuterJoin)
                .JoinAlias(() => condutor.Condutor, () => motorista, JoinType.LeftOuterJoin)
                .SelectList(list => list
                    .Select(() => tbMdfe.Id).WithAlias(() => alias.Id)
                    .Select(() => tbMdfe.EmissaoEm).WithAlias(() => alias.DataEmissao)
                    .Select(() => tbEmissao.NumeroDocumento).WithAlias(() => alias.NumeroDocumento)
                    .Select(() => tbMdfe.Status).WithAlias(() => alias.Status)
                    .Select(() => tbMdfe.TipoEmitente).WithAlias(() => alias.TipoEmitente)
                    .Select(() => tbEstadoCarreg.Nome).WithAlias(() => alias.UFCarregamento)
                    .Select(() => tbEstadoDesca.Nome).WithAlias(() => alias.UFDescarregamento)
                    .Select(() => tbMdfe.ValorTotalCarga).WithAlias(() => alias.ValorTotalCarga)
                    .Select(() => tbMdfe.PesoBrutoCarga).WithAlias(() => alias.PesoBrutoTotalCarga)
                    .Select(() => tbMdfe.QuantidadeCTe).WithAlias(() => alias.QuantidadeCTe)
                    .Select(() => tbMdfe.QuantidadeNFe).WithAlias(() => alias.QuantidadeNFe)
                    .Select(() => tbEmissao.Chave).WithAlias(() => alias.Chave)
                    .Select(() => tbEmissao.XmlAutorizado).WithAlias(() => alias.XmlAutorizado)
                    .Select(() => tbEvento.XmlRetorno).WithAlias(() => alias.XmlCancelamento)
                    .Select(() => tbEmpresa.RazaoSocial).WithAlias(() => alias.NomeEmitente)
                    .Select(() => veiculo.Descricao).WithAlias(() => alias.NomeVeiculoTracao)
                    .Select(() => veiculo.Placa).WithAlias(() => alias.PlacaVeiculoTracao)
                    .Select(() => motorista.Nome).WithAlias(() => alias.NomeMotorista)
                );

            query.TransformUsing(Transformers.AliasToBean<MdfeGridDto>());
            filtro?.Aplicar(query);

            return query.List<MdfeGridDto>();
        }

        public void SalvarEmissao(MDFeEmissao emissao)
        {
            Sessao.SaveOrUpdate(emissao);
            Sessao.Flush();
        }

        public void SalvarEvento(MDFeEvento evento)
        {
            Sessao.SaveOrUpdate(evento);
            Sessao.Flush();
        }

        public MDFeEletronico BuscarPelaChave(string chave)
        {
            MDFeEletronico mdfeAlias = null;
            MDFeEmissao mdfeEmissaoAlias = null;

            var query = Sessao.QueryOver(() => mdfeAlias)
                .Inner.JoinAlias(() => mdfeAlias.Emissao, () => mdfeEmissaoAlias)
                .Where(() => mdfeEmissaoAlias.Chave == chave);

            var mdfe = query.SingleOrDefault<MDFeEletronico>();

            return mdfe;
        }

        public IEnumerable<IEnvelope> BuscarXmlExportacao(DateTime inicio, DateTime fim, EmpresaDTO empresa)
        {
            MDFeEletronico tbMfe = null;
            MDFeEmissao tbEmissao = null;
            MDFeEmitente tbEmitente = null;
            XmlExportacaoMDFe xml = null;

            var query = Sessao.QueryOver(() => tbMfe)
                .SelectList(list => list
                    .Select(() => tbEmissao.Chave).WithAlias(() => xml.Chave)
                    .Select(() => tbEmissao.XmlAutorizado).WithAlias(() => xml.Xml)
                    .Select(() => tbMfe.Status).WithAlias(() => xml.Status))
                .JoinAlias(() => tbMfe.Emissao, () => tbEmissao, JoinType.InnerJoin)
                .JoinAlias(() => tbMfe.Emitente, () => tbEmitente, JoinType.InnerJoin);

            var and = Restrictions.Conjunction();

            var pIsAutorizado = Projections.Property(() => tbEmissao.Autorizado);
            var pData = Projections.Cast(NHibernateUtil.Date, Projections.Property(() => tbEmissao.RecebidoEm));
            var pEmpresa = Projections.Property(() => tbEmitente.Empresa);

            and.Add(Restrictions.Eq(pIsAutorizado, true));
            and.Add(Restrictions.Between(pData, inicio, fim));
            and.Add(Restrictions.Eq(pEmpresa, empresa));

            if (BuildMode.IsProducao)
            {
                and.Add(Restrictions.Eq(Projections.Property(() => tbEmissao.Ambiente), TipoAmbiente.Producao));
            }

            query.Where(and);
            query.TransformUsing(Transformers.AliasToBean<XmlExportacaoMDFe>());

            return query.List<XmlExportacaoMDFe>();
        }

        public void SalvarSeguro(MDFeSeguroCarga seguroCarga)
        {
            Sessao.SaveOrUpdate(seguroCarga);
            Sessao.Flush();
        }

        public void DeletarSeguroCarga(MDFeSeguroCarga seguroCarga)
        {
            Sessao.Delete(seguroCarga);
            Sessao.Flush();
        }

        public void SalvarContratante(MDFeContratante contratante)
        {
            Sessao.SaveOrUpdate(contratante);
            Sessao.Flush();
        }

        public void DeletarContratante(MDFeContratante contratante)
        {
            Sessao.Delete(contratante);
            Sessao.Flush();
        }

        public void SalvarCiot(MDFeCiot mdfeCiot)
        {
            Sessao.SaveOrUpdate(mdfeCiot);
            Sessao.Flush();
        }

        public void DeletarCiot(MDFeCiot ciot)
        {
            Sessao.Delete(ciot);
            Sessao.Flush();
        }

        public new MDFeEletronico Refresh(MDFeEletronico mdfe)
        {
            Sessao.Refresh(mdfe);
            return mdfe;
        }

        public MDFeEmissaoHistorico BuscaUltimaEmissaoHistorico(MDFeEletronico mdfe)
        {
            var query = Sessao.QueryOver<MDFeEmissaoHistorico>()
                .Where(e => e.MDFeEletronico == mdfe && e.Finalizada == false)
                .OrderBy(e => e.Id).Desc.Take(1);

            return query.SingleOrDefault();
        }

        public void SalvarHistorico(MDFeEmissaoHistorico emissaoHistorio)
        {
            Sessao.SaveOrUpdate(emissaoHistorio);
        }

        public void Salvar(MdfeAutorizacaoInformacaoPagamento informacaoPagamento)
        {
            Sessao.SaveOrUpdate(informacaoPagamento);
        }

        public void Salvar(MdfeAutorizacaoComponentePagamentoFrete componentePagamentoFrete)
        {
            Sessao.SaveOrUpdate(componentePagamentoFrete);
        }

        public void Salvar(MdfeAutorizacaoParcela parcela)
        {
            Sessao.SaveOrUpdate(parcela);
        }

        public void Deletar(MdfeAutorizacaoParcela parcela)
        {
            Sessao.Delete(parcela);
        }

        public void Deletar(MdfeAutorizacaoComponentePagamentoFrete componentePagamentoFrete)
        {
            Sessao.Delete(componentePagamentoFrete);
        }

        public void Deletar(MdfeAutorizacaoInformacaoPagamento informacaoPagamento)
        {
            Sessao.Delete(informacaoPagamento);
        }

        public void AtualizarDescarregamento(MDFeDescarregamento descarregamento)
        {
            var hql = $"update {nameof(MDFeDescarregamento)} as m set m.{nameof(descarregamento.Cancelado)} = true where" +
                      $" m.{nameof(descarregamento.Id)} = :id";

            var queryHql = Sessao.CreateQuery(hql);

            queryHql.SetInt32("id", descarregamento.Id);

            var qtd = queryHql.ExecuteUpdate();

            if (qtd > 1)
                throw new InvalidOperationException(
                    $"Era para ter atualizado apenas um descarregamento de id {descarregamento.Id}, verifiquei que foram atualiazados quantidade: {qtd}");
        }
    }
}