using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task7SQSLambda.Helpers
{
    public static class Constants
    {
        public static string FileSuccessfullyProcessed()
        {
            return "File successfully processed";
        }
        public static string InvalidFilename()
        {
            return "Invalid filename";
        }
    }
}
