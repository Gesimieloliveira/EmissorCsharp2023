using System;
using FusionCore.Repositorio.Filtros;
using FusionLibrary.VisaoModel;

namespace FusionWPF.Dialogos.Models
{
    public class FiltroPeriodoContexto : ViewModel
    {
        public FiltroPeriodoContexto()
        {
            PeriodoInicio = DateTime.Now;
            PeriodoFinal = DateTime.Now;
        }

        public DateTime? PeriodoInicio
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public DateTime? PeriodoFinal
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public event EventHandler<FiltroPeriodo> Confirmou;

        public void Confirmar()
        {
            if (PeriodoInicio == null)
            {
                throw new InvalidOperationException("Preciso que selecione um Inicio!");
            }

            if (PeriodoFinal != null && PeriodoFinal < PeriodoInicio)
            {
                throw new InvalidOperationException("Periodo final não pode ser maior que inicial");
            }

            var inico = (DateTime) PeriodoInicio;
            var fim = PeriodoFinal ?? DateTime.MaxValue;

            Confirmou?.Invoke(this, new FiltroPeriodo(inico, fim));
        }
    }
}