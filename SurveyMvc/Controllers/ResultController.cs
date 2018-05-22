using MtsSurvey.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using SurveyMvc.Models.Result;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SurveyMvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ResultController : Controller
    {


        public void ExportToCSV2()
        {
            StringWriter sw = new StringWriter();

            sw.WriteLine("\"ID\",\"Naam\",\"Startdatum\",\"Einddatum\"");

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=resultaten.csv");
            Response.ContentType = "application/octet-stream";

            ResultClass db = new ResultClass();
            SurveyContext SurveyContextObj = new SurveyContext();
            var users = SurveyContextObj.DbSurveyResult.Select(p => new { p.SurveyId, p.SurveyPoint, p.SurveyDate, p.SurveyReply }).ToList();

            foreach (var user in users)
            {
                sw.WriteLine(string.Format("{0},{1},{2},{3}\n",

                user.SurveyId,
                user.SurveyPoint,
                user.SurveyDate,
                user.SurveyReply

                ));
            }
            Response.Write(sw.ToString());
            Response.End();

        }
        public ActionResult ResultIndex()
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            ViewBag.SurveyBag = new SelectList(SurveyContextObj.DbSurveyMaster, "SurveyId", "SurveyCaption");


            return View();
        }
        public ActionResult GetChartData(int SurveyId)
        {
            SurveyContext SurveyContextObj = new SurveyContext();
            ResultClass ResultClassObj = new ResultClass();

            SurveyMaster SurveyMasterObj = SurveyContextObj.DbSurveyMaster.Where(p => p.SurveyId == SurveyId).FirstOrDefault();
            if (SurveyMasterObj != default(SurveyMaster))
            {
                ResultClassObj.SurveyId = SurveyMasterObj.SurveyId;
                ResultClassObj.SurveyCaption = SurveyMasterObj.SurveyCaption;
                ResultClassObj.DateStart = SurveyMasterObj.DateStart.ToString("dd/MMM/yyyy");
                ResultClassObj.DateEnd = SurveyMasterObj.DateEnd.ToString("dd/MMM/yyyy");
                ResultClassObj.ResultSQs = new List<ResultSQ>();

                var SurveyQuestObjs = SurveyContextObj.DbSurveyQuestion.Where(p => p.SurveyId == SurveyMasterObj.SurveyId).ToList();
                foreach (var SurveyQuestObj in SurveyQuestObjs)
                {
                    ResultSQ ResultSQs = new ResultSQ();
                    ResultSQs.QuestionId = SurveyQuestObj.QuestionId;
                    ResultSQs.SurveySeq = SurveyQuestObj.SurveySeq;
                    ResultSQs.SurveyType = SurveyQuestObj.SurveyType;
                    ResultSQs.Surveyquestion = SurveyQuestObj.Surveyquestion;
                    if (SurveyQuestObj.SurveyType == 1)
                    {
                        ResultSQs.ChartData = new ChartClass();

                        ResultSQs.ChartData.labels = new List<string>();
                        ResultSQs.ChartData.datasets = new List<datasetsClass>();
                        List<int> Chartdata = new List<int>();

                        var SurveyAnsResultList = from SA in SurveyContextObj.DbSurveyAnswer.Where(p => p.AnswerID == SurveyQuestObj.PossibleAnswersID).OrderBy(ord => ord.AnswerSeq).Select(s => new { s.AnswerSeq, s.AnswerText })
                                                  join SR in SurveyContextObj.DbSurveyResult.Where(p => p.SurveyId == SurveyId && p.QuestionId == SurveyQuestObj.QuestionId && p.SurveyPoint != 0)
                                                  .GroupBy(grp => grp.SurveyPoint)
                                                  .Select(g => new { g.Key, count = g.Count() })
                                                  on SA.AnswerSeq equals SR.Key into SJR
                                                  from SJRD in SJR.DefaultIfEmpty()
                                                  select new
                                                  {
                                                      AnswerSeq = SA.AnswerSeq,
                                                      AnswerText = SA.AnswerText.Substring(0, 10),
                                                      count = (SJRD == null ? 0 : SJRD.count)
                                                  };

                        foreach (var SARData in SurveyAnsResultList)
                        {
                            ResultSQs.ChartData.labels.Add(SARData.AnswerText);
                            Chartdata.Add(SARData.count);

                        }
                        ResultSQs.ChartData.datasets.Add(new datasetsClass() { label = "One", fillColor = "rgba(151,187,205,0.5)", highlightFill = "rgba(151,187,205,0.8)", highlightStroke = "rgba(151,187,205,0.75)", strokeColor = "rgba(151,187,205,1)", data = Chartdata });

                    }
                    else
                    {
                        ResultSQs.SurveyNote = new List<NoteClass>();
                        var SurveyNoteList = from SR in SurveyContextObj.DbSurveyResult.Where(p => p.SurveyId == SurveyId && p.QuestionId == SurveyQuestObj.QuestionId)
                                             select new NoteClass()
                                             {
                                                 CustomerName = SR.NavCustomerMaster.CustomerName,
                                                 CustomerNote = SR.SurveyReply
                                             };


                        ResultSQs.SurveyNote.AddRange(SurveyNoteList);

                    }
                    ResultClassObj.ResultSQs.Add(ResultSQs);
                }

            }
            return Json(ResultClassObj, JsonRequestBehavior.AllowGet);
        }


    }
}