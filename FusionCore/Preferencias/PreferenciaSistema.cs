using FusionCore.Helpers.Maquina;
using System;
using FusionCore.Repositorio.Base;

namespace FusionCore.Preferencias
{
    public sealed class PreferenciaSistema : EntidadeBase<Guid>
    {
        private PreferenciaSistema()
        {
            //nhiberante
        }

        public PreferenciaSistema(string chave, string valor, bool regraGlobal = false) : this()
        {
            Id = Guid.NewGuid();
            Chave = chave;
            Valor = valor;

            IdMaquina = regraGlobal ? Preferencias.Global : GetIdMaquina();
        }

        public Guid Id { get; private set; }
        protected override Guid ChaveUnica => Id;
        public string IdMaquina { get; private set; }
        public string Chave { get; set; }
        public string Valor { get; set; }

        public static string GetIdMaquina() =>  IdMaquinaProvider.Computa();

    }
}