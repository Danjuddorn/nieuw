﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MtsSurvey.Models
{
    public class UserLoginModel
    {
        [Required]
        [Display(Name = "Email")]
        public string UserEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public int Password { get; set; }

       
        public bool IsValidCustomer(string _UserEmail, int _password, ref int _CustomerId)
        {
            SurveyContext SurveyContextObj = new SurveyContext();

            CustomerMaster CustomerMasteObj = SurveyContextObj.DbCustomerMaster.Where(p => p.Email == _UserEmail && p.passcode == _password).FirstOrDefault();

            if (CustomerMasteObj == default(CustomerMaster))
            {
                return false;
            }
            else
            {
                _CustomerId = CustomerMasteObj.CustomerId;
                 return true;
            }
        }
       
        public bool IsValidUser(string _UserEmail, int _password, ref int _UserId)
        {
            SurveyContext SurveyContextObj = new SurveyContext();

            AdminLogin AdminLoginObj = SurveyContextObj.DbAdminLogin.Where(p => p.Email == _UserEmail && p.passcode == _password).FirstOrDefault();

            if (AdminLoginObj == default(AdminLogin))
            {
                return false;
            }
            else
            {
                _UserId = AdminLoginObj.AdminLoginId;
                return true;
            }
        }

    }
}