using System;
using FusionCore.Core.Flags;
using FusionCore.Excecoes.RegraNegocio;
using FusionCore.Repositorio.Filtros;
using FusionLibrary.VisaoModel;

namespace FusionWPF.Dialogos.Models
{
    public class FiltroAniversariantesContexto : ViewModel
    {
        private readonly DateTime _now = DateTime.Now;
        private FiltroPeriodoNascimento _periodo;

        internal FiltroAniversariantesContexto()
        {
            MesInicial = (Mes) _now.Month;
            MesFinal = (Mes) _now.Month;
        }

        public Mes MesInicial
        {
            get => GetValue<Mes>();
            set => SetValue(value);
        }

        public Mes MesFinal
        {
            get => GetValue<Mes>();
            set => SetValue(value);
        }

        public void Confirmar()
        {
            if (MesFinal < MesInicial)
            {
                throw new RegraNegocioException("Mês final não pode ser menor que o mês inicial");
            }

            _periodo = new FiltroPeriodoNascimento((int) MesInicial, (int) MesFinal);
        }

        public FiltroPeriodoNascimento PeriodoEscolhido()
        {
            return _periodo;
        }
    }
}