using System;
using FusionCore.NfceSincronizador.Contratos;
using FusionCore.NfceSincronizador.Flags;

namespace FusionCore.FusionAdm.Configuracoes
{
    public class Balanca : ISincronizavelAdm
    {
        public byte Id { get; set; } = 1;
        public byte TamanhoCodigo { get; set; } = 4;
        public byte DigitoVerificador { get; set; } = 2;
        public ModoDeOperacao ModoDeOperacao { get; set; } = ModoDeOperacao.Preco;
        public bool Ativo { get; set; }
        public DateTime AlteradoEm { get; set; }
        public byte CasasDecimais { get; set; } = 2;
        public byte InicioQuantificador { get; set; }

        public string Referencia => Id.ToString();

        public EntidadeSincronizavel EntidadeSincronizavel => EntidadeSincronizavel.Balanca;
    }
}
