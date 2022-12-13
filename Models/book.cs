//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookStoreAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public book()
        {
            this.authors = new HashSet<author>();
            this.genres = new HashSet<genre>();
        }
    
        public int? book_id { get; set; }
        public string title { get; set; }
        public Nullable<int> total_pages { get; set; }
        public Nullable<decimal> rating { get; set; }
        public string isbn { get; set; }
        public Nullable<System.DateTime> published_date { get; set; }
        public Nullable<int> publisher_id { get; set; }
    
        public virtual publisher publisher { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<author> authors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<genre> genres { get; set; }
    }
}
