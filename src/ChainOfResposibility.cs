using System.Reflection.Metadata;

namespace MyNetCoreWebAPI.ChainOfResponsibility
{
    public class ChainOfResposibility
    {
        private static void Main(string[] args)
        {
            Handler handler = new ValidateUserNameHandler().
                setNextHandler(new ValidatePasswordHandler()).
                setNextHandler(new AuthorizationHandler());

            AuthenticationService authenticationService = new AuthenticationService(handler);
            authenticationService.login("Pankaj", "abc@123");
            Console.ReadKey();

        }
    }

    public class AuthenticationService
    {
        private Handler _handler;
        public AuthenticationService(Handler handler)
        {
            _handler = handler;
        }

        public bool login(string username, string password)
        {
            if (_handler.handle(username, password))
            {
                Console.WriteLine("Authenication succesful");
                return true;
            }
            Console.WriteLine("Authenication failed !!");
            return false;
        }
    }


    public abstract class Handler
    {
        private Handler _next;

        public Handler setNextHandler(Handler next)
        {
            _next = next;
            return _next;
        }
        public abstract bool handle(string userName, string password);

        public bool callNexthandle(string userName, string password)
        {
            if (_next == null)
                return true;
            return _next.handle(userName, password);
        }
    }

    public class ValidateUserNameHandler : Handler
    {
        private Dictionary<string, string> userDB = new Dictionary<string, string>();
        public ValidateUserNameHandler()
        {
            userDB.Add("Pankaj", "abc@123");
            userDB.Add("Riya", "abc@123");

        }
        public override bool handle(string userName, string password)
        {
            if (userDB.Keys.Contains(userName))
                return callNexthandle(userName, password);
            return false;
        }
    }

    public class ValidatePasswordHandler : Handler
    {
        private Dictionary<string, string> userDB = new Dictionary<string, string>();
        public ValidatePasswordHandler()
        {
            userDB.Add("Pankaj", "abc@123");
            userDB.Add("Riya", "abc@123");
        }
        public override bool handle(string userName, string password)
        {
            if (userDB[userName].Equals(password))
                return callNexthandle(userName, password);
            return false;

        }
    }

    public class AuthorizationHandler : Handler
    {
        private Dictionary<string, string> userRoleDB = new Dictionary<string, string>();
        public AuthorizationHandler()
        {
            userRoleDB.Add("Riya", "admin,superuser");
            userRoleDB.Add("Pankaj", "admin");
        }
        public override bool handle(string userName, string password)
        {
            if (userRoleDB.Keys.Contains(userName))
                return callNexthandle(userName, password);
            return false;
        }
    }

}
