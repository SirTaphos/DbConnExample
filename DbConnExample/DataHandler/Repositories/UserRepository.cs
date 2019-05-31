using DbConnExample.Domain.Handlers;
using DbConnExample.Domain.Models;

namespace DbConnExample.Domain
{
    public class UserRepository
    {
        /// <summary>
        /// Simple sql for inserting values into data table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string Create()
        {
            return
                $"insert into Users (Name, GroupId, Username, Password, Salt) values (@name, @groupId, @username, @password, @salt)";
        }        

        /// <summary>
        /// Get and return all users
        /// To return a single specific user, use 'where parameter = value'
        /// </summary>
        /// <returns></returns>
        public string Read()
        {
            return $"select * from Users";
        }

        /// <summary>
        /// Update user according to the sent parameters
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string Update()
        {
            return $"update Users set Name = @name, GroupId = @groupId";
        }

        /// <summary>
        /// Delete user based on id
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string Delete(User user)
        {
            return $"delete from users where Id = @id";
        }    
    }
}
