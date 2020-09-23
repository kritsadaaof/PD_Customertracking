using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PD_CusTracking.Models;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace PD_CusTracking.Controllers
{
    public class HomeController : Controller
    {
        private Entities DbFile = new Entities();
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

        public void Receive(Object sender,EventArgs e)
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
    }
}