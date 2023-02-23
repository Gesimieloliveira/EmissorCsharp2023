using System.Collections.Generic;
using FusionCore.FusionAdm.MdfeEletronico;
using FusionCore.FusionAdm.MdfeEletronico.Flags;

namespace Fusion.Visao.MdfeEletronico.Aba.Entidades
{
    public class GridSeguroCarga
    {
        public MDFeResponsavelSeguro ResponsavelSeguro { get; set; }
        public string CnpjResponsavel { get; set; }
        public string CpfResponsavel { get; set; }
        public string NomeSeguradora { get; set; }
        public string CnpjSeguradora { get; set; }
        public string NumeroApolice { get; set; }
        public MDFeSeguroCarga MDFeSeguroCarga { get; set; }
        public List<MdfeSeguroAverbacao> Averbacoes { get; set; } = new List<MdfeSeguroAverbacao>();
    }
}