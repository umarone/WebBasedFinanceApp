using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Infrastructure.AppServices;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using Newtonsoft.Json;
using PagedList;

namespace MudasirRehmanAlp.EmailsSetup
{
    public class MailSettingsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        // GET: MailSettings

        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<MailSetting> listobj = new List<MailSetting>();
            listobj = db.MailSettings.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID).OrderByDescending(a=>a.Id).ToList();
            PagedList<MailSetting> model = new PagedList<MailSetting>(listobj, page, pageSize);
            return View(model);
        }
        // GET: MailSettings/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailSetting mailSetting = db.MailSettings.Find(id);
            if (mailSetting == null)
            {
                return HttpNotFound();
            }
            return View(mailSetting);
        }
        // GET: MailSettings/Print/5
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailSetting mailSetting = db.MailSettings.Find(id);
            if (mailSetting == null)
            {
                return HttpNotFound();
            }
            return View(mailSetting);
        }
        // GET: MailSettings/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            MailSetting obj = new MailSetting();
            obj.MailCode = dALCode.AutoGenerateMailSettingCode(OrganizationID/*, BranchId*/);
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            return View(obj);
        }

        // POST: MailSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MailSetting mailSetting)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            if (ModelState.IsValid)
            {
                mailSetting.MailCode = dALCode.AutoGenerateMailSettingCode(OrganizationID/*, BranchId*/);
                mailSetting.IsDeleted = false;
                mailSetting.CreatedBy = UserID;
                mailSetting.CreatedDate = DateTime.Now;
                db.MailSettings.Add(mailSetting);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Mail Settings has been saved successfully";
                return RedirectToAction("Index");
            }
            return View(mailSetting);
        }

        // GET: MailSettings/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailSetting mailSetting = db.MailSettings.Find(id);
            if (mailSetting == null)
            {
                return HttpNotFound();
            }

            return View(mailSetting);
        }

        // POST: MailSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MailSetting mailSetting)
        {

            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                var findmailSetting = db.MailSettings.Find(mailSetting.Id);

                findmailSetting.OrganizationID = mailSetting.OrganizationID;
               
                findmailSetting.MailCode = mailSetting.MailCode;
                findmailSetting.FromEmail = mailSetting.FromEmail;
                findmailSetting.HostEmail = mailSetting.HostEmail;
                findmailSetting.PortNo = mailSetting.PortNo;
                findmailSetting.UserNameEmail = mailSetting.UserNameEmail;
                if (!String.IsNullOrEmpty(mailSetting.Password))
                {
                    findmailSetting.Password = mailSetting.Password;
                }
                findmailSetting.IsActive = mailSetting.IsActive;
                findmailSetting.EnableSSL = mailSetting.EnableSSL;
                findmailSetting.IsDeleted = false;
                findmailSetting.UpdatedBy = UserID;
                findmailSetting.UpdatedDate = DateTime.Now;
                db.Entry(findmailSetting).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Mail Settings has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(mailSetting);
        }

        // GET: MailSettings/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailSetting mailSetting = db.MailSettings.Find(id);
            if (mailSetting == null)
            {
                return HttpNotFound();
            }

            mailSetting.IsDeleted = true;
            mailSetting.DeletedBy = UserID;
            mailSetting.DeletedDate = DateTime.Now;
            db.Entry(mailSetting).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Mail Settings has been deleted successfully";
            return RedirectToAction("Index");
        }

        // GET: MailSettings/MakeDefault/5
        public ActionResult MakeDefault(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DefaultedListMailSettings();
            MailSetting mailSetting = db.MailSettings.Find(id);
            mailSetting.IsDefault = true;
            mailSetting.DefaultBy = UserID;
            mailSetting.DefaultDate = DateTime.Now;

            if (mailSetting == null)
            {
                return HttpNotFound();
            }

            db.Entry(mailSetting).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Mail Settings has been defaulted set successfully";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public void DefaultedListMailSettings()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var objList = db.MailSettings.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId == BranchId).ToList();

            foreach (var item in objList)
            {
                var obj = db.MailSettings.Find(item.Id);
                obj.IsDefault = false;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();

            }
        }
        //--------- 
        [HttpPost]
        [Obsolete]
        public async Task<ActionResult> EmailSendingAsync(EmailViewModel viewModel)
        {
            EmailkitService emailkitService = new EmailkitService();
            MailSettingViewModel mailSetting = new MailSettingViewModel();
            bool isEmailSend = false;
            MailSetting findObj = db.MailSettings.Find(viewModel.Id);
            if (findObj != null)
            {
                mailSetting.FromEmail = findObj.FromEmail;
                mailSetting.HostEmail = findObj.HostEmail;
                mailSetting.PortNo = Convert.ToInt32(findObj.PortNo);
                mailSetting.UserNameEmail = findObj.UserNameEmail;
                mailSetting.Password = findObj.Password;
                mailSetting.EnableSSL = Convert.ToBoolean(findObj.EnableSSL);
            }
            isEmailSend = await emailkitService.SendAsync(mailSetting, viewModel.ToEmail, null, viewModel.Subject,viewModel.Body);
            if (isEmailSend)
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Email has been send successfully";
            }
            return RedirectToAction("Index");
        }
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            MailSetting _searchObject = JsonConvert.DeserializeObject<MailSetting>(searchModel);
            List<MailSetting> _listObject = new List<MailSetting>();
            _listObject = db.MailSettings.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId== BranchId).ToList();
            if (!String.IsNullOrEmpty(_searchObject.MailCode) && !String.IsNullOrEmpty(_searchObject.FromEmail))
            {
                _listObject = _listObject.Where(r => r.MailCode != null && r.MailCode.ToString().ToLower().Contains(_searchObject.MailCode) || r.FromEmail.ToString().ToLower().Contains(_searchObject.FromEmail)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.MailCode))
            {
                _listObject = _listObject.Where(r => r.MailCode != null && r.MailCode.ToString().ToLower().Contains(_searchObject.MailCode)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.FromEmail))
            {
                _listObject = _listObject.Where(r => r.FromEmail != null && r.FromEmail.ToString().ToLower().Contains(_searchObject.FromEmail)).ToList();
            }
            return PartialView("_Search", _listObject);
        }
    }
}
