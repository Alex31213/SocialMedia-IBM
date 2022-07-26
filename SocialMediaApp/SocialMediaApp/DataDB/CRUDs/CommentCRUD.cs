using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApp.DataDB.CRUDs
{
    public class CommentCRUD
    {
        public void Add(Comment addedComment)
        {
            SocialMediaContext database = new SocialMediaContext();
            database.Comments.Add(addedComment);
            database.SaveChanges();
        }
        public List<Comment> GetAllUserComments(int UserId)
        {
            SocialMediaContext database = new SocialMediaContext();
            List<User> users = database.Users.ToList();
            List<Comment> comments = database.Comments.ToList();
            return comments.FindAll(x => x.UserId == UserId);
        }

        public List<Comment> GetAllComments()
        {
            SocialMediaContext database = new SocialMediaContext();
            List<User> users = database.Users.ToList();
            List<Comment> comments = database.Comments.ToList();
            return comments.FindAll(x => users.SingleOrDefault(y => y.UserId == x.UserId).IsDeleted == false);
        }

        public Comment GetByID(int Id)
        {
            SocialMediaContext database = new SocialMediaContext();
            List<Comment> comments = database.Comments.ToList();
            return comments.SingleOrDefault(x => x.CommentId == Id);
        }

        public void RemoveByID(int Id)
        {
            SocialMediaContext database = new SocialMediaContext();
            var comments = database.Comments;
            comments.Remove(comments.ToList().SingleOrDefault(x => x.CommentId == Id));
            database.SaveChanges();
        }

        public void UpdateByID(int Id, Comment updatedComment)
        {
            SocialMediaContext database = new SocialMediaContext();
            var comments = database.Comments;
            updatedComment.CommentId = Id;
            database.Entry(comments.SingleOrDefault(x => x.CommentId == Id)).CurrentValues.SetValues(updatedComment);
            database.SaveChanges();
        }

        public void ChangeTextComment(int Id, string NewText)
        {
            SocialMediaContext database = new SocialMediaContext();
            var comments = database.Comments;
            Comment updatedComment = comments.SingleOrDefault(x => x.CommentId == Id);
            updatedComment.Text = NewText;
            database.Entry(comments.SingleOrDefault(x => x.CommentId == Id)).CurrentValues.SetValues(updatedComment);
            database.SaveChanges();
        }
    }
}
