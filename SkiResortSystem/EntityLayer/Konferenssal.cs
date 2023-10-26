using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class Konferenssal
    {
        [Key]
        public string KonferensId { get; set; }
        public string KonferensBenämning { get; set; }
        public int AntalPersoner { get; set; }
        public Konferenssal()
        {
            
        }
        public Konferenssal(string konferensBenämning, int antalPersoner)
        {
            KonferensBenämning = konferensBenämning;
            AntalPersoner = antalPersoner;
        }
    }
}
