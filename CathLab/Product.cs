//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CathLab
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public int ID { get; set; }
        public string PartNumber { get; set; }
        public string SerialNumber { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public Nullable<int> LocationID { get; set; }
        public Nullable<System.DateTime> DateUsed { get; set; }
        public Nullable<int> LotNumber { get; set; }
        public Nullable<int> StatusID { get; set; }
    
        public virtual Location Location { get; set; }
        public virtual PartNumber PartNumber1 { get; set; }
        public virtual Status Status { get; set; }
    }
}
