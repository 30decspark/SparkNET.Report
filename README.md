## Usage

To use **SparkNET.Report** for rendering reports, follow these steps:

### Example Code:

```csharp
using SparkNET.Report;

// Create a new report instance
using var report = new SparkReprt();

// Set the report path (RDLC file)
report.ReportPath = "Test.rdlc";

// Render the report in PDF format
byte[] pdfReport = report.Render("PDF").data;
