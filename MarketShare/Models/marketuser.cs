using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MarketShare.Models
{
    public class marketuser
    {
        public float sellingprice { set; get; }
        //model specific fields 
        [Required]
        [Display(Name = "How much is the sum")]
        public string Captcha { get; set; }
       
        public string name { set; get; }
      
        public string emailid { set; get; }
        
        public string password { set; get; }
        public string securityquestion { set; get; }
        public string answer { set; get; }
    
        public string newpassword { get; set; }
        public string confirmpassword { get; set; }
        public string  contactno{ get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public DateTime dateofmsg { get; set; }
        public int feedbackid { get; set; }
        public int productid{ get; set; }
        public string productname { get; set; }
        public float mrp { get; set; }
        public int quantity { get; set; }
        public string productimage { get; set; }
        public int orderid { get; set; }
        public DateTime orderdate { get; set; }
        public string orderstatus { get; set; }
        public float totalprice { get; set; }
        public string totalitems { get; set; }
      
        public int wishlistid { get; set; }
       

    }
}