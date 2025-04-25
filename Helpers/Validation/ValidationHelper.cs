using Foodtek.DTOs;

namespace MyTasks.Helpers.Validations
{
    public static class ValidationHelper
    {
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 6)
                throw new Exception("Password is required and must be at least 6 characters.");

            return true;

        }
        public static bool IsValidName(string Name)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name) || Name.Length > 50)
                throw new Exception("Name Is Required And Should not be more than 50 character");
            foreach (char c in Name)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                    throw new Exception("Name Is Required To Contais of character and white spaces ");
            }
            return true;
        }
        public static bool IsValidEmail(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email))
                throw new Exception("EmailIs  Required");

            int atIndex = Email.IndexOf('@');
            int dotIndex = Email.LastIndexOf('.');

            if (atIndex < 1 || dotIndex < atIndex + 2 || dotIndex >= Email.Length - 2)
                throw new Exception("EmailIs  Required");

            string domain = Email.Substring(atIndex + 1, dotIndex - atIndex - 1);
            string extension = Email.Substring(dotIndex + 1);

            if (domain.Length < 2 || extension.Length < 2)
                throw new Exception("EmailIs  Required");

            foreach (char c in Email.Substring(0, atIndex))
            {
                if (!char.IsLetterOrDigit(c) && c != '.' && c != '_' && c != '%' && c != '+' && c != '-')
                    throw new Exception("EmailIs  Required");
            }
            return true;
        }
        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone) || phone.Length != 10)
                throw new Exception("Phone Is Required And Should be 10 digit");
           
            if (!phone.StartsWith("07"))
                throw new Exception("Phone number must start with '07'");

            foreach (char c in phone)
            {
                if (!char.IsDigit(c))
                    throw new Exception("Phone Is Required To Contais of digits ");
            }
            return true;
        }
        public static bool IsValidbirthdate(DateTime birthdate)
        {
            if (birthdate == default)
                throw new Exception("Birthdate is required");

            if (birthdate > DateTime.Now)
                throw new Exception("Invalid Birthdate ");

            var age = DateTime.Today.Year - birthdate.Year;
            if (birthdate.Date > DateTime.Today.AddYears(-age)) age--;

            if (age < 10 || age > 120)
                throw new Exception("Birthdate must indicate an age between 10 and 120 years");

            return true;

        }
    }
}
