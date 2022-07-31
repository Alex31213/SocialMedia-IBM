using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SocialMediaApp.DataDB.CRUDs
{
    public class UserCRUD : ICRUD<User>
    {
        public void Add(User addedUser)
        {
            SocialMediaContext database = new SocialMediaContext();
            database.Users.Add(addedUser);
            database.SaveChanges();
        }
        public bool CheckUserIsDeletedById(int Id)
        {
            SocialMediaContext database = new SocialMediaContext();
            List<User> users = database.Users.ToList();
            return users.SingleOrDefault(x => x.UserId == Id).IsDeleted;
        }

        public List<User> GetAllUsers()
        {
            SocialMediaContext database = new SocialMediaContext();
            List<User> users = database.Users.ToList();
            return users.FindAll(x => x.IsDeleted == false);
        }

        public User GetByID(int Id)
        {
            SocialMediaContext database = new SocialMediaContext();
            List<User> users = database.Users.ToList();
            return users.SingleOrDefault(x => x.UserId == Id);
        }
        public User GetUserByUsername(string Username)
        {
            SocialMediaContext database = new SocialMediaContext();
            List<User> users = database.Users.ToList();
            return users.SingleOrDefault(x => x.Username == Username);
        }
        public User GetUsersByEMail(string EMail)
        {
            SocialMediaContext database = new SocialMediaContext();
            List<User> users = database.Users.ToList();
            return users.SingleOrDefault(x => x.Email == EMail);
        }
        public void RemoveByID(int Id)
        {
            SocialMediaContext database = new SocialMediaContext();
            var users = database.Users;
            users.Remove(users.ToList().SingleOrDefault(x => x.UserId == Id));
            database.SaveChanges();
        }
        public void UpdateByID(int Id, User updatedUser)
        {
            SocialMediaContext database = new SocialMediaContext();
            var users = database.Users;
            updatedUser.UserId = Id;
            database.Entry(users.SingleOrDefault(x => x.UserId == Id)).CurrentValues.SetValues(updatedUser);
            database.SaveChanges();
        }
        public void ChangePassword(int Id, string NewPassword)
        {
            SocialMediaContext database = new SocialMediaContext();
            var users = database.Users;
            User updatedUser = users.SingleOrDefault(x => x.UserId == Id);
            updatedUser.Password = NewPassword;
            database.Entry(users.SingleOrDefault(x => x.UserId == Id)).CurrentValues.SetValues(updatedUser);
            database.SaveChanges();
        }
        public void ChangeProfilePictureLink(int Id, string NewProfilePictureLink)
        {
            SocialMediaContext database = new SocialMediaContext();
            var users = database.Users;
            User updatedUser = users.SingleOrDefault(x => x.UserId == Id);
            updatedUser.ProfilePictureLink = NewProfilePictureLink;
            database.Entry(users.SingleOrDefault(x => x.UserId == Id)).CurrentValues.SetValues(updatedUser);
            database.SaveChanges();
        }
        public void ChangeRole(int Id, string NewRole)
        {
            SocialMediaContext database = new SocialMediaContext();
            var users = database.Users;
            User updatedUser = users.SingleOrDefault(x => x.UserId == Id);
            updatedUser.Role = NewRole;
            database.Entry(users.SingleOrDefault(x => x.UserId == Id)).CurrentValues.SetValues(updatedUser);
            database.SaveChanges();
        }
        public void ChangeDeletedState(int Id, bool isDeleted)
        {
            SocialMediaContext database = new SocialMediaContext();
            var users = database.Users;
            User updatedUser = users.SingleOrDefault(x => x.UserId == Id);
            updatedUser.IsDeleted = isDeleted;
            database.Entry(users.SingleOrDefault(x => x.UserId == Id)).CurrentValues.SetValues(updatedUser);
            database.SaveChanges();
        }
    }
}
