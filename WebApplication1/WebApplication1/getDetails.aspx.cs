using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Models;
using System.Data.SqlClient;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        SerialPort sps = new SerialPort();

        private SerialPort myport;

        private string in_data;
        private string uid;
        private string st;
        private string lat;
        private string lon;
        private string fo;
        private string fl;
        private string date;
        private string time;
        private string tp;
        private string msg;
        private string rMobile;
        private string fName;


        protected void Page_Load(object sender, EventArgs e)
        {
            myport = new SerialPort();
            myport.BaudRate = 9600;
            myport.PortName = "COM4";
            myport.Parity = Parity.None;
            myport.DataBits = 8;
            myport.StopBits = StopBits.One;
            myport.DataReceived += myport_DataReceived;

            try
            {
                myport.Open();
                TextBox1.Text = "";


            }
            catch (Exception ex)
            {
                TextBox1.Text = ex.Message + "error";
            }


        }


        void myport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            in_data = myport.ReadLine();

            string v = in_data.ToString();

            if (v[0] == 'A' && v[1] == 'T' && v[2] == 'D' && v[3] == '9')
            {
                string[] data = v.Split(',');

                st = data[1];
                lat = data[2];
                lon = data[3];
                fo = data[4];
                fl = data[5];
                date = data[6];
                time = data[7];
                uid = data[8];
                uid = uid.Replace("\r", string.Empty);


                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=""D:\Education\4th Year\Mini Project\Web Site\Project Web Site\2017.10.09\AccidentTracker\AccidentTracker\App_Data\AccidentTracker.mdf"";Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");

                con.Open();

                String str = "INSERT INTO Details(UserID,Status,Latitude,Longitude,Force,Flame,Date,Time) VALUES('" + uid + "','" + st + "','" + lat + "','" + lon + "','" + fo + "','" + fl + "','" + date + "','" + time + "') ";

                SqlCommand cmd = new SqlCommand(str, con);

                int OBJ = Convert.ToInt32(cmd.ExecuteNonQuery());

                if (OBJ > 0)
                {
                    if (st == "Accident")
                    {
                        getDetails();

                        sendSMS();
                    }

                    Label1.Text = "Data is Successfully inserted in database";
                }
                else
                {
                    Label1.Text = "Data is not Successfully inserted in database";
                }

                con.Close();
            }

        }


        public void  getDetails()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=""D:\Education\4th Year\Mini Project\Web Site\Project Web Site\2017.10.09\AccidentTracker\AccidentTracker\App_Data\AccidentTracker.mdf"";Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"))
                {
                    String rM = "SELECT * FROM Users WHERE UserID='" + uid + "'";
                    SqlCommand rMob = new SqlCommand(rM, con);
                    con.Open();
                    using (SqlDataReader oReader = rMob.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            rMobile = oReader["rMobile"].ToString();
                            fName = oReader["FirstName"].ToString();
                        }

                        con.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }


        public void sendSMS()
        {
            sps.PortName = "COM11";

            sps.Open();

            msg = fName + "'s Vehicle Was Accident. Place - http://maps.google.com/maps?q=" + lat + "," + lon + "\nForce :- " + fo + "\nFlame :- " + fl + "\nDate :- " + date + "\nTime - " + time;

            tp = Char.ConvertFromUtf32(34) + rMobile + Char.ConvertFromUtf32(34);
            sps.Write("AT+CMGF=1" + Char.ConvertFromUtf32(13));
            sps.Write("AT+CMGS=" + tp + Char.ConvertFromUtf32(13));
            sps.Write(msg + Char.ConvertFromUtf32(26) + Char.ConvertFromUtf32(13));

            sps.Close();
        }

    }
}