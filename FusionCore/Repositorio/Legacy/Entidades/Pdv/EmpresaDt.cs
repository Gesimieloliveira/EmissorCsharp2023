using System;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;

namespace FusionCore.Repositorio.Legacy.Entidades.Pdv
{
    public class EmpresaDt : IEntidade
    {
        public int Id { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string Cnpj { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public DateTime? AtividadeIniciadaEm { get; set; }
        public string Email { get; set; }
        public DateTime? CadastradoEm { get; set; }
        public DateTime? AlteradoEm { get; set; }
    }
}