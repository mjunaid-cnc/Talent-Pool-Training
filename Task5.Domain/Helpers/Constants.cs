using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task5.Domain.Helpers
{
    public static class Constants
    {
        public static string IncorrectPassword()
        {
            return "Incorrect password";
        }
        public static string UserDoesNotExist() {
            return "User does not exist";
        }
        public static string UserAlreadyExists()
        {
            return "User already exists";
        }
        public static string SomethingWentWrong()
        {
            return "Something went wrong";
        }
        public static string UnauthorizedRequest()
        {
            return "Unauthorized request";
        }
        public static string PleaseSelectAFileToUpload()
        {
            return "Please select a file to upload";
        }
        public static string InvalidFileType()
        {
            return "Invalid file type";
        }
    }
}
