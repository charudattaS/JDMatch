using JDMatch.Application.Interfaces;
using UglyToad.PdfPig;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;

namespace JDMatch.Infrastructure.Services
{
    public class TextExtractionService : ITextExtractionService
    {
        public async Task<string> ExtractTextAsync(string filePath, string fileExtension)
        {
            fileExtension = fileExtension.ToLower();

            string text;

            if (fileExtension == ".pdf")
                text = ExtractFromPdf(filePath);
            else if (fileExtension == ".docx")
                text = ExtractFromDocx(filePath);
            else
                text = await File.ReadAllTextAsync(filePath);

            return CleanText(text);
        }

        private string CleanText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            // Remove NULL characters
            text = text.Replace("\0", "");

            // Remove other non-printable control characters except newline and tab
            text = new string(text
                .Where(c => !char.IsControl(c) || c == '\n' || c == '\r' || c == '\t')
                .ToArray());

            return text;
        }


        private string ExtractFromPdf(string filePath)
        {
            var sb = new StringBuilder();

            using var document = PdfDocument.Open(filePath);

            foreach (var page in document.GetPages())
            {
                sb.AppendLine(page.Text);
            }

            return sb.ToString();
        }

        private string ExtractFromDocx(string filePath)
        {
            var sb = new StringBuilder();

            using var wordDoc = WordprocessingDocument.Open(filePath, false);

            var body = wordDoc.MainDocumentPart?.Document.Body;

            if (body != null)
            {
                foreach (var text in body.Descendants<Text>())
                {
                    sb.Append(text.Text + " ");
                }
            }

            return sb.ToString();
        }
    }
}
