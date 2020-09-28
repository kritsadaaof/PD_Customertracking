using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PD_CusTracking.Models;//1
//using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

namespace PD_CusTracking.Controllers
{
    public class HomeController : Controller
    {
        private Entities DbFile = new Entities(); //2
        public ActionResult Index()
        {
            return View();
        } 
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Receive()
        { 
            return View();
        }

        public void Receives(Object sender,EventArgs e)
        {
            Delivery_PD_Process DbFile = new Delivery_PD_Process();
            
        }
        public static string insertdata(string tag, string truck, string user)
        {
            string msg = "";
            SqlConnection con = new SqlConnection("Data Source=localhost\\SQLEXPRESS;initial catalog=WMS_PD;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("insert into Delivery_PD_Process values(@tag, @truck,@user)", con);
            cmd.Parameters.AddWithValue("@tag", tag);
            cmd.Parameters.AddWithValue("@truck", truck);
            cmd.Parameters.AddWithValue("@user", user);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            if (i == 1)
            {
                msg = "true";
            } else
            {
                msg = "false";
            }
            return msg;
        }

            public string CheckUser(string USER)
        {
            try
            {
                var data = (from Ms_M in DbFile.WMS_PD_Master_User
                            where Ms_M.Code_Mem.Equals(USER)
                            select new
                            {
                                Ms_M.Mem_Name
                            }).ToList();
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            catch { return null; }
        }//CheckLocation  //CheckUser

        public string CheckTAG(string BARCODE)
        {
               try
               {
                   var data = (from TR_Pro in DbFile.WMS_PD_Product
                               where TR_Pro.Barcode.Equals(BARCODE)
                               select new
                               {
                                   TR_Pro.Barcode,
                                   TR_Pro.PRO_Cus
                                   
                               }).ToList();
                   string jsonlog = new JavaScriptSerializer().Serialize(data);
                   return jsonlog;
               }
               catch { return null; }  
        }
        public string SaveData(string BARCODE,string TRUCK,string USER)
        {
            try
            {
                var CheckDLY = DbFile.Delivery_PD_Process.OrderByDescending(a=>a.Delivery_WO).FirstOrDefault();

                int WO = int.Parse(CheckDLY.Delivery_WO) + 1; 

                var DLY = new Delivery_PD_Process();
                DLY.Delivery_WO = WO.ToString();
                DLY.Delivery_TAG = BARCODE;
                DLY.Delivery_Truck = TRUCK;
                DLY.Delivery_User = USER;
                DLY.Delivery_Status = "T";
                DLY.Delivery_Date = DateTime.Now;
                DbFile.Delivery_PD_Process.Add(DLY);
                DbFile.SaveChanges();
                return "S";
            }
            catch { return "F"; }
        }


    }
}