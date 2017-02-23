using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace MarketShare.Models
{
    public class admin
    {
        public string adminid { set; get; }
        public string password { set; get; }
        public string emailid { get; set; }
        public string message { get; set; }
        public int categoryid{ get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
        public string type { get; set; }
        public int productid { get; set; }
        public string productname { get; set; }
        public float mrp { get; set; }
        public float sellingprice{ get; set; }
        public float subtotalprice { get; set; }
        public string brand { get; set; }
        public int quantity { get; set; }
        public string size{ get; set; }
        public string color { get; set; }
        public string shortdescp { get; set; }
        public string longdescp { get; set; }
        public string productimage{ get; set; }
        public string sellerid { get; set; }
            
        public DateTime updatedate { get; set; }
        public DateTime addeddate { get; set; }
        public string status { get; set; }
        public int orderid{ get; set; }
        public string customerid { get; set; }
        public string productids { get; set; }
        public string prices{ get; set; }
        public float totalprice { get; set; }
       
        public DateTime orderdate { get; set; }
        public string orderstatus { get; set; }
        public string paymenttype { get; set; }
        public string paymentstatus { get; set; }
        public string deliverby { get; set; }

        public string salemonth { get; set; }

        public DateTime deliverddate { get; set; }
        public DateTime shippingdate { get; set; }
        public string courierno { get; set; }
        public string couriertype { get; set; }
        public string shippingaddress { get; set; }



    }
}