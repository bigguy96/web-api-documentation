using System;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlToOpenXml;
using System.IO;
using System.Linq;
using WebApiDocumentation.Extensions;

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

        
        //TODO: Look a cloning document and replace text.
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

                AddParagraph("Response Content Type");
                if (operation.RequestBodies.Any())
                {
                    var contentType = operation.RequestBodies?.SingleOrDefault(requestBody => requestBody.ContentType.Equals("application/json"));
                    var tbl = CreateTable();
                    var tr = new TableRow();
                    var contentTypeCell = new TableCell();
                    var idCell = new TableCell();
                    var typeCell = new TableCell();

                    //TableWidth width = table.GetDescendents<TableWidth>().First();
                    //width.Width = "5000";
                    //width.Type = TableWidthUnitValues.Pct;

                    contentTypeCell.Append(new Paragraph(new Run(new Text("Content Type"))));
                    idCell.Append(new Paragraph(new Run(new Text("Id"))));
                    typeCell.Append(new Paragraph(new Run(new Text("Type"))));

                    tr.Append(contentTypeCell);
                    tr.Append(idCell);
                    tr.Append(typeCell);

                    tr = new TableRow();
                    contentTypeCell = new TableCell();
                    idCell = new TableCell();
                    typeCell = new TableCell();

                    contentTypeCell.Append(new Paragraph(new Run(new Text(contentType?.ContentType))));
                    idCell.Append(new Paragraph(new Run(new Text(contentType?.Id))));
                    typeCell.Append(new Paragraph(new Run(new Text(contentType?.Type))));

                    tr.Append(contentTypeCell);
                    tr.Append(idCell);
                    tr.Append(typeCell);

                    tbl.Append(tr);

                    _body.Append(tbl);

                    if (contentType?.Id != null)
                    {
                        AddParagraph("Sample Request Body");
                        AddParagraph(contentType.Id.FormatJson(apiDetails.Components));
                    }
                    else
                    {
                        AddParagraph("application/json");
                    }
                }

                //        < h4 > Responses </ h4 >
                //        < table class= "table table-bordered table-hover" >

                //             < thead class= "thead-dark" >

                //                  < tr >

                //                      < th scope = "col" > Name </ th >

                //                       < th scope = "col" > Description </ th >

                //                    </ tr >

                //                </ thead >

                //                < tbody >
                //                    @foreach(var response in operation.Responses)
                //                {
                //                    < tr >
                //                        < td > @response.Name </ td >
                //                        < td > @response.Description </ td >
                //                    </ tr >
                //                }
                //            </ tbody >
                //        </ table >

                //        < h4 > Parameters </ h4 >
                //        < table class= "table table-bordered table-hover" >

                //             < thead class= "thead-dark" >

                //                  < tr >

                //                      < th scope = "col" > Name </ th >

                //                       < th scope = "col" > In </ th >

                //                        < th scope = "col" > Type </ th >

                //                         < th scope = "col" > Description </ th >

                //                          < th scope = "col" > Required </ th >

                //                           < th scope = "col" > Enums </ th >

                //                        </ tr >

                //                    </ thead >

                //                    < tbody >
                //                        @foreach(var parameter in operation.Parameters)
                //                {
                //                    < tr >
                //                        < td > @parameter.Name </ td >
                //                        < td > @parameter.In </ td >
                //                        < td > @parameter.Type </ td >
                //                        < td > @parameter.Description </ td >
                //                        < td > @parameter.IsRequired </ td >
                //                        < td > @string.Join(", ", parameter.Enumerations.Select(e => e.Value)) </ td >
                //                    </ tr >
                //                }
                //            </ tbody >
                //        </ table >


            }

            AddTable();

            document.Save();
            //Process.Start(fileName);
        }

        private Table CreateTable()
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

            return tbl;
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