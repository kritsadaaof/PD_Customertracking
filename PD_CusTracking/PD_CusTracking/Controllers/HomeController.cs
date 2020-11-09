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
using System.IO;
using System.Net;
using System.Text;

namespace PD_CusTracking.Controllers
{
    public class HomeController : Controller
    {
        private Entities DbFile = new Entities(); //2 EworkTTLiveEntities
        private EworkTTLiveEntities DbFileETT = new EworkTTLiveEntities();
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

        public ActionResult Return()
        {
            return View();
        }

        public ActionResult ReturnDoc()
        {
            return View();
        }
        public string GETWO(string WO)
        {
            try
            {
                var data = (from Dl_TR in DbFile.Delivery_PD_Process
                            join WMS_Pros in DbFile.WMS_PD_Product on Dl_TR.Delivery_TAG equals WMS_Pros.Barcode into WMS_Pro1
                            from WMS_Pro in WMS_Pro1.DefaultIfEmpty()
                            where Dl_TR.Delivery_WO.Equals(WO) && Dl_TR.Delivery_Status.Equals("T")
                            select new
                            {
                                Dl_TR.Delivery_WO,
                                Dl_TR.Delivery_TAG,
                                Dl_TR.Delivery_Truck,
                                WMS_Pro.PRO_Cus
                            }).AsEnumerable().GroupBy(k => new { k.PRO_Cus }).Select(k => k.First()).ToList();
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            catch { return null; }
        }
        public string GETWODoc(string WO)
        {
            try
            {
                var data = (from Dl_TR in DbFile.Delivery_PD_Process
                            join WMS_Pros in DbFile.WMS_PD_Product on Dl_TR.Delivery_TAG equals WMS_Pros.Barcode into WMS_Pro1
                            from WMS_Pro in WMS_Pro1.DefaultIfEmpty()
                            where Dl_TR.Delivery_WO.Equals(WO)&&Dl_TR.Delivery_Doc==null
                            select new
                            {
                                Dl_TR.Delivery_WO,
                                Dl_TR.Delivery_TAG,
                                Dl_TR.Delivery_Truck,
                                WMS_Pro.PRO_Cus
                            }).AsEnumerable().GroupBy(k => new { k.PRO_Cus }).Select(k => k.First()).ToList();
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            catch { return null; }
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
        }

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
        public string CheckTAG_Return(string WO, string CUS, string BARCODE)
        {
            try
            {
                var data = (from DL_TR in DbFile.Delivery_PD_Process
                            join WMS_Pros in DbFile.WMS_PD_Product on DL_TR.Delivery_TAG equals WMS_Pros.Barcode into WMS_Pro1
                            from WMS_Pro in WMS_Pro1.DefaultIfEmpty()
                            where DL_TR.Delivery_WO.Equals(WO) && DL_TR.Delivery_TAG.Equals(BARCODE) && WMS_Pro.PRO_Cus.Equals(CUS) && DL_TR.Delivery_Status.Equals("T")
                            select new
                            {
                                DL_TR.Delivery_TAG

                            }).ToList();
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            catch { return null; }
        }
        public string CheckTAG_ReturnDOC(string WO, string CUS)
        {
            try
            {
                var data = (from DL_TR in DbFile.Delivery_PD_Process
                            join WMS_Pros in DbFile.WMS_PD_Product on DL_TR.Delivery_TAG equals WMS_Pros.Barcode into WMS_Pro1
                            from WMS_Pro in WMS_Pro1.DefaultIfEmpty()
                            where DL_TR.Delivery_WO.Equals(WO) && WMS_Pro.PRO_Cus.Equals(CUS) && DL_TR.Delivery_Status.Equals("T")
                            select new
                            {
                                DL_TR.Delivery_WO,
                                WMS_Pro.PRO_Cus

                            }).FirstOrDefault();

                Session["PicNAME"] = (data.Delivery_WO) + (data.PRO_Cus); 
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            catch { return null; }
        }
        public string CheckWO(string BARCODE)
        {
            try
            {
                var data = (from ETT_TR in DbFileETT.TR_Record
                            where ETT_TR.WO_No.Equals(BARCODE)
                            select new
                            {
                                ETT_TR.WO_No,
                                ETT_TR.Truck_Code

                            }).ToList();
                string jsonlog = new JavaScriptSerializer().Serialize(data); 

                return jsonlog;
            }
            catch { return null; }
        }
        public string SaveData(string BARCODE, string WO, string USER)
        {
            try
            {
                var ETT_TR = DbFileETT.TR_Record.Where(a => a.WO_No.Equals(WO)).FirstOrDefault();
                var DLY = new Delivery_PD_Process();
                DLY.Delivery_WO = WO;
                DLY.Delivery_TAG = BARCODE;
                DLY.Delivery_Truck = ETT_TR.Truck_Code;
                DLY.Delivery_User = USER;
                DLY.Delivery_Status = "T";
                DLY.Delivery_Date = DateTime.Now;
                DbFile.Delivery_PD_Process.Add(DLY);
                DbFile.SaveChanges();
             //   lineReceiveNotification(WO);
                return "S";
            }
            catch { return "F"; }
        }
        public string SaveReturn(string BARCODE, string WO, string USER)
        {
            try
            {
                var ETT_TR = DbFileETT.TR_Record.Where(a => a.WO_No.Equals(WO)).FirstOrDefault();
                var DLY = DbFile.Delivery_PD_Process.Where(a => a.Delivery_TAG.Equals(BARCODE)).FirstOrDefault();
                DLY.Delivery_User = USER;
                DLY.Delivery_Status = "S";
                DLY.Delivery_Date_Cus = DateTime.Now;
                DbFile.SaveChanges();
                lineReturnNotification(WO);
                return "S";
            }
            catch { return "F"; }
        }

        public string SaveDOC(string WO, string CUS)
        { 
             try
              {
            var data = (from DL_TR in DbFile.Delivery_PD_Process
                            join WMS_Pros in DbFile.WMS_PD_Product on DL_TR.Delivery_TAG equals WMS_Pros.Barcode into WMS_Pro1
                            from WMS_Pro in WMS_Pro1.DefaultIfEmpty()
                            where DL_TR.Delivery_WO.Equals(WO) && WMS_Pro.PRO_Cus.Equals(CUS) && DL_TR.Delivery_Status.Equals("T")
                            select new
                            {
                                DL_TR.Delivery_WO,
                                WMS_Pro.PRO_Cus,
                                DL_TR.Delivery_Doc,
                                DL_TR.Delivery_TAG,
                                DL_TR.ID
                            }).ToList();
                var dataDL = DbFile.Delivery_PD_Process.Where(a => a.Delivery_WO.Equals(WO)).FirstOrDefault();
                Session["PicNAME"] = WO;
                dataDL.Delivery_Doc = WO +".JPG";
                dataDL.Delivery_Pic = WO + ".JPG";
                DbFile.SaveChanges();
                UploadNotification(WO);
                return "S";
            }
            catch { return "F"; }
        } 
        [HttpPost]
        public string UploadFilesDoc(HttpPostedFileBase file, string NAME)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Content/Doc"),
                    Path.GetFileName(Session["PicNAME"].ToString() + ".jpg"));
                    file.SaveAs(path);
                    return "S";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    return null;
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
                return null;
            }

        }
        [HttpPost]
        public string UploadFilesPic(HttpPostedFileBase file, string NAME)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Content/Pic"),
                    Path.GetFileName(Session["PicNAME"].ToString() + ".jpg"));
                    file.SaveAs(path);
                    return "S";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    return null;
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
                return null;
            }

        }

        [HttpPost]
        public string SaveUploadDoc(string BARCODE)
        {
            try
            {
                var DLY = DbFile.Delivery_PD_Process.Where(a => a.Delivery_TAG.Equals(BARCODE)).FirstOrDefault();
                DLY.Delivery_Doc = BARCODE + ".jpg";
                DbFile.SaveChanges();
                return "S";
            }
            catch { return "N"; }
        }
        [HttpPost]
        public string SaveUploadPic(string BARCODE)
        {
            try
            {
                var DLY = DbFile.Delivery_PD_Process.Where(a => a.Delivery_TAG.Equals(BARCODE)).FirstOrDefault();
                DLY.Delivery_Doc = BARCODE + ".jpg";
                DbFile.SaveChanges();
                return "S";
            }
            catch { return "N"; }
        }
        public string lineReceiveNotification(string WO)
        {
            var getWheel = DbFile.Delivery_PD_Process.Where(a => a.Delivery_WO.Equals(WO)).ToList();
            int sum = getWheel.Count();
            string token = "C4EEhomujMG5xHobcPBJXS7Nfsb7gX7du38kCDGww3s";// Delivery PD
            var WOBarcode = DbFile.Delivery_PD_Process.Where(a => a.Delivery_WO.Equals(WO)).FirstOrDefault();
            var getDriver = DbFileETT.TR_Record.Where(a => a.WO_No.Equals(WOBarcode.Delivery_WO)).FirstOrDefault();
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                var postData = string.Format("message={0}", "\n" + "*********แจ้งเตือน*********" + "\n");
                postData += string.Format("บาร์โค้ด WO : " + WOBarcode.Delivery_WO + "\n");
                
                foreach (var item in getWheel)
                {
                    postData += string.Format("บาร์โค้ด Tag ล้อ : " + item.Delivery_TAG + "\n");
                }
                postData += string.Format("จำนวน : " + sum + "ล้อ"+"\n");
                postData += string.Format("คนขับรถ : " + getDriver.Driv_Name + "\n");
                postData += string.Format("วันที/เวลาที่รับของขึ้นรถ : " + DateTime.Now + "\n");
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);

                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }
        private string lineReturnNotification(string WO)
        {

            string token = "C4EEhomujMG5xHobcPBJXS7Nfsb7gX7du38kCDGww3s";// Delivery PD
            var WOBarcode = DbFile.Delivery_PD_Process.Where(a => a.Delivery_WO.Equals(WO)).FirstOrDefault();
            var getBarcode  = DbFile.WMS_PD_Product.Where(a => a.Barcode.Equals(WOBarcode.Delivery_TAG)).FirstOrDefault();
          
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                var postData = string.Format("message={0}", "\n" + "*********แจ้งเตือน*********" + "\n");
                postData += string.Format("บาร์โค้ด WO : " + WOBarcode.Delivery_WO + "\n");
                postData += string.Format("บาร์โค้ด Tag ล้อ : " + WOBarcode.Delivery_TAG + "\n");
                postData += string.Format("ลูกค้า : " + getBarcode.PRO_Cus + "\n");
                postData += string.Format("วันที/เวลาส่งของถึงลูกค้า : " + DateTime.Now + "\n");
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);

                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }
        private string UploadNotification(string WO)
        {

            string token = "C4EEhomujMG5xHobcPBJXS7Nfsb7gX7du38kCDGww3s";// Delivery PD
            var WOBarcode = DbFile.Delivery_PD_Process.Where(a => a.Delivery_WO.Equals(WO)).FirstOrDefault();
            var getBarcode = DbFile.WMS_PD_Product.Where(a => a.Barcode.Equals(WOBarcode.Delivery_TAG)).FirstOrDefault();

            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                var postData = string.Format("message={0}", "\n" + "*********แจ้งเตือนอัพโหลดรูปภาพสำเร็จ*********" + "\n");
                postData += string.Format("บาร์โค้ด WO : " + WOBarcode.Delivery_WO + "\n");
                postData += string.Format("บาร์โค้ด Tag ล้อ : " + WOBarcode.Delivery_TAG + "\n");
                postData += string.Format("ลูกค้า : " + getBarcode.PRO_Cus + "\n");
                postData += string.Format("เอกสารที่อัพโหลด : http://mmbkk.dyndns.org:8084/pd-delivery/Content/Doc/" + WOBarcode.Delivery_Doc + "\n");
                postData += string.Format("รูปภาพสถานที่อัพโหลด : http://mmbkk.dyndns.org:8084/pd-delivery/Content/Pic/" + WOBarcode.Delivery_Pic + "\n");
                postData += string.Format("วันที/เวลาอัพโหลดรูปภาพ : " + DateTime.Now + "\n");
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);

                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }

    }
}