using System.Data.SqlClient;

namespace PsychometricWeb.Repository
{
    public class BaseRepo
    {
        private readonly IConfiguration _configuration;

        public BaseRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected SqlConnection Connection()
        {
            return new SqlConnection(_configuration.GetConnectionString("MyConnection"));
        }
    }
}
