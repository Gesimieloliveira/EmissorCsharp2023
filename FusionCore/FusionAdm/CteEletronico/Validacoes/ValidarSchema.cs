﻿using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace FusionCore.FusionAdm.CteEletronico.Validacoes
{
    public class ValidarSchema
    {
        public void Validar(string xml, string caminhoSchema)
        {
            var cfg = new XmlReaderSettings { ValidationType = ValidationType.Schema };

            // Carrega o arquivo de esquema
            var schemas = new XmlSchemaSet();
            cfg.Schemas = schemas;
            // Quando carregar o eschema, especificar o namespace que ele valida
            // e a localização do arquivo 
            schemas.Add(null, caminhoSchema);
            // Especifica o tratamento de evento para os erros de validacao
            cfg.ValidationEventHandler += ValidationEventHandler;
            // cria um leitor para validação
            var validator = XmlReader.Create(new StringReader(xml), cfg);
            try
            {
                // Faz a leitura de todos os dados XML
                while (validator.Read())
                {
                }
            }
            catch (XmlException err)
            {
                // Um erro ocorre se o documento XML inclui caracteres ilegais
                // ou tags que não estão aninhadas corretamente
                throw new Exception("Ocorreu o seguinte erro durante a validação XML:" + "\n" + err.Message);
            }
            finally
            {
                validator.Close();
            }
        }

        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            throw new Exception("Erros da validação : " + e.Message);
        }
    }
}