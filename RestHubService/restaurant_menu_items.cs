//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RestHubService
{
    using System;
    using System.Collections.Generic;
    
    public partial class restaurant_menu_items
    {
        public int menu_item_id { get; set; }
        public int restaurant_branch_id { get; set; }
        public string menu_item_name { get; set; }
        public string menu_item_description { get; set; }
        public string menu_item_photo { get; set; }
        public decimal menu_item_price { get; set; }
        public bool admin_approval_status { get; set; }
    }
}
