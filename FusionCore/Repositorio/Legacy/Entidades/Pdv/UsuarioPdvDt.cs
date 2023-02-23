using System;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;

namespace FusionCore.Repositorio.Legacy.Entidades.Pdv
{
    public class UsuarioPdvDt : IEntidade
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public DateTime? CadastradoEm { get; set; }

        public DateTime? AlteradoEm { get; set; }
    }
}
