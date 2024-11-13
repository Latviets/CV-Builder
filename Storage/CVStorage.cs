using CV_builder.Models;
using System.Text.Json;

namespace CV_builder.Storage
{
    public class CVStorage
    {
        private List<CVModel> _cvs = new List<CVModel>();
        private readonly string _filePath = "cvs.json";

        public CVStorage()
        {
            LoadCVs();
        }

        public void AddCV(CVModel cv)
        {
            _cvs.Add(cv);
            SaveCVs();
        }

        public List<CVModel> GetCvList()
        {
            return _cvs;
        }

        private void SaveCVs()
        {
            var jsonData = JsonSerializer.Serialize(_cvs);
            File.WriteAllText(_filePath, jsonData);
        }

        private void LoadCVs()
        {
            if (File.Exists(_filePath))
            {
                var jsonData = File.ReadAllText(_filePath);
                _cvs = JsonSerializer.Deserialize<List<CVModel>>(jsonData) ?? new List<CVModel>();
            }
        }
    }
}
