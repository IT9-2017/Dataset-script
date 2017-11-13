using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EpiCourious_userratings_script
{
    class Program
    {
        static void Main(string[] args)
        {

            loopUsers();
            Console.ReadLine();

        }
        

        public static string HtmlFetcher()
        {
            string htmlCode = "";

            using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
            {
                htmlCode = client.DownloadString(BuildURL());
            }

            return htmlCode;
        }

        public static string SearchString()
        {
            string htmlCode = HtmlFetcher();


            string s = "\"reviewerInfo\":\"SoftSculptor\",\"rating\":4,\"location\":\"Richmond, VA\",\"userId\":\"2bd764c9 - f720 - 4ae0 - a955 - c7b2793c377e\"";

            MatchCollection allMatchResults = null;
            Regex regexObj = new Regex(@"reviewerInfo.*?.recipeId");         
            MatchCollection matches = regexObj.Matches(htmlCode);
            foreach (Match match in matches)
            {
                Console.WriteLine(match);
            }

            /*
            Regex reg = new Regex("\\b" + Regex.Escape("reviewerInfo") + "\\b", RegexOptions.IgnoreCase);
            MatchCollection matches = reg.Matches(s);
            foreach (Match match in matches)
            {
                string sub = match.Value;
            }
            */


            #region Search UserName
            string searchUsername = getBetween(htmlCode, "\"reviewerInfo\":\"", "\"");
#endregion

            #region Search Rating
            string searchRating = getBetween(htmlCode, "\"rating\":", ",");
     
            string caseSwitch = searchRating;
            int ratingValue = 0;

            switch (caseSwitch)
            {
                case "0":
                    ratingValue = 0;
                    break;
                case "1":
                    ratingValue = 1;
                    break;
                case "2":
                    ratingValue = 2;
                    break;
                case "3":
                    ratingValue = 3;
                    break;
                case "4":
                    ratingValue = 4;
                    break;
                default:
                    break;
            }


            #endregion

            #region Search ID

            string searchUserID = getBetween(htmlCode, "\"userId\":\"", "\"");


            #endregion

            return searchUsername+ " " + searchRating + " " + searchUserID;
            
        }

        public static string BuildURL()
        {
            HtmlWeb client = new HtmlWeb();
            HtmlDocument doc = client.Load("https://www.epicurious.com/search/Spinach%20Noodle%20Casserole");
            HtmlNodeCollection Nodes = doc.DocumentNode.SelectNodes("//a[@href]");
            string recipeURL = "";
            string startURL = "https://www.epicurious.com";


            foreach (var link in Nodes)
            {
                if (link.Attributes["href"].Value.ToString().Length >= 2)
                {
                    recipeURL = (link.Attributes["href"].Value);
                    break;
                }
                
            }
            

            return startURL + recipeURL;
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        public static void loopUsers()
        {

            Regex review = new Regex("\\b" + Regex.Escape("reviewerInfo") + "\\b", RegexOptions.IgnoreCase); ;
            int count = review.Matches(HtmlFetcher()).Count;

      

            Console.WriteLine("here is count "+ count);
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(SearchString());
            }

            
        }
    }
}
