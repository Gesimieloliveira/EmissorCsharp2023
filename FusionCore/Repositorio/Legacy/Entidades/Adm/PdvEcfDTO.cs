using System;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public class PdvEcfDTO : IEntidade
    {
        public Int16 Id { get; set; }
        public Boolean Ativo { get; set; } = true;
        public String NumeroEcf { get; set; }
        public String Serie { get; set; }
        public String Modelo { get; set; }
        public DateTime? AlteradoEm { get; set; }
    }
}