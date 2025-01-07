using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using CV_builder.Models;

namespace CV_builder.Services
{
    public class DocumentService
    {
        public byte[] GenerateWordDocument(CVModel cv)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = doc.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());

                    // Add Title
                    AddHeading(body, $"{cv.Name} {cv.Surname} - CV");

                    // Personal Information
                    AddHeading(body, "Personal Information", 2);
                    AddParagraph(body, $"Email: {cv.Email}");
                    AddParagraph(body, $"Phone: {cv.Phone}");
                    AddParagraph(body, $"Address: {cv.Address}");

                    // Education Section
                    AddHeading(body, "Education", 2);
                    foreach (var edu in cv.Education)
                    {
                        AddParagraph(body, $"Institution: {edu.Institution}");
                        AddParagraph(body, $"Field of Study: {edu.FieldOfStudy}");
                        AddParagraph(body, $"Faculty: {edu.Faculty}");
                        AddParagraph(body, $"Level: {edu.EducationLevel}");
                        AddParagraph(body, $"Status: {edu.Status}");
                        AddParagraph(body, ""); // Empty line between entries
                    }

                    // Work Experience Section
                    AddHeading(body, "Work Experience", 2);
                    foreach (var work in cv.WorkExperience)
                    {
                        AddParagraph(body, $"Company: {work.Company}");
                        AddParagraph(body, $"Position: {work.JobTitle}");
                        AddParagraph(body, $"Workload: {work.Workload}");
                        AddParagraph(body, $"Location: {work.Address}");
                        AddParagraph(body, $"Period: {work.StartDate.ToShortDateString()} - {work.EndDate.ToShortDateString()}");
                        AddParagraph(body, ""); // Empty line between entries
                    }
                }

                return ms.ToArray();
            }
        }

        private void AddHeading(Body body, string text, int level = 1)
        {
            Paragraph para = body.AppendChild(new Paragraph());
            Run run = para.AppendChild(new Run());
            RunProperties runProps = run.AppendChild(new RunProperties());
            runProps.AppendChild(new Bold());
            runProps.AppendChild(new FontSize { Val = (level == 1) ? "36" : "28" });
            run.AppendChild(new Text(text));
        }

        private void AddParagraph(Body body, string text)
        {
            Paragraph para = body.AppendChild(new Paragraph());
            Run run = para.AppendChild(new Run());
            run.AppendChild(new Text(text));
        }
    }
} 