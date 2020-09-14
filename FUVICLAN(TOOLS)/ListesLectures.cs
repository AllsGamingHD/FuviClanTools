using Newtonsoft.Json;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FUVICLAN_TOOLS_
{
    public partial class ListesLectures : Form
    {
        public ListesLectures()
        {
            InitializeComponent();
        }
        Browser browser;
        private void ListesLectures_Load(object sender, EventArgs e)
        {
            var Listes = Properties.Settings.Default.ListesLecture;
            richTextBox1.Text = Listes;
            richTextBox1.Lines.ToList().ForEach(a =>
            {
                if (a != string.Empty)
                    radListView1.Items.Add(a);
            });
            radListView1.SelectedIndex = 0;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            radListView1.Enabled = false;
            panel1.Visible = true;
            radProgressBar1.ShowProgressIndicators = true;
            radProgressBar1.Value1 = 0;
            string[] arg = { "--window-size=1000,1400" };
            browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = Properties.Settings.Default.headless,
                DefaultViewport = new ViewPortOptions { Height = 1080, Width = 1400 },
                Args = arg,
                ExecutablePath = Properties.Settings.Default.chromepath
            });
            radProgressBar1.Value1 = 5;
            var page = browser.PagesAsync().Result[0];

            var cookiesfile = File.ReadAllText("cookies.json");
            var cookiesdata = JsonConvert.DeserializeObject<List<CookieParam>>(cookiesfile);
            await page.SetCookieAsync(cookiesdata[0]);
            radProgressBar1.Value1 = 10;
            await page.GoToAsync("https://www.fuvi-clan.com/");
            radProgressBar1.Value1 = 25;
            // Récupération listes de lectures
            StringBuilder sb = new StringBuilder();
            await page.WaitForSelectorAsync("#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > img", new WaitForSelectorOptions { Visible = true });
            await page.EvaluateFunctionAsync("()=>document.querySelector(\"#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > img\").click()");
            await page.WaitForSelectorAsync("#btn_create_fuvilist", new WaitForSelectorOptions { Visible = true });
            radProgressBar1.Value1 = 50; 
            try
            {
                var nblist = await page.EvaluateFunctionAsync("()=>document.querySelector(\"#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown.show > ul > li > div > div.col-12.col-xl-4 > div > select\").length");
                for (int l = 2; l <= Convert.ToInt32(nblist); l++)
                {
                    var select_name = await page.EvaluateFunctionAsync("()=>document.querySelector(\"#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown.show > ul > li > div > div.col-12.col-xl-4 > div > select > option:nth-child(" + l + ")\").textContent");
                    sb.AppendLine(select_name.ToString().Trim());
                }
            }
            catch { }
            radProgressBar1.Value1 = 65; 
            Properties.Settings.Default.ListesLecture = sb.ToString();
            Properties.Settings.Default.Save();
            radProgressBar1.Value1 = 75;
            await browser.CloseAsync();
            radListView1.Items.Clear();
            richTextBox1.Text = "";
            radProgressBar1.Value1 = 90;
            var Listes = Properties.Settings.Default.ListesLecture;
            richTextBox1.Text = Listes;
            radProgressBar1.Value1 = 95;
            richTextBox1.Lines.ToList().ForEach(a =>
            {
                if (a != string.Empty)
                    radListView1.Items.Add(a);
            });
            radListView1.SelectedIndex = 0;
            radListView1.Enabled = true;
            radProgressBar1.Value1 = 100;
            panel1.Visible = false;
            radProgressBar1.ShowProgressIndicators = false;
            radProgressBar1.Value1 = 0;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            radProgressBar1.Maximum = radListView1.Items.Count;
            radProgressBar1.Text = "0/" + radProgressBar1.Maximum + "  |  (0.00 %)";
            panel1.Visible = true;
            button1.Enabled = false;
            button2.Enabled = false;
            string[] arg = { "--window-size=1000,1400" };
            browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = Properties.Settings.Default.headless,
                DefaultViewport = new ViewPortOptions { Height = 1080, Width = 1400 },
                Args = arg,
                ExecutablePath = Properties.Settings.Default.chromepath
            });

            var page = browser.PagesAsync().Result[0];

            var cookiesfile = File.ReadAllText("cookies.json");
            var cookiesdata = JsonConvert.DeserializeObject<List<CookieParam>>(cookiesfile);
            await page.SetCookieAsync(cookiesdata[0]);

            await page.GoToAsync("https://www.fuvi-clan.com/myaccount/download-lists/");


            await page.WaitForSelectorAsync("#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > img", new WaitForSelectorOptions { Visible = true });
            await page.EvaluateFunctionAsync("()=>document.querySelector(\"#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > img\").click()");
            await page.WaitForSelectorAsync("#btn_create_fuvilist", new WaitForSelectorOptions { Visible = true });
            var nblist = await page.EvaluateFunctionAsync("()=>document.querySelector(\"#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown.show > ul > li > div > div.col-12.col-xl-4 > div > select\").length");

            for (int i = 2; i <= Convert.ToInt32(nblist); i++)
            {
                radListView1.Items.RemoveAt(radListView1.Items.Count - 1);
                avancement_suppression++;
                radProgressBar1.Value1++;
                radProgressBar1.Text = avancement_suppression + "/" + radProgressBar1.Maximum + "  |  (" + (((double)avancement_suppression / (double)radProgressBar1.Maximum) * 100).ToString("0.00") + " %)"; await page.WaitForSelectorAsync("body > div.wrap.container > div > main > div > div.col-lg-8.col-xl-9.mt-lg-5.mb-5 > div.fuvilist_container > div:nth-child(1) > div.col-12.col-lg-5.col-xl-4 > div > div > button.btn.btn-outline-danger.delete_fuvi_list", new WaitForSelectorOptions { Visible = true });
                await page.EvaluateFunctionAsync("()=>window.confirm = () => true");
                await page.EvaluateFunctionAsync("()=>document.querySelector(\"body > div.wrap.container > div > main > div > div.col-lg-8.col-xl-9.mt-lg-5.mb-5 > div.fuvilist_container > div:nth-child(1) > div.col-12.col-lg-5.col-xl-4 > div > div > button.btn.btn-outline-danger.delete_fuvi_list\").click()");
                await page.WaitForNavigationAsync(new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Networkidle0 } });
            }
            button1.Enabled = true;
            button2.Enabled = true;
            await browser.CloseAsync();
        }
        int avancement_suppression = 0;
        private void radListView1_ItemRemoving(object sender, Telerik.WinControls.UI.ListViewItemCancelEventArgs e)
        {
        }

        private void radListView1_ItemRemoved(object sender, Telerik.WinControls.UI.ListViewItemEventArgs e)
        {

        }
    }
}
