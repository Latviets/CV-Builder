using CV_builder.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CV_builder.Storage
{
    public class CVStorage
    {
        private List<CVModel> _cvs = new List<CVModel>();
        private readonly string _filePath = "cvs.json";
        private int _id = 0;

        public CVStorage()
        {
            Console.WriteLine("Initializing CVStorage...");
            LoadCVs();
            if (_cvs.Any())
            {
                _id = _cvs.Max(c => c.Id);
            }
        }

        public void AddCV(CVModel cv)
        {
            cv.Id = ++_id;
            _cvs.Add(cv);
            SaveCVs();
        }

        public List<CVModel> GetCvList()
        {
            return _cvs ?? new List<CVModel>();
        }

        public void SaveCVs()
        {
            var json = JsonConvert.SerializeObject(_cvs, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        private void LoadCVs()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _cvs = JsonConvert.DeserializeObject<List<CVModel>>(json) ?? new List<CVModel>();
            }
            else
            {
                _cvs = new List<CVModel>();
            }
        }
    }
}
