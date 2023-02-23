using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.CteEletronico.Cancelar;
using FusionCore.FusionAdm.CteEletronico.CCe;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.CteEletronicoOs.Cancelamento;
using FusionCore.FusionAdm.CteEletronicoOs.Flags;
using FusionCore.FusionAdm.CteEletronicoOs.Perfil;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using TipoEmissao = FusionCore.FusionAdm.CteEletronico.Flags.TipoEmissao;
using TipoServico = FusionCore.FusionAdm.CteEletronicoOs.Flags.TipoServico;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FusionCore.FusionAdm.CteEletronicoOs.Emissao
{
    public class CteOs : ICancelarCte, ICartaCorrecaoCte
    {
        public CteOs()
        {
            Modal = Modal.Rodoviario;
            Status = Status.Pendente;
            PrecoServico = new PrecoServico();
            LocalInicialPrestacao = new LocalInicialPrestacao();
            LocalFinalPrestacao = new LocalFinalPrestacao();
            Tributacao = new CteOsTributacao();
            Seguros = new List<CteOsSeguro>();
            Percursos = new List<CteOsPercurso>();
            Componentes = new List<CteOsComponenteValorPrestacao>();
            DocumentoReferenciado = new List<CteOsDocumentoReferenciado>();
        }

        public int Id { get; set; }

        public PerfilCteOs Perfil { get; set; }

        public string NaturezaOperacao { get; set; }

        public TipoCte Tipo { get; set; }

        public TipoServico Servico { get; set; }

        public Modal Modal { get; set; }

        public PrecoServico PrecoServico { get; set; }

        public PerfilCfopDTO PerfilCfop { get; set; }

        public string Observacao { get; set; }

        public LocalInicialPrestacao LocalInicialPrestacao { get; set; }

        public LocalFinalPrestacao LocalFinalPrestacao { get; set; }

        public DateTime EmissaoEm { get; set; }

        public CteOsTributacao Tributacao { get; set; }

        public PessoaEntidade Tomador { get; set; }

        public EmpresaDTO Emitente { get; set; }

        public IList<CteOsSeguro> Seguros { get; set; }

        public IList<CteOsPercurso> Percursos { get; set; }

        public IList<CteOsComponenteValorPrestacao> Componentes { get; set; }

        public IList<CteOsDocumentoReferenciado> DocumentoReferenciado { get; set; }

        public Veiculo Veiculo { get; set; }

        public CteOsRodoviario Rodoviario { get; set; }

        public CteOsNormal Normal { get; set; }

        public CteOsEmissaoFinalizada Emissao { get; set; }

        public CteOsImpostoCst TributacaoIcms { get; set; }

        public CteOsTributacaoFederal TributacaoFederal { get; set; }

        public CteOsImpostoDifal TributacaoDifal { get; set; }

        public CteOsConfigImposto ConfigImposto { get; set; }

        public CteOsCancelado Cancelado { get; set; }

        public short SerieEmissao { get; set; }

        public int NumeroEmissao { get; set; }

        public Status Status { get; set; }

        public TipoFretamento TipoFretamento { get; set; } = TipoFretamento.Eventual;
        public DateTime? ViagemEm { get; set; }

        public object GetCte()
        {
            return this;
        }

        public string NumeroFiscal => NumeroEmissao.ToString();
        public string Chave => Emissao.Chave;
        public EstadoDTO Estado => Emitente.EstadoDTO;
        public TipoAmbiente TipoAmbiente => Emissao.AmbienteSefaz;
        public string CnpjOuCpf => Emitente.DocumentoUnicoFormatado;
        public string Protocolo => Emissao.Protocolo;
        public string CnpjCpfEmitente => Emitente.DocumentoUnicoFormatado;
        public EmissorFiscal EmissorFiscal => Perfil.EmissorFiscal;
        public Documento Documento => Documento.CTeOs;
        public TipoEmissao TipoEmissao { get; set; } = TipoEmissao.Normal;
        public void SetHistorico(CteEmissaoHistorico historico)
        {
            //todo CT-E ignored por hora   
        }
    }
}