using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ReenbitBooking.Application.Common.Interfaces;

namespace ReenbitBooking.Infrastructure.Data;

public class SqlConnectionFactory(IConfiguration _configuration) : ISqlConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
    }
}