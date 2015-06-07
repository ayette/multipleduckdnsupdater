using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultipleDuckDNSUpdater
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }




        private void btnSave_Click(object sender, EventArgs e)
        {
            base.OnDeactivate(e);
            this.Hide();            
            timer.Enabled = true;
            timer_Tick(this, e);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            var subdomain = new List<string>(ConfigurationManager.AppSettings["subdomain"].Split(new char[] { ';' }));

            foreach (string s in subdomain)
            {
                UpdateIP(s);
            }
        }

        public async Task UpdateIP(string subdomain)
        {
            try
            {

                HttpClient client = new HttpClient();


                Task<string> getStringTask = client.GetStringAsync(string.Format("https://duckdns.org/update/{0}/{1}", subdomain, ConfigurationManager.AppSettings["token"]));

                string urlContents = await getStringTask;
                Console.WriteLine(string.Format("Subdomain: {0}, Status: {1}",subdomain, urlContents));                
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            txtToken.Text = ConfigurationManager.AppSettings["token"].ToString();

            var subdomain = new List<string>(ConfigurationManager.AppSettings["subdomain"].Split(new char[] { ';' }));

            lstSubDomain.Items.Clear();
            foreach (string s in subdomain)
            {
                lstSubDomain.Items.Add(s);
            }
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            this.Show();
            this.Activate();
        }


    }
}
