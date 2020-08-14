using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Models.ViewModel
{
    public class ContentListView
    {
        public int User_Id { get; set; }
        public int Id { get; set; }
        public string FIO { get; set; }
        public string Message { get; set; }
        public int Forum_id { get; set; }
    }
}