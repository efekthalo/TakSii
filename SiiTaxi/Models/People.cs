//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SiiTaxi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class People
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public People()
        {
            this.TaxiPeople = new HashSet<TaxiPeople>();
            this.Taxi = new HashSet<Taxi>();
        }
    
        public int PeopleId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AltEmail { get; set; }
        public bool IsApprover { get; set; }
        public string Phone { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaxiPeople> TaxiPeople { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Taxi> Taxi { get; set; }
    }
}
