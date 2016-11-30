namespace <%= name %>.Repository {
    public class Program {

        // EFCore won't run as library. See: https://github.com/aspnet/EntityFramework/issues/5320
        // Since we’re pretending this class library is an application, we need to add a static void Main()
        public static void Main(string[] args) {
        }
    }
}