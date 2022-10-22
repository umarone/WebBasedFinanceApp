using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Extensions.Logging;
using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using Newtonsoft.Json;
using PagedList;

namespace MudasirRehmanAlp.PurchaseSetup.GoodsSettings
{
    public class GoodReceivedReturnsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        GoodReturnDAL goodReturnDAL = new GoodReturnDAL();
        
        // GET: GoodReceivedReturns
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<GoodReceivedReturn> listobj = new List<GoodReceivedReturn>();
            listobj = db.GoodReceivedReturns.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId == BranchId).OrderByDescending(a=>a.GoodsReceivedReturnID).ToList();
            PagedList<GoodReceivedReturn> model = new PagedList<GoodReceivedReturn>(listobj, page, pageSize);
            return View(model);
        }

        // GET: GoodReceivedReturns/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodReceivedReturn goodReceivedReturn = db.GoodReceivedReturns.Find(id);
            var goodsReturnDetailsList = db.GoodReceivedReturnDetails.Where(a => a.GoodsReceivedReturnID == id).ToList();
            ViewBag.GoodsReturnDetailsList = goodsReturnDetailsList;
            if (goodReceivedReturn == null)
            {
                return HttpNotFound();
            }
            return View(goodReceivedReturn);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodReceivedReturn goodReceivedReturn = db.GoodReceivedReturns.Find(id);
            var goodsReturnDetailsList = db.GoodReceivedReturnDetails.Where(a => a.GoodsReceivedReturnID == id).ToList();
            ViewBag.GoodsReturnDetailsList = goodsReturnDetailsList;
            if (goodReceivedReturn == null)
            {
                return HttpNotFound();
            }
            return View(goodReceivedReturn);
        }

        // GET: GoodReceivedReturns/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["OrganizationID"]);
            GoodReceivedReturn obj = new GoodReceivedReturn();
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            obj.GRReturnNO = dALCode.AutoGenerateGoodReceivedsReturnCode(OrganizationID, BranchId);
            return View(obj);
        }

        // POST: GoodReceivedReturns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string ObjMasterItem, string ObjChilds)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = goodReturnDAL.AddGoodsRetrun(ObjMasterItem, ObjChilds, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Good Received Returns has been saved successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: GoodReceivedReturns/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodReceivedReturn goodReceivedReturn = db.GoodReceivedReturns.Find(id);
            var goodsReturnDetailsList = db.GoodReceivedReturnDetails.Where(a => a.GoodsReceivedReturnID == id).ToList();
            ViewBag.GoodsReturnDetailsList = goodsReturnDetailsList;
            if (goodReceivedReturn == null)
            {
                return HttpNotFound();
            }
            return View(goodReceivedReturn);
        }

        // POST: GoodReceivedReturns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
     
        public ActionResult Edit(string ObjMasterItem, string ObjChilds)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = goodReturnDAL.UpdateGoodsRetrun(ObjMasterItem, ObjChilds, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Good Received Returns has been updated successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: GoodReceivedReturns/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodReceivedReturn goodReceivedReturn = db.GoodReceivedReturns.Find(id);
            if (goodReceivedReturn == null)
            {
                return HttpNotFound();
            }
            var getmessage = goodReturnDAL.DeleteGoodsRetrun(goodReceivedReturn, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Good Received Returns has been updated successfully";
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public JsonResult LoadGoodReceivedDetails(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }

            var Result = (from p in db.GoodReceiveds.Where(a => a.GoodReceivedNoteId == id)
                          select new GoodsReceivedViewModel
                          {
                              GoodReceivedNoteId = p.GoodReceivedNoteId,
                              GRNNO = p.GRNNO,
                              OrganizationID = p.OrganizationID,
                              OrganizationName = p.OrganizationDefinition.OrganizationUnitName,
                              GoodsReceivedDate = p.GRNDate,
                              
                          }).FirstOrDefault();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadDetailsOfGoodReceivedDetails(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }

            var Result = db.GoodReceivedDetails.Where(a => a.GoodReceivedNoteID == id && a.Active == true && a.BalanceQuantity !=0).ToList();

            return PartialView("_PartialViewGoodsReceivedDetails", Result);
        }
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            GoodReceivedReturn _searchObject = JsonConvert.DeserializeObject<GoodReceivedReturn>(searchModel);
            List<GoodReceivedReturn> _listObject = new List<GoodReceivedReturn>();
            _listObject = db.GoodReceivedReturns.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId == BranchId).ToList();
            if (!String.IsNullOrEmpty(_searchObject.GRReturnNO) && !String.IsNullOrEmpty(_searchObject.GRReturnNO))
            {
                _listObject = _listObject.Where(r => r.GRReturnNO != null && r.GRReturnNO.ToString().ToLower().Contains(_searchObject.GRReturnNO)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.GRReturnNO))
            {
                _listObject = _listObject.Where(r => r.GRReturnNO != null && r.GRReturnNO.ToString().ToLower().Contains(_searchObject.GRReturnNO)).ToList();
            }

            return PartialView("_Search", _listObject);
        }
    }
}
