## Usage

To use **SparkNET.Report** for rendering reports.

### Example Code:

```csharp
using SparkNET.Report;

// Report parameters
var param = new ReportParameters();
param.Add("TITLE", "Testing report");

// Report data
List<Person> people = new List<Person>
{
    new Person { Name = "John", Age = 30 },
    new Person { Name = "Alice", Age = 25 },
    new Person { Name = "Bob", Age = 40 }
};

// Create a report
using var report = new SparkReprt();
report.ReportPath = "C:\Test.rdl";
report.EnableExternalImages = true;
report.SetParameters(param);
report.AddDataSet("DataSet1", people);

// Generate report as PDF
var file = report.Render("pdf");
byte[] pdf = file.data;
string mimeType = file.mimeType;
string fileName = file.fileName;
```
