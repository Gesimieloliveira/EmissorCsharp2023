using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.MdfeEletronico.Autorizador;
using FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFeEletronico
    {
        private readonly IList<MDFeDescarregamento> _descarregamentos = new List<MDFeDescarregamento>();

        public MDFeEletronico()
        {
            CriadoEm = DateTime.Now;
            Emitente = new MDFeEmitente();
            Rodoviario = new MDFeRodoviario();
            Status = MDFeStatus.EmDigitacao;
            EmissaoEm = DateTime.Now;

            MunicipioCarregamentos = new List<MDFeMunicipioCarregamento>();
            Lacres = new List<MDFeLacre>();
            SeguroCargas = new List<MDFeSeguroCarga>();
            Percursos = new List<MDFePercurso>();
        }

        public int Id { get; set; }
        public MDFeEmitente Emitente { get; set; }
        public MDFeRodoviario Rodoviario { get; set; }
        public EmissorFiscal EmissorFiscal { get; set; }
        public MDFeEmissao Emissao { get; set; }
        public MDFeTipoEmitente TipoEmitente { get; set; }
        public MDFeTipoDoTransportador TipoDoTransportador { get; set; }
        public Modal Modal { get; set; }
        public MDFeProcessoEmissao ProcessoEmissao { get; set; }
        public string VersaoAplicativo { get; set; }
        public EstadoDTO EstadoCarregamento { get; set; }
        public EstadoDTO EstadoDescarregamento { get; set; }
        public int QuantidadeCTe { get; private set; }
        public int QuantidadeNFe { get; private set; }
        public decimal ValorTotalCarga { get; set; }
        public MDFeUnidadeMedida UnidadeMedida { get; set; } = MDFeUnidadeMedida.KG;
        public decimal PesoBrutoCarga { get; set; }
        public string Observacao { get; set; }
        public MDFeTipoEmissao TipoEmissao { get; set; }
        public MDFeStatus Status { get; set; }
        public DateTime? PrevisaoInicioViagemEm { get; set; }
        public bool IsCalcularTotalCargaAutomatico { get; set; } = true;

        public IList<MDFeMunicipioCarregamento> MunicipioCarregamentos { get; set; }
        public IEnumerable<MDFeDescarregamento> Descarregamentos => _descarregamentos;
        public IList<MDFeLacre> Lacres { get; set; }
        public IList<MDFePercurso> Percursos { get; set; }
        public IList<MDFeEvento> Eventos { get; set; }
        public IList<MdfeAutorizacaoInformacaoPagamento> InformacaoPagamentos { get; set; } = new List<MdfeAutorizacaoInformacaoPagamento>();
        public IList<MDFeSeguroCarga> SeguroCargas { get; set; }

        public bool TemEmissao => Emissao != null;
        public DateTime EmissaoEm { get; set; }

        public short SerieEmissao { get; set; }
        public long NumeroFiscalEmissao { get; set; }
        public int CodigoNumericoEmissao { get; set; }
        public bool CargaFechada { get; set; }

        public ProdutoPredominante ProdutoPredominante { get; set; } = new ProdutoPredominante();

        public IList<MDFeEventoPagamento> EventosPagamentos { get; set; } = new List<MDFeEventoPagamento>();
        public DateTime CriadoEm { get; private set; }

        public CategoriaComercialVeiculo CategoriaComercialVeiculo { get; set; } =
            CategoriaComercialVeiculo.VeiculoComercial2Eixos;

        public void AdicionarDescarregamento(MDFeDescarregamento descarregamento)
        {
            if (_descarregamentos.Any(i => descarregamento == i))
            {
                return;
            }

            descarregamento.Anexar(this);

            _descarregamentos.Add(descarregamento);

            AtualizarQuantidades();
        }

        private void AtualizarQuantidades()
        {
            QuantidadeCTe = _descarregamentos.Count(i => i.ModeloDocumento == ModeloDocumento.CTe);
            QuantidadeNFe = _descarregamentos.Count(i => i.ModeloDocumento == ModeloDocumento.NFe);
        }

        public void RemoverDescarregamento(MDFeDescarregamento descarregamento)
        {
            _descarregamentos.Remove(descarregamento);

            AtualizarQuantidades();
        }

        public bool ContemNumeroDocumento()
        {
            return NumeroFiscalEmissao != 0;
        }
    }
}