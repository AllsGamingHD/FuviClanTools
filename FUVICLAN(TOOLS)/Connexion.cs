using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace FUVICLAN_TOOLS_
{
    class Connexion
    {
        public async void Connect(string mail, string pass)
        {
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false,
                DefaultViewport = null,
                ExecutablePath = Properties.Settings.Default.chromepath
            });

            var page = browser.PagesAsync().Result[0];

            await page.GoToAsync("https://www.fuvi-clan.com/myaccount/");

            await page.WaitForSelectorAsync("#username");
            await page.TypeAsync("#username", mail);

            await page.WaitForSelectorAsync("#password");
            await page.TypeAsync("#password", pass);

            await page.ClickAsync("#rememberme");

            await page.ClickAsync("#customer_login > div:nth-child(1) > form > div.d-flex.justify-content-md-between.align-items-center.mt-3 > p.form-group.form-row > button");
            
            await Task.Delay(2000);


            await page.WaitForSelectorAsync("body > div.wrap.container > div > main > div > div.col-lg-4.col-xl-3.my-5 > div > div", new WaitForSelectorOptions { Visible = true });
            await page.WaitForSelectorAsync("body > div.wrap.container > div > main > div > div.col-lg-4.col-xl-3.my-5 > div > div > h6", new WaitForSelectorOptions { Visible = true });

            var FuviUserName = await page.EvaluateFunctionAsync(
              "()=>document.querySelector(\"body > div.wrap.container > div > main > div > div.col-lg-4.col-xl-3.my-5 > div > div > h4\").textContent"
            );
            var FuviInscription = await page.EvaluateFunctionAsync(
              "()=>document.querySelector(\"body > div.wrap.container > div > main > div > div.col-lg-4.col-xl-3.my-5 > div > div > h6\").textContent"
            );
            Properties.Settings.Default.FuviUserName = FuviUserName.ToString();
            Properties.Settings.Default.FuviInscriDate = FuviInscription.ToString();
            MessageBox.Show("Connexion reussi !" + Environment.NewLine + Environment.NewLine + "Salut, " + Properties.Settings.Default.FuviUserName + Environment.NewLine + Properties.Settings.Default.FuviInscriDate, "Connexion reussi !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MessageBox.Show("Le logiciel va enregistré les cookies de votre session pour vous evité de vous reconnecter souvent, aucune données n'est sauvegardé sur serveur." + Environment.NewLine + "Elle est sauvegardé à l'emplacement racine de l'application.", "Connexion reussi !", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var Cookies = await page.GetCookiesAsync();
            File.WriteAllText("cookies.json", JsonConvert.SerializeObject(Cookies));

            await browser.CloseAsync();
        }
    }
}
