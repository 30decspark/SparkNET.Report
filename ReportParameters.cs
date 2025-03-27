using Microsoft.Reporting.NETCore;

namespace SparkNET.Report
{
    public class ReportParameters
    {
        private readonly Dictionary<string, string?> _parameters = [];

        public void Add(string name, string? value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Parameter name cannot be null or empty.", nameof(name));
            }

            _parameters[name] = value;
        }

        internal void Clear()
        {
            _parameters.Clear();
        }

        internal int Count()
        {
            return _parameters.Count;
        }
        
        internal IEnumerable<ReportParameter> ToList()
        {
            return _parameters.Select(p => new ReportParameter(p.Key, p.Value)).ToList();
        }
    }
}
