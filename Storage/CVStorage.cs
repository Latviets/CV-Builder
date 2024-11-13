using CV_builder.Models;

namespace CV_builder.Storage
{
    public class CVStorage
    {
        private List<CVModel> _cvList = new List<CVModel>();
        private int _id = 0;

        public void AddCV(CVModel cv)
        {
            cv.Id = ++_id;
            _cvList.Add(cv);         
        }

        public List<CVModel> GetCvList()
        {
            return _cvList;
        }

        public CVModel GetCVByOwnerName(string name)
        {
            return _cvList.FirstOrDefault(cv => cv.Name == name);
        }
    }
}
