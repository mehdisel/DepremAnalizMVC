using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DepremAnalizSistemi.Helpers;
using DepremAnalizSistemi.Models;
using DepremAnalizSistemi.Models.ViewModel;

namespace DepremAnalizSistemi.Controllers
{
    public class KullanicilarController : Controller
    {
        private DepremModel db = new DepremModel();


        public ActionResult Index()
        {
            return View(db.Kullanicilar.ToList());
        }



        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Ad,Soyad,TcNo,Email,Telefon,Sifre,SifreOnay")] Kullanicilar kullanicilar)
        {
            kullanicilar.ErrorMessage = "";
            bool emailVarMi = db.Kullanicilar.Any(x => x.Email == kullanicilar.Email);
            bool tcNoVarMi = db.Kullanicilar.Any(x => x.TcNo == kullanicilar.TcNo);
            bool telefonVarMi = db.Kullanicilar.Any(x => x.Telefon == kullanicilar.Telefon);
            if (ModelState.IsValid)
            {
                if (emailVarMi || tcNoVarMi || telefonVarMi)
                {
                    string olanlar = string.Format($"{(emailVarMi ? "Email " : "")} {(tcNoVarMi ? " Tcno " : "")} {(telefonVarMi ? "Telefon " : "")}");

                   
                    kullanicilar.ErrorMessage += olanlar + " sistemde kayıtlı.";
                    return PartialView(@"~\Views\Depremler\RegisterPartial.cshtml", kullanicilar);
                }
                db.Kullanicilar.Add(kullanicilar);
                db.SaveChanges();
                return Json(new { url = Url.Action("Index", "Depremler") });

            }
            return PartialView(@"~\Views\Depremler\RegisterPartial.cshtml", kullanicilar);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Email,Password")] UserLoginViewModel userLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                Kullanicilar user = db.Kullanicilar.Where(x => x.Email == userLoginViewModel.Email && x.Sifre == userLoginViewModel.Password).FirstOrDefault();

                if (user != null)
                {
                    Session["kullanici"] = user.ID;
                    return Json(new { url = Url.Action("Index", "Depremler") });
                }
                else
                {
                    //bu durumda model valid ama bilgiler uyuşmuyor
                    userLoginViewModel.LoginErrorMessage = "E-posta ve şifre uyuşmuyor";
                    userLoginViewModel.Password = null;
                    return PartialView(@"~\Views\Depremler\LoginPartial.cshtml", userLoginViewModel);

                }
            }
            else
            {
                return PartialView(@"~\Views\Depremler\LoginPartial.cshtml", userLoginViewModel);
            }
        }
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword([Bind(Include = "Email,Password")] UserLoginViewModel userLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                Kullanicilar user = db.Kullanicilar.Where(x => x.Email == userLoginViewModel.Email).FirstOrDefault();

                if (user != null)
                {
                    string icerik = string.Format($"Email:{user.Email}\nŞifre:{user.Sifre}");
                    bool GonderdiMi = MailSendHelper.Gonder("Deprem Analiz Sistemi Şifre Hatırlatma", icerik, user.Email);
                    return GonderdiMi ? Json(new { url = Url.Action("Index", "Depremler") }) : (ActionResult)PartialView("PasswordPartial");
                }
                else
                {

                    return PartialView("PasswordPartial");
                }
            }
            else
            {
                return PartialView("PasswordPartial");
            }
        }
        public ActionResult Cikis()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Depremler");
        }
        [HttpPost]
        public ActionResult GetUser()
        {
            int userID = Convert.ToInt32(Session["kullanici"]);
            Kullanicilar girisYapan = db.Kullanicilar.Find(userID);
            girisYapan.Sifre = null;
            return PartialView(@"~\Views\Depremler\AccountPartial.cshtml", girisYapan);
        }
        [HttpPost]
        public ActionResult GetUsers(string Arama)
        {
            if (Session["kullanici"] == null)
            {
                return Json(null);
            }
            int kullaniciID = (int)Session["kullanici"];
            List<Kullanicilar> kullanicilar;
            kullanicilar = db.Kullanicilar.Where(x => x.ID != kullaniciID).ToList();
            List<UserViewModel> users = new List<UserViewModel>();
            UserViewModel user;
            foreach (var kullanici in kullanicilar)
            {
                user = new UserViewModel();
                user.ID = kullanici.ID;
                user.AdSoyad = (kullanici.Ad + " " + kullanici.Soyad).ToUpper();
                user.Email = kullanici.Email;
                users.Add(user);
            }
            if (!string.IsNullOrEmpty(Arama))
            {
                users = users.Where(x => x.AdSoyad.Contains(Arama.ToUpper())).ToList();

            }
            return Json(users);

        }
        [HttpPost]
        public ActionResult SendMessage([Bind(Include = "AliciID,GonderenID,DepremID,Mesaj,MesajBasligi")]Mesajlar message)
        {
            int kullaniciID = (int)Session["kullanici"];
            Mesajlar mesaj = new Mesajlar();
            mesaj.AliciID = message.AliciID;
            mesaj.DepremID = message.DepremID;
            mesaj.GonderenID = kullaniciID;
            mesaj.MesajBasligi = message.MesajBasligi;
            mesaj.Mesaj = message.Mesaj;
            mesaj.GonderimZamani = DateTime.Now;
            mesaj.GonderenSildiMi = false;
            mesaj.AliciSildiMi = false;
            mesaj.OkunduMu = false;
            db.Mesajlar.Add(mesaj);
            db.SaveChanges();
            return Json("başarılı");
        }
        [HttpPost]
        public ActionResult GetInbox()
        {
            int kullaniciID = (int)Session["kullanici"];
            return PartialView(@"~\Views\Depremler\MessagesPartial.cshtml", db.Mesajlar.Where(x => x.AliciID == kullaniciID && x.AliciSildiMi == false).OrderByDescending(x => x.GonderimZamani).ToList());
        }
        [HttpPost]
        public ActionResult GetSendMessages()
        {
            int kullaniciID = (int)Session["kullanici"];
            return PartialView(@"~\Views\Depremler\SendMessagesPartial.cshtml", db.Mesajlar.Where(x => x.GonderenID == kullaniciID && x.GonderenSildiMi == false).OrderByDescending(x => x.GonderimZamani).ToList());
        }
        [HttpPost]
        public ActionResult DeleteMessage(int MesajID)
        {
            int kullaniciID = (int)Session["kullanici"];
            Mesajlar mesaj = db.Mesajlar.Where(x => x.ID == MesajID).FirstOrDefault();
            if (mesaj.AliciID == kullaniciID)
            {
                mesaj.AliciSildiMi = true;
            }
            else if (mesaj.GonderenID == kullaniciID)
            {
                mesaj.GonderenSildiMi = true;
            }
            db.SaveChanges();
            return Json("Başarılı");
        }
        [HttpPost]
        public ActionResult GetMessage(int MesajID)
        {
            int kullaniciID = (int)Session["kullanici"];
            Mesajlar mesaj = db.Mesajlar.Where(x => x.ID == MesajID).FirstOrDefault();
            mesaj.OkunduMu = true;
            db.SaveChanges();
            if (mesaj.AliciID == kullaniciID)
            {
                var _mesaj = db.Mesajlar.Where(x => x.ID == MesajID).Select(x => new { x.ID, x.MesajBasligi, x.GonderimZamani, x.Mesaj, x.GonderenID, x.AliciID, x.Gonderen.Ad, x.Gonderen.Soyad, x.Gonderen.Email, x.Depremler.Id, x.Depremler.DepremTarihi, x.Depremler.DepremYeri, x.Depremler.Siddet, x.Depremler.Derinlik, x.Depremler.Enlem, x.Depremler.Boylam }).FirstOrDefault();
                
                return Json(_mesaj);
            }
            else
            {
                var _mesaj = db.Mesajlar.Where(x => x.ID == MesajID).Select(x => new { x.ID, x.MesajBasligi, x.GonderimZamani, x.Mesaj, x.GonderenID, x.AliciID, x.Alici.Ad, x.Alici.Soyad, x.Alici.Email, x.Depremler.Id, x.Depremler.DepremTarihi, x.Depremler.DepremYeri, x.Depremler.Siddet, x.Depremler.Derinlik, x.Depremler.Enlem, x.Depremler.Boylam }).FirstOrDefault();
                return Json(_mesaj);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeUserSettings([Bind(Include = "ID,Email,Telefon")]AccountSettingsViewModel kullanici)
        {
            bool emailVarMi = db.Kullanicilar.Any(x => x.Email == kullanici.Email && x.ID != kullanici.ID);
            bool telefonVarMi = db.Kullanicilar.Any(x => x.Telefon == kullanici.Telefon && x.ID != kullanici.ID);
            Kullanicilar bilgileriDegisecekOlanKullanici = db.Kullanicilar.Find(kullanici.ID);
            if (ModelState.IsValid)
            {
                if (emailVarMi || telefonVarMi)
                {
                    string olanlar = string.Format($"{(emailVarMi ? "Email " : "")} {(telefonVarMi ? "Telefon " : "")}");

                    // return Json(kullanicilar,JsonRequestBehavior.AllowGet);
                    bilgileriDegisecekOlanKullanici.ErrorMessage += olanlar + " sistemde kayıtlı.";
                    bilgileriDegisecekOlanKullanici.Sifre = null;
                    return PartialView(@"~\Views\Depremler\AccountPartial.cshtml", bilgileriDegisecekOlanKullanici);
                }
                bilgileriDegisecekOlanKullanici.Email = kullanici.Email;
                bilgileriDegisecekOlanKullanici.Telefon = kullanici.Telefon;
                bilgileriDegisecekOlanKullanici.SifreOnay = bilgileriDegisecekOlanKullanici.Sifre;
                db.SaveChanges();
                return Json(new { url = Url.Action("Index", "Depremler") });
            }
            return PartialView(@"~\Views\Depremler\AccountPartial.cshtml", bilgileriDegisecekOlanKullanici);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword([Bind(Include = "ID,Sifre,YeniSifre,YeniSifreOnay")]PasswordViewModel password)
        {
            Kullanicilar sifresiDegisecekOlanKullanici = db.Kullanicilar.Find(password.ID);
            if (ModelState.IsValid)
            {
                if (password.Sifre!=sifresiDegisecekOlanKullanici.Sifre)
                {
                    sifresiDegisecekOlanKullanici.PasswordErrorMessage = "Şifrenizi doğru girmediniz!";
                    sifresiDegisecekOlanKullanici.Sifre = null;
                    return PartialView(@"~\Views\Depremler\AccountPartial.cshtml", sifresiDegisecekOlanKullanici);
                }
                if (password.YeniSifre==password.Sifre)
                {
                    sifresiDegisecekOlanKullanici.PasswordErrorMessage = "Yeni şifre ve eski şifre aynı olamaz!";
                    sifresiDegisecekOlanKullanici.Sifre = null;
                    return PartialView(@"~\Views\Depremler\AccountPartial.cshtml", sifresiDegisecekOlanKullanici);
                }
                sifresiDegisecekOlanKullanici.Sifre = password.YeniSifre;
                sifresiDegisecekOlanKullanici.SifreOnay = password.YeniSifre;
                db.SaveChanges();
                return Json(new { url = Url.Action("Index", "Depremler") });
            }
            return PartialView(@"~\Views\Depremler\AccountPartial.cshtml", sifresiDegisecekOlanKullanici);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
