using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class Roll
    {
        private static int _antalRoller = 0;
        [Key]
        public string RollID { get; set; }
        public string Rolltyp { get; set; }
        public List<Behörighet> BehörighetID { get; set; }
        public Roll()
        {
            
        }
        public Roll(string rolltyp, List<Behörighet> behörighetID)
        {
            _antalRoller++;
            RollID = "ROLL" + _antalRoller.ToString("000");
            Rolltyp = rolltyp;
            BehörighetID = behörighetID;
        }
    }
}
