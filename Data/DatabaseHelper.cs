using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using test_basic_dotnet_forms.Models;

namespace test_basic_dotnet_forms.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString = 
            "Data Source=localhost;Initial Catalog=test_basic_dotnet_forms;Integrated Security=True;TrustServerCertificate=True";

        public DatabaseHelper()
        {
            CreateTable();
        }

        private void CreateTable()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var sql = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'People')
                    CREATE TABLE People (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Name NVARCHAR(100),
                        Email NVARCHAR(100)
                    )";
                new SqlCommand(sql, conn).ExecuteNonQuery();
            }
        }

        public List<Person> GetAll()
        {
            var list = new List<Person>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM People", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Person
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString()
                    });
                }
            }
            return list;
        }

        public void Add(Person person)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var sql = "INSERT INTO People (Name, Email) VALUES (@Name, @Email)";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Name", person.Name);
                cmd.Parameters.AddWithValue("@Email", person.Email);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Person person)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var sql = "UPDATE People SET Name = @Name, Email = @Email WHERE Id = @Id";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", person.Id);
                cmd.Parameters.AddWithValue("@Name", person.Name);
                cmd.Parameters.AddWithValue("@Email", person.Email);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var sql = "DELETE FROM People WHERE Id = @Id";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}