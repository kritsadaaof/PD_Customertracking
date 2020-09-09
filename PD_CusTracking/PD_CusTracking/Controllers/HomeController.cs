using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PD_CusTracking.Models;
using System.Web.Script.Serialization;

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

        public ActionResult Receive()
        {
            return View();
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