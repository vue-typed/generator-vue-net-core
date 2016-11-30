namespace VueNetCoreBoilerplate.Service.Users.Dto {
    public class SignUpDto {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}