using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data.Entity.Validation;


namespace MarketShare.Controllers
{
    public class UserController : Controller
    {
        
        DB.demodbEntities db = new DB.demodbEntities();
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["democon"].ConnectionString);

        // GET: User
        public ActionResult Index()
        {
            List<MarketShare.Models.admin> imd = new List<Models.admin>();
            var query = (from data in db.product_master select data).Take(12);
            foreach (var item in query)
                imd.Add(new Models.admin()
                {
                    productid = item.productid,
                    productname = item.productname,
                    mrp = (float)item.mrp,
                    sellingprice = (float)item.sellingprice,
                    brand = item.brand,
                    status = item.status,
                    quantity = (int)item.quantity,
                    updatedate = (DateTime)item.updatedate,
                   productimage=item.productimage

                });
            return View(imd);
        }
      public ActionResult aboutus()
        {
          
            return View();
        }
      public ActionResult checkout()
        {
            return View();
        }
      public ActionResult thankyou(string orderid)
        {
            ViewBag.orderid = orderid;
            return View();
        }
       
      public ActionResult viewcart()
        { 
            return View();
        }
      [HttpGet]
      public ActionResult showcart(string productids)
        {
            string []productid = productids.Trim(',').Split(',');
            var totalrecords = "";
            double totalprice = 0.0;
            int k = 0;
            foreach (var pid in productid)
            {
                int ppid = int.Parse(pid);
                var query = (from data in db.product_master where data.productid==ppid select data).ToList();
                totalprice = totalprice +(double)query.FirstOrDefault().sellingprice;
                totalrecords += "<tr><td class='text-center'>";
                totalrecords += "<img src='../productimage/" + query.FirstOrDefault().productimage + "' height='60' width='60' />";
                totalrecords += "<br />" + query.FirstOrDefault().productname + "</td>";
                totalrecords += "<td class='text-center'><label>1</label></td>";
                totalrecords += "<td class='text-center'>" + query.FirstOrDefault().sellingprice + "</td>";
                totalrecords += "<td class='text-center'>" + query.FirstOrDefault().sellingprice + "</td>";
                totalrecords += "<td class='text-center'><input type='button' value='Remove' class='btn btn-primary remlink' data-id='"+k+"' /></td></tr>";
                k++;
            }
            string result = totalrecords;
            return Json(new { result,totalprice}, JsonRequestBehavior.AllowGet);
        }
      [HttpGet]
      public ActionResult contactus()
        {
            return View();
        }
      [HttpPost]
      public ActionResult contactus(MarketShare.Models.marketuser model)
        {
            DateTime dt = DateTime.Now;
            try
            {
                DB.user_feedback obj = new DB.user_feedback();
                obj.name = model.name;
                obj.emailid = model.emailid;
                obj.contactno = model.contactno;
                obj.subject = model.subject;
                obj.message = model.message;
                obj.dateofmsg = dt;
                db.user_feedback.Add(obj);
                db.SaveChanges();
                ViewBag.msg = "Successfully Submitted";
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ViewBag.msg = "Successfully Submitted";
                ModelState.Clear();
            }
        }
        public ActionResult mywishlist()
        {
            List<MarketShare.Models.marketuser> md = new List<Models.marketuser>();
            var query = (from wlist in db.wishlist_product join plist in db.product_master on wlist.productid equals plist.productid
                         select new
            {
                 plist.productimage,
                 plist.productname,
                 plist.mrp
            });
            foreach (var item in query)
            {
                md.Add(new Models.marketuser()
                {
                    productimage = item.productimage,
                    productname = item.productname,
                    mrp=(float)item.mrp,
                   
                });
            }
            return View(md);
           
        }
       
        public ActionResult productdetails(int productid)
        {
            List<MarketShare.Models.admin> imd = new List<Models.admin>();
            var a = (from plist in db.product_master
                     join clist in db.product_category on plist.categoryid equals clist.categoryid
                     where plist.productid == productid
                     select new
                     {
                         plist.productid,
                         clist.category,
                         clist.subcategory,
                         clist.type,
                         plist.productname,
                         plist.mrp,
                         plist.sellingprice,
                         plist.sellerid,
                         plist.brand,
                         plist.status,
                         plist.quantity,
                         plist.shortdescp,
                         plist.longdescp,
                         plist.color,
                         plist.size,
                         plist.addeddate,
                         plist.updatedate,
                         plist.productimage
                     });
            if (a.FirstOrDefault() != null)
            {
                ViewBag.productid = a.FirstOrDefault().productid;

                imd.Add(new Models.admin()
                {

                    category = a.FirstOrDefault().category,
                    subcategory = a.FirstOrDefault().subcategory,
                    type = a.FirstOrDefault().type,
                    productid = a.FirstOrDefault().productid,
                    productname = a.FirstOrDefault().productname,
                    mrp = (float)a.FirstOrDefault().mrp,
                    sellingprice = (float)a.FirstOrDefault().sellingprice,
                    brand = a.FirstOrDefault().brand,
                    status = a.FirstOrDefault().status,
                    quantity = (int)a.FirstOrDefault().quantity,
                    updatedate = (DateTime)a.FirstOrDefault().updatedate,
                    productimage = a.FirstOrDefault().productimage,
                    shortdescp = a.FirstOrDefault().shortdescp,
                    longdescp = a.FirstOrDefault().longdescp,
                    color = a.FirstOrDefault().color,
                    addeddate = (DateTime)a.FirstOrDefault().addeddate,
                    size = a.FirstOrDefault().size,
                    sellerid = a.FirstOrDefault().sellerid,
                });
            }
            return View(imd);
        }

        public ActionResult CaptchaImage(string prefix, bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            //generate new question 
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);

            //store answer 
            Session["Captcha" + prefix] = a + b;

            //image stream 
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise 
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x -r, y -r, r, r);
                    }
                }

                //add question 
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

                //render as Jpeg 
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            return img;
        }

//here is the post action or result in the same controller: )
        [HttpGet]

      public ActionResult changepassword()
        {
            return View();
        }
      [HttpPost]
      public ActionResult changepassword(MarketShare.Models.marketuser model)
        {
        //    SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["democon"].ConnectionString);
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand("update user_master set password='" + model.newpassword + "' where emailid='" + Session["emailid"] + "' and password='" + model.password + "'", con);
        //    int i = cmd.ExecuteNonQuery();
        //    if (i > 0)
        //    {
        //        ViewBag.msg = "Password Changed Successfully";
        //        return View(model);
        //    }
        //    else
        //    {
        //        ViewBag.msg = "Invalid Information";
        //        return View(model);
        //    }
        //}
        string uid = Session["emailid"].ToString();

        var u = (from userlist in db.user_master
                 where userlist.emailid == uid && userlist.password == model.password
                 select new
                 {
                     userlist.emailid,
                     userlist.password
                 }).ToList();
            if (u.FirstOrDefault() != null)
            {
                var cm = (from data in db.user_master where data.emailid == uid select data).FirstOrDefault();
                cm.password = model.newpassword;
                
                db.SaveChanges();
                ViewBag.msg = "Password Changed Successfully";                
            }
            else
            {
                ViewBag.msg = "Invalid Current Password.";
            }
            return View(model);
        }
      public ActionResult userhome()
        {
            return View();
        }
      public ActionResult login()
        {
            return View();
        }
      [HttpPost]
      public ActionResult login(MarketShare.Models.marketuser md)
        {
            var u = (from userlist in db.user_master
                        where userlist.emailid == md.emailid && userlist.password == md.password
                        select new
                        {
                            userlist.emailid,
                            userlist.password,
                            userlist.name
                        }).ToList();
            if(u.FirstOrDefault()!=null)
            {
                Session["emailid"] = u.FirstOrDefault().emailid;
                Session["name"] = u.FirstOrDefault().name;
                return RedirectToAction("userhome", "User");
            }
            else
            {
                ViewBag.msg = "Invalid login Credentials.";
            }
            return View();
        }
      public ActionResult register()
        {
            return View();
        }
      [HttpPost]
      public ActionResult register(MarketShare.Models.marketuser model)
        {
            //validate captcha 
            if (Session["Captcha"] == null || Session["Captcha"].ToString() != model.Captcha)
            {
                ModelState.AddModelError("Captcha", "Wrong value of sum, please try again.");
                //dispay error and generate a new captcha 
                return View(model);
            }
            else
            {
                try
                {
                    DB.user_master obj = new DB.user_master();
                    obj.name = model.name;
                    obj.emailid = model.emailid;
                    obj.password = model.password;
                    obj.securityquestion = model.securityquestion;
                    obj.answer = model.answer;
                    db.user_master.Add(obj);

                    db.SaveChanges();
                    return View();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ViewBag.msg = "Successfully Submitted";
                    ModelState.Clear();
                }
            }
           

        }
      public ActionResult forgotpassword()
        {
            return View();
        }
      [HttpPost]
      public ActionResult forgotpassword(MarketShare.Models.marketuser md)
        {
            var u = (from userlist in db.user_master
                        where userlist.emailid == md.emailid && userlist.securityquestion ==
md.securityquestion && userlist.answer == md.answer
                        select new
                        {
                            userlist.password
                        }).ToList();
            if(u.FirstOrDefault()!=null)
            {
                ViewBag.msg = "Your Password is:" + u.FirstOrDefault().password;
            }
            else
            {
                ViewBag.msg = "Invalid input Credentials.";
            }
            return View();
        }
      public ActionResult logout()
        {
            Session.Remove("emailid");
            Session.Abandon();
            return RedirectToAction("login","User");
        }
        [HttpGet]
        public ActionResult autosearch(string searchtxt)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select distinct productname from product_master where productname like '" + searchtxt + "%'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            List<string> d = new List<string>();
            while (dr.Read())
            {

                string name = dr[0].ToString();
                d.Add(name);
            }

            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult searchitem(string searchtxt)
        {
            List<MarketShare.Models.admin> imd = new List<Models.admin>();
            var query = (from data in db.product_master where data.productname.Contains(searchtxt) select data);
            foreach (var item in query)
                imd.Add(new Models.admin()
                {
                    productid = item.productid,
                    productname = item.productname,
                    mrp = (float)item.mrp,
                    sellingprice = (float)item.sellingprice,
                    brand = item.brand,
                    status = item.status,
                    quantity = (int)item.quantity,
                    updatedate = (DateTime)item.updatedate,
                    productimage = item.productimage

                });
            return View(imd);
        }

        public ActionResult myorders()
       {
            List<MarketShare.Models.marketuser> md = new List<Models.marketuser>();
            var query = (from data in db.order_details select data);
            foreach (var item in query)
            {
                var productid = item.productids.Split(',');
                var prices = item.prices.Split(',');
                string productdetails ="<table>";
                int k = 0;
                foreach (var pid in productid)
                {
                    int pid1 = int.Parse(pid);
                    k++;
                    var pquery = (from pdata in db.product_master where pdata.productid == pid1 select pdata);
                    productdetails+= "<tr><td>"+k+": "+pquery.FirstOrDefault().productname+"(Rs: "+ prices[k-1]+ "/-)</td></tr>";
                }
                    productdetails += "</table>";
                md.Add(new Models.marketuser()
                {
                    orderid = item.orderid,
                    orderdate = (DateTime)item.orderdate,
                    orderstatus = item.orderstatus,
                    totalprice =(float)item.totalprice,
                    totalitems=productdetails

                });
            }
            return View(md);
       }
        public ActionResult mencategory(string category,string subcategory)
        {
            List<MarketShare.Models.admin> imd = new List<Models.admin>();
           
            if (subcategory == "All")
            {
                var query = (from plist in db.product_master
                             join clist in db.product_category on plist.categoryid equals clist.categoryid
                             where clist.category == "Men"
                             select new
                             {
                                 plist.productid,
                                 plist.productname,
                                 plist.mrp,
                                 plist.sellingprice,
                                 plist.brand,
                                 plist.status,
                                 plist.quantity,
                                 plist.updatedate,
                                 plist.productimage
                             }).ToList();
                foreach (var item in query)
                    imd.Add(new Models.admin()
                    {
                        productid = item.productid,
                        productname = item.productname,
                        mrp = (float)item.mrp,
                        sellingprice = (float)item.sellingprice,
                        brand = item.brand,
                        status = item.status,
                        quantity = (int)item.quantity,
                        updatedate = (DateTime)item.updatedate,
                        productimage = item.productimage

                    });
            }
            else
            {
               
                    var query = (from plist in db.product_master
                                 join clist in db.product_category on plist.categoryid equals clist.categoryid
                                 where clist.category == "Men" && clist.subcategory==subcategory
                                 select new
                                 {
                                     plist.productid,
                                     plist.productname,
                                     plist.mrp,
                                     plist.sellingprice,
                                     plist.brand,
                                     plist.status,
                                     plist.quantity,
                                     plist.updatedate,
                                     plist.productimage
                                 }).ToList();
                foreach (var item in query)
                    imd.Add(new Models.admin()
                    {
                        productid = item.productid,
                        productname = item.productname,
                        mrp = (float)item.mrp,
                        sellingprice = (float)item.sellingprice,
                        brand = item.brand,
                        status = item.status,
                        quantity = (int)item.quantity,
                        updatedate = (DateTime)item.updatedate,
                        productimage = item.productimage

                    });

            }

            return View(imd);
       }
        public ActionResult insertwishdata(string productid)
        {
            
            string customerid = Session["emailid"].ToString();
            DB.wishlist_product obj = new DB.wishlist_product();
            obj.productid =int.Parse(productid);
            obj.customerid = customerid;          
            db.wishlist_product.Add(obj);
            db.SaveChanges();
            string msg ="Product Added to Wishlist";
            return Json(new { msg }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult womencategory(string category, string subcategory)
        {
            List<MarketShare.Models.admin> imd = new List<Models.admin>();
            if (subcategory == "All")
            {
                var query = (from plist in db.product_master
                             join clist in db.product_category on plist.categoryid equals clist.categoryid
                             where clist.category == "Women"
                             select new
                             {
                                 plist.productid,
                                 plist.productname,
                                 plist.mrp,
                                 plist.sellingprice,
                                 plist.brand,
                                 plist.status,
                                 plist.quantity,
                                 plist.updatedate,
                                 plist.productimage
                             }).ToList();
                foreach (var item in query)
                    imd.Add(new Models.admin()
                    {
                        productid = item.productid,
                        productname = item.productname,
                        mrp = (float)item.mrp,
                        sellingprice = (float)item.sellingprice,
                        brand = item.brand,
                        status = item.status,
                        quantity = (int)item.quantity,
                        updatedate = (DateTime)item.updatedate,
                        productimage = item.productimage

                    });
            }
            else
            {
                var query = (from plist in db.product_master
                             join clist in db.product_category on plist.categoryid equals clist.categoryid
                             where clist.category == "Women" && clist.subcategory == subcategory
                             select new
                             {
                                 plist.productid,
                                 plist.productname,
                                 plist.mrp,
                                 plist.sellingprice,
                                 plist.brand,
                                 plist.status,
                                 plist.quantity,
                                 plist.updatedate,
                                 plist.productimage
                             }).ToList();
                foreach (var item in query)
                    imd.Add(new Models.admin()
                    {
                        productid = item.productid,
                        productname = item.productname,
                        mrp = (float)item.mrp,
                        sellingprice = (float)item.sellingprice,
                        brand = item.brand,
                        status = item.status,
                        quantity = (int)item.quantity,
                        updatedate = (DateTime)item.updatedate,
                        productimage = item.productimage

                    });
                }
           return View(imd);
        }
        public ActionResult kidscategory(string category, string subcategory)
        {
            List<MarketShare.Models.admin> imd = new List<Models.admin>();
            if (subcategory == "All")
            {
                var query = (from plist in db.product_master
                             join clist in db.product_category on plist.categoryid equals clist.categoryid
                             where clist.category == "kids"
                             select new
                             {
                                 plist.productid,
                                 plist.productname,
                                 plist.mrp,
                                 plist.sellingprice,
                                 plist.brand,
                                 plist.status,
                                 plist.quantity,
                                 plist.updatedate,
                                 plist.productimage
                             }).ToList();
                foreach (var item in query)
                    imd.Add(new Models.admin()
                    {
                        productid = item.productid,
                        productname = item.productname,
                        mrp = (float)item.mrp,
                        sellingprice = (float)item.sellingprice,
                        brand = item.brand,
                        status = item.status,
                        quantity = (int)item.quantity,
                        updatedate = (DateTime)item.updatedate,
                        productimage = item.productimage

                    });

            }
            else
            {
                var query = (from plist in db.product_master
                             join clist in db.product_category on plist.categoryid equals clist.categoryid
                             where clist.category == "kids" && clist.subcategory == subcategory
                             select new
                             {
                                 plist.productid,
                                 plist.productname,
                                 plist.mrp,
                                 plist.sellingprice,
                                 plist.brand,
                                 plist.status,
                                 plist.quantity,
                                 plist.updatedate,
                                 plist.productimage
                             }).ToList();
                foreach (var item in query)
                    imd.Add(new Models.admin()
                    {
                        productid = item.productid,
                        productname = item.productname,
                        mrp = (float)item.mrp,
                        sellingprice = (float)item.sellingprice,
                        brand = item.brand,
                        status = item.status,
                        quantity = (int)item.quantity,
                        updatedate = (DateTime)item.updatedate,
                        productimage = item.productimage

                    });
            }
                
            return View(imd);
        }
       
        public ActionResult viewmyorders(int oid)
        {
            var cm = (from data in db.order_details where data.orderid == oid select data).FirstOrDefault();

            ViewBag.orderid = cm.orderid;
            ViewBag.customerid = cm.customerid;
            ViewBag.orderstatus = cm.orderstatus;
            ViewBag.orderdate = cm.orderdate;
            ViewBag.totalprice = cm.totalprice;
            ViewBag.shippingaddress = cm.shippingaddress;
            ViewBag.courierno = cm.courierno;
            ViewBag.couriertype = cm.couriertype;
            ViewBag.shippingdate = cm.shippingdate;
            ViewBag.deliverby = cm.deliverby;
            var productid = cm.productids.Split(',');
            List<MarketShare.Models.admin> imd = new List<Models.admin>();

            foreach (var pid in productid)
            {
                int pid1 = int.Parse(pid);
                var u = (from data in db.product_master
                         where data.productid == pid1
                         select data).FirstOrDefault();
                var subtotalprice = (float)u.sellingprice * 1;
                imd.Add(new MarketShare.Models.admin
                {
                    productid = u.productid,
                    productname = u.productname,
                    sellingprice = (float)u.sellingprice,
                    quantity = 1,
                    subtotalprice = subtotalprice,


                });
            }

            return View(imd);
           
        }
        
        [HttpGet]
        public ActionResult saveorder(string name,string city,string landmark,string state,string pincode,string address,string emailid,string mobileno,string productids)
        {
            productids=productids.Trim(',');
            string [] productidarr = productids.Split(',');
            string prices = "";
            double totalprice = 0;
            string quantity = "";
            foreach (string pid in productidarr)
            {
                int pid1 = int.Parse(pid);
                var u = (from productlist in db.product_master
                         where productlist.productid ==pid1
                         select new
                         {
                             productlist.sellingprice
                         }).ToList();
                prices= prices+","+u.FirstOrDefault().sellingprice;
                quantity = quantity + ",1";
                totalprice =totalprice+(double)u.FirstOrDefault().sellingprice;
           }
           prices = prices.Trim(',');
           quantity = quantity.Trim(',');
           DateTime dt = DateTime.Now;
           string add = "Name:"+name + "<br/>"+"City:" + city + "<br/>"+"Landmark:" + landmark + "<br/>"+"State:" + state + "<br/>"+"Pincode/Zipcode:" + pincode + "<br/>"+"Address:" + address + "<br/>"+"Email Id:" + emailid + "<br/>"+"Mobile No:" + mobileno + "<br/>";
           DB.order_details obj = new DB.order_details();
           obj.customerid = Session["emailid"].ToString();
           obj.productids = productids;
           obj.quantity =quantity;
           obj.prices =prices;
           obj.totalprice =totalprice;
           obj.orderdate = dt;
           obj.orderstatus ="pending";
           obj.paymenttype = "COD";
           obj.paymentstatus ="pending";
           obj.shippingaddress = add;
           db.order_details.Add(obj);
           db.SaveChanges();
            var ss = Session["emailid"].ToString();
            var u1 = (from orderlist in db.order_details
                      where orderlist.customerid == ss
                      select orderlist).Max(orderlist => orderlist.orderid);
            int orderid = u1;
            string msg = "MS"+orderid;
            return Json(new { msg }, JsonRequestBehavior.AllowGet);
           }  
    }
}