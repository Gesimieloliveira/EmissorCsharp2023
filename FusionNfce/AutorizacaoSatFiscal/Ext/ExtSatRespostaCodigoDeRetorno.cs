using OpenAC.Net.Sat;

namespace FusionNfce.AutorizacaoSatFiscal.Ext
{
    public static class ExtSatRespostaCodigoDeRetorno
    {
        public static SatMensagemDeCodigoRetorno MensagemDoCodigoDeRetorno(this SatResposta resposta)
        {

            if (resposta == null) return null;

            var codigoDeRetorno = resposta.CodigoDeRetorno;

            switch (codigoDeRetorno)
            {

                #region AtivarSat
                case 4000:
                    return SatMensagemDeCodigoRetorno.Cria("Ativado corretamente", "SAT Ativado com Sucesso.", codigoDeRetorno);
                case 4001:
                    return SatMensagemDeCodigoRetorno.Cria("Erro na criação do certificado", "processo de ativação foi interrompido.", codigoDeRetorno);
                case 4002:
                    return SatMensagemDeCodigoRetorno.Cria("SEFAZ não reconhece este SAT (CNPJ inválido)", "Verificar junto a SEFAZ o CNPJ cadastrado.", codigoDeRetorno);
                case 4003:
                    return SatMensagemDeCodigoRetorno.Cria("SAT já ativado ou pendente a Vinculação do AC", "SAT disponível para uso ou pendente de Vinculação do AC.", codigoDeRetorno);
                case 4004:
                    return SatMensagemDeCodigoRetorno.Cria("SAT com uso cessado", "SAT bloqueado por cessação de uso.", codigoDeRetorno);
                case 4005:
                    return SatMensagemDeCodigoRetorno.Cria("Erro de comunicação com a SEFAZ", "Tentar novamente.", codigoDeRetorno);
                case 4006:
                    return SatMensagemDeCodigoRetorno.Cria("CSR ICP-BRASIL criado com sucesso", "Processo de criação do CSR para certificação ICP-BRASIL com sucesso", codigoDeRetorno);
                case 4007:
                    return SatMensagemDeCodigoRetorno.Cria("Erro na criação do CSR ICP-BRASIL", "Processo de criação do CSR para certificacao ICP-BRASIL com erro", codigoDeRetorno);
                case 4098:
                    return SatMensagemDeCodigoRetorno.Cria("SAT em processamento. Tente novamente.", "Em casos onde o SAT estiver processando outra função", codigoDeRetorno);
                case 4099:
                    return SatMensagemDeCodigoRetorno.Cria("Erro desconhecido na ativação", "Informar ao administrador.", codigoDeRetorno);
                case 4129:
                    return SatMensagemDeCodigoRetorno.Cria("Rejeição: Solicitações de emissão de certificados excedidas", "Novas tentativas de ativação para o contribuinte em questão poderão ser realizadas após 30 dias.", codigoDeRetorno);
                case 4200:
                    return SatMensagemDeCodigoRetorno.Cria("Rejeição: Status do Equipamento SAT difere do esperado", "SAT pode não estar vinculado ao contribuinte.", codigoDeRetorno);
                case 4219:
                    return SatMensagemDeCodigoRetorno.Cria("Rejeição: CNPJ não corresponde ao informado no processo de declaração de posse.", "Verificar vinculação e CNPJ usado na ativação.", codigoDeRetorno);
                case 4239:
                    return SatMensagemDeCodigoRetorno.Cria("Rejeição: Versão do arquivo XML não suportada.", "Verificar versão do arquivo XML.", codigoDeRetorno);
                case 4241:
                    return SatMensagemDeCodigoRetorno.Cria("Rejeição: Diferença de transmissão e recebimento da mensagem superior a 5 minutos.", "Possível problema de comunicação com o servidor NTP ou relógio interno do SAT incorreto.", codigoDeRetorno);
                case 4250:
                    return SatMensagemDeCodigoRetorno.Cria("Rejeição: UF informada pelo SAT não é atendida pelo Web Service.", "UF informada de forma errada na ativação.", codigoDeRetorno);
                case 4251:
                    return SatMensagemDeCodigoRetorno.Cria("Rejeição: Certificado enviado não confere com o escolhido na declaração de posse.", "Verificar tipo de certificado, ICP-Brasil ou AC-SAT, no sistema e efetuar a vinculação correta ou corrigir a informação no processo de ativação.", codigoDeRetorno);
                case 4255:
                    return SatMensagemDeCodigoRetorno.Cria("Rejeição: CSR enviado inválido.", "Tentar novamente.", codigoDeRetorno);

                #endregion

                #region AssociarAssinatura

                case 13000:
                    return SatMensagemDeCodigoRetorno.Cria("Assinatura do AC Registrada", "Confirmação de recebimento da SEFAZ", codigoDeRetorno);
                case 13001:
                    return SatMensagemDeCodigoRetorno.Cria("Código ativação inválido", "Verificar o código e tentar mais uma vez.", codigoDeRetorno);
                case 13002:
                    return SatMensagemDeCodigoRetorno.Cria("Erro de comunicação com a SEFAZ", "Não foi possível enviar assinatura à SEFAZ", codigoDeRetorno);
                case 13003:
                    return SatMensagemDeCodigoRetorno.Cria("Assinatura fora do padrão informado", "Corrigir dados", codigoDeRetorno);
                case 13004:
                    return SatMensagemDeCodigoRetorno.Cria("CNPJ da Software House + CNPJ do emitente assinado no campo \"signAC\" difere do informado no campo \"CNPJvalue\" ", "Corrigir dados", codigoDeRetorno);
                case 13007:
                    return SatMensagemDeCodigoRetorno.Cria("CNPJ do emitente difere daquele constante da parametrização de uso.", "Corrigir dados", codigoDeRetorno);
                case 13098:
                    return SatMensagemDeCodigoRetorno.Cria("SAT em processamento. Tente novamente.", "Em casos onde o SAT estiver processando outra função", codigoDeRetorno);
                case 13099:
                    return SatMensagemDeCodigoRetorno.Cria("Erro desconhecido", "Informar o administrador.", codigoDeRetorno);

                #endregion

                #region EnviaDados

                case 6000:
                    return SatMensagemDeCodigoRetorno.Cria("Emitido com sucesso + conteúdo notas.", "Retorno CF-e-SAT ao AC para contingência.", codigoDeRetorno);
                case 6001:
                    return SatMensagemDeCodigoRetorno.Cria("Código de ativação inválido.", "Verificar o código e tentar mais uma vez.", codigoDeRetorno);
                case 6002:
                    return SatMensagemDeCodigoRetorno.Cria("SAT ainda não ativado.", "Efetuar ativação.", codigoDeRetorno);
                case 6003:
                    return SatMensagemDeCodigoRetorno.Cria("SAT não vinculado ao AC", "Efetuar vinculação", codigoDeRetorno);
                case 6004:
                    return SatMensagemDeCodigoRetorno.Cria("Vinculaçãodo do AC não confere", "Efetuar vinculação", codigoDeRetorno);
                case 6005:
                    return SatMensagemDeCodigoRetorno.Cria("Tamanho  do CF-e-SAT superior a 1.500KB", "Dividir CF-e-SAT em dois ou mais documentos.", codigoDeRetorno);
                case 6006:
                    return SatMensagemDeCodigoRetorno.Cria("SAT bloqueado pelo contribuiente", "Não é possível realizar venda", codigoDeRetorno);
                case 6007:
                    return SatMensagemDeCodigoRetorno.Cria("SAT bloqueado pela SEFAZ", "Não é possível realizar venda", codigoDeRetorno);
                case 6008:
                    return SatMensagemDeCodigoRetorno.Cria("SAT bloqueado por falta de comunicação", "Não é possível realizar venda até ser restabelicida a comunicação com a SEFAZ.", codigoDeRetorno);
                case 6009:
                    return SatMensagemDeCodigoRetorno.Cria("SAT bloqueado código de ativação incorreto", "Aguarde o número de minutos conforme tabela de bloqueio em caso de tentativas erradas do código de ativação.", codigoDeRetorno);
                case 6010:
                    return SatMensagemDeCodigoRetorno.Cria("Erro de validação do conteúdo.", "Informar o erro de acordo com a tabela do item 6.3", codigoDeRetorno);
                case 6098:
                    return SatMensagemDeCodigoRetorno.Cria("SAT em processamento. Tente novamente.", "Em caso onde o SAT estiver processando outra função", codigoDeRetorno);
                case 6099:
                    return SatMensagemDeCodigoRetorno.Cria("Erro desconhecido na emissão.", "Informar o administrador.", codigoDeRetorno);

                #endregion

                #region ExtrairLogs

                case 15000:
                    return SatMensagemDeCodigoRetorno.Cria("Transferência completa", "Arquivos de logs extraídos", codigoDeRetorno);
                case 15001:
                    return SatMensagemDeCodigoRetorno.Cria("Código de ativação inválido", "Verificar o código e tentar mais uma vez.", codigoDeRetorno);
                case 15002:
                    return SatMensagemDeCodigoRetorno.Cria("Transferência em andamento", "Aguardar termino de transmissão", codigoDeRetorno);
                case 15098:
                    return SatMensagemDeCodigoRetorno.Cria("SAT em processamento. Tente novamente.", "Em casos onde o SAT estiver processando outra função", codigoDeRetorno);
                case 15099:
                    return SatMensagemDeCodigoRetorno.Cria("Erro desconhecido", "Informar o administrador.", codigoDeRetorno);

                #endregion

                #region BloquearSAT

                case 16000:
                    return SatMensagemDeCodigoRetorno.Cria("Equipamento SAT bloqueado com sucesso.", "Confirmação de bloqueo do SAT", codigoDeRetorno);
                case 16001:
                    return SatMensagemDeCodigoRetorno.Cria("Código de ativação inválido.", "Verificar o código e tentar mais uma vez.", codigoDeRetorno);
                case 16002:
                    return SatMensagemDeCodigoRetorno.Cria("Equipamento SAT já está bloqueado.", "Equipamento SAT já bloqueado.", codigoDeRetorno);
                case 16003:
                    return SatMensagemDeCodigoRetorno.Cria("Erro de comunicação com a SEFAZ", "Não foi possível bloquear o Equipamento SAT.", codigoDeRetorno);
                case 16004:
                    return SatMensagemDeCodigoRetorno.Cria("Não existe parametrização de bloqueio disponível. Verifique configurações na SEFAZ", "O contribuiente não configurou o Equipamento SAT para bloqueio na retaguarda", codigoDeRetorno);
                case 16098:
                    return SatMensagemDeCodigoRetorno.Cria("SAT em processamento. Tente novamente.", "Em casos onde o SAT estiver processando outra função", codigoDeRetorno);
                case 16099:
                    return SatMensagemDeCodigoRetorno.Cria("Erro desconhecido", "Informar o administrador.", codigoDeRetorno);

                #endregion

                #region DesbloquearSAT

                case 17000:
                    return SatMensagemDeCodigoRetorno.Cria("Equipamento SAT desbloqueado com sucesso.", "Confirmação de desbloqueo do SAT", codigoDeRetorno);
                case 17001:
                    return SatMensagemDeCodigoRetorno.Cria("Código de ativação inválido.", "Verificar o código e tentar mais uma vez.", codigoDeRetorno);
                case 17002:
                    return SatMensagemDeCodigoRetorno.Cria("SAT bloqueado pelo contribuiente. Verifique na SEFAZ", "Não foi possível desbloquear o Equipamento SAT. O contribuinte não configurou o Equipamento SAT para desbloqueio na retaguarda", codigoDeRetorno);
                case 17003:
                    return SatMensagemDeCodigoRetorno.Cria("SAT bloeado pela SEFAZ", "Não foi possível desbloquear o Equipamento SAT.", codigoDeRetorno);
                case 17004:
                    return SatMensagemDeCodigoRetorno.Cria("Erro de comunicação com a SEFAZ", "Não foi possível desbloquear o Equipamento SAT.", codigoDeRetorno);
                case 17098:
                    return SatMensagemDeCodigoRetorno.Cria("SAT em processamento. Tente novamente.", "Em casos onde o SAT estiver processando outra função", codigoDeRetorno);
                case 17099:
                    return SatMensagemDeCodigoRetorno.Cria("Erro desconhecido", "Informar o administrador.", codigoDeRetorno);

                #endregion

                #region AtualizarSAT

                case 14000:
                    return SatMensagemDeCodigoRetorno.Cria("Software Atualizado com Sucesso", "Confirmação de atualização do software do SAT", codigoDeRetorno);
                case 14001:
                    return SatMensagemDeCodigoRetorno.Cria("Código de ativação inválido.", "Verificar o código e tentar mais uma vez.", codigoDeRetorno);
                case 14002:
                    return SatMensagemDeCodigoRetorno.Cria("Atualização em Andamento", "SAT em processo de Atualização. Aguardar...", codigoDeRetorno);
                case 14003:
                    return SatMensagemDeCodigoRetorno.Cria("Erro na atualização", "Não foi possível Atualizar o SAT", codigoDeRetorno);
                case 14004:
                    return SatMensagemDeCodigoRetorno.Cria("Arquivo de atualização inválido", "Em casos onde o Hash do arquivo de atualização recebido pelo equipamento não for válido.", codigoDeRetorno);
                case 14098:
                    return SatMensagemDeCodigoRetorno.Cria("SAT em processamento. Tente novamente", "Em casos onde o SAT estiver processando outra função.", codigoDeRetorno);
                case 14099:
                    return SatMensagemDeCodigoRetorno.Cria("Erro desconhecido", "Informar o administrador.", codigoDeRetorno);

                #endregion

                #region ConsultarSAT

                case 8000:
                    return SatMensagemDeCodigoRetorno.Cria("SAT em operação.", "Verifica se o SAT está ativo.", codigoDeRetorno);
                case 8089:
                    return SatMensagemDeCodigoRetorno.Cria("SAT em processamento. Tente novamente.", "Em casos onde o SAT estiver processando outra função", codigoDeRetorno);
                case 8099:
                    return SatMensagemDeCodigoRetorno.Cria("Erro desconhecido", "Informar o administrador.", codigoDeRetorno);

                #endregion

                #region Cancelar Ultima Venda

                case 7000:
                    return SatMensagemDeCodigoRetorno.Cria("Cupom cancelado com sucesso + contéudo CF-e-SAT cancelado.", "Retorno contéudo CF-e-SAT cancelado ao AC.", codigoDeRetorno);
                case 7001:
                    return SatMensagemDeCodigoRetorno.Cria("Código ativação inválido", "Verificar o código e tentar mais uma vez.", codigoDeRetorno);
                case 7002:
                    return SatMensagemDeCodigoRetorno.Cria("Cupom inválido", "Informar o administrador.", codigoDeRetorno);
                case 7004:
                    return SatMensagemDeCodigoRetorno.Cria("Vinculação do AC não confere", "Efetuar vinculação", codigoDeRetorno);
                case 7006:
                    return SatMensagemDeCodigoRetorno.Cria("SAT bloqueado pelo contribuinte", "Não é possível realizar venda.", codigoDeRetorno);
                case 7007:
                    return SatMensagemDeCodigoRetorno.Cria("SAT bloqueado pela SEFAZ", "Não é possível realizar venda", codigoDeRetorno);
                case 7008:
                    return SatMensagemDeCodigoRetorno.Cria("SAT bloqueado por falta de comunicação", "Não é possível realizar venda até ser restabelecida a comunicação com a SEFAZ.", codigoDeRetorno);
                case 7009:
                    return SatMensagemDeCodigoRetorno.Cria("SAT bloqueado, código de ativação incorreto", "Aguarde o número de minutos conforme tabela de bloqueio em caso de tentativas erradas do código de ativação (vide 2.3.7)", codigoDeRetorno);
                case 7010:
                    return SatMensagemDeCodigoRetorno.Cria("Erro de validação do conteúdo", "Informar o erro de acordo com a tabela do item 6.3.2", codigoDeRetorno);
                case 7011:
                    return SatMensagemDeCodigoRetorno.Cria("SAT bloqueado por vencimento do certificado digital.", "Não é possível realizar a venda.", codigoDeRetorno);
                case 7098:
                    return SatMensagemDeCodigoRetorno.Cria("SAT em processamento. Tente novamente.", "Em casos onde o SAT estiver processando outra função", codigoDeRetorno);
                case 7099:
                    return SatMensagemDeCodigoRetorno.Cria("Erro desconhecido no cancelamento.", "Informar o administrador.", codigoDeRetorno);

                #endregion

                #region ConsultarStatusOperacional

                case 10000:
                    return SatMensagemDeCodigoRetorno.Cria("Resposta com Sucesso.", "Informações de status do SAT.", codigoDeRetorno);
                case 10001:
                    return SatMensagemDeCodigoRetorno.Cria("Código de ativação inválido", "Verificar o código e tentar mais uma vez", codigoDeRetorno);
                case 10098:
                    return SatMensagemDeCodigoRetorno.Cria("SAT em processamento. Tente novamente.", "Em casos onde o SAT estiver processando outra função", codigoDeRetorno);
                case 10099:
                    return SatMensagemDeCodigoRetorno.Cria("Erro desconhecido", "Informar o administrador.", codigoDeRetorno);

                #endregion

                #region ComunicarCertificadoDigitalICPBrasil

                case 5000:
                    return SatMensagemDeCodigoRetorno.Cria("Certificado transmitido com Sucesso", "Certificado reconhecido pela SEFAZ", codigoDeRetorno);
                case 5001:
                    return SatMensagemDeCodigoRetorno.Cria("Código de ativação inválido.", "Verificar o código e tentar mais uma vez.", codigoDeRetorno);
                case 5002:
                    return SatMensagemDeCodigoRetorno.Cria("Erro na comunicação com a SEFAZ.", "Tentar novamente.", codigoDeRetorno);
                case 5003:
                    return SatMensagemDeCodigoRetorno.Cria("Certificado Inválido", "Em casos onde o Software Básico ou SEFAZ rejeitar o certificado informado", codigoDeRetorno);
                case 5098:
                    return SatMensagemDeCodigoRetorno.Cria("SAT em processamento. Tente novamente.", "Em casos onde o SAT estiver processando outra função", codigoDeRetorno);
                case 5099:
                    return SatMensagemDeCodigoRetorno.Cria("Erro desconhecido", "Informar o administrador.", codigoDeRetorno);

                #endregion

                #region TrocaCodigoDeAtivacao

                case 18000:
                    return SatMensagemDeCodigoRetorno.Cria("Código de ativação alterado com sucesso.", "Confirmacao de troca do código de ativação", codigoDeRetorno);
                case 18001:
                    return SatMensagemDeCodigoRetorno.Cria("Código de ativação inválido.", "Verificar o código atual e tentar mais uma vez.", codigoDeRetorno);
                case 18002:
                    return SatMensagemDeCodigoRetorno.Cria("Código de ativação de emergência Incorreto.", "Não foi possível alterar o código de ativação.", codigoDeRetorno);
                case 18098:
                    return SatMensagemDeCodigoRetorno.Cria("SAT em processamento. Tente novamente.", "Em casos onde o SAT estiver processando outra função", codigoDeRetorno);
                case 18099:
                    return SatMensagemDeCodigoRetorno.Cria("Erro desconhecido", "Informar o administrador.", codigoDeRetorno);

                #endregion

                default:
                    return SatMensagemDeCodigoRetorno.Cria("Erro na emissão", "Erro na emissão", codigoDeRetorno);
            }
        }
    }

    public class SatMensagemDeCodigoRetorno
    {
        private SatMensagemDeCodigoRetorno() { }

        public string Mensagem { get; set; }
        public string Observacao { get; set; }
        public int CodigoDeRetorno { get; set; }

        public bool AtivacaoSucesso => CodigoDeRetorno == 4000 || CodigoDeRetorno == 4003;

        public static SatMensagemDeCodigoRetorno Cria(string mensagem, string observacao, int codigoDeRetorno)
        {
            return new SatMensagemDeCodigoRetorno
            {
                Mensagem = mensagem,
                Observacao = observacao,
                CodigoDeRetorno = codigoDeRetorno
            };
        }
    }
}