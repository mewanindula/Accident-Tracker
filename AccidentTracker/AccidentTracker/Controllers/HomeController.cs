using AccidentTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AccidentTracker.Controllers
{
    public class HomeController : Controller
    {
        SerialPort sps1 = new SerialPort();
        SerialPort sps2 = new SerialPort();

        AccidentTrackerEntities db = new AccidentTrackerEntities();

        private string uMobile;
        private string uId;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About Us";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Us";

            return View();
        }

        public ActionResult ShowDetails()
        {
             var det = db.Details.ToList();
             return View(det);
        }


        [Authorize]
        public ActionResult Show()
        {
            if(Session["UserID"] == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "User");
            }
            else
            {
                Response.ClearHeaders();
                Response.AddHeader("Cache-Control","no-cache, no-store, max-age=0, must-revalidate");
                Response.AddHeader("Pragma","no-cache");
            }


            var n = HttpContext.User.Identity.Name;

            string UID = Session["UserID"].ToString();

            AccidentTrackerEntities dc = new AccidentTrackerEntities();
            List<Detail> detailList = dc.Details.Where(a => a.UserID == UID).ToList();
            DataViewModel dataVM = new DataViewModel();

            List<DataViewModel> dataVMList = detailList.Select(x => new DataViewModel
            {
                UserID = x.UserID,
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                uMobile = x.User.uMobile,
                rMobile = x.User.rMobile,
                DateOfBirth = x.User.DateOfBirth,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Status = x.Status,
                Force = x.Force,
                Flame = x.Flame,
                Date = x.Date,
                Time = x.Time
            }).ToList();

            return View(dataVMList);
            

        }


        public  ActionResult SendSMS()
        {
            //var n = HttpContext.User.Identity.Name;

            uId = Session["UserId"].ToString();

            uId = uId.Replace("\r", string.Empty);

            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=""D:\Education\4th Year\Mini Project\Web Site\Project Web Site\2017.10.09\AccidentTracker\AccidentTracker\App_Data\AccidentTracker.mdf"";Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"))
                {
                    String rM = "SELECT * FROM Users WHERE UserID='" + uId + "'";
                    SqlCommand rMob = new SqlCommand(rM, con);
                    con.Open();
                    using (SqlDataReader oReader = rMob.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            uMobile = oReader["uMobile"].ToString();
                        }

                        con.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string tp;
            string msg;

            sps1.PortName = "COM11";
            sps2.PortName = "COM11";

            sps1.Open();

            msg = uMobile;

            tp = Char.ConvertFromUtf32(34) + uId + Char.ConvertFromUtf32(34);
            sps1.Write("AT+CMGF=1" + Char.ConvertFromUtf32(13));
            sps1.Write("AT+CMGS=" + tp + Char.ConvertFromUtf32(13));
            sps1.Write(msg + Char.ConvertFromUtf32(26) + Char.ConvertFromUtf32(13));

            sps1.Close();

            var t = Task.Run(async delegate
            {
                await Task.Delay(3000);
                return 42;
            });
            t.Wait();

            sps2.Open();

            msg = "0717292148";

            tp = Char.ConvertFromUtf32(34) + uId + Char.ConvertFromUtf32(34);
            sps2.Write("AT+CMGF=1" + Char.ConvertFromUtf32(13));
            sps2.Write("AT+CMGS=" + tp + Char.ConvertFromUtf32(13));
            sps2.Write(msg + Char.ConvertFromUtf32(26) + Char.ConvertFromUtf32(13));

            sps2.Close();

            return RedirectToAction("Show", "Home");
        }

    }
}