//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TigersProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class instruktor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public instruktor()
        {
            this.lekce = new HashSet<lekce>();
            this.druh = new HashSet<druh>();
            this.jazyk = new HashSet<jazyk>();
            this.dispozice1 = new HashSet<dispozice>();
        }
    
        public int ID { get; set; }
        public string JMENO { get; set; }
        public string PRIJMENI { get; set; }
        public string TELEFON { get; set; }
        public float SAZBA { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<lekce> lekce { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<druh> druh { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<jazyk> jazyk { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dispozice> dispozice1 { get; set; }
    }
}
