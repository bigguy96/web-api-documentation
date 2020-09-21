using System;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlToOpenXml;
using System.IO;

namespace WordDocumentGenerator
{
    public class WordGenerator
    {
        public void Generate(string content)
        {
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            const string filename = "test.docx";
            var html = content;

            if (File.Exists(filename)) File.Delete(filename);

            using var generatedDocument = new MemoryStream();
            using (var package = WordprocessingDocument.Create(generatedDocument, WordprocessingDocumentType.Document))
            {
                var mainPart = package.MainDocumentPart;
                if (mainPart == null)
                {
                    mainPart = package.AddMainDocumentPart();
                    new Document(new Body()).Save(mainPart);
                }

                var converter = new HtmlConverter(mainPart);
                converter.ParseHtml(html);

                mainPart.Document.Save();
            }

            File.WriteAllBytes(Path.Combine(myDocuments, filename), generatedDocument.ToArray());

            //System.Diagnostics.Process.Start(filename);
        }
    }
}

//https://github.com/onizet/html2openxml/wiki