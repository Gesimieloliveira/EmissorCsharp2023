using System;
using FusionCore.Repositorio.Base;

namespace FusionCore.Configuracoes
{
    public class ConfiguracaoEstoqueFaturamento : EntidadeBase<Guid>
    {
        public static Guid IdStatic = new Guid("4064006a-e4b2-429e-a710-9dbc0275a2a7");

        public Guid Id { get; set; } = IdStatic;

        public bool MovimentarEstoque { get; set; } = true;

        protected override Guid ChaveUnica => Id;
    }
}