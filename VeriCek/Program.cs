using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using VeriCek.Model;

namespace VeriCek
{
    class Program
    {
        static void Main(string[] args)
        {
            Timer t = new Timer(TimerCallback, null, 0, 10000);
            Console.Read();
        }
        private static void TimerCallback(Object o)
        {
            Console.WriteLine("Son veri çekilme tarihi: " + DateTime.Now);
            XmlDocument doc = new XmlDocument();
            doc.Load("http://udim.koeri.boun.edu.tr/zeqmap/xmlt/SonAY.xml");
            //http://udim.koeri.boun.edu.tr/zeqmap/xmlt/SonAY.xml
            //http://udim.koeri.boun.edu.tr/zeqmap/xmlt/sonHafta.xml
            XmlNodeList elemList = doc.GetElementsByTagName("earhquake");
            DepremModel context = new DepremModel();
            Depremler deprem;
            int etkilenenAdet = 0;
            string depremYeri;
            decimal depremEnlem;
            DateTime depremTarihi;
            DateTime ilkDepremTarihi = Convert.ToDateTime(elemList[0].Attributes["name"].Value);
            List<Depremler> depremler = new List<Depremler>();
            using (DepremModel db = new DepremModel())
            {
                db.Configuration.LazyLoadingEnabled = false;
                depremler = db.Depremler.Where(x => x.DepremTarihi >= ilkDepremTarihi).ToList();
            }


            for (int i = 0; i < elemList.Count; i++)
            {
                depremTarihi = Convert.ToDateTime(elemList[i].Attributes["name"].Value);
                depremYeri = elemList[i].Attributes["lokasyon"].Value.TrimEnd();
                depremEnlem = Convert.ToDecimal(elemList[i].Attributes["lat"].Value.Replace('.', ','));
                if (depremler.Any(x => x.DepremTarihi == depremTarihi && x.DepremYeri == depremYeri && x.Enlem == depremEnlem))
                {
                    continue;
                }
                else if (depremler.Any(x => x.DepremYeri != depremYeri && x.DepremTarihi == depremTarihi && x.Enlem == depremEnlem))
                {
                    etkilenenAdet++;
                    deprem = context.Depremler.Where(x => x.DepremTarihi == depremTarihi).FirstOrDefault();
                    deprem.DepremYeri = elemList[i].Attributes["lokasyon"].Value.TrimEnd();
                    deprem.Enlem = Convert.ToDecimal(elemList[i].Attributes["lat"].Value.Replace('.', ','));
                    deprem.Boylam = Convert.ToDecimal(elemList[i].Attributes["lng"].Value.Replace('.', ','));
                    deprem.Siddet = Convert.ToDecimal(elemList[i].Attributes["mag"].Value.Replace('.', ','));
                    deprem.Derinlik = Convert.ToDecimal(elemList[i].Attributes["Depth"].Value.Replace('.', ','));
                    Console.WriteLine(deprem.Id.ToString());
                    context.SaveChanges();
                }
                else
                {
                    etkilenenAdet++;
                    deprem = new Depremler();
                    deprem.DepremTarihi = Convert.ToDateTime(elemList[i].Attributes["name"].Value);
                    deprem.DepremYeri = elemList[i].Attributes["lokasyon"].Value.TrimEnd();
                    deprem.Enlem = Convert.ToDecimal(elemList[i].Attributes["lat"].Value.Replace('.', ','));
                    deprem.Boylam = Convert.ToDecimal(elemList[i].Attributes["lng"].Value.Replace('.', ','));
                    deprem.Siddet = Convert.ToDecimal(elemList[i].Attributes["mag"].Value.Replace('.', ','));
                    deprem.Derinlik = Convert.ToDecimal(elemList[i].Attributes["Depth"].Value.Replace('.', ','));
                    context.Depremler.Add(deprem);
                    context.SaveChanges();
                }
            }

            //for (int i = 0; i < elemList.Count; i++)
            //{
            //    depremTarihi = Convert.ToDateTime(elemList[i].Attributes["name"].Value);
            //    depremYeri = elemList[i].Attributes["lokasyon"].Value.TrimEnd();
            //    depremEnlem = Convert.ToDecimal(elemList[i].Attributes["lat"].Value.Replace('.', ','));
            //    if (context.Depremler.Any(x => x.DepremTarihi == depremTarihi && x.DepremYeri == depremYeri && x.Enlem == depremEnlem))
            //    {
            //        continue;
            //    }
            //    else if (context.Depremler.Any(x => x.DepremYeri != depremYeri && x.DepremTarihi == depremTarihi && x.Enlem == depremEnlem))
            //    {
            //        etkilenenAdet++;
            //        deprem = context.Depremler.Where(x => x.DepremTarihi == depremTarihi).FirstOrDefault();
            //        deprem.DepremYeri = elemList[i].Attributes["lokasyon"].Value.TrimEnd();
            //        deprem.Enlem = Convert.ToDecimal(elemList[i].Attributes["lat"].Value.Replace('.', ','));
            //        deprem.Boylam = Convert.ToDecimal(elemList[i].Attributes["lng"].Value.Replace('.', ','));
            //        deprem.Siddet = Convert.ToDecimal(elemList[i].Attributes["mag"].Value.Replace('.', ','));
            //        deprem.Derinlik = Convert.ToDecimal(elemList[i].Attributes["Depth"].Value.Replace('.', ','));
            //        Console.WriteLine(deprem.Id.ToString());
            //        context.SaveChanges();
            //    }
            //    else
            //    {
            //        etkilenenAdet++;
            //        deprem = new Depremler();
            //        deprem.DepremTarihi = Convert.ToDateTime(elemList[i].Attributes["name"].Value);
            //        deprem.DepremYeri = elemList[i].Attributes["lokasyon"].Value.TrimEnd();
            //        deprem.Enlem = Convert.ToDecimal(elemList[i].Attributes["lat"].Value.Replace('.', ','));
            //        deprem.Boylam = Convert.ToDecimal(elemList[i].Attributes["lng"].Value.Replace('.', ','));
            //        deprem.Siddet = Convert.ToDecimal(elemList[i].Attributes["mag"].Value.Replace('.', ','));
            //        deprem.Derinlik = Convert.ToDecimal(elemList[i].Attributes["Depth"].Value.Replace('.', ','));
            //        context.Depremler.Add(deprem);
            //        context.SaveChanges();
            //    }
            //}

            Console.WriteLine("Etkilenen Adet: " + etkilenenAdet);
            GC.Collect();
        }
    }
}
