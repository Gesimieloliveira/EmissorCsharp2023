﻿namespace FusionCore.FusionPdv.ModeloEcf
{
    public class BematechMp3000 : ModeloEcfTemplate
    {
        public override ModeloEmissor ModeloAcbrEcf { get; set; } = ModeloEmissor.Bematech;

        public override string ObterModeloEcf { get; set; } = "Bematech MP 3000 TH FI";

        public override string VelocidadeModeloEcf { get; set; } = "9600";
    }
}
