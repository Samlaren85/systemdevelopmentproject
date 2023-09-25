using DataLayer;
using EntityLayer;

namespace BusinessLayer
{
    public sealed class SessionController
    {
        private static SessionController? _instance = null!;
        public static User? LoggedIn { get; private set; }
        private SessionController(User user)
        {
            LoggedIn = user;
        }

        /// <summary>
        /// Används för att skapa en session med en annan användare
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static SessionController Instance(User user)
        {
            if (_instance == null)
            {
                _instance = new SessionController(user);
            }
            return _instance;
        }
        /// <summary>
        /// Används för kontrollera så att en användare är inloggad
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public static SessionController Instance()
        {
            if (_instance == null)
            {
                throw new ApplicationException("No user found!");
            }
            return _instance;
        }

        /// <summary>
        /// Används för att skapa upp en session och kontrollera inlogg och lösenord
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public static SessionController Instance(String ID, String password)
        {
            if (_instance == null)
            {
                UnitOfWork unitOfWork = new UnitOfWork();
                User credentials = unitOfWork.UserRepository.FirstOrDefault(u => u.UserID == ID);
                if (credentials != null && credentials.Password.Equals(password))
                {
                    _instance = new SessionController(credentials);
                    return _instance;
                }
                else
                {
                    throw new ApplicationException("Inloggning misslyckad!\nSkriv in ett giltigt användarnamn / lösenord!");
                }
            }
            else
            { throw new ApplicationException("Inloggning misslyckad!\nEn användare är redan inloggad i systemet"); }

        }
        /// <summary>
        /// Raderar pågående session så att inloggad person inte längre är aktiv i applikationen
        /// </summary>
        public static void Terminate()
        {
            _instance = null!;
            LoggedIn = null;
        }
    }
}
