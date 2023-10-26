using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class Behörighet
    {
        [Key]
        public int BehörighetId { get; set; }
        public string Behörighetstyp { get; set; }
        public Behörighet()
        {
            
        }
        public Behörighet(string behörighetstyp)
        {
            Behörighetstyp = behörighetstyp;
        }
    }
   
}
