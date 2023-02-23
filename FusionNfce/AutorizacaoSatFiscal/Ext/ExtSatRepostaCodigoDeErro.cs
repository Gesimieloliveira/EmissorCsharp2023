using OpenAC.Net.Sat;

namespace FusionNfce.AutorizacaoSatFiscal.Ext
{
    public static class ExtSatRepostaCodigoDeErro
    {
        public static SatMensagemDeCodigoErro MensagemDoCodigoDeErro(this SatResposta resposta)
        {
            var codigoDeErro = resposta.CodigoDeErro;

            switch (codigoDeErro)
            {
                #region Tabela de erros e alertas - CFe-SAT VEnda
                case 1002:
                    return SatMensagemDeCodigoErro.Cria("Código da UF inválido", 1002, Tipo.Erro, "Código da UF não confere com a Tabela do IBGE", @"Válido até 31/12/2015");
                case 1003:
                    return SatMensagemDeCodigoErro.Cria("Código da UF não Confere com o registro do SAT", 1003, Tipo.Erro, "Código da UF diferente da UF registrada no SAT", @"Válido até 31/12/2015");
                case 1004:
                    return SatMensagemDeCodigoErro.Cria("Validação se o leiaoute está dentre os aceitos pelo SAT", 1004, Tipo.Erro, "Rejeição: Versão do leiaute do arquivo de entrada do SAT não é válida. Nova redação, efeitos a partir de 01.07.17 Rejeição: Versão do leioute do arquivo de entrada do SAT não é válida de acordo com a Tabela de Vigência de Leiaute disponível nos parâmetros de gestão.", @"As alterações serão incorporadas na versão 0.08 do leiaute do CF-e-SAT");
                case 1005:
                    return SatMensagemDeCodigoErro.Cria("Validação se o leiaute de entrada está dentre os aceitos pelo SAT, porém não é a atual", 1005, Tipo.Alerta, "Alerta: Versão do leiaute do arquivo de entrada do SAT não é a mais atual - Nova redação, efeitos a partir de 01.07.17 - Alerta: Versão do leiaute do arquivo de entrada do SAT não é a mais atual de acordo com a Tabela de Vigência de Leiaute disponível nos parâmetros de gestão.", @"As alterações serão incorporadas na versão 0.08 do leiaute do CF-e-SAT");
                case 1226:
                    return SatMensagemDeCodigoErro.Cria("Código da UF do Emitente diverge da UF receptora.", 1226, Tipo.Erro, "Rejeição: Código da UF do Emitente diverge da UF receptora.", @"");
                case 1450:
                    return SatMensagemDeCodigoErro.Cria("Validação se código diferente de 59", 1450, Tipo.Erro, "Rejeição: Código de modelo de documento fiscal diferente de 59", @"");
                case 1258:
                    return SatMensagemDeCodigoErro.Cria("Validação se data/hora é anterior à data/hora do último CF-e-SAT emitido ou cancelado", 1258, Tipo.Erro, "Rejeição: Data/hora inválida. Problemas com o relógio interno do SAT-CF-e", @"");
                case 1224:
                    return SatMensagemDeCodigoErro.Cria("CNPJ com zeros, nulo ou DV inválido", 1224, Tipo.Erro, "Rejeição: CNPJ da Software House inválido", @"");
                case 1455:
                    return SatMensagemDeCodigoErro.Cria("Assinatura do Aplicativo Comercial não é válida.", 1455, Tipo.Erro, "Rejeição: Assinatura do Aplicativo Comercial não é válida", @"");
                case 1207:
                    return SatMensagemDeCodigoErro.Cria("CNPJ do emitente:com zeros, nulo ou DV inválido.", 1207, Tipo.Erro, "Rejeição: CNPJ do emitente inválido", @"");
                case 1203:
                    return SatMensagemDeCodigoErro.Cria("CNPJ do emitente não corresponde ao contrbuinte autorizado para uso do SAT", -1, Tipo.Erro, "Rejeição: Emitente não autorizado para uso do SAT.", @"");
                case 1229:
                    return SatMensagemDeCodigoErro.Cria("IE Emitente com zeros ou nulo", 1229, Tipo.Erro, "Rejeição: IE do emitente não informada", @"");
                case 1230:
                    return SatMensagemDeCodigoErro.Cria("IE não corresponde ao Contribuinte de uso do SAT", 1230, Tipo.Erro, "Rejeição: IE do emitente diferente da IE do contribuinte autorizado para uso do SAT", @"Checar com dado recebido na parametrização do SAT");
                case 1457:
                    return SatMensagemDeCodigoErro.Cria("Regime Especial de Tributação do ISSQN diferente de 1, 2,3, 4, e 5", 1457, Tipo.Erro, "Rejeição: Código de Natureza da Operação para ISSQN inválido", @"");
                case 1507:
                    return SatMensagemDeCodigoErro.Cria("Indicador de rateio do Desconto/Acréscimo sobre subtotal entre itens sujeitos à tributação pelo ISSQN", 1507, Tipo.Erro, "Rejeição: Indicador de rateio para ISSQN inválido", @"");
                case 1235:
                    return SatMensagemDeCodigoErro.Cria("CNPJ do destinatário com zeros, dígito de controle inválido ou igual ao do emitente do CF-e-SAT", 1235, Tipo.Erro, "Rejeição: CNPJ destinatário inválido", @"");
                case 1237:
                    return SatMensagemDeCodigoErro.Cria("CPF do destanatário com zeros ou dígito de controle inválido", 1237, Tipo.Erro, "Rejeição: CPF do destinatário inválido", @"");
                case 1019:
                    return SatMensagemDeCodigoErro.Cria("Validação se numeração dos itens é crescente", 1019, Tipo.Erro, "Rejeião: numeração dos itens não é sequencial crescente", @"");
                case 1459:
                    return SatMensagemDeCodigoErro.Cria("Descrição do produto ou serviço em branco", 1459, Tipo.Erro, "Rejeição: Código do produto ou serviço em branco", @"");
                case 1460:
                    return SatMensagemDeCodigoErro.Cria("Redação atual, efeitos até 30.06.17. --- GTIN não é válido --- Implementação facultativa até 30.06.17 e obrigatória a partir de 01.07.17. --- GTIN não é válido de acordo com o dígito verificador", 1460, Tipo.Erro, "Rejeição: GTIN do item (N) inválido", @"Validação do dígito verificador.");
                case 1461:
                    return SatMensagemDeCodigoErro.Cria("Descrição do produto ou serviço em branco", 1461, Tipo.Erro, "Rejeição: Descrição do produto ou serviço em branco", @"");
                case 1462:
                    return SatMensagemDeCodigoErro.Cria("CFOP não é válido para CF-e-SAT (diferente de 5xxx)", 1462, Tipo.Erro, "Rejeição: CFOP não é de Operação de saída prevista para CF-e-SAT", @"");
                case 1463:
                    return SatMensagemDeCodigoErro.Cria("Unidade Comercial do produto ou serviço em branco", 1463, Tipo.Erro, "Rejeição: Unidade Comercia do produto ou serviço em branco", @"");
                case 1464:
                    return SatMensagemDeCodigoErro.Cria("Quantidade comercial não é maior ou igual a zero", 1464, Tipo.Erro, "Rejeição: Quantidade Comercial do item (N) inválido", @"");
                case 1465:
                    return SatMensagemDeCodigoErro.Cria("Valor unitário não é maior ou igual a zero", 1465, Tipo.Erro, "Rejeição: Valor Unitário do item(N) inválido.", @"");
                case 1467:
                    return SatMensagemDeCodigoErro.Cria("Regra de cálculo inválida (diferente de \"A\" e \"T\")", 1467, Tipo.Erro, "Rejeição: Valor Unitário do item(N) inválido.", @"");
                case 1468:
                    return SatMensagemDeCodigoErro.Cria("Valor do Desconto sobre item não e maior ou igual a zero", 1468, Tipo.Erro, "Rejeição: Valor do Desconto do item(N) inválido.", @"");
                case 1469:
                    return SatMensagemDeCodigoErro.Cria("Outras despesas acessórias sobre item não é maior ou igual a zero", 1469, Tipo.Erro, "Rejeição: Valor de outras despesas acessórias do item (N) inváido.", @"");
                case 1535:
                    return SatMensagemDeCodigoErro.Cria("Código da credenciadora de cartão de débito ou crédito diferente dos previstos no Anexo 3", 1535, Tipo.Erro, "Rejeição: código da credenciadora de cartão de débito ou crédito inválido", @"");
                case 1220:
                    return SatMensagemDeCodigoErro.Cria("Valor do rateio do Desconto sobre o subtotal não é maior que zero", 1220, Tipo.Erro, "Rejeição: Valor do rateio do desconto sobre subtotal do item (N) inválido.", @"");
                case 1228:
                    return SatMensagemDeCodigoErro.Cria("Valor do rateio do acréscimo sobre subtotal não é maior que zero", 1228, Tipo.Erro, "Rejeição: Valor do rateio do desconto sobre subtotal do item(N) inválido", @"");
                case 1751:
                    return SatMensagemDeCodigoErro.Cria("Não informado código do produto com CFOP 5656", 1751, Tipo.Erro, "Rejeição: não informado código do produto.", @"Nova redação, efeitos a partir de 01.07.17.");
                case 1752:
                    return SatMensagemDeCodigoErro.Cria("Código do produto CPOP 5656 fora do padrão ANP.", 1752, Tipo.Erro, "Rejeição: código de produto informado fora do padrão ANP.", @"Nova redação, efeitos a partir de 01.07.17.");
                case 1534:
                    return SatMensagemDeCodigoErro.Cria("Valor não é maior ou igual a zero.", 1534, Tipo.Erro, "Rejeição: Valor aproximado dos tributos do produto negativo.", @"");
                case 1533:
                    return SatMensagemDeCodigoErro.Cria("Valor não é maior ou igual a zero.", 1533, Tipo.Erro, "Rejeição: Valor aproximado dos tributos do CF-e_SAT negativo.", @"");
                case 1471:
                    return SatMensagemDeCodigoErro.Cria("Origem da mercadoria inválida (diferente de 0, 1, 2, 3, 4, 5, 6, 7, 8)", 1471, Tipo.Erro, "Rejeição: Origem da mercadoria do Item (N) inválido (diferente de 0, 1, 2, 3, 4, 5, 6, 7, 8)", @"");
                case 1472:
                    return SatMensagemDeCodigoErro.Cria("Tributação do ICMS inválida (diferente de 00, 20, 90)", 1472, Tipo.Erro, "Rejeição: CST do item (N) inválido (diferente de 00, 20, 90)", @"");
                case 1473:
                    return SatMensagemDeCodigoErro.Cria("Alíquota efetiva do imposto não é maior ou igual a zero.", 1473, Tipo.Erro, "Rejeição: Alíquota efetiva do ICMS do item (N) não é maior ou igual a zero.", @"");
                case 1601:
                    return SatMensagemDeCodigoErro.Cria("Código de regime tributário é incompativel com o grupo de ICMS00.", 1601, Tipo.Alerta, "Alerta: Código de regime tributário é incompatível com o grupo de ICMS00.", @"Implementação facultativa até 30.06.17 e obriatória a partir de 01.07.17.");
                case 1475:
                    return SatMensagemDeCodigoErro.Cria("Tributação do ICMS inválida(diferente de 40 e 41 e 60)", 1475, Tipo.Erro, "Rejeição:CST do Item (N) inválido (diferente de 40 e 41 e 60)", @"");
                case 1602:
                    return SatMensagemDeCodigoErro.Cria("Código de regime tributário é incompatível com o grupo de ICMS40.", 1602, Tipo.Alerta, "Alerta: Código de regime tributário é incompatível com o grupo de ICMS40.", @"Implementação facultativa até 30.06.17 e obrigatória a partir de 01.07.17.");
                case 1476:
                    return SatMensagemDeCodigoErro.Cria("Código de situação da Operação - Simples nacional inválido (diferente de 102, 300, 400 e 500)", 1476, Tipo.Erro, "Rejeição:Código de situação da operação - Simples nacional - do Item (N) inválido (diferente de 102, 300, 400 e 500)", @"");
                case 1603:
                    return SatMensagemDeCodigoErro.Cria("Código de regime tributário é incompatível com o grupo de ICMSN102.", 1603, Tipo.Alerta, "Alerta: Código de regime tributário é incompatível com o grupo de ICMSSN102.", @"Implementação o facultativa até 30.06.17 e obrigatória a partir de 01.07.17.");
                case 1477:
                    return SatMensagemDeCodigoErro.Cria("Código de Situação da Operação - Simples nacional inválido (diferente de 900)", 1477, Tipo.Erro, "Rejeição:Código de situação da operação - Simples Nacional - do Item (N) inválido (diferente de 900)", @"");
                case 1604:
                    return SatMensagemDeCodigoErro.Cria("Código de regime tributário é incompativel com o grupo de ICMSSN900", 1604, Tipo.Alerta, "Alerta: Código de regime tributário é incompatível com o grupo de ICMSSN900.", @"Implementação facultativa até 30.06.17 e obrigatória a partir de 01.07.17.");
                case 1478:
                    return SatMensagemDeCodigoErro.Cria("Código de Situação Tributária do PIS Inválido (diferente de 01, 02 e 05)", 1478, Tipo.Erro, "Rejeição; Código de Situação Tributária do PIS Inválido (diferente de 01, 02 e 05)", @"");
                case 1479:
                    return SatMensagemDeCodigoErro.Cria("Validação de número positivo ou igual a zero.", 1479, Tipo.Erro, "Rejeição: Base de cálculo do PiS do item (N) inválido", @"");
                case 1480:
                    return SatMensagemDeCodigoErro.Cria("Validação de número positivo ou igual a zero.", 1480, Tipo.Erro, "Rejeição: Alíquota do PIS do item (N) não é maior ou igual a zero.", @"");
                case 1482:
                    return SatMensagemDeCodigoErro.Cria("Código de Situação Tributária do PIS Inválido (diferente de 03)", 1482, Tipo.Erro, "Rejeição: Código de Situação Tributária do PIS Inválido (diferente de 03)", @"");
                case 1483:
                    return SatMensagemDeCodigoErro.Cria("Validação de número positivo ou igual a zero.", 1483, Tipo.Erro, "Rejeição: Qtde Vendida do item (N) não é maior ou igual a zero.", @"");
                case 1484:
                    return SatMensagemDeCodigoErro.Cria("Validação de número positivo ou igual a zero.", 1484, Tipo.Erro, "Rejeição: Alíquota do PIS em R$ do item (N) não é maior ou igual a zero.", @"");
                case 1486:
                    return SatMensagemDeCodigoErro.Cria("Código de Situação Tributária do PIS Inválido (diferente de 04, 06, 07, 08 e 09)", -1, Tipo.Erro, "", @"");
                case 1487:
                    return SatMensagemDeCodigoErro.Cria("Código de Situação Tributária do PIS inválido (diferente de 49)", 1487, Tipo.Erro, "Rejeição: Código de Situação tributária do PIS inválido (diferente de 49)", "");
                case 1488:
                    return SatMensagemDeCodigoErro.Cria("Código de Situação Tributária do PIS inválido (diferente de 99)", 1488, Tipo.Erro, "Rejeição: Código de Situação Tributária do PIS inválido (diferente de 99)", "");
                case 1490:
                    return SatMensagemDeCodigoErro.Cria("Código de Situação Tributária da COFINS Inválido (diferente de 01, 02 e 05)", 1490, Tipo.Erro, "Rejeição: Código de Situação Tributária da COFINS Inválido (diferente de 01, 02, e 05)", "");
                case 1491:
                    return SatMensagemDeCodigoErro.Cria("Validação de número positivo ou igual a zero.", 1491, Tipo.Erro, "Rejeição: Base de cálculo do COFINS do item (N) inválido.", "");
                case 1492:
                    return SatMensagemDeCodigoErro.Cria("Validação de número positivo ou igual a zero.", 1492, Tipo.Erro, "Rejeição: Alíquota da COFINS do item (N) não é maior ou igual a zero.", "");
                case 1494:
                    return SatMensagemDeCodigoErro.Cria("Código de Situação Tributária da COFINS Inválido (diferente de 03)", 1494, Tipo.Erro, "Rejeição: Código de Situação Tributária da COFINS Inválido (diferente de 03)", "");
                case 1496:
                    return SatMensagemDeCodigoErro.Cria("Validação de número positivo ou igual a zero.", 1496, Tipo.Erro, "Rejeição: Alíquota da COFINS em R$ do item (N) não é maior ou igual a zero.", "");
                case 1498:
                    return SatMensagemDeCodigoErro.Cria("Código de Situação Tributária da COFINS Inválido (diferente de 04, 06, 07, 08 e 09)", 1498, Tipo.Erro, "Rejeição: Código de Situação Tributária da COFINS Inválido (diferente de 04, 06, 07, 08 e 09)", "");
                case 1499:
                    return SatMensagemDeCodigoErro.Cria("Código de Situação Tributária da COFINS Inválido (diferente de 49)", 1499, Tipo.Erro, "Rejeição: Código de Situação Tributária da COFINS Inválido (diferente de 49)", "");
                case 1500:
                    return SatMensagemDeCodigoErro.Cria("Código de Situação Tributária da COFINS Inválido (diferente de 99)", 1500, Tipo.Erro, "Rejeição: Código de situação Tributária da COFINS Inválido (diferente de 99)", "");
                case 1501:
                    return SatMensagemDeCodigoErro.Cria("Informado grupo de tributação do ISSQN (id:U01) sem informar a IM (id:C13)", 1501, Tipo.Erro, "Rejeição: Operação com tributação de ISSQN sem informar a Inscrição Municipal", "");
                case 1503:
                    return SatMensagemDeCodigoErro.Cria("Validação de número positivo ou igual a zero", 1503, Tipo.Erro, "Rejeição: Valor das deduções para o ISSQN do item(N) não é maior ou igual a zero.", "");
                case 1505:
                    return SatMensagemDeCodigoErro.Cria("Vaidação de número maior ou igual a 2,00(2%) e menor ou igual a 5,00(5%).", 1505, Tipo.Erro, "Rejeição: Alíquota efetivva do ISSQN do item (N) não é maior ou igual a 2,00(2%) e menor ou igual a 5,00(5%).", "");
                case 1287:
                    return SatMensagemDeCodigoErro.Cria("Validação se código do Município do FG - ISSQN com digito inválido. Exceto os códigos descritos no Anexo 2 que presentam dígito inválido.", 1287, Tipo.Erro, "Rejeição: Código Município do FG - ISSQN: dígito inválido. Exceto os códigos descritos no Anexo 2 que apresentam dígito inválido.", "");
                case 1509:
                    return SatMensagemDeCodigoErro.Cria("Se informada TAG, validação de codigo diferente de brancos.", 1509, Tipo.Erro, "Rejeição: Código municipal de Tributação do ISSQN do item (N) em branco.", "");
                case 1510:
                    return SatMensagemDeCodigoErro.Cria("Natureza da Operação de ISSQN diferente de 1, 2, 3, 4, 5, 6, 7 e 8", 1510, Tipo.Erro, "Rejeição: Código de Natureza da Operação para ISSQN inválido.", "");
                case 1511:
                    return SatMensagemDeCodigoErro.Cria("Indicador de Incentivo Fiscal do ISSQN diferente de 1 e 2", 1511, Tipo.Erro, "Rejeição: Indicador de Incentivo Fiscal do ISSQN do item (N) inválido (diferente de 1 e 2)", "");
                case 1527:
                    return SatMensagemDeCodigoErro.Cria("Código do Meio de Pagamento empregado para quitação do CF-e-SAT", 1527, Tipo.Erro, "Rejeição: Valor do Meio de Pagamento inválido.", "");
                case 1528:
                    return SatMensagemDeCodigoErro.Cria("Valor do Meio de Pagamento empregado para quitação do CF-e-SAT, número menor ou igual a zero", 1528, Tipo.Erro, "Rejeição: Valor do Meio de Pagamento inválido.", "");
                case 1408:
                    return SatMensagemDeCodigoErro.Cria("Validação se totalizador menor ou igual ao somatório dos valores de Meio de Pagamento (id:VA03)", 1408, Tipo.Erro, "Rejeição: Valor total do CF-e-SAT maior que o somatório dos valores de Meio de Pagamento empregados em seu pagamento.", "");
                case 1409:
                    return SatMensagemDeCodigoErro.Cria("Validação se conteúdo menor ou igual ao máximo permitido no arquivo de Parametrização de Uso", 1409, Tipo.Erro, "Rejeição: Valor total do CF-e-SAT supera o máximo permitido no arquivo de Parametrização de Uso", "");
                case 1073:
                    return SatMensagemDeCodigoErro.Cria("Se informada TAG, validação de número positivo ou igual a zero.", 1073, Tipo.Erro, "Rejeição: Valor de Desconto sobre total não é maior ou igual a zero.", "");
                case 1074:
                    return SatMensagemDeCodigoErro.Cria("Se informada TAG, validação de número positivo ou igual a zero.", 1074, Tipo.Erro, "Rejeição: Valor de Acréscimo sobre total não é maior ou igual a zero.", "");
                case 1084:
                    return SatMensagemDeCodigoErro.Cria("Formato do Certificado Inválido", 1084, Tipo.Erro, "Formatação do Certificado não é válido.", "");
                case 1085:
                    return SatMensagemDeCodigoErro.Cria("Assinatura do Aplicativo Comercial", 1085, Tipo.Erro, "Assinatura do Aplicativo Comercial não confere com o registro do SAT", "Válido até 31/12/2015");
                case 1998:
                    return SatMensagemDeCodigoErro.Cria("Dados de entrada resultam em valores negativos", 1998, Tipo.Erro, "Rejeição: Não é possível gerar o cupom com os dados de entrada informados, pois resultam valores negativos.", "");
                case 1999:
                    return SatMensagemDeCodigoErro.Cria("Erro desconhecido", 1999, Tipo.Erro, "Rejeição: Erro não identificado", "");
                #endregion

                #region Tabela de erros e Alertas - CF-e-SAT Cancelamento

                case 1270:
                    return SatMensagemDeCodigoErro.Cria("Validação se Chave em branco, zeros ou nulo", 1270, Tipo.Erro, "Rejeição: Chave de acesso do CFe a ser cancelado inválido", "");
                case 1412:
                    return SatMensagemDeCodigoErro.Cria("Validar se o CFe Cancelmaento refere-se a um CFe emitido nos 30 minutos anteriores ao pedido de cancelamento", 1412, Tipo.Erro, "Rejeição: CFe de cancelamento não corresponde a um CFe emitido nos 30 minutos anteriores ao pedido de cancelamento", "");
                case 1210:
                    return SatMensagemDeCodigoErro.Cria("Verificar se o intervalo de tempo entre a emissão do CF-e a ser cancelado e a emissão do respectivo CF-e de cancelamento é não maior que 30 (trinta) minutos.", 1210, Tipo.Erro, "Rejeição: Intervalo de tempo entre a emissão do CF-e a ser cancelado e a emissão do respectivo CF-e de cancelamento é maior que 30 (trinta) minutos.", "");
                case 1454:
                    return SatMensagemDeCodigoErro.Cria("CNPJ com zeros, nulo ou DV inválido", 1454, Tipo.Erro, "Rejeição: CNPJ da Software House inválido", "");
                case 1218:
                    return SatMensagemDeCodigoErro.Cria("CF-e-SAT já está cancelado", 1218, Tipo.Erro, "Chave de acesso do CF-e-SAT já consta como cancelado", "");

                #endregion

                #region Tabela de erros e Alertas - Associação do AC

                case 1451:
                    return SatMensagemDeCodigoErro.Cria("SEFAZ não aceita vinculação do contribuiente como o desenvolvedor do AC informado", 1451, Tipo.Erro, "Rejeição: Houve uma quebra de vinculo entre o CNPJ do contribuiente e o CNPJ do desenvolvedor do AC. Esta restrição impede a vinculação com esses dados.", "");
                case 1540:
                    return SatMensagemDeCodigoErro.Cria("CNPJ da Software House + CNPJ do emitente assinado no campo \"signAC\" difere do informado no campo \"CNPJvalue\" ", 1540, Tipo.Erro, "Rejeição: CNPJ da Software house + CNPJ do emitente assinado no campo \"signAC\" difere do informado no campo \"CNPJvalue\" ", "");
                case 1111:
                    return SatMensagemDeCodigoErro.Cria("Dados informados no processo de assinatura não são válidos conforme controles da retaguarda", 1111, Tipo.Erro, "Rejeição: Dados informados no processo de assinatura não são válidos conforme controles da retaguarda", "");

                #endregion



                default:
                    return SatMensagemDeCodigoErro.Cria("Não catalogado", 9999, Tipo.Erro, "Não catalogado", "Não catalogado");
            }
        }
    }

    public class SatMensagemDeCodigoErro
    {
        private SatMensagemDeCodigoErro() { }

        public string RegraDeValidacao { get; set; }
        public string Descricao { get; set; }
        public string Observacao { get; set; }
        public int CodigoDeErro { get; set; }
        public Tipo Tipo { get; set; }

        public static SatMensagemDeCodigoErro Cria(string regraValidacao, int codigoDeErro, Tipo tipo, string descricao, string observacao)
        {
            return new SatMensagemDeCodigoErro
            {
                Observacao = observacao,
                CodigoDeErro = codigoDeErro,
                Descricao = descricao,
                RegraDeValidacao = regraValidacao,
                Tipo = tipo
            };
        }
    }

    public enum Tipo
    {
        Erro,
        Alerta
    }
}