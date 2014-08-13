using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Syncbak.SyncboxSetup.Models;
using Syncbak.SyncboxSetup.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Syncbak.SyncboxSetup.Repositories;

namespace Syncbak.SyncboxSetup.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Syncbak.SyncboxSetup.Repositories.StepsRepository repos = new Syncbak.SyncboxSetup.Repositories.StepsRepository();

            //var result = repos.GetInstallSteps(0);

            return View();
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.Message = "Edit";
            ViewBag.StationId = id;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GetCategories()
        {
            Syncbak.SyncboxSetup.Repositories.StepsRepository repos = new Syncbak.SyncboxSetup.Repositories.StepsRepository();

            var result = repos.GetCategories();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSteps(int? categoryId)
        {
            Syncbak.SyncboxSetup.Repositories.StepsRepository repos = new Syncbak.SyncboxSetup.Repositories.StepsRepository();

            var result = repos.GetSteps(categoryId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInstallSteps(int stationId)
        {
            StepsRepository repos = new StepsRepository();

            var result = repos.GetInstallSteps(stationId);

            var ret = new JsonNetResult();
            ret.SerializerSettings.Converters.Add(new IsoDateTimeConverter());
            //ret.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            ret.Data = result;
            return ret;

            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddInitialSteps(int stationId)
        {
            StepsRepository repo = new StepsRepository();

            var result = repo.AddInitialSteps(stationId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetStations()
        {
            StationRepository repos = new StationRepository();

            var result = repos.GetStation(null, null, true);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStepInfo(int stationId)
        {
            StepsRepository repo = new StepsRepository();
            var result = repo.GetStepInfo(stationId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveInstallStep(int installStepId, string accountOwner, int stepId, DateTimeOffset? startDate, DateTimeOffset? endDate, string status, string owner, int stationId, bool currentStep)
        {
            StepsRepository repos = new StepsRepository();

            var result = repos.SaveInstallStep(installStepId, accountOwner, stepId, startDate, endDate, status, owner, stationId, currentStep);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveInstallStepInfo(int installStepId, int stationId, string accountOwner, bool isComplete)
        {
            StepsRepository repos = new StepsRepository();

            var result = repos.SaveInstallStepInfo(installStepId, stationId, accountOwner, isComplete);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}