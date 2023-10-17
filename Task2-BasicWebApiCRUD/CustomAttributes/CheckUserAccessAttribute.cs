namespace Task2_BasicWebApiCRUD.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CheckUserAccessAttribute : Attribute
    {
    }
}
