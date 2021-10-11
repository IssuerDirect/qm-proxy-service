using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net3000
{
    public static class standardMessages
    {
        public static apiResponse saved {
            get {
                return new apiResponse() { title = "Saved", icon = "fa-check-circle", message = "Your changes have been saved", cssClass = "success" };
            }
        }
        public static apiResponse invalid
        {
            get
            {
                return new apiResponse() { code = 400, title = "Invalid", icon = "fa-times-circle", message = "This request in invalid", cssClass = "danger", success = false };
            }
        }

        public static apiResponse weakPassword
        {
            get
            {
                return new apiResponse() { code = 400, title = "Weak Password", icon = "fa-times-circle", message = "Password must be at least 8 characters and includes characters and numbers", cssClass = "danger", success = false };
            }
        }

        public static apiResponse passwordNotMatched
        {
            get
            {
                return new apiResponse() { code = 400, title = "Password Not Matched", icon = "fa-times-circle", message = "Password and password confirm are not the same", cssClass = "danger", success = false };
            }
        }

        public static apiResponse loggedin
        {
            get
            {
                return new apiResponse() { title = "Logged In", icon = "fa-check-circle", message = "You have successfully logged in", cssClass = "success" };
            }
        }
        public static apiResponse invalidLogin
        {
            get
            {
                return new apiResponse() { code = 401, title = "Invalid Login Credentials", icon = "fa-times-circle", message = "The credentials you used are invalid", cssClass = "danger", count = 0, success = false };
            }
        }
        public static apiResponse unauthorized
        {
            get
            {
                return new apiResponse() { code = 403, title = "Unauthorized", icon = "fa-times-circle", message = "Login is required to access the resource you requested", cssClass = "danger", count = 0, success = false };
            }
        }
        public static apiResponse notFound
        {
            get
            {
                return new apiResponse() { code = 404, title = "Not Found", icon = "fa-times-circle", message = "The record you were trying to reach was not found", cssClass = "danger", count = 0, success = false };
            }
        }
        public static apiResponse found
        {
            get
            {
                return new apiResponse() { code = 302, title = "Found", icon = "fa-check-circle", message = "Found the record you requested", cssClass = "success" };
            }
        }
        public static apiResponse notActive
        {
            get
            {
                return new apiResponse() { code = 401, title = "Not Active", icon = "fa-times-circle", message = "The account you were trying to access is not active", cssClass = "danger", success = false };
            }
        }
        public static apiResponse accountNotFound {
            get
            {
                return new apiResponse() { code = 404, title = "Account Not Found", icon = "fa-times-circle", message = "I can't find an account for this website", cssClass = "danger", success = false };
            }
        }

        public static apiResponse deleted
        {
            get
            {
                return new apiResponse() { title = "Deleted", icon = "fa-check-circle", message = "Your selection has been deleted", cssClass = "success" };
            }
        }
    }

    public class apiResponse
    {
        public bool success { get; set; } = true;
        public string title { get; set; }
        public string message { get; set; }       
        public string icon { get; set; } = "fa-check-circle";
        public string cssClass { get; set; } = "success";
        public int code { get; set; } = 200;
        public dynamic data { get; set; }
        public int count { get; set; } = 1;
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string html
        {
            get {
                return $"<div class='alert alert-{cssClass} alert - dismissible fade show' role='alert'><i class='fa {icon} mr-2'></i><strong>{title}</strong>. {message}.<button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button></div>";
            }
        }
    }
}
