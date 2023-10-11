using EntityLayer;
using SkiResortSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiResortSystem.ViewModels
{
    public class ActivityOverviewViewModel:ObservableObject
    {
        public string Bokningsnummer { get; set; }
        public IList<Aktivitetsbokning> Activities { get; set; }
        public ActivityOverviewViewModel()
        {
            
        }
        public ActivityOverviewViewModel(Bokning booking, IList<Aktivitetsbokning> activities)
        {
            Bokningsnummer = booking.BokningsID;
            Activities = activities;
        }
    }
}
