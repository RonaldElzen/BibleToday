using System;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace BibleToday
{
    internal class Api
    {
        private string api = "http://api.preachingcentral.com/bible.php?passage=";
        private readonly string[] preferences;

        private readonly Sql sql = new Sql();

        /// <summary>
        ///     Constructor for Api object
        /// </summary>
        public Api()
        {
            preferences = sql.getPreferences();
        }

        /// <summary>
        ///     Method to get verses
        /// </summary>
        /// <param name="book"></param>
        /// <param name="chapter"></param>
        /// <param name="beginVerse"></param>
        /// <param name="endVerse"></param>
        /// <param name="usedTranslation"></param>
        /// <returns>String with text from verses</returns>
        public async Task<string> getVerses(string book, int chapter, int beginVerse, int endVerse,
            string usedTranslation)
        {
            api = "http://api.preachingcentral.com/bible.php?passage=";
            var xml = await new WebClient().DownloadStringTaskAsync(new Uri(api));
            //Get the xml:)
            //create new xml doc
            var doc = new XmlDocument();

            string verseText = "";
            string translation;

            //Checks to use the right translation
            if (usedTranslation == "")
            {
                translation = preferences[1];
            }
            else
            {
                translation = "&version=" + usedTranslation;
            }

            api += book + chapter + ":" + beginVerse + "-" + endVerse + translation;
            xml = await new WebClient().DownloadStringTaskAsync(new Uri(api));


            //Get the xml
            //create new xml doc
            doc = new XmlDocument();
            //load downloaded xml in doc
            doc.LoadXml(xml);

            //Get data from XML 
            var verseList = doc.GetElementsByTagName("item");
            for (var i = 0; i < verseList.Count; i++)
            {
                verseText += verseList[i].InnerText;
            }

            return verseText;
        }
    }
}