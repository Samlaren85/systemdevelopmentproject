using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    /// <summary>
    /// Följande användartyper finns tillgängliga: 1=Systemadministratör 2=Receptionist 3=Skidshop 4=Avdelningschef reception 5=Avdelningschef skidshop 6=Ekonomichef 7=Marknadschef 8=VD
    /// </summary>
    public class Användare
    {
        [Key]
        public string UserID { get; set; }
        public string Password { get; set; }

        private int _userTypeValue;
        public Roll RollID { get; set; }
        public int UserType
        {
            get { return _userTypeValue; }

            set
            {
                if (value < 1 || value > 8) //Värdet till höger (det höga) kommer behöva justeras utefter hur många användartyper vi önskar använda i systemet /Jonathan
                {
                    throw new ArgumentOutOfRangeException(nameof(UserType));
                }

                _userTypeValue = value;
            }
        }
        private static int _userCount;

       
        public Användare()
        {

        }
        /// <summary>
        /// Konstruktor för klassen Användare
        /// </summary>
        /// <param name="password"></param>
        /// <param name="usertype"></param>
        /// <param name="rollID"></param>
        public Användare(string password, int usertype, Roll rollID)
        {
            _userCount++;
            UserID = "A" + _userCount.ToString("000000");
            Password = password;
            UserType = usertype;
            RollID = rollID;
        }
    }
}
