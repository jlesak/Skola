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
    
    public partial class dispozice
    {
        public int ID { get; set; }
        public System.DateTime ZACATEK { get; set; }
        public string POZNAMKA { get; set; }
        public short KLUB { get; set; }
        public string Property1 { get; set; }
    
        public virtual instruktor instruktor { get; set; }
    }
}
