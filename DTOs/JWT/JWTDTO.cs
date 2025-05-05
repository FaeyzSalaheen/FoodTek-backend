using System.Globalization;

namespace Foodtek.DTOs.JWT
{
    public class JWTDTO
    {

        public String fullname { get; set; }
        public String Username { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public String ProfileImage { get; set; }
        public String birthdate { get; set; }
        public int role { get; set; }
        public int IsActive { get; set; }
    }
}
