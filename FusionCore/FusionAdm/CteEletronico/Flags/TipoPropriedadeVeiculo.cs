﻿using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoPropriedadeVeiculo
    {
        [Description("Próprio")]
        Proprio,

        [Description("Terceiro")]
        Terceiro
    }
}