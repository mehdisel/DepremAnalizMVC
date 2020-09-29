using DepremAnalizSistemi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using DepremAnalizSistemi.Models.ViewModel;

namespace DepremAnalizSistemi.Controllers
{
    public class DepremlerController : Controller
    {
        DepremModel db = new DepremModel();
        static List<string> parantezsizDepremYerleri; //kandlliden verileri çekerken bazı yerlerin parantezli bazılarının parantezsiz olmasından dolayı parantezsizleri bu lis te tuttum.
        // GET: Depremler

        public ActionResult Index()
        {
            Helpers.MailSendHelper.Gonder("merhaba mehdi", "programa hoş geldin", "mehdi.sel.94@gmail.com");
            return View();
        }
        [HttpPost]
        public ActionResult Index(List<Depremler> data, int page = 1)
        {
            
            List<Depremler> depremler = new List<Depremler>();
            foreach (var item in db.Depremler)
            {
                if (data != null)
                {
                    if (data.Any(x => x.Id == item.Id))
                    {
                        depremler.Add(item);
                    }
                }
                else
                {
                    break;
                }
            }
           
            return Request.IsAjaxRequest()
            ? (ActionResult)PartialView("DepremTablo", depremler.OrderByDescending(x => x.DepremTarihi).ToPagedList(page, 10))
            : View(depremler.OrderByDescending(x => x.DepremTarihi).ToPagedList(page, 10));
        }

        [HttpPost]
        public JsonResult GetEarthQuakes([Bind(Include = "DepremYeri,BaslangicTarihi,BitisTarihi,MinSiddet,MaxSiddet,MinDerinlik,MaxDerinlik,Enlem,Boylam")] DepremViewModel depremViewModel)//kriterlere göre depremleri getiren ana action
        {
            List<Depremler> depremler;
            depremler = db.Depremler.Where(x => (x.DepremTarihi >= depremViewModel.BaslangicTarihi && x.DepremTarihi <= depremViewModel.BitisTarihi)&&(x.Siddet>=depremViewModel.MinSiddet&&x.Siddet<=depremViewModel.MaxSiddet)&&(x.Derinlik>=depremViewModel.MinDerinlik&&x.Derinlik<=depremViewModel.MaxDerinlik)).OrderBy(x => x.Siddet).ToList();
            if (depremViewModel.Enlem != null && depremViewModel.Boylam != null)
            {
                decimal? EnlemPY=depremViewModel.Enlem+ 0.09009009009M;
                decimal? EnlemNY=depremViewModel.Enlem- 0.09009009009M;
                decimal? BoylamPY=depremViewModel.Boylam+ 0.11764705882M;
                decimal? BoylamNY=depremViewModel.Boylam- 0.11764705882M;
                depremler = depremler.Where(x => (x.Enlem < EnlemPY && x.Enlem > EnlemNY)&&(x.Boylam<BoylamPY&&x.Boylam>BoylamNY)).ToList();
            }
            else if (depremViewModel.DepremYeri != "seciniz")//deprem yeri seçiniz değil ise ve
            {
                if (!parantezsizDepremYerleri.Contains(depremViewModel.DepremYeri))  //depremyeri parantezsizDepremYerleri içinde yok ise
                {
                    depremler = depremler.Where(x=>x.DepremYeri.Contains("(" + depremViewModel.DepremYeri + ")")).OrderBy(x => x.Siddet).ToList(); //parantezli olarak veritabanında uyuşan baslangıç ve bitiş tarihleri arasındaki verileri linq sorgusunda getirdim.
                }
                else
                {
                    depremler = depremler.Where(x =>x.DepremYeri.Contains(depremViewModel.DepremYeri)).OrderBy(x => x.Siddet).ToList();//deprem yeri seciniz değil ise ve  parantezsizDepremYerleri içinde var ise veritabanındaki baslangıç ve bitiş tarihleri arasındaki verileri linq sorgusunda getirdim.
                }
            }

            return Json(depremler.Select(x=> new { x.Id,x.DepremTarihi,x.DepremYeri,x.Boylam,x.Derinlik,x.Enlem,x.Siddet}));

        }
        [HttpGet]
        public ActionResult GetFirstAndLastRecord()//ilk ve son depremi getiren action
        {
            List<Depremler> depremler = new List<Depremler>();
            Depremler ilkDeprem = db.Depremler.OrderBy(x => x.DepremTarihi).Take(1).FirstOrDefault();//ilk depremi getiren sorgu
            depremler.Add(ilkDeprem);
            Depremler sonDeprem = db.Depremler.OrderByDescending(x => x.DepremTarihi).Take(1).FirstOrDefault();//son depremi getiren sorgu
            depremler.Add(sonDeprem);
            return Json(depremler.Select(x=>new {x.DepremTarihi }), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetCityNames(DateTime baslangicTarihi, DateTime bitisTarihi/*List<Depremler> gelenVeri*/)//baslangic ve bitis tarihine göre depremlerin oldugu yerleri getiren action
        {

            // JsonConvert.DeserializeObject<List<Depremler>>(GetEarthQuakes(baslangicTarihi, bitisTarihi).Data.ToString());

            List<Depremler> depremYeriListesi = new List<Depremler>();
            parantezsizDepremYerleri = new List<string>();
            List<Depremler> depremYerleri = db.Depremler.Where(x => x.DepremTarihi >= baslangicTarihi && x.DepremTarihi <= bitisTarihi).OrderBy(x => x.DepremYeri).ToList();//deprem yerine göre ascending sıralanmış depremler

            //var depremYerleri = db.Depremler.Select(x => new { x.DepremYeri }).Distinct();
            Depremler deprem;
            //if (gelenVeri==null)
            //{
            //    return null;
            //}
            foreach (var item in depremYerleri)//gelen depremlerin yerlerinin isimlendirilmesinin içinde deprem yerleri oldugu için gelen veriyi foreach ile dönüp istediğim şekilde parçalayıp aldım.
            {
                deprem = new Depremler();
                if (item.DepremYeri.Split('(', ')').Count() > 1)
                {
                    if (item.DepremYeri.Split('(', ')')[1].IndexOf(":") > 0)
                    {
                        deprem.DepremYeri = item.DepremYeri.Split('(', ')', ' ')[0].Trim();

                    }
                    else
                    {
                        deprem.DepremYeri = item.DepremYeri.Split('(', ')')[1].Trim();
                    }
                }
                else
                {
                    deprem.DepremYeri = item.DepremYeri.Trim();
                    parantezsizDepremYerleri.Add(deprem.DepremYeri);
                }
                depremYeriListesi.Add(deprem);
            }
            return Json(depremYeriListesi.Select(x => new { x.DepremYeri }).Distinct().OrderBy(x => x.DepremYeri), JsonRequestBehavior.AllowGet);
            //return Json(gelenVeri, JsonRequestBehavior.AllowGet); ;
        }
    }
}