using DataLayer;
using EntityLayer;

namespace BusinessLayer
{
    public sealed class SessionController
    {
        private static SessionController? _instance = null!;
        public static Användare? LoggedIn { get; private set; }
        private SessionController(Användare användare)
        {
            LoggedIn = användare;
        }

        /// <summary>
        /// Används för att skapa en session med en annan användare
        /// </summary>
        /// <param name="användare"></param>
        /// <returns></returns>
        public static SessionController Instance(Användare användare)
        {
            if (_instance == null)
            {
                _instance = new SessionController(användare);
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
                Användare credentials = unitOfWork.AnvändarRepository.FirstOrDefault(a => a.UserID == ID);
                if (credentials != null && credentials.Password.Equals(password))
                {
                    _instance = new SessionController(credentials);
                    Behörighetskontroll(credentials);
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

        public static void Behörighetskontroll(Användare användare)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            if (användare != null)
            {
                if (användare.UserType == 1) //Inloggad som systemadministratör
                {
                    Behörighet användarBehörighet = new Behörighet("1 - Systemadministratör");
                    if (användare.RollID != null)
                    {
                        användare.RollID.Rolltyp = "Systemadministratör";
                        unitOfWork.AnvändarRepository.Update(användare);
                    }
                    
                    användare.RollID.BehörighetID.Add(användarBehörighet);
                }
                else if (användare.UserType == 2) //Inloggad som receptionist
                {
                    Behörighet användarBehörighet = new Behörighet("2 - Receptionist");
                    if (användare.RollID != null)
                    {
                        användare.RollID.Rolltyp = "Receptionist";
                    }
                    användare.RollID.BehörighetID.Add(användarBehörighet);
                }
                else if (användare.UserType == 3) //Inloggad som Skidshop
                {
                    Behörighet användarBehörighet = new Behörighet("3 - Skidshop");
                    if (användare.RollID != null)
                    {
                        användare.RollID.Rolltyp = "Skidshopspersonal";
                    }
                    användare.RollID.BehörighetID.Add(användarBehörighet);
                }
                else if (användare.UserType == 4) //Inloggad som avdelningschef reception
                {
                    Behörighet användarBehörighet = new Behörighet("4 - Avdelningschef reception");
                    if (användare.RollID != null)
                    {
                        användare.RollID.Rolltyp = "Avdelningschef";
                    }
                    användare.RollID.BehörighetID.Add(användarBehörighet);
                }
                else if (användare.UserType == 5) //Inloggad som avdelningschef skidshop
                {
                    Behörighet användarBehörighet = new Behörighet("5 - Avdelningschef skidshop");
                    if (användare.RollID != null)
                    {
                        användare.RollID.Rolltyp = "Avdelningschef";
                    }
                    användare.RollID.BehörighetID.Add(användarBehörighet);
                }
                else if (användare.UserType == 6) //Inloggad som ekonomichef
                {
                    Behörighet användarBehörighet = new Behörighet("6 - Ekonomichef");
                    if (användare.RollID != null)
                    {
                        användare.RollID.Rolltyp = "Ekonomichef";
                    }
                    användare.RollID.BehörighetID.Add(användarBehörighet);
                }
                else if (användare.UserType == 7) //Inloggad som marknadschef
                {
                    Behörighet användarBehörighet = new Behörighet("7 - Marknadschef");
                    if (användare.RollID != null)
                    {
                        användare.RollID.Rolltyp = "Marknadschef";
                    }
                    användare.RollID.BehörighetID.Add(användarBehörighet);
                }
                else if (användare.UserType == 8) //Inloggad som VD
                {
                    Behörighet användarBehörighet = new Behörighet("8 - Verkställande Direktör");
                    if (användare.RollID != null)
                    {
                        användare.RollID.Rolltyp = "VD";
                    }
                    användare.RollID.BehörighetID.Add(användarBehörighet);
                }
            }
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
