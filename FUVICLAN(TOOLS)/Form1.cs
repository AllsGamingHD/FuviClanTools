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
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;
using static FUVICLAN_TOOLS_.FUVICOOKIE;

namespace FUVICLAN_TOOLS_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void seConnecterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            USERLOG UL = new USERLOG();
            UL.ShowDialog();
        }
        Browser browser;
        private async void Form1_Load(object sender, EventArgs e)
        {
            radProgressBar1.Visible = true;
            if (Directory.Exists(@"C:\Program Files\Google\Chrome\Application"))
            {
                Properties.Settings.Default.chromepath = @"C:\Program Files\Google\Chrome\Application\" + "chrome.exe";
            }
            if (Directory.Exists(@"C:\Program Files (x86)\Google\Chrome\Application"))
            {
                Properties.Settings.Default.chromepath = @"C:\Program Files (x86)\Google\Chrome\Application\" + "chrome.exe";
            }
            Properties.Settings.Default.Save();
            radProgressBar1.Value1 = 5;
            label4.Text = "Les musiques publié sur FuviClan du : \n" + radDateTimePicker1.Text + " au " + radDateTimePicker2.Text + " \nseront ajouter a vos listes de lecture.";
            string[] arg = { "--window-size=1000,1400" };
            if (File.Exists("cookies.json"))
            {
                DateTime modification = File.GetLastWriteTime(@"cookies.json");
                var Compare = DateTime.Now.Subtract(modification);
                if (Compare.TotalDays > 7)
                {
                    File.Delete("cookies.json");
                    USERLOG UL = new USERLOG();
                    UL.ShowDialog();
                }
            }
            else
            {
                USERLOG UL = new USERLOG();
                UL.ShowDialog();
            }
            radProgressBar1.Value1 = 10;
            while (!File.Exists("cookies.json"))
            {
                await Task.Delay(100);
            }
            radProgressBar1.Value1 = 15;
            browser = await Puppeteer.LaunchAsync(new LaunchOptions
           {
               Headless = Properties.Settings.Default.headless,
               DefaultViewport = new ViewPortOptions { Height = 1080, Width = 1400 },
               Args = arg,
                ExecutablePath = Properties.Settings.Default.chromepath
            });
            radProgressBar1.Value1 = 30;
            var page = browser.PagesAsync().Result[0];

            var cookiesfile = File.ReadAllText("cookies.json");
            var cookiesdata = JsonConvert.DeserializeObject<List<CookieParam>>(cookiesfile);
            await page.SetCookieAsync(cookiesdata[0]);
            radProgressBar1.Value1 = 35;
            await page.GoToAsync("https://www.fuvi-clan.com/");
            radProgressBar1.Value1 = 45;
            var NbCat = await page.EvaluateFunctionAsync(
              "()=>document.querySelector(\"#custom_cat\").length"
            );
            for (int i = 2; i <= Convert.ToInt32(NbCat); i++)
            {
                if (i%2 == 0)
                {
                    radProgressBar1.Value1++;
                }
                var CatName = await page.EvaluateFunctionAsync(
                  "()=>document.querySelector(\"#custom_cat > option:nth-child(" + i + ")\").textContent"
                );
                var CatUrl = await page.EvaluateFunctionAsync(
                  "()=>document.querySelector(\"#custom_cat > option:nth-child(" + i + ")\").value"
                );
                if (CatName.ToString().Contains("("))
                {
                    radCheckedListBox1.Items.Add(new ListViewDataItem { Enabled = true, Text = CatName.ToString(), Tag = CatUrl.ToString() });
                }
                else
                {
                    radCheckedListBox1.Items.Add(new ListViewDataItem { Enabled = false, Text = CatName.ToString() });
                }
            }

            // Récupération listes de lectures
            StringBuilder sb = new StringBuilder();
            await page.WaitForSelectorAsync("#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > img", new WaitForSelectorOptions { Visible = true });
            await page.EvaluateFunctionAsync("()=>document.querySelector(\"#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > img\").click()");
            await page.WaitForSelectorAsync("#btn_create_fuvilist", new WaitForSelectorOptions { Visible = true });
            radProgressBar1.Value1 = 75; 
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
            radProgressBar1.Value1 = 90;
            Properties.Settings.Default.ListesLecture = sb.ToString();
            Properties.Settings.Default.Save();
            radProgressBar1.Value1 = 100;
            pictureBox1.Visible = false;
            await browser.CloseAsync();
            radCheckedListBox1.SelectedIndex = 0;
            mesListesDeLecturesToolStripMenuItem.Enabled = true;
            radProgressBar1.Visible = false;
        }

        private async void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            await browser.CloseAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool exists = false;
            radCheckedListBox1.Items.ToList().ForEach(x => {
                if (x.CheckState == ToggleState.On)
                {
                    exists = false;
                    radListControl1.Items.ToList().ForEach(a =>
                    {
                        if (a.Text == x.Text)
                            exists = true;
                    });
                    if (!exists)
                    {
                        radListControl1.Items.Add(new RadListDataItem { Text = x.Text, Tag = x.Tag });
                    }
                }
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            radListControl1.Items.Remove(radListControl1.SelectedItem);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (radListControl1.Items.Count > 0)
            {
                //MessageBox.Show("Telechargement de : " + radListControl1.Items[0].Text.Substring(0, radListControl1.Items[0].Text.LastIndexOf("(")).Trim() + Environment.NewLine + "URL = " + radListControl1.Items[0].Tag); ;
                for (int i = 0; i < radListControl1.Items.Count(); i++)
                {
                    panel1.Visible = true;
                    timer1.Start();
                    var url = radListControl1.Items[i].Tag.ToString();
                    var pack_name = radListControl1.Items[i].Text;
                    var nbtrack = Convert.ToDecimal(pack_name.Substring(pack_name.LastIndexOf("(") + 1, pack_name.Length - pack_name.LastIndexOf("(") - 2));
                    lbl_PackName.Text = "Pack en cours d'ajouts : " + pack_name;
                    if (checkBox1.Checked == true)
                    {
                        await new ListCreator().Create_List_Date(url, pack_name, nbtrack, radDateTimePicker1.Value, radDateTimePicker2.Value);
                    }
                    else
                    {
                        await new ListCreator().Create_List_NoDate(url, pack_name, nbtrack);
                    }
                }
                button4.Visible = true;
                timer1.Stop();
            }
            else
            {
                MessageBox.Show("Veuillez ajoutez des catégorie à ajouter a vos listes de lecture avant de commencer.");
            }
        }

        private void radDateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            label4.Text = "Les musiques publié sur FuviClan du : \n" + radDateTimePicker1.Text + " au " + radDateTimePicker2.Text + " \nseront ajouter a vos listes de lecture.";
        }

        private void radDateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            label4.Text = "Les musiques publié sur FuviClan du : \n" + radDateTimePicker1.Text + " au " + radDateTimePicker2.Text + " \nseront ajouter a vos listes de lecture.";
        }

        private void mesListesDeLecturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListesLectures LL = new ListesLectures();
            LL.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            prog_Pages.Maximum = ListCreator.totalpages;
            prog_Pages.Text = "Progression des pages" + Environment.NewLine + ListCreator.avancement_pages + "/" + ListCreator.totalpages + " -> (" + (((double)ListCreator.avancement_pages / (double)ListCreator.totalpages) * 100).ToString("0.00") + " %)";
            prog_Pages.Value1 = ListCreator.avancement_pages;


            prog_currpages.Maximum = ListCreator.totaltrackinpages;
            prog_currpages.Text = "Progression de l'ajouts de tracks par page" + Environment.NewLine + ListCreator.current_track + " / " + ListCreator.totaltrackinpages + "-> (" + (((double)ListCreator.current_track / (double)ListCreator.totaltrackinpages) * 100).ToString("0.00") + " %)";
            prog_currpages.Value1 = ListCreator.current_track;

            prog_Tracks.Maximum = ListCreator.totaltracks;
            prog_Tracks.Text = "Progression de l'ajouts de tracks par page" + Environment.NewLine + ListCreator.avancement_tracks + " / " + ListCreator.totaltracks + "-> (" + (((double)ListCreator.avancement_tracks / (double)ListCreator.totaltracks) * 100).ToString("0.00") + " %)";
            prog_Tracks.Value1 = ListCreator.avancement_tracks;


            double pourcentage_moy = (((double)ListCreator.avancement_tracks / (double)ListCreator.totaltracks) * 100) + (((double)ListCreator.avancement_pages / (double)ListCreator.totalpages) * 100);
            pourcentage_moy = Math.Round((pourcentage_moy / 2),  2);
            try
            {
                prog_general.Maximum = 100;
                prog_general.Text = "Avancement général" + Environment.NewLine + pourcentage_moy + " %";
                prog_general.Value1 = Convert.ToInt32(pourcentage_moy);
            }
            catch
            {

            }
        }

        private void lbl_PackName_TextChanged(object sender, EventArgs e)
        {
            lbl_PackName.Left = ((this.panel1.Width / 2) - (lbl_PackName.Size.Width / 2)) / 2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            timer1.Stop();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                radDateTimePicker1.Enabled = true;
                radDateTimePicker2.Enabled = true;
                button3.Text = "Valider (Avec Filtre Date)";
            }
            else
            {
                radDateTimePicker1.Enabled = false;
                radDateTimePicker2.Enabled = false;
                button3.Text = "Valider (Sans Filtre Date)";
            }
        }

        private void propriétéToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Propriete P = new Propriete();
            P.ShowDialog();
        }
    }
}