using System;
using FusionCore.Repositorio.Base;

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public class NcmDTO : Entidade
    {
        private DateTime? _fimVigencia;

        public NcmDTO()
        {
            Cest = string.Empty;
        }

        public string Id { get; set; }
        protected override int ReferenciaUnica => Id.GetHashCode();
        public string Descricao { get; set; }
        public string Cest { get; set; }

        public DateTime? InicioVigencia { get; set; }

        public DateTime? FimVigencia
        {
            get => _fimVigencia;
            set
            {
                _fimVigencia = value;
                DefineVencido();
            }
        }
        public bool Vencido { get; set; }

        public void DefineVencido()
        {
            if (_fimVigencia.HasValue)
            {
                Vencido = DateTime.Now > _fimVigencia.Value;
                return;
            }

            Vencido = false;
        }

        public override string ToString()
        {
            return $"{Id} - {Descricao}";
        }
    }
}