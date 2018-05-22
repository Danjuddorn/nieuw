using MtsSurvey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MtsSurvey.Controllers
{
    public class SurveyController : Controller
    {
        public ActionResult SurveyIndex(int _Argsurveyid = 0, int _ArgCustId = 0)
        {
            UserVM model = new UserVM();
            int result = SurveyCommonTask.CreateSurveyModel(_ArgCustId, _Argsurveyid, ref model);

            if (result == 1)
            {
                return RedirectToAction("CompleteSurvey", "Survey");
            }
            else
            {
                return View(model); 
            }

           
        }

        public ActionResult CompleteSurvey()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SurveyIndex(UserVM model)
        {
            SurveyCommonTask.SaveSurveyModel(model);
            FormsAuthentication.SignOut();
            return RedirectToAction("CompleteSurvey", "Survey");
        }
    }
}