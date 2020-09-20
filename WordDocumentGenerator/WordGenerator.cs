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
            const string filename = "test.docx";
            string html = content;

            if (File.Exists(filename)) File.Delete(filename);

            using (MemoryStream generatedDocument = new MemoryStream())
            {
                using (WordprocessingDocument package = WordprocessingDocument.Create(generatedDocument, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = package.MainDocumentPart;
                    if (mainPart == null)
                    {
                        mainPart = package.AddMainDocumentPart();
                        new Document(new Body()).Save(mainPart);
                    }

                    HtmlConverter converter = new HtmlConverter(mainPart);
                    converter.ParseHtml(html);

                    mainPart.Document.Save();
                }

                File.WriteAllBytes(filename, generatedDocument.ToArray());
            }

            System.Diagnostics.Process.Start(filename);
        }
    }
}

//https://github.com/onizet/html2openxml/wiki