using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class StaticUserRepository : IUserRepository
    {
        //Creating static private list of users
        private List<User> Users = new List<User>()
        {
            //new User()
            //{
            //    FirstName = "Read Only", LastName = "User" , EmailAddress = "readonly@user.com",
            //    Id = Guid.NewGuid(), UserName = "readonly@user.com" , Password = "readonly@user",
            //    Roles = new List<string>{ "reader" }
            //},
            //new User()
            //{
            //    FirstName = "Read Write", LastName = "User" , EmailAddress = "readwrite@user.com",
            //    Id = Guid.NewGuid(), UserName = "readwrite@user.com" , Password = "readwrite@user",
            //    Roles = new List<string>{ "reader" , "writer" }
            //}
        };


        public async Task<User> AuthenticateAsync(string username, string password)
        {
            //StringComparison.InvariantCultureIgnoreCase to make username case insensitive
            var user = Users.Find(x => x.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase) &&
            x.Password.Equals(password));

            if(user != null)
            {
                return user;
            }
            return null ;

        }
    }
}
