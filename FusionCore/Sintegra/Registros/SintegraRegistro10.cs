using System;
using FusionCore.FusionAdm.EntradaOutras;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using SintegraBr.Classes;

namespace FusionCore.Sintegra.Registros
{
    public class SintegraRegistro10
    {
        private readonly DateTime _dataInicialInformacoes;
        private readonly DateTime _dataFinalInformacoes;
        private readonly EmpresaDTO _empresa;
        private readonly FinalidadeArquivo _codigoFinalidadeArquivo;

        public SintegraRegistro10(DateTime dataInicialInformacoes, DateTime dataFinalInformacoes, EmpresaDTO empresa, FinalidadeArquivo codigoFinalidadeArquivo)
        {
            _dataInicialInformacoes = dataInicialInformacoes;
            _dataFinalInformacoes = dataFinalInformacoes;
            _empresa = empresa;
            _codigoFinalidadeArquivo = codigoFinalidadeArquivo;
        }

        public Registro10 MontaRegistro10()
        {
            var r10 = new Registro10(_empresa.Cnpj.TrimSefaz(14),
                _empresa.InscricaoEstadual.TrimSefaz(14),
                _empresa.RazaoSocial.TrimSefaz(35),
                _empresa.CidadeDTO.Nome.TrimSefaz(30),
                _empresa.CidadeDTO.SiglaUf.TrimSefaz(2),
                "0000000000",
                _dataInicialInformacoes,
                _dataFinalInformacoes,
                ObterCodigoFinalidade(),
                CodEstruturaArquivo.Cod3,
                CodNaturezaOperacoes.Cod3
                );

            return r10;
        }

        private CodFinalidadeArquivo ObterCodigoFinalidade()
        {
            switch (_codigoFinalidadeArquivo)
            {
                case FinalidadeArquivo.Normal:
                    return CodFinalidadeArquivo.Cod1;
                case FinalidadeArquivo.RetificacaoTotalArquivo:
                    return CodFinalidadeArquivo.Cod2;
                case FinalidadeArquivo.RetificacaoAditivaArquivo:
                    return CodFinalidadeArquivo.Cod3;
                case FinalidadeArquivo.Desfazimento:
                    return CodFinalidadeArquivo.Cod5;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}