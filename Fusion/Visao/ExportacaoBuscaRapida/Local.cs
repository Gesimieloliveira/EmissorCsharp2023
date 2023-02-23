using System;
using FusionCore.Repositorio.Base;

namespace Fusion.Visao.ExportacaoBuscaRapida
{
    public class Local : EntidadeBase<Guid>
    {
        public Local(string fullName, int id)
        {
            Localizacao = fullName;
            Id = id;
        }

        public string Localizacao { get; set; }
        public int Id { get; }
        protected override Guid ChaveUnica => Guid.NewGuid();
    }
}