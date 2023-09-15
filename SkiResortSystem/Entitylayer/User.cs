using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitylayer
{
    /// <summary>
    /// Following usertypes is avaliable: 1=System administrator 2=Reception worker 3=Skishop worker 4=Chief of reception 5=Chief of skishop 6=Chief of economy 7=Chief of marketing 8=CEO
    /// </summary>
    public class User
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public int _userTypeValue;
        public int userType
        {
            get { return _userTypeValue; }

            set
            {
                if (value < 1 || value > 6) //Värdet till höger (det höga) kommer behöva justeras utefter hur många användartyper vi önskar använda i systemet /Jonathan
                {
                    throw new ArgumentOutOfRangeException((nameof(userType)));
                }

                _userTypeValue = value;
            }
        }
        private static int _userCount;
        public User() //Konstruktor för användare
        {

        }
        public User(string password, int usertype)
        {
            _userCount++;
            UserID = "U" + _userCount.ToString("000000"); //Sista strängen här kan justeras utifrån hur vi bestämmer utformingen av lösenorden.
            Password = password;
            userType = usertype;
        }
    }
}
