using Microsoft.Reporting.NETCore;

namespace SparkNET.Report
{
    public class SparkReport : IDisposable
    {
        private Dictionary<string, string?> _parameters = [];
        private readonly List<ReportDataSource> _dataSources = [];
        private bool _disposed = false;

        public string ReportPath { get; set; } = string.Empty;
        public string ReportName { get; set; } = string.Empty;
        public bool EnableExternalImages { get; set; }
        public bool EnableHyperlinks { get; set; }

        public void SetParameters(Dictionary<string, string?> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                throw new ArgumentException("Parameters cannot be null or empty.", nameof(parameters));
            }

            _parameters = parameters;
        }

        public void AddDataset(string datasetName, object source)
        {
            if (string.IsNullOrWhiteSpace(datasetName))
            {
                throw new ArgumentNullException(nameof(datasetName), "Dataset name cannot be null or empty.");
            }

            ArgumentNullException.ThrowIfNull(source);
            _dataSources.Add(new ReportDataSource(datasetName, source));
        }

        public (byte[] data, string mimeType, string fileName) Render(string format = "pdf")
        {
            (string type, string mimeType, string extension) = format switch
            {
                "xlsx" => ("EXCELOPENXML", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "xlsx"),
                "html" => ("HTML5", "text/html", "html"),
                "pdf" => ("PDF", "application/pdf", "pdf"),
                "docx" => ("WORDOPENXML", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "docx"),
                _ => throw new ArgumentException($"Format '{format}' is not supported.", nameof(format))
            };

            try
            {
                using var report = new LocalReport();
                report.ReportPath = ReportPath;
                report.DisplayName = ReportName;
                report.EnableExternalImages = EnableExternalImages;
                report.EnableHyperlinks = EnableHyperlinks;

                if (_parameters.Count > 0)
                {
                    report.SetParameters(_parameters.Select(p => new ReportParameter(p.Key, p.Value)).ToList());
                }

                if (_dataSources.Count > 0)
                {
                    _dataSources.ForEach(x => report.DataSources.Add(x));
                }

                string fileName = (ReportName ?? Path.GetFileNameWithoutExtension(ReportPath)) + "." + extension;
                return (report.Render(type), mimeType, fileName);
            }
            catch (Exception ex)
            {
                throw new Exception(GetExceptionMessage(ex));
            }
        }

        private static string GetExceptionMessage(Exception ex)
        {
            Exception innerEx = ex;
            while (innerEx.InnerException != null)
            {
                innerEx = innerEx.InnerException;
            }
            return innerEx.Message;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _dataSources.Clear();
                _parameters.Clear();
            }

            _disposed = true;
        }

        ~SparkReport()
        {
            Dispose(false);
        }
    }
}
