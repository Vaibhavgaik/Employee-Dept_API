using Swashbuckle.AspNetCore.Filters;

namespace JewelAPI.Models
{
    public class UserExample : IExamplesProvider<User>
    {
        public User GetExamples()
        {
            return new User
            {
                Password = "******" 
            };
        }
    }
}
