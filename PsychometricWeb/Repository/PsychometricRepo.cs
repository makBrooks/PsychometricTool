using Dapper;
using Microsoft.Extensions.Configuration;
using PsychometricWeb.Models;
using System.Data;
using System.Data.SqlClient;

namespace PsychometricWeb.Repository
{
    public class PsychometricRepo :BaseRepo, IPsychometricRepo
    {
        private readonly IConfiguration _configuration;
        public PsychometricRepo(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }
        public List<Psychometriclist> GetNameSearch(Psychometriclist objNameSearch)
        {
            try
            {
                var p = new DynamicParameters();
                var con = Connection();
                p.Add("@P_Action", "Name");
                p.Add("@P_Name", objNameSearch.Name);
                p.Add("@Msg", 0);
                var result = con.Query<Psychometriclist>("USP_Psychometric_Tool", p, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<Psychometriclist> GetPsychometricView()
        {
            try
            {
                var p = new DynamicParameters();
                var con = Connection();
                p.Add("@P_Action", "View");
                p.Add("@Msg", 0);
                var result = con.Query<Psychometriclist>("USP_Psychometric_Tool", p, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<Psychometriclist> GetPsychometricViewSearch(Psychometriclist objSearch)
        {
            try
            {
                var p = new DynamicParameters();
                var con = Connection();
                p.Add("@P_Action", "SearchView");
                //p.Add("@P_Email", objSearch.Email);
                //p.Add("@P_Phone", objSearch.Phone);
                //p.Add("@P_Name", objSearch.Name);
                p.Add("@P_UId", Convert.ToInt32(objSearch.UID));
                p.Add("@Msg", 0);
                var result = con.Query<Psychometriclist>("USP_Psychometric_Tool", p, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public string SubmitPsychometricTool(DataTable objPsycho)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:MyConnection"]))
                {
                    connection.Open();

                    var parameters = new
                    {
                        P_Type = objPsycho,
                        P_Action = "Insert",
                        Msg = 0
                    };

                    var result = connection.Execute(
                        "USP_Psychometric_Tool",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return "1";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> InsertRegistration(Registration Reg)
        {
            try
            {
                var p = new DynamicParameters();
                var con = Connection();
                p.Add("@P_Action", "Regi");
                p.Add("@P_UName", Reg.UNAME);
                p.Add("@P_Name", Reg.FULLNAME);
                p.Add("@P_UPassword", Reg.PASSWORD);
                p.Add("@P_Email", Reg.EMAIL);
                p.Add("@P_Phone", Reg.PHONE);
                p.Add("@P_AadhaarNo", Reg.ADHARNUMBER);
                p.Add("@P_AadhaarImg", Reg.UPLOADPATH);
                p.Add("@Msg", dbType: DbType.Int32, direction: ParameterDirection.Output, size: 5215585);

                await con.ExecuteAsync("USP_Psychometric_Tool", p, commandType: CommandType.StoredProcedure);
                int returnVal = p.Get<Int32>("@Msg");
                return returnVal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public UsersDto? Login(Login Log, out int returnVal)
        {
            try
            {
                var p = new DynamicParameters();
                var con = Connection();
                p.Add("@P_Action", "Login");
                p.Add("@P_UName", Log.userName);
                p.Add("@Msg", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var userDetails = con.Query<UsersDto>("USP_Psychometric_Tool", p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                returnVal = p.Get<int>("Msg");

                return userDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
