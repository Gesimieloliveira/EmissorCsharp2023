using System;
using NFe.Servicos.Retorno;

namespace FusionCore.FusionAdm.Fiscal.NF.CCe
{
    public class FalhaEmissaoCce : EventArgs
    {
        public string MotivoFalha { get; set; }
        public int StatusRetornoServico { get; set; }
        public string MotivoFalhaServico { get; set; }
        public int StatusRetonoEvento { get; set; }
        public string MotivoFalhaEvento { get; set; }
        public bool PossuiRetornoServico { get; }
        public bool PossuiRetonroEvento { get; }

        public FalhaEmissaoCce(string motivoFalha)
        {
            MotivoFalha = motivoFalha;
        }

        public FalhaEmissaoCce(RetornoRecepcaoEvento resposta)
        {
            PossuiRetornoServico = true;

            StatusRetornoServico = resposta.Retorno.cStat;
            MotivoFalhaServico = resposta.Retorno.xMotivo;

            var retornoEvento = resposta.Retorno.retEvento;

            if (retornoEvento == null || retornoEvento.Count == 0)
                return;

            PossuiRetonroEvento = true;

            StatusRetonoEvento = retornoEvento[0].infEvento.cStat;
            MotivoFalhaEvento = retornoEvento[0].infEvento.xMotivo;
        }
    }
}