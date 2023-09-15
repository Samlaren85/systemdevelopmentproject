﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Businesslayer
{
    public sealed class SessionController
    {
        private static SessionController _instance;
        public static User? LoggedIn { get; private set; }
        private SessionController(User user)
        {
            LoggedIn = user;
        }
       
        // Kommer vi inte kunna använda denna logik när referenstyper inte är nullable i denna frameworkversion? /Jonathan
        public static SessionController Instance(User user)
        {
            if (_instance == null)
            {
                _instance = new SessionController(user);
            }
            return _instance;
        }
        public static SessionController Instance()
        {
            if (_instance == null)
            {
                throw new ApplicationException("No user found!");
            }
            return _instance;
        }

        public static SessionController Instance(String ID, String password)
        {
            if (_instance == null)
            {
                UnitOfWork unitOfWork = new UnitOfWork();
                User credentials = unitOfWork.UserRepository.FirstOrDefault(u => u.UserId == ID);
                if (credentials != null && credentials.Password.Equals(password))
                {
                    LoggedIn = credentials;
                    _instance = new SessionController(credentials);
                    return _instance;
                }
                else
                {
                    throw new ApplicationException("Login failed!\nEnter correct username and/or password!");
                }
            }
            else
            { throw new ApplicationException("Login failed!\nA user is allready logged in to the system"); }
           
        }
        public static void Terminate()
        {
            _instance = null;
            LoggedIn = null;
        }
    }
}
