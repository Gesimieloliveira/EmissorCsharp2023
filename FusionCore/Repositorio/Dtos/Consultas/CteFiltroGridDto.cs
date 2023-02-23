using System;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.Helpers.Hidratacao;

namespace FusionCore.Repositorio.Dtos.Consultas
{
    public class CteFiltroGridDto
    {
        public string TextoPesquisado { get; set; } = string.Empty;
        public DateTime? DataEmissaoInicial { get; set; }
        public DateTime? DataEmissaoFinal { get; set; }
        public CteStatus? Status { get; set; }
        public string NumeroDocumento { get; set; } = string.Empty;

        public bool IsEfetuaFiltro()
        {
            return TextoPesquisado.IsNotNullOrEmpty() ||
                   NumeroDocumento.IsNotNullOrEmpty() ||
                   DataEmissaoInicial != null ||
                   DataEmissaoFinal != null ||
                   Status != null;
        }
    }
}