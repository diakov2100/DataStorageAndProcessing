namespace DataStorageAndProcessing.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Net;
    using System.Text;
    public sealed class Configuration : DbMigrationsConfiguration<DataStorageAndProcessing.Data.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataStorageAndProcessing.Data.Context context)
        {
            string[] SiteList = { "http://cwur.org/2012.php",
               "http://cwur.org/2013.php",
            "http://cwur.org/2014.php",
            "http://cwur.org/2015.php",
            "http://cwur.org/2016.php"};

            int RaitId = 1;
            foreach (string link in SiteList)
            {
                Raiting TecRaiting = new Raiting() { Id = RaitId, Year = int.Parse(link.Substring(16, 4)) };
                context.Raitings.Add(TecRaiting);
                context.SaveChanges();

                WebClient webClient = new WebClient();
                webClient.Encoding = Encoding.UTF8;
                string page = webClient.DownloadString(link);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(page);


                List<List<string>> table = doc.DocumentNode.SelectSingleNode("//table[@class='table table-bordered table-hover']")
                            .Descendants("tr")
                            .Skip(1)
                            .Where(tr => tr.Elements("td").Count() > 1)
                            .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                            .ToList();
                foreach (List<string> list in table)
                {
                    var tec = list[2];
                    Location TecLoc = context.Locations.FirstOrDefault(x => x.CountryName == tec);
                    if (TecLoc == null)
                    {
                        TecLoc = new Location()
                        {
                            Id = context.Locations.Count(),
                            CountryName = list[2]
                        };
                        context.Locations.AddOrUpdate(x => x.CountryName, TecLoc);
                        context.SaveChanges();
                    }

                    tec = list[1];
                    Institution TecInst = context.Institutions.FirstOrDefault(x => x.Name == tec);
                    if (TecInst == null)
                    {
                        TecInst = new Institution()
                        {
                            Id = context.Institutions.Count(),
                            Name = list[1],
                            Location = TecLoc,
                            LocationID = TecLoc.Id
                        };
                        context.Institutions.Add(TecInst);
                        context.SaveChanges();
                    }

                    int TecBroadImpact, TecPatents;
                    double TecScore;

                    if (list.Count == 13)
                    {
                        TecBroadImpact = int.Parse(list[10]);
                        TecPatents = STRparse(list[11]);
                        TecScore = double.Parse(list[12].Replace('.', ','));
                    }
                    else
                    {
                        TecBroadImpact = 0;
                        TecPatents = STRparse(list[10]);
                        TecScore = double.Parse(list[11].Replace('.', ','));
                    }

                    InstitutionRaiting TecInstRait = new InstitutionRaiting()
                    {
                        AlumniEmployment = STRparse(list[5]),
                        BroadImpact = TecBroadImpact,
                        Citations = STRparse(list[9]),
                        Id = context.InstiutionRaitings.Count(),
                        Influence = STRparse(list[8]),
                        Institution = TecInst,
                        InstitutionID = TecInst.Id,
                        NationalRank = int.Parse(list[3]),
                        Patents = TecPatents,
                        Publications = STRparse(list[7]),
                        QualityofEducation = STRparse(list[4]),
                        QualityofFaculty = STRparse(list[6]),
                        Raiting = context.Raitings.First(x => x.Id == RaitId),
                        RaitingID = TecRaiting.Id,
                        Score = TecScore,
                        WordRank = int.Parse(list[0])

                    };
                    context.InstiutionRaitings.Add(TecInstRait);
                    context.SaveChanges();

                }
                RaitId++;
            }
        }
        int STRparse(string str)
        {
            if (str.EndsWith("+"))
            {
                return int.Parse(str.Remove(str.Length - 1, 1));
            }
            return int.Parse(str);
        }
    }
}