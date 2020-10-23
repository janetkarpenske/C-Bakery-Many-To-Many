using System.Collections.Generic;

namespace Bakery.Models
{
    public class Treat
    {
        public Treat()
        {
            this.Flavors = new HashSet<FlavorTreat>();
        }

        public int TreatId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public ICollection<FlavorTreat> Flavors { get;}
    }
}

//Item to Treat, Category to Flavor