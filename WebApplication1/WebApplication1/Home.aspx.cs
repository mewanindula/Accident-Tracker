using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Home : System.Web.UI.Page
    {
        string lat, lon;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            StringBuilder table = new StringBuilder();

            SqlConnection con1 = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\Indula\Documents\Accident_Tracker.mdf;Integrated Security=True;Connect Timeout=30");

            con1.Open();

            String str1 = "SELECT * FROM Details";

            SqlCommand cmd = new SqlCommand(str1, con1);

            SqlDataReader rd = cmd.ExecuteReader();

            table.Append("<table border='1'>");
            table.Append("<tr><th>Telephone</th><th>Status</th><th>Latitude</th><th>Longitude</th><th>Force</th><th>Flame</th></tr>");

            if(rd.HasRows)
            {
                while(rd.Read())
                {
                    table.Append("<tr>");
                    table.Append("<td>" + rd[1] + "</td>");
                    table.Append("<td>" + rd[2] + "</td>");
                    table.Append("<td>" + rd[3] + "</td>");
                    table.Append("<td>" + rd[4] + "</td>");
                    table.Append("<td>" + rd[5] + "</td>");
                    table.Append("<td>" + rd[6] + "</td>");
                    table.Append("</tr>");
                }
            }
            table.Append("</table>");

            PlaceHolder1.Controls.Add(new Literal { Text = table.ToString()});

            rd.Close();
            rd.Dispose();

            con1.Close();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            SqlConnection con1 = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\Indula\Documents\Accident_Tracker.mdf;Integrated Security=True;Connect Timeout=30");

            con1.Open();

            String str1 = "SELECT top 1 * FROM Details WHERE Status = 'Place' ORDER BY ID DESC";

            SqlCommand cmd = new SqlCommand(str1, con1);

            SqlDataReader rd = cmd.ExecuteReader();

            if (rd.HasRows)
            {
                if(rd.Read())
                {
                    lat = rd[3].ToString();
                    lon = rd[4].ToString();
                }
            }

            Response.Redirect("http://maps.google.com/maps?q=" + lat + "," + lon);
        }

           
    }
}