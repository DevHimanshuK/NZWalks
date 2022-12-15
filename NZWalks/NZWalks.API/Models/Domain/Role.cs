namespace NZWalks.API.Models.Domain
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        //Navigation Properties
        //currently we are not going to use this, but it is a good practice to create
        public List<User_Role> User_Roles { get; set; }
    }
}
