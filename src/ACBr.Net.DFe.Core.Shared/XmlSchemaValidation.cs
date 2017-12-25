using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace ACBr.Net.DFe.Core
{
    /// <summary>
    /// Classe estatica para validação de xml usando schema.
    /// </summary>
    public static class XmlSchemaValidation
    {
        /// <summary>
        /// Valida o arquivo XML com o schema informado.
        /// </summary>
        /// <param name="arquivoXml">The arquivo XML.</param>
        /// <param name="schema">The schema nf.</param>
        /// <param name="erros">The erro.</param>
        /// <param name="avisos">The avisos.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool ValidarXml(string arquivoXml, string schema, out string[] erros, out string[] avisos)
        {
            var errorList = new List<string>();
            var avisosList = new List<string>();

            if (string.IsNullOrEmpty(arquivoXml))
            {
                errorList.Add("Arquivo Xml não encontrado.");
                erros = errorList.ToArray();
                avisos = avisosList.ToArray();
                return false;
            }

            if (!File.Exists(schema))
            {
                errorList.Add("Arquivo de Schema não encontrado.");
                erros = errorList.ToArray();
                avisos = avisosList.ToArray();
                return false;
            }

            try
            {
                var xmlSchema = XmlSchema.Read(new XmlTextReader(schema), (sender, args) =>
                {
                    switch (args.Severity)
                    {
                        case XmlSeverityType.Warning:
                            // ReSharper disable once AccessToModifiedClosure
                            avisosList.Add(args.Message);
                            break;

                        case XmlSeverityType.Error:
                            // ReSharper disable once AccessToModifiedClosure
                            errorList.Add(args.Message);
                            break;
                    }

                    // Erro na validação do schema XSD
                    if (args.Exception != null)
                    {
                        // ReSharper disable once AccessToModifiedClosure
                        errorList.Add("\nErro: " + args.Exception.Message + "\nLinha " + args.Exception.LinePosition + " - Coluna "
                                      + args.Exception.LineNumber + "\nSource: " + args.Exception.SourceUri);
                    }
                });

                var settings = new XmlReaderSettings { ValidationType = ValidationType.Schema };
                settings.Schemas.Add(xmlSchema);

                using (var xmlReader = XmlReader.Create(new StringReader(arquivoXml), settings))
                {
                    while (xmlReader.Read())
                    {
                    }
                }
            }
            catch (Exception exception)
            {
                errorList.Add(exception.Message);
            }

            erros = errorList.ToArray();
            avisos = avisosList.ToArray();
            errorList = null;
            avisosList = null;

            return (erros.Length < 1);
        }
    }
}