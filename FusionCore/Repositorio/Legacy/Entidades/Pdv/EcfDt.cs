using System;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;
using FusionCore.Repositorio.Legacy.Flags;

namespace FusionCore.Repositorio.Legacy.Entidades.Pdv
{
    public class EcfDt : IEntidade
    {
        public int Id { get; set; }
        public int Ativo { get; set; } = IntBinario.Sim;
        public string NumeroEcf { get; set; } = string.Empty;
        public string Porta { get; set; } = string.Empty;
        public string Velocidade { get; set; } = string.Empty;
        public string Serie { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string ModeloAcbr { get; set; } = string.Empty;
        public int EmUso { get; set; } = IntBinario.Nao;
        public DateTime? AlteradoEm { get; set; }
        public bool ControlePorta { get; set; }
    }
}