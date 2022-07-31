using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApp.DataDB.CRUDs
{
    public class FollowCRUD : ICRUD<Follow>
    {
        public void Add(Follow addedFollow)
        {
            SocialMediaContext database = new SocialMediaContext();
            database.Follows.Add(addedFollow);
            database.SaveChanges();
        }

        public List<Follow> GetAllFollows()
        {
            SocialMediaContext database = new SocialMediaContext();
            List<User> users = database.Users.ToList();
            List<Follow> follows = database.Follows.ToList();
            return follows.FindAll(x => users.SingleOrDefault(y => y.UserId == x.FirstUserId).IsDeleted == false);
        }

        public List<Follow> GetUserFollows(int UserId)
        {
            SocialMediaContext database = new SocialMediaContext();
            List<User> users = database.Users.ToList();
            List<Follow> follows = database.Follows.ToList();
            return follows.FindAll(x => users.SingleOrDefault(y => y.UserId == UserId).IsDeleted == false);
        }

        public Follow GetByID(int Id)
        {
            SocialMediaContext database = new SocialMediaContext();
            List<Follow> follows = database.Follows.ToList();
            return follows.SingleOrDefault(x => x.FollowId == Id);
        }

        public void RemoveByID(int Id)
        {
            SocialMediaContext database = new SocialMediaContext();
            var follows = database.Follows;
            follows.Remove(follows.ToList().SingleOrDefault(x => x.FollowId == Id));
            database.SaveChanges();
        }

        public void RemoveByUserIDs(int FirstUserId, int SecondUserId)
        {
            SocialMediaContext database = new SocialMediaContext();
            var follows = database.Follows;
            List<User> users = database.Users.ToList();
            Follow foundLike = follows.ToList().SingleOrDefault(x => x.FirstUserId == FirstUserId
            && x.SecondUserId == SecondUserId);
            follows.Remove(foundLike);
            database.SaveChanges();
        }
        public void UpdateByID(int Id, Follow updatedFollow)
        {
            SocialMediaContext database = new SocialMediaContext();
            var follows = database.Follows;
            updatedFollow.FollowId = Id;
            database.Entry(follows.SingleOrDefault(x => x.FollowId == Id)).CurrentValues.SetValues(updatedFollow);
            database.SaveChanges();
        }
    }
}
