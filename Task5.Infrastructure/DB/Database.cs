using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task5.Domain.Models;

namespace Task5.Infrastructure.DB
{
    public class Database
    {
        public DataTable GetTable(string str, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            DataTable objResult = new DataTable();
            SqlDataReader reader;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(str, con))
                {
                    //cmd.Parameters.AddWithValue("@Email", parameterValue);
                    reader = cmd.ExecuteReader();
                    objResult.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }
            return objResult;
        }

        public int ExecuteData(IConfiguration configuration, string str, params IDataParameter[] sqlParams)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            int rows = -1;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(str, conn))
                {
                    if (sqlParams != null)
                    {
                        foreach (IDataParameter para in sqlParams)
                        {
                            cmd.Parameters.Add(para);
                        }
                        rows = cmd.ExecuteNonQuery();
                    }
                }
            }

            return rows;
        }

        public dynamic ExecuteStoredProcedure(AddEmployeeRequestModel addEmployeeRequest, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd;
            SqlDataAdapter da;
            DataTable dt = new DataTable();

            try
            {
                cmd = new SqlCommand("SP_EMPLOYEES", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", addEmployeeRequest.ActionId);
                cmd.Parameters.AddWithValue("@Id", addEmployeeRequest.EmployeeId);
                cmd.Parameters.AddWithValue("@Name", addEmployeeRequest.Name);
                cmd.Parameters.AddWithValue("@Salary", addEmployeeRequest.Salary);
                cmd.Parameters.AddWithValue("@Email", addEmployeeRequest.Email);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
