using System;
namespace deepu_dating.API.Extension
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dob){
             var age=DateTime.Today.Year - dob.Year;

             if(dob > DateTime.Today.AddYears(-age) ){

                 age--;
             }
             return age;


        }
    }
}