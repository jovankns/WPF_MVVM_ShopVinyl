//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WCFShopVinyl
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblArtist
    {
        public tblArtist()
        {
            this.tblAlbums = new HashSet<tblAlbum>();
        }
    
        public int ArtistID { get; set; }
        public string ArtistName { get; set; }
    
        public virtual ICollection<tblAlbum> tblAlbums { get; set; }
    }
}