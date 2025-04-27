namespace Foodtek.DTOs
{
    public class SignUpInputDTO :propshared
    {
   


      public string? fullname { get; set; }
      public string Username { get; set; }  
      public string? Email { get; set; }
      public string? PhoneNumber { get; set; }
      public string? password { get; set; }
      public string ProfileImage { get; set; }
      public DateTime? JoinDate { get; set; }
       //public int IsActive { get; set; }
       //public string? CreatedBy { get; set; }
       // public string? CreatedAt { get; set; }
       // public string? UpdatedBy { get; set; }
       // public string? UpdatedAt { get; set; }
        public DateTime? birthdate { get; set; }
        public int role { get; set; }




    }
}
