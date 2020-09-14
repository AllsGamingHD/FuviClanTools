using Newtonsoft.Json;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace FUVICLAN_TOOLS_
{
    class ListCreator
    {
        public static int avancement_tracks = 0, totaltracks = 0;
        public static int avancement_pages = 0, totalpages = 0;
        public static int current_track = 0, totaltrackinpages = 0;
        int count_totaltrackinlist = 0;

        public async Task<object> Create_List_NoDate(string url, string ListName, decimal nb_track)
        {
            avancement_pages = 0;
            avancement_tracks = 0;
            totalpages = 0;
            totaltrackinpages = 0;
            count_totaltrackinlist = 0;
            current_track = 0;
            totaltrackinpages = 0;
            totaltracks = (int)nb_track;
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = Properties.Settings.Default.headless,
                DefaultViewport = new ViewPortOptions { Height = 1080, Width = 1920 },
                ExecutablePath = Properties.Settings.Default.chromepath
            });

            var page = browser.PagesAsync().Result[0];

            var cookiesfile = File.ReadAllText("cookies.json");
            var cookiesdata = JsonConvert.DeserializeObject<List<CookieParam>>(cookiesfile);
            await page.SetCookieAsync(cookiesdata[0]);

            await page.GoToAsync(url);

            await page.WaitForSelectorAsync("#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > span", new WaitForSelectorOptions { Visible = true });
            var TrackList = await page.EvaluateFunctionAsync(
                  "()=>document.querySelector(\"#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > span\").textContent"
            );
            var NbTrackInList = Convert.ToInt32(TrackList);
            var NbPages = Math.Ceiling(nb_track / 20);
            totalpages = (int)NbPages;
            int nb_list = 0;
            var nom_playlist = ListName.Substring(0, ListName.IndexOf("(") - 1).Trim();
            for (int i = 1; i <= NbPages; i++)
            {
                //MessageBox.Show(NbTrackInList.ToString());
                avancement_pages = i;
                TrackList = await page.EvaluateFunctionAsync(
                    "()=>document.querySelector(\"#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > span\").textContent"
                );
                NbTrackInList = Convert.ToInt32(TrackList);
                if (i == 1 || (NbTrackInList == 40))
                {

                    //MessageBox.Show("here");
                    nb_list++;
                    ListName = nom_playlist + "_" + nb_list;


                    // Créer liste
                    await page.WaitForSelectorAsync("#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > img", new WaitForSelectorOptions { Visible = true });
                    await page.EvaluateFunctionAsync("()=>document.querySelector(\"#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > img\").click()");
                    await page.WaitForSelectorAsync("#btn_create_fuvilist", new WaitForSelectorOptions { Visible = true });
                    await page.EvaluateFunctionAsync("()=>document.querySelector(\"#btn_create_fuvilist\").click()");
                    await page.WaitForSelectorAsync("#fuvilist_title", new WaitForSelectorOptions { Visible = true });
                    await page.TypeAsync("#fuvilist_title", ListName);
                    await page.WaitForSelectorAsync("#form_add_fuvilist > div.modal-footer > button.btn.btn-primary", new WaitForSelectorOptions { Visible = true });
                    await page.EvaluateFunctionAsync("()=>document.querySelector(\"#form_add_fuvilist > div.modal-footer > button.btn.btn-primary\").click()");
                    await page.WaitForNavigationAsync(new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Networkidle0 } });
                    count_totaltrackinlist = 0;
                }

                if (i != 1)
                {
                    await page.GoToAsync(url + "/page/" + i);
                }

                // Ajouts des tracks
                await page.WaitForSelectorAsync("#fuvi_download_table > tbody", new WaitForSelectorOptions { Visible = true });
                var nb_track_inpage = await page.EvaluateFunctionAsync("()=>document.querySelector(\"#fuvi_download_table > tbody\").rows.length");
                current_track = 0;
                totaltrackinpages = Convert.ToInt32(nb_track_inpage);
                count_totaltrackinlist += totaltrackinpages;
                for (int t = 1; t <= Convert.ToInt32(nb_track_inpage); t++)
                {
                    current_track++;
                    avancement_tracks++;
                    await page.WaitForSelectorAsync("#fuvi_download_table > tbody > tr:nth-child(" + t + ") > td.action_col.align-middle.text-nowrap.dtr-control > div > a.text-muted.add_to_fuvilist > span", new WaitForSelectorOptions { Visible = true });
                    await page.EvaluateFunctionAsync("()=>document.querySelector(\"#fuvi_download_table > tbody > tr:nth-child(" + t + ") > td.action_col.align-middle.text-nowrap.dtr-control > div > a.text-muted.add_to_fuvilist > span\").click()");
                    await Task.Delay(20);
                }
                TrackList = await page.EvaluateFunctionAsync(
                  "()=>document.querySelector(\"#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > span\").textContent"
                );
                NbTrackInList = Convert.ToInt32(TrackList);

                while (NbTrackInList != count_totaltrackinlist)
                {
                    TrackList = await page.EvaluateFunctionAsync(
                      "()=>document.querySelector(\"#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > span\").textContent"
                    );
                    NbTrackInList = Convert.ToInt32(TrackList);
                }
            }
            await browser.CloseAsync();
            return null;
        }

        public async Task<object> Create_List_Date(string url, string ListName, decimal nb_track, DateTime date_depart, DateTime date_fin)
        {
            avancement_pages = 0;
            avancement_tracks = 0;
            totalpages = 0;
            totaltrackinpages = 0;
            count_totaltrackinlist = 0;
            current_track = 0;
            totaltrackinpages = 0;
            totaltracks = (int)nb_track;
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = Properties.Settings.Default.headless,
                DefaultViewport = new ViewPortOptions { Height = 1080, Width = 1920 },
                ExecutablePath = Properties.Settings.Default.chromepath
            });

            var page = browser.PagesAsync().Result[0];

            var cookiesfile = File.ReadAllText("cookies.json");
            var cookiesdata = JsonConvert.DeserializeObject<List<CookieParam>>(cookiesfile);
            await page.SetCookieAsync(cookiesdata[0]);

            await page.GoToAsync(url);

            await page.WaitForSelectorAsync("#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > span", new WaitForSelectorOptions { Visible = true });
            var TrackList = await page.EvaluateFunctionAsync(
                  "()=>document.querySelector(\"#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > span\").textContent"
            );
            var NbTrackInList = Convert.ToInt32(TrackList);
            var NbPages = Math.Ceiling(nb_track / 20);
            totalpages = (int)NbPages;
            int nb_list = 0;
            var nom_playlist = ListName.Substring(0, ListName.IndexOf("(") - 1).Trim();
            for (int i = 1; i <= NbPages; i++)
            {
                //MessageBox.Show(NbTrackInList.ToString());
                avancement_pages = i;
                TrackList = await page.EvaluateFunctionAsync(
                    "()=>document.querySelector(\"#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > span\").textContent"
                );
                NbTrackInList = Convert.ToInt32(TrackList);
                if (i == 1 || (NbTrackInList == 40))
                {

                    //MessageBox.Show("here");
                    nb_list++;
                    ListName = nom_playlist + "_" + nb_list;


                    // Créer liste
                    await page.WaitForSelectorAsync("#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > img", new WaitForSelectorOptions { Visible = true });
                    await page.EvaluateFunctionAsync("()=>document.querySelector(\"#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > img\").click()");
                    await page.WaitForSelectorAsync("#btn_create_fuvilist", new WaitForSelectorOptions { Visible = true });
                    await page.EvaluateFunctionAsync("()=>document.querySelector(\"#btn_create_fuvilist\").click()");
                    await page.WaitForSelectorAsync("#fuvilist_title", new WaitForSelectorOptions { Visible = true });
                    await page.TypeAsync("#fuvilist_title", ListName);
                    await page.WaitForSelectorAsync("#form_add_fuvilist > div.modal-footer > button.btn.btn-primary", new WaitForSelectorOptions { Visible = true });
                    await page.EvaluateFunctionAsync("()=>document.querySelector(\"#form_add_fuvilist > div.modal-footer > button.btn.btn-primary\").click()");
                    await page.WaitForNavigationAsync(new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Networkidle0 } });
                    count_totaltrackinlist = 0;
                }

                if (i != 1)
                {
                    await page.GoToAsync(url + "/page/" + i);
                }


                


                // Ajouts des tracks
                await page.WaitForSelectorAsync("#fuvi_download_table > tbody", new WaitForSelectorOptions { Visible = true });
                var nb_track_inpage = await page.EvaluateFunctionAsync("()=>document.querySelector(\"#fuvi_download_table > tbody\").rows.length");
                current_track = 0;
                totaltrackinpages = Convert.ToInt32(nb_track_inpage);
                count_totaltrackinlist += totaltrackinpages;
                for (int t = 1; t <= Convert.ToInt32(nb_track_inpage); t++)
                {
                    //Verification  dates
                    var raw_year = await page.EvaluateFunctionAsync("()=>document.querySelector(\"#fuvi_download_table > tbody > tr:nth-child(" + t + ") > td.date_col.align-middle.text-center.sorting_1 > time > em\").textContent");

                    var raw_month = await page.EvaluateFunctionAsync("()=>document.querySelector(\"#fuvi_download_table > tbody > tr:nth-child(" + t + ") > td.date_col.align-middle.text-center.sorting_1 > time > strong\").textContent");

                    var raw_day = await page.EvaluateFunctionAsync("()=>document.querySelector(\"#fuvi_download_table > tbody > tr:nth-child(" + t + ") > td.date_col.align-middle.text-center.sorting_1 > time > span\").textContent");
                    string s_month = raw_month.ToString();

                    switch (s_month)
                    {
                        case "Sep":
                            s_month = "09";

                            break;
                        case "Oct":
                            s_month = "10";
                            break;
                        case "Nov":
                            s_month = "11";
                            break;
                        case "Déc":
                            s_month = "12";
                            break;
                        case "Jan":
                            s_month = "01";
                            break;
                        case "Fév":
                            s_month = "02";
                            break;
                        case "Mar":
                            s_month = "03";
                            break;
                        case "Avr":
                            s_month = "04";
                            break;
                        case "Mai":
                            s_month = "05";
                            break;
                        case "Juin":
                            s_month = "06";
                            break;
                        case "Juil":
                            s_month = "07";
                            break;
                        case "Août":
                            s_month = "08";
                            break;
                        default:
                            s_month = "N/A";
                            break;
                    }
                    int year = (int)raw_year;
                    int month = Convert.ToInt32(s_month);
                    int day = (int)raw_day;
                    DateTime FuviDate = new DateTime(year, month, day);
                    if (FuviDate <= date_depart)
                    {
                        if (FuviDate >= date_fin)
                        {
                            current_track++;
                            avancement_tracks++;
                            await page.WaitForSelectorAsync("#fuvi_download_table > tbody > tr:nth-child(" + t + ") > td.action_col.align-middle.text-nowrap.dtr-control > div > a.text-muted.add_to_fuvilist > span", new WaitForSelectorOptions { Visible = true });
                            await page.EvaluateFunctionAsync("()=>document.querySelector(\"#fuvi_download_table > tbody > tr:nth-child(" + t + ") > td.action_col.align-middle.text-nowrap.dtr-control > div > a.text-muted.add_to_fuvilist > span\").click()");
                            await Task.Delay(20);
                        }
                        else
                        {
                            totaltrackinpages--;
                            MessageBox.Show("LEAVE");
                            return null;
                        }
                    }
                    else
                    {
                        totaltrackinpages--;
                    }
                }
                TrackList = await page.EvaluateFunctionAsync(
                  "()=>document.querySelector(\"#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > span\").textContent"
                );
                NbTrackInList = Convert.ToInt32(TrackList);

                while (NbTrackInList != count_totaltrackinlist)
                {
                    TrackList = await page.EvaluateFunctionAsync(
                      "()=>document.querySelector(\"#menu-menu-principal > li.menu-item.menu-item-type-fuvilist.menu-item-type-fuvilist-cart.nav-item.dropdown > a > span\").textContent"
                    );
                    NbTrackInList = Convert.ToInt32(TrackList);
                }
            }
            await browser.CloseAsync();
            return null;
        }
    }
}