using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.FusionAdm.MdfeEletronico
{
    public class MDFeDescarregamento : EntidadeBase<int>
    {
        private readonly IList<MDFeProdutoPerigoso> _produtosPerigosos;

        private MDFeDescarregamento()
        {
            //nhibernate
            _produtosPerigosos = new List<MDFeProdutoPerigoso>();
        }

        public MDFeDescarregamento(CidadeDTO cidade, string chave, decimal valorTotal, string segundoCodigoBarras = null) 
            : this()
        {
            Cidade = cidade;
            ChaveDocumento = chave;
            SegundoCodigoBarras = segundoCodigoBarras ?? string.Empty;
            ModeloDocumento = ChaveSefazHelper.ExtrairModelo(chave);
            ValorTotal = valorTotal;
            Cancelado = false;

            if (ModeloDocumento != ModeloDocumento.NFe && ModeloDocumento != ModeloDocumento.CTe)
            {
                throw new InvalidOperationException("Preciso de uma Chave de NF-e ou CT-e");
            }

            if (SegundoCodigoBarras.Length > 0 && SegundoCodigoBarras.Length != 36)
            {
                throw new InvalidOperationException("Caso informado Segundo Código o mesmo deve ter 36 digitos");
            }
        }

        public int Id { get; private set; }
        protected override int ChaveUnica => Id;
        public MDFeEletronico Mdfe { get; private set; }
        public CidadeDTO Cidade { get; private set; }
        public ModeloDocumento ModeloDocumento { get; private set; }
        public string ChaveDocumento { get; private set; }
        public string SegundoCodigoBarras { get; private set; }
        public decimal ValorTotal { get; private set; }
        public bool Cancelado { get; private set; }

        public IEnumerable<MDFeProdutoPerigoso> ProdutosPerigosos => _produtosPerigosos;

        public void Anexar(MDFeEletronico mdfe)
        {
            if (Mdfe == mdfe)
            {
                return;
            }

            if (Mdfe != null && Mdfe != mdfe)
            {
                throw new InvalidOperationException("Descarregamento já anexado a outro MDFE");
            }

            Mdfe = mdfe;
        }

        public void AdicionarProdutoPerigoso(MDFeProdutoPerigoso informacao)
        {
            informacao.Anexar(this);

            if (_produtosPerigosos.Any(i => i == informacao))
            {
                return;
            }

            _produtosPerigosos.Add(informacao);
        }

        public void ThrowExceptionSeInvalido(MDFeTipoEmitente tipoEmitente)
        {
            if (tipoEmitente == MDFeTipoEmitente.PrestadorServicoDeTransporte && ModeloDocumento != ModeloDocumento.CTe)
            {
                throw new InvalidOperationException("MDF-e do tipo Prestação de Serviço de Transporte aceita apenas CT-e");
            }

            if (tipoEmitente == MDFeTipoEmitente.TransportadorDeCargaPropria && ModeloDocumento != ModeloDocumento.NFe)
            {
                throw new InvalidOperationException("MDF-e do tipo Transporte de Carga Própria aceita apenas NF-e");
            }
        }

        public void DocumentoCanceladoPelaSefaz()
        {
            Cancelado = true;
        }

        public void ZeraId()
        {
            Id = 0;
        }
    }
}