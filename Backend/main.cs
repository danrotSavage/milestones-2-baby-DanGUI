using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class main
{
    public static void Main(string[] args)
    {
        Service service = new Service();
        service.LoadData();
        //service.DeleteData();
        //Console.WriteLine(service.DeleteData());
        //Console.WriteLine(service.Register("bardam1701@gmail.com", "123Aabc", "shmanmana"));
        //Console.WriteLine(service.Register("bardam1702@gmail.com", "123Aabc", "shmanmana"));
       //Console.WriteLine(service.Register("bardam1703@gmail.com", "123Aabc", "shmanmana"));
       // Console.WriteLine(service.Register("bardam1704@gmail.com", "123Aabc", "shmanmana"));
       // Console.WriteLine(service.Login("bardam1702@gmail.com", "123Aabc").ErrorMessage);
       // Console.WriteLine(service.Login("bardam1702@gmail.com", "123Aabc").ErrorMessage);
       // Console.WriteLine(service.Login("bardam1703@gmail.com", "123Aabc").ErrorMessage);

        //Console.WriteLine(service.Logout("bardam1702@gmail.com").ErrorMessage);
       // Console.WriteLine(service.Login("bardam1705@gmail.com", "123Aabc").ErrorMessage);

        Console.WriteLine(service.Login("bardam1702@gmail.com", "123Aabc").ErrorMessage);
        // Console.WriteLine(service.Logout("bardam1703@gmail.com").ErrorMessage);
        //Console.WriteLine(service.LimitColumnTasks("bardam1702@gmail.com", 1,9).ErrorMessage);

        Console.WriteLine(service.AddTask("bardam1702@gmail.com", "newtitle8", "**", new DateTime(2025, 11, 3)).ErrorMessage);
        Console.WriteLine(service.AddTask("bardam1702@gmail.com", "newtitle2", "**", new DateTime(2025, 11, 3)).ErrorMessage);
        Console.WriteLine(service.GetColumn("bardam1702@gmail.com", "backlog").ErrorMessage);
        Console.WriteLine(service.Logout("bardam1702@gmail.com").ErrorMessage);
        
        Console.ReadKey();
    }

}
