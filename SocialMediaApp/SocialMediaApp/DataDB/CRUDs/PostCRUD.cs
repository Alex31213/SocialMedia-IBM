using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApp.DataDB.CRUDs
{
    public class PostCRUD : ICRUD<Post>
    {
        public void Add(Post addedPost)
        {
            SocialMediaContext database = new SocialMediaContext();
            database.Posts.Add(addedPost);
            database.SaveChanges();
        }

        public List<Post> GetAllPosts()
        {
            SocialMediaContext database = new SocialMediaContext();
            List<User> users = database.Users.ToList();
            List<Post> posts = database.Posts.ToList();
            return posts.FindAll(x => users.SingleOrDefault(y => y.UserId == x.UserId).IsDeleted == false);
        }

        public List<Post> GetAllUserPosts(int UserId)
        {
            SocialMediaContext database = new SocialMediaContext();
            List<User> users = database.Users.ToList();
            List<Post> posts = database.Posts.ToList();
            return posts.FindAll(x => x.UserId == UserId);
        }

        public Post GetByID(int Id)
        {
            SocialMediaContext database = new SocialMediaContext();
            List<Post> posts = database.Posts.ToList();
            return posts.SingleOrDefault(x => x.PostId == Id);
        }

        public void RemoveByID(int Id)
        {
            SocialMediaContext database = new SocialMediaContext();
            var posts = database.Posts;
            posts.Remove(posts.ToList().SingleOrDefault(x => x.PostId == Id));
            database.SaveChanges();
        }

        public void UpdateByID(int Id, Post updatedPost)
        {
            SocialMediaContext database = new SocialMediaContext();
            var posts = database.Posts;
            updatedPost.PostId = Id;
            database.Entry(posts.SingleOrDefault(x => x.PostId == Id)).CurrentValues.SetValues(updatedPost);
            database.SaveChanges();
        }

        public void ChangeTextPost(int Id, string NewText)
        {
            SocialMediaContext database = new SocialMediaContext();
            var posts = database.Posts;
            Post updatedReport = posts.SingleOrDefault(x => x.PostId == Id);
            updatedReport.Text = NewText;
            database.Entry(posts.SingleOrDefault(x => x.PostId == Id)).CurrentValues.SetValues(updatedReport);
            database.SaveChanges();
        }
    }
}
