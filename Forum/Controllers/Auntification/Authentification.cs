using Forum.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Controllers.Auntification
{
    public class Authentification
    {

        private static bool _status = false;
        public static string _logIn { get; set; }
        public static string _password { get; set; }
        public static string _FIO { get; set; }
        public static int _Id { get { return GetUserId(_logIn); } }
        public static string _role { get; set; }

        //статус аунтификации пользователя в системе
        public static bool Status { get { return _status; } }

        public Authentification(string login, string password)
        {
            _logIn = login;
            _password = password;
            UserVerification();
        }

        private static bool UserVerification()
        {
            string cipherPass = Encript.Encrypt(_password);
            Entities db = new Entities();
            Users users = db.Users.Where(n => n.LogIn == _logIn).FirstOrDefault();
            if (users != null)
                if (users.Password == cipherPass)
                {
                    _FIO = users.FIO;
                    _status = true;
                    _role = GetRole((int)users.Role_id);
                    return true;
                }
            return false;
        }
        public static void ExitSession()
        {
            _status = false;
            _logIn = "";
            _password = "";
            _FIO = "";
            _role = "";
        }

        public static void Registration(RegistrUserModel user)
        {
            Entities db = new Entities();
            Users users = new Users() { FIO = user.FIO, LogIn = user.LogIn, Password = Encript.Encrypt(user.Password), Role_id = int.Parse(user.Role) };
            db.Users.Add(users);
            db.SaveChanges();
            _FIO = users.FIO;
            _password = users.Password;
            _role = GetRole(int.Parse(user.Role));
            _status = true;
        }

        public static bool NoMatches(string login)
        {
            Entities db = new Entities();
            Users users = db.Users.Where(n => n.LogIn == login).FirstOrDefault();
            if (users == null)
                return true;
            else
                return false;
        }

        public static string GetRole(int role_id)
        {
            Entities db = new Entities();
            return db.Role.Where(n => n.Id == role_id).Select(n => n.Role1).FirstOrDefault();
        }
        private static int GetUserId(string login)
        {
            Entities db = new Entities();
            return db.Users.Where(n => n.LogIn == login).Select(n => n.Id).FirstOrDefault();
        }
    }
}