using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer
{
    /// <summary>
    /// Följande användartyper finns tillgängliga: 1=Systemadministratör 2=Receptionist 3=Skidshop 4=Avdelningschef reception 5=Avdelningschef skidshop 6=Ekonomichef 7=Marknadschef 8=VD
    /// </summary>
    public class Användare
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UserId { get; set; }
        public string AnvändarNamn {  get; set; }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                if (value != null)
                {
                    if (value.Any(ch => char.IsLetterOrDigit(ch) && value.Any(ch => char.IsAsciiLetterUpper(ch)) && value.Any(ch => char.IsAsciiLetterLower(ch)) && value.Any(ch => char.IsAsciiDigit(ch))))
                    {
                        password = value;
                    }
                    else throw new InvalidOperationException("Lösenordet måste innehålla minst ett specialtecken, en stor bokstav, en liten bokstav och en siffra!");
                }
                else throw new InvalidOperationException("Lösenordet måste vara satt på användaren!");
                password = value;
            }
        }

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
            Password = password;
            UserType = usertype;
            RollID = rollID;
        }
    }
}
