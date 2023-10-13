namespace BancaApi.Models.DTO
{
    public class LoginRequest
    {
        
        public required string UserName { get; set; }
       
        public required string PinPwd { get; set;}
      
    }
}
