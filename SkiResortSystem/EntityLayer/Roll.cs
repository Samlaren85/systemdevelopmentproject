using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class Roll
    {
        [Key]
        public int RollId { get; set; }
        public string Rolltyp { get; set; }
        public List<Behörighet> BehörighetID { get; set; }
        public Roll()
        {
            
        }
        public Roll(string rolltyp, List<Behörighet> behörighetID)
        {
            Rolltyp = rolltyp;
            BehörighetID = behörighetID;
        }
    }
}
