using PsychometricWeb.Models;
using System.Data;

namespace PsychometricWeb.Repository
{
    public interface IPsychometricRepo
    {
        List<Psychometriclist> GetNameSearch(Psychometriclist objNameSearch);
        public string SubmitPsychometricTool(DataTable objPsycho);
        public List<Psychometriclist> GetPsychometricViewSearch(Psychometriclist objSearch);
        List<Psychometriclist> GetPsychometricView();

        UsersDto? Login(Login Log, out int returnVal);
        Task<int> InsertRegistration(Registration scheme);
    }
}
