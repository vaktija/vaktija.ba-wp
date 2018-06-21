using System;
using System.Collections.Generic;
using Vaktija.ba.Views;
using Windows.Data.Xml.Dom;

namespace Vaktija.ba.Helpers
{
    class LiveTile
    {
        public static void Update()
        {
            Day juce = Year.year.months[DateTime.Now.AddDays(-1).Month - 1].days[DateTime.Now.AddDays(-1).Day - 1];
            Day danas = Year.year.months[DateTime.Now.Month - 1].days[DateTime.Now.Day - 1];
            Day sutra = Year.year.months[DateTime.Now.AddDays(1).Month - 1].days[DateTime.Now.AddDays(1).Day - 1];

            string tempStr = "";
            foreach (var it in danas.vakti)
            {
                if (it.time.DayOfWeek == DayOfWeek.Friday && it.rbr == 2) it.name = "Podne (Džuma)".ToLower(); //Ako je petak i vakat podna, postavit dzumu
                tempStr += "<text id=\"" + (it.rbr + 2).ToString() + "\">" + it.time.ToString("HH:mm") + " " + it.name.ToLower() + "</text>\n";
            }

            #region Lock screen details
            string row1 = "";
            string row2 = "";
            string row3 = "";
            row1 = danas.vakti[0].time.ToString("HH:mm") + " zora       " + " izl sunca " + danas.vakti[1].time.ToString("HH:mm");
            row2 = danas.vakti[2].time.ToString("HH:mm") + " podne   " + "    ikindija " + danas.vakti[3].time.ToString("HH:mm");
            row3 = danas.vakti[4].time.ToString("HH:mm") + " akšam   " + "        jacija " + danas.vakti[5].time.ToString("HH:mm") + "\n" + Memory.location.ime.ToLower();
            #endregion

            var nextPrayer = Get.Next_Prayer_Time();

            string xml = "<tile>\n";
            xml += "<visual version=\"2\">\n";
            xml += "  <binding template=\"TileWide310x150Text02\" fallback=\"TileWideText02\" hint-lockDetailedStatus1=\"" + row1.ToLower() + "\" hint-lockDetailedStatus2=\"" + row2.ToLower() + "\" hint-lockDetailedStatus3=\"" + row3.ToLower() + "\">\n";
            xml += "  <image id=\"1\" src=\"Assets/Wide310x150Logo.png\" alt=\"alt text\"/>";
            xml += tempStr;
            xml += "  <text id=\"1\">" + Memory.location.ime.ToLower() + "</text>\n";
            xml += "  </binding>\n";
            xml += "  <binding template=\"TileSquare150x150Text01\" fallback=\"TileSquareText01\">\n";
            xml += "  <text id=\"1\">" + nextPrayer.time.ToString("HH:mm") + "</text>";
            xml += "  <text id=\"2\">" + nextPrayer.name.ToLower() + "</text>";
            xml += "  </binding>\n";
            xml += "</visual>\n";
            xml += "</tile>";
            xml = xml.Replace("izlazak sunca", "izl sunca");

            Windows.Data.Xml.Dom.XmlDocument txml = new Windows.Data.Xml.Dom.XmlDocument();
            txml.LoadXml(xml);
            Windows.UI.Notifications.TileNotification tNotification = new Windows.UI.Notifications.TileNotification(txml);
            Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Update(tNotification);

            RegisterLiveTiles();
        }
        public static void RegisterLiveTiles()
        {
            UnregisterAllScheduledLiveTiles();

            Day juce = Year.year.months[DateTime.Now.AddDays(-1).Month - 1].days[DateTime.Now.AddDays(-1).Day - 1];
            Day danas = Year.year.months[DateTime.Now.Month - 1].days[DateTime.Now.Day - 1];
            Day sutra = Year.year.months[DateTime.Now.AddDays(1).Month - 1].days[DateTime.Now.AddDays(1).Day - 1];

            List<Day> jds = new List<Day> { juce, danas, sutra };

            string tempStr = "";
            foreach (var it in danas.vakti)
            {
                if (it.time.DayOfWeek == DayOfWeek.Friday && it.rbr == 2) it.name = "Podne (Džuma)".ToLower(); //Ako je petak i vakat podna, postavit dzumu
                tempStr += "<text id=\"" + (it.rbr + 2).ToString() + "\">" + it.time.ToString("HH:mm") + " " + it.name.ToLower() + "</text>\n";
            }

            #region Lock screen details
            string row1 = "";
            string row2 = "";
            string row3 = "";
            row1 = danas.vakti[0].time.ToString("HH:mm") + " zora       " + " izl sunca " + danas.vakti[1].time.ToString("HH:mm");
            row2 = danas.vakti[2].time.ToString("HH:mm") + " podne   " + "    ikindija " + danas.vakti[3].time.ToString("HH:mm");
            row3 = danas.vakti[4].time.ToString("HH:mm") + " akšam   " + "        jacija " + danas.vakti[5].time.ToString("HH:mm") + "\n" + Memory.location.ime.ToLower();
            #endregion

            var nextPrayer = Get.Next_Prayer_Time();
            System.Diagnostics.Debug.WriteLine("Next prayer: " + nextPrayer.name);

            string xml = "<tile>\n";
            xml += "<visual version=\"2\">\n";
            xml += "  <binding template=\"TileWide310x150Text02\" fallback=\"TileWideText02\" hint-lockDetailedStatus1=\"" + row1.ToLower() + "\" hint-lockDetailedStatus2=\"" + row2.ToLower() + "\" hint-lockDetailedStatus3=\"" + row3.ToLower() + "\">\n";
            xml += "  <image id=\"1\" src=\"Assets/Wide310x150Logo.png\" alt=\"alt text\"/>";
            xml += tempStr;
            xml += "  <text id=\"1\">" + Memory.location.ime.ToLower() + "</text>\n";
            xml += "  </binding>\n";
            xml += "  <binding template=\"TileSquare150x150Text01\" fallback=\"TileSquareText01\">\n";
            xml += "  <text id=\"1\">[text1]</text>";
            xml += "  <text id=\"2\">[text2]</text>";
            xml += "  </binding>\n";
            xml += "</visual>\n";
            xml += "</tile>";
            xml = xml.Replace("izlazak sunca", "izl sunca");

            Vakat trenV = Get.Current_Prayer_Time();
            var currentPrayer = Get.Current_Prayer_Time();

            int j = 0;

            bool el = false;

            foreach (var day in jds)
            {
                foreach (var it in day.vakti)
                {
                    if (el)
                    {
                        j++;
                        if (j > 6)
                        {
                            var notifier = Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication();
                            var scheduled = notifier.GetScheduledTileNotifications();
                            for (int i = 0; i < scheduled.Count; i++)
                            {
                                System.Diagnostics.Debug.WriteLine("Livetile at: " + scheduled[i].DeliveryTime);
                            }
                            return;
                        }
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(xml.Replace("[text1]", it.time.ToString("H:mm")).Replace("[text2]", it.name.ToLower()));
                        try
                        {
                            Windows.UI.Notifications.ScheduledTileNotification scheduledTile = new Windows.UI.Notifications.ScheduledTileNotification(doc, trenV.time);
                            Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().AddToSchedule(scheduledTile);
                        }
                        catch
                        {
                            System.Diagnostics.Debug.WriteLine("Greška pri registraciji livetile za " + it.name + " (" + it.time.ToString() + ")");
                        }
                    }

                    trenV = it;

                    if (it.time == nextPrayer.time) el = true;
                }
            }
        }
        public static void UnregisterAllScheduledLiveTiles()
        {
            var notifier = Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication();
            var scheduled = notifier.GetScheduledTileNotifications();
            for (int i = 0; i < scheduled.Count; i++)
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().RemoveFromSchedule(scheduled[i]);
            }
        }
        public static void Reset()
        {
            var updater = Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication();
            var scheduled = updater.GetScheduledTileNotifications();
            for (int i = 0; i < scheduled.Count; i++)
            {
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().RemoveFromSchedule(scheduled[i]);
            }

            string xml = "<tile>\n";
            xml += "<visual version=\"2\">\n";
            xml += "  <binding template=\"TileSquare150x150Image\" fallback=\"TileSquareImage\">\n";
            xml += "  <image id=\"1\" src=\"Assets/Square150x150Logo.png\" alt=\"alt text\"/>";
            xml += "  </binding>\n";
            xml += "  <binding template=\"TileWide310x150Image\" fallback=\"TileWideImage\">\n";
            xml += "  <image id=\"1\" src=\"Assets/Wide310x150Logo.png\" alt=\"alt text\"/>";
            xml += "  </binding>\n";
            xml += "  <binding template=\"TileSquare310x310Image\">\n";
            xml += "  <image id=\"1\" src=\"Assets/Square310x310Logo.png\" alt=\"alt text\"/>";
            xml += "  </binding>\n";
            xml += "</visual>\n";
            xml += "</tile>";

            Windows.Data.Xml.Dom.XmlDocument txml = new Windows.Data.Xml.Dom.XmlDocument();
            txml.LoadXml(xml);
            Windows.UI.Notifications.TileNotification tNotification = new Windows.UI.Notifications.TileNotification(txml);
            Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Update(tNotification);
        }
    }
}
