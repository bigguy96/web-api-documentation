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
        private Body _body;

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

        public void Generate(ApiDetails apiDetails)
        {
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fileName = Path.Combine(myDocuments, "word.docx");

            if (File.Exists(fileName)) File.Delete(fileName);

            using var document = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document);

            // Add a main document part. 
            var mainPart = document.AddMainDocumentPart();
            mainPart.Document = new Document();

            _body = mainPart.Document.AppendChild(new Body());

            AddParagraph(apiDetails.WebApiTitle);
            AddParagraph(apiDetails.WebApiUrl);

            foreach (var operation in apiDetails.Operations)
            {
                AddParagraph(operation.Name);
                AddParagraph(operation.FullPath);
                AddParagraph("Description");
                AddParagraph(operation.Description);
                AddParagraph("Summary");
                AddParagraph(operation.Summary);


            }

            AddTable();

            document.Save();
            //Process.Start(fileName);
        }

        private void AddTable()
        {
            //Create table
            var tbl = new Table();

            //Create the table properties
            var tblProperties = new TableProperties();

            //Create Table Borders
            var tblBorders = new TableBorders();

            var topBorder = new TopBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Thick),
                Color = "000000"
            };
            tblBorders.AppendChild(topBorder);

            var bottomBorder = new BottomBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Thick),
                Color = "000000"
            };
            tblBorders.AppendChild(bottomBorder);

            var rightBorder = new RightBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Thick),
                Color = "000000"
            };
            tblBorders.AppendChild(rightBorder);

            var leftBorder = new LeftBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Thick),
                Color = "000000"
            };
            tblBorders.AppendChild(leftBorder);

            var insideHBorder = new InsideHorizontalBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Thick),
                Color = "000000"
            };
            tblBorders.AppendChild(insideHBorder);

            var insideVBorder = new InsideVerticalBorder
            {
                Val = new EnumValue<BorderValues>(BorderValues.Thick),
                Color = "000000"
            };

            tblBorders.AppendChild(insideVBorder);
            tblProperties.AppendChild(tblBorders);
            tbl.AppendChild(tblProperties);

            var tr = new TableRow();
            var nameCell = new TableCell();
            var dbTypeCell = new TableCell();
            var isForeignKeyCell = new TableCell();
            var isPrimaryKeyCell = new TableCell();
            var isNullableCell = new TableCell();
            var descriptionCell = new TableCell();

            nameCell.Append(new Paragraph(new Run(new Text("test"))));
            dbTypeCell.Append(new Paragraph(new Run(new Text("TEST"))));
            isForeignKeyCell.Append(new Paragraph(new Run(new Text("test"))));
            isPrimaryKeyCell.Append(new Paragraph(new Run(new Text("test"))));
            isNullableCell.Append(new Paragraph(new Run(new Text("test"))));
            descriptionCell.Append(new Paragraph(new Run(new Text("test"))));

            tr.Append(nameCell);
            tr.Append(dbTypeCell);
            tr.Append(isPrimaryKeyCell);
            tr.Append(isForeignKeyCell);
            tr.Append(isNullableCell);
            tr.Append(descriptionCell);

            tbl.Append(tr);

            _body.Append(tbl);
        }

        private void AddParagraph(string content)
        {
            var paragraph = _body.AppendChild(new Paragraph());
            var run = paragraph.AppendChild(new Run());
            run.AppendChild(new Text(content));
            paragraph.ParagraphProperties = new ParagraphProperties(new ParagraphStyleId { Val = "Heading3" });
        }
    }
}

//https://github.com/onizet/html2openxml/wiki