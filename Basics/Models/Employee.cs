namespace Basics.Models
{
    public class Employee{
        public int Id {get;set;}
        public String Name {get;set;} = String.Empty;
        public String Surname {get;set;} = String.Empty;
        public String FullName=> $"{Name} {Surname.ToUpper()}";
        public int Age{get;set;}
    }
}