//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestRiverWatchApi
{
    using System;
    using System.Collections.Generic;
    
    public partial class MetalBarCode
    {
        public int ID { get; set; }
        public string SampleNumber { get; set; }
        public string NumberSample { get; set; }
        public string LabID { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public bool Filtered { get; set; }
        public bool ContainMetal { get; set; }
        public string BoxNumber { get; set; }
        public bool Verified { get; set; }
        public Nullable<System.DateTime> LogDate { get; set; }
        public Nullable<System.DateTime> AnalyzeDate { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> DateLastModified { get; set; }
        public string UserLastModified { get; set; }
        public Nullable<int> SampleID { get; set; }
    }
}
