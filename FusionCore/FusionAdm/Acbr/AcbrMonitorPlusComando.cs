namespace FusionCore.FusionAdm.Acbr
{
    public enum AcbrMonitorPlusComando 
    {
        NFe_ImprimirDANFEPDF,
        NFe_ImprimirDANFE,
        NFe_ImprimirEventoPDF,
        NFe_ValidarNfeRegraNegocios,
        NFe_ImprimirRelatorio,
        CTE_ImprimirDACTePDF,
        CTE_ImprimirEventoPDF,
        MDFE_ImprimirDAMDFEPDF,

        /// <summary>
        /// Parametros
        /// Caminho do arquivo XML,
        /// Nome da impressora
        /// </summary>
        SAT_ImprimirExtratoVenda,

        /// <summary>
        /// Parametros - 
        /// Caminho do arquivo XMl,
        /// Diretorio onde sera salvo o pdf
        /// </summary>
        SAT_gerarpdfextratovenda,

        /// <summary>
        /// Cancelamento Impressão de Extrato
        /// </summary>
        SAT_ImprimirExtratoCancelamento,

        ACBr_LerIni,

        ACBr_DataHora,

        ESCPOS_Ativar,

        ESCPOS_Desativar,

        ESCPOS_ImprimirLinha
    }
}
