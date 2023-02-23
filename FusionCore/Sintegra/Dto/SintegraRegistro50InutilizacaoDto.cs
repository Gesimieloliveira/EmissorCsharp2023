using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.Sintegra.Dto
{
    public class SintegraRegistro50InutilizacaoDto
    {
        public DateTime EmissaoEm { get; set; }
        public ModeloDocumento ModeloDocumento { get; set; }
        public short Serie { get; set; }
        public int NumeroInicial { get; set; }
        public int NumeroFinal { get; set; }


        public List<Registro50InutilizacaoDto> ObterRegistros50()
        {
            if (NumeroInicial == NumeroFinal)
                return new List<Registro50InutilizacaoDto>
                {
                    new Registro50InutilizacaoDto
                    {
                        Numero = NumeroInicial,
                        Serie = Serie,
                        ModeloDocumento = ModeloDocumento,
                        EmissaoEm = EmissaoEm
                    }
                };


            var listaRegistro50Inutilizacao = new List<Registro50InutilizacaoDto>();

            for (var numeroInicial = NumeroInicial; numeroInicial <= NumeroFinal; numeroInicial++)
            {
                listaRegistro50Inutilizacao.Add(new Registro50InutilizacaoDto
                {
                    Serie = Serie,
                    ModeloDocumento = ModeloDocumento,
                    Numero = numeroInicial,
                    EmissaoEm = EmissaoEm
                });
            }

            return listaRegistro50Inutilizacao;

        }
    }
}