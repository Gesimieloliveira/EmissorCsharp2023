using System;
using FusionCore.Repositorio.Base;

namespace FusionCore.ConfiguracaoTransmissao.Nfce.Entidade
{
    public class ConfiguracaoTransmissaoNfce : EntidadeBase<Guid>
    {
        public Guid Guid { get; private set; } = Identificador;
        public Transmissao Transmissao { get; private set; } = Transmissao.Assincrono;
        protected override Guid ChaveUnica => Guid;
        public static Guid Identificador => new Guid("6A6D8E7A-8181-4D4A-8CF6-D78A4F7D6B4B");
    }
}