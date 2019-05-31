using DbConnExample.Domain;
using DbConnExample.Domain.Handlers;
using DbConnExample.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DbConnExample
{
    public class UserDal
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\bjorn\source\repos\DbConnExample\DbConnExample\DbConnExample.Db.mdf;Integrated Security=True";
        UserRepository repo = new UserRepository();
        public void CreateUser(User user)
        {
            if (!DoesUsernameExist(user.Username))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var salt = EncryptionHandler.GenerateSalt();
                    using (SqlCommand cmd = new SqlCommand(repo.Create(), conn))
                    {
                        cmd.Parameters.AddWithValue("@name", user.Name);
                        cmd.Parameters.AddWithValue("@groupId", (int)user.GroupId);
                        cmd.Parameters.AddWithValue("@username", user.Username);
                        cmd.Parameters.AddWithValue("@password", EncryptionHandler.EncryptPassword(user.Password, salt));
                        cmd.Parameters.AddWithValue("@salt", salt);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            else Console.WriteLine("Username already exists");
        }

        public List<User> ReadUsers()
        {
            var users = new List<User>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(repo.Read(), conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        GroupId = (Enums.GroupEnums)reader["GroupId"],
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        Salt = reader["Salt"].ToString()
                    });
                }
            }
            return users;
        }

        public void UpdateUser(User user)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(repo.Update(), conn))
                {
                    cmd.Parameters.AddWithValue("@name", user.Name);
                    cmd.Parameters.AddWithValue("@groupId", (int)user.GroupId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Check if chosen username is unique
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool DoesUsernameExist(string username)
        {
            bool check = false;
            ReadUsers().ForEach(u => check = u.Username.ToLower().Equals(username.ToLower()));
            return check;
        }
    }
}
