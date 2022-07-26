using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApp.DataDB.CRUDs
{
    public class LikeCRUD
    {
        public void Add(Like addedLike)
        {
            SocialMediaContext database = new SocialMediaContext();
            database.Likes.Add(addedLike);
            database.SaveChanges();
        }

        public List<Like> GetAllLikesFromPost(int PostId)
        {
            SocialMediaContext database = new SocialMediaContext();
            List<User> users = database.Users.ToList();
            List<Like> likes = database.Likes.ToList();
            return likes.FindAll(x => users.SingleOrDefault(y => y.UserId == PostId).IsDeleted == false);
        }

        public List<Like> GetAllLikesFromUser(int UserId)
        {
            SocialMediaContext database = new SocialMediaContext();
            List<User> users = database.Users.ToList();
            List<Like> likes = database.Likes.ToList();
            return likes.FindAll(x => users.SingleOrDefault(y => y.UserId == UserId).IsDeleted == false);
        }

        public Like GetByID(int Id)
        {
            SocialMediaContext database = new SocialMediaContext();
            List<Like> likes = database.Likes.ToList();
            return likes.SingleOrDefault(x => x.LikeId == Id);
        }

        public void RemoveByID(int Id)
        {
            SocialMediaContext database = new SocialMediaContext();
            var likes = database.Likes;
            likes.Remove(likes.ToList().SingleOrDefault(x => x.LikeId == Id));
            database.SaveChanges();
        }

        public void RemoveByUserAndPost(int UserId, int PostId)
        {
            SocialMediaContext database = new SocialMediaContext();
            var likes = database.Likes;
            List<User> users = database.Users.ToList();
            Like foundLike = likes.ToList().SingleOrDefault(x => x.UserId == UserId && x.PostId == PostId);
            likes.Remove(foundLike);
            database.SaveChanges();
        }

        public void UpdateByID(int Id, Like updatedLike)
        {
            SocialMediaContext database = new SocialMediaContext();
            var likes = database.Likes;
            updatedLike.LikeId = Id;
            database.Entry(likes.SingleOrDefault(x => x.LikeId == Id)).CurrentValues.SetValues(updatedLike);
            database.SaveChanges();
        }
    }
}
