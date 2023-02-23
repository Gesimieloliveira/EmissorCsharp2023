using System;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.Repositorio.Dtos.Consultas;

namespace FusionCore.Repositorio.Filtros
{
    public class FiltroDocumentoReceberFaturamento
    {
        public int PessoaId { get; set; }
        public Situacao? Situacao { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime? DataVencimentoInicial { get; set; }
        public DateTime? DataVencimentoFinal { get; set; }
        public EmpresaComboBoxDTO Empresa { get; set; }
    }
}