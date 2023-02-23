using System;
using System.Collections.Generic;
using Fusion.Visao.CteEletronicoOs.Emitir.Aba.Model;
using FusionCore.Excecoes.RegraNegocio;
using FusionCore.Facades;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.CteEletronicoOs.Flags;
using FusionCore.FusionAdm.CteEletronicoOs.Perfil;
using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionCore.FusionAdm.Servico.CteEletronicoOs.Perfil;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Flags;
using NFe.Classes;
using NHibernate.Util;
using static System.String;

namespace Fusion.Visao.CteEletronicoOs.Builder
{
    public class CteOsBuilder
    {
        public CteOsBuilder(CteOs cteOs)
        {
            CteOs = cteOs;
        }

        private CteOs CteOs { get; set; }
        public string Taf => CteOs.Rodoviario?.Taf;
        public string NumeroRegistroEstadual => CteOs.Rodoviario?.NumeroDoRegimeEstadual;
        public EmpresaDTO EmpresaEmitente => CteOs.Emitente;

        public void ComPerfil(AbaCteOsPerfilCteOsModel model)
        {
            CteOs.Perfil = BuscarPerfilCteOsPor(model.ItemSelecionado.Id);
        }

        public void ComCabecalho(AbaCteOsCabecalhoCteOsModel model)
        {
            CteOs.Tipo = model.TipoCte;
            CteOs.Servico = model.TipoServico;
            CteOs.Modal = model.Modal;
            CteOs.PrecoServico.Valor = model.ValorServico.Arredondar(2);
            CteOs.PrecoServico.AReceber = model.ValorAReceber.Arredondar(2);
            CteOs.EmissaoEm = model.EmitidaEm ?? DateTime.Now;
            CteOs.Observacao = model.Observacao;
            CteOs.LocalInicialPrestacao.Cidade = model.InicioCidade;
            CteOs.LocalInicialPrestacao.EstadoUF = model.InicioEstado;
            CteOs.LocalFinalPrestacao.Cidade = model.FinalCidade;
            CteOs.LocalFinalPrestacao.EstadoUF = model.FinalEstado;
            CteOs.NaturezaOperacao = model.NaturezaOperacao;
            CteOs.PerfilCfop = model.PerfilCfop;
            CteOs.TipoEmissao = model.TipoEmissao;
            CteOs.TipoFretamento = model.TipoFretamento;
            CteOs.ViagemEm = model.ViagemEm;

            ResolveTributacao(model);
        }

        public void ComEmitenteTomador(AbaEmitenteTomadorModel model)
        {
            CteOs.Tomador = model.Tomador;
            CteOs.Emitente = model.Emitente;
        }

        public CteOs Construir()
        {
            return CteOs;
        }

        private static PerfilCteOs BuscarPerfilCteOsPor(int perfilId)
        {
            using (var repositorio = new RepositorioPerfilCteOs(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                return new ServicoPerfilCteOs(repositorio).Buscar(perfilId);
            }
        }

        private void ResolveTributacao(AbaCteOsCabecalhoCteOsModel model)
        {
            if (model.Ibpt == null) return;

            CteOs.Tributacao.CteOs = CteOs;

            var bc = new BaseCalculo(model.ValorServico);

            var federal = model.Ibpt.ImpostoFederalAproximado(bc);
            var estadual = model.Ibpt.ImpostoEstadualAproximado(bc);
            var total = federal + estadual;

            CteOs.Tributacao.ValorIbpt = total.Arredondar(2);
        }

        public void AtualizarSeguros(AbaServicoSeguroRodoOsModel model)
        {
            CteOs.Seguros.Clear();

            model.ListaSeguro.ForEach(seguroGrid => { CteOs.Seguros.Add(seguroGrid.CteOsSeguro); });
        }

        public void ComVeiculo(Veiculo veiculo)
        {
            CteOs.Veiculo = veiculo;
        }

        public void DeletaRodoviario()
        {
            CteOs.Rodoviario = null;
        }

        public void CriaRodoviarioSeNaoExistir()
        {
            if (CteOs.Rodoviario == null) CteOs.Rodoviario = new CteOsRodoviario(CteOs);
        }

        public void ComTaf(string taf)
        {
            CteOs.Rodoviario.CteOsId = CteOs.Rodoviario.CteOsId;
            CteOs.Rodoviario.Taf = taf;
        }

        public void ComNumeroRegistroEstadual(string numeroRegistroEstadual)
        {
            CteOs.Rodoviario.CteOsId = CteOs.Rodoviario.CteOsId;
            CteOs.Rodoviario.NumeroDoRegimeEstadual = numeroRegistroEstadual;
        }

        public void ComDescricao(string descricao)
        {
            CteOs.Normal.CteOsId = CteOs.Normal.CteOsId;
            CteOs.Normal.CteOs = CteOs;
            CteOs.Normal.DescricaoServicoPrestado = descricao;
        }

        public void ComQuantidade(decimal quantidade)
        {
            CteOs.Normal.CteOsId = CteOs.Normal.CteOsId;
            CteOs.Normal.CteOs = CteOs;
            CteOs.Normal.QuantidadePassageirosVolumes = quantidade;

            CteOs.Normal.DescricaoServicoPrestado.TrimOrEmpty();
        }

        public bool ExisteRodoViario()
        {
            return CteOs.Rodoviario != null;
        }

        public void CriaCteNormalSeNaoExistir()
        {
            if (CteOs.Normal == null) CteOs.Normal = new CteOsNormal();
        }

        public void ComDescricaoServicoPrestado(string descricaoServicoPrestado)
        {
            CteOs.Normal.CteOsId = CteOs.Normal.CteOsId;
            CteOs.Normal.CteOs = CteOs;
            CteOs.Normal.DescricaoServicoPrestado = descricaoServicoPrestado;
        }

        public void ComQuantidadePassageirosVolumes(decimal quantidadePassageirosOuVolumes)
        {
            CteOs.Normal.CteOsId = CteOs.Normal.CteOsId;
            CteOs.Normal.CteOs = CteOs;
            CteOs.Normal.QuantidadePassageirosVolumes = quantidadePassageirosOuVolumes;
        }

        public PerfilCteOs GetPerfil()
        {
            return CteOs.Perfil;
        }

        public void AdicionarSeguro(CteOsSeguro seguro)
        {
            CteOs.Seguros.Add(seguro);
        }

        public bool IsAutorizado()
        {
            return CteOs.Status == Status.Autorizada;
        }

        public void ComTributacao(AbaCTeOsTributacaoModel model)
        {
            CteOs.ConfigImposto = CriaConfigImposto(CteOs, model);
            CteOs.TributacaoIcms = CriaImpostoCst(CteOs, model);
            CteOs.TributacaoDifal = CriaImpostoDifal(CteOs, model);
            AddImpostoFederal(model);
        }

        private CteOsConfigImposto CriaConfigImposto(CteOs cteOs, AbaCTeOsTributacaoModel model)
        {
            var configImposto = new CteOsConfigImposto
            {
                CteOs = cteOs,
                CteOsId = 0,
                IsCalculosAutomaticos = false,
                IsCreditoIcmsAutomatico = false,
                IsPartilha = model.UsarIcmsPartilha,
                UsarTributacaoFederal = model.UsarTributacaoFederal
            };

            if (cteOs.ConfigImposto != null)
                configImposto.CteOsId = cteOs.Id;

            return configImposto;
        }

        private CteOsImpostoCst CriaImpostoCst(CteOs cteOs, AbaCTeOsTributacaoModel model)
        {
            var impostoCst = new CteOsImpostoCst
            {
                BaseCalculo = model.BaseCalculoIcms,
                ValorCredito = model.ValorCredito,
                Aliquota = model.AliquotaIcms,
                PercentualCredito = model.PercentualCredito,
                Valor = model.ValorIcms,
                TributacaoIcms = model.TributacaoSelecionada.ObterTributacao(),
                CteOs = cteOs,
                PercentualReducao = model.PercentualReducaoIcms
            };

            if (cteOs.TributacaoIcms != null)
                impostoCst.CteOsId = cteOs.Id;

            return impostoCst;
        }

        private CteOsImpostoDifal CriaImpostoDifal(CteOs cteOs, AbaCTeOsTributacaoModel model)
        {
            var impostoDifal = new CteOsImpostoDifal
            {
                PercentualFcp = model.PercnetualPartilhaFcp,
                Observacao = model.Observacao ?? Empty,
                ValorIcmsFcp = model.ValorIcmsFcp,
                BaseCalculo = model.PartilhaBaseCalculo,
                PercentualAliquotaInterestadual = model.AliquotaInterestadual,
                PercentualAliquotaInterna = model.AliquotaInternaUFFim,
                PercentualProvisorio = model.PercentualProvisorioUFFim,
                ValorIcmsUfInicio = model.ValorIcmsUFInicio,
                ValorIcmsUfTermino = model.ValorIcmsUFFIm,
                CteOs = cteOs
            };

            if (cteOs.TributacaoDifal != null)
                impostoDifal.CteOsId = cteOs.Id;

            return impostoDifal;
        }

        private void AddImpostoFederal(AbaCTeOsTributacaoModel model)
        {
            if (model.UsarTributacaoFederal == false)
            {
                CteOs.TributacaoFederal = null;
                return;
            }

            CteOs.TributacaoFederal = new CteOsTributacaoFederal
            {
                CteOs = CteOs,
                CteOsId = CteOs.TributacaoFederal?.CteOsId ?? default(int),
                ValorPis = model.ValorPis,
                ValorCofins = model.ValorCofins,
                ValorClss = model.ValorClss,
                ValorImpostoRenda = model.ValorImpostoRenda,
                ValorInss = model.ValorInss
            };
        }

        public bool IsRegimeNormal()
        {
            return GetPerfil().EmissorFiscal.Empresa.RegimeTributario != RegimeTributario.SimplesNacional;
        }

        public bool IsRegimeSimplesNacional()
        {
            return !IsRegimeNormal();
        }


        public void AutoPreencherTaf()
        {
            if (!IsNullOrWhiteSpace(CteOs.Emitente.Taf)) CteOs.Rodoviario.Taf = CteOs.Emitente.Taf;

            if (!IsNullOrWhiteSpace(CteOs.Perfil.Taf)) CteOs.Rodoviario.Taf = CteOs.Perfil.Taf;
        }


        public void AutoPreencherNumeroRegistroEstadual()
        {
            if (!IsNullOrWhiteSpace(CteOs.Emitente.NumeroRegistroEstadual)) CteOs.Rodoviario.NumeroDoRegimeEstadual = CteOs.Emitente.NumeroRegistroEstadual;

            if (!IsNullOrWhiteSpace(CteOs.Perfil.NumeroRegistroEstadual)) CteOs.Rodoviario.NumeroDoRegimeEstadual = CteOs.Perfil.NumeroRegistroEstadual;
        }

        public void Validar()
        {
            if (IsRegimeNormal() && CteOs.TributacaoIcms == null) throw new RegraNegocioException("Tributação ICMS CST obrigatório");

            if (IsNullOrWhiteSpace(CteOs.Emitente?.InscricaoEstadual)) throw new RegraNegocioException("Falta a IE no cadastro da Empresa");

            if (IsNullOrWhiteSpace(CteOs.Normal?.DescricaoServicoPrestado)) throw new InvalidOperationException("Preciso de uma Descrição para o Serviço Prestado");

            if (IsNullOrWhiteSpace(CteOs.Rodoviario?.Taf) &&
                IsNullOrWhiteSpace(CteOs.Rodoviario?.NumeroDoRegimeEstadual))
                throw new InvalidOperationException(
                    "Preciso do TAF ou Número do Registro Estadual (apenas um dos dois)");

            if (!IsNullOrWhiteSpace(CteOs.Rodoviario?.Taf) &&
                !IsNullOrWhiteSpace(CteOs.Rodoviario?.NumeroDoRegimeEstadual))
                throw new InvalidOperationException(
                    "Preciso que informe apenas um dos registros: TAF ou Número do Registro Estadual");

            if (!IsNullOrWhiteSpace(CteOs.Rodoviario?.Taf) && CteOs.Rodoviario.Taf.Length != 12) throw new InvalidOperationException("TAF precisa ter o tamanho exato de 12 números");

            if (!IsNullOrWhiteSpace(CteOs.Rodoviario?.NumeroDoRegimeEstadual) &&
                CteOs.Rodoviario.NumeroDoRegimeEstadual.Length != 25)
                throw new InvalidOperationException(
                    "Número Registro Estadual precisa ter o tamanho exato de 25 números");
        }

        public IEnumerable<CteOsPercurso> ListarPercusos()
        {
            return CteOs.Percursos;
        }

        public void Add(CteOsPercurso percurso)
        {
            percurso.CteOs = CteOs;
            CteOsFacade.Salvar(percurso);

            CteOs.Percursos.Add(percurso);
        }

        public void Remove(CteOsPercurso percurso)
        {
            CteOsFacade.Deletar(percurso);
            CteOs.Percursos.Remove(percurso);
        }

        public void Remove(CteOsComponenteValorPrestacao componente)
        {
            CteOsFacade.Deletar(componente);
            CteOs.Componentes.Remove(componente);
        }

        public void Remove(CteOsDocumentoReferenciado documentoReferenciado)
        {
            CteOsFacade.Deletar(documentoReferenciado);
            CteOs.DocumentoReferenciado.Remove(documentoReferenciado);
        }

        public void Add(CteOsComponenteValorPrestacao componente)
        {
            componente.CteOs = CteOs;
            CteOsFacade.Salvar(componente);
            CteOs.Componentes.Add(componente);
        }

        public void Add(CteOsDocumentoReferenciado documentoReferenciado)
        {
            documentoReferenciado.CteOs = CteOs;
            CteOsFacade.Salvar(documentoReferenciado);
            CteOs.DocumentoReferenciado.Add(documentoReferenciado);
        }

        public IEnumerable<CteOsComponenteValorPrestacao> ListarComponentes()
        {
            return CteOs.Componentes;
        }

        public IEnumerable<CteOsDocumentoReferenciado> ListarDocumentosReferenciados()
        {
            return CteOs.DocumentoReferenciado;
        }

        public bool IsDenegada()
        {
            return CteOs.Status == Status.Denegada;
        }
    }
}