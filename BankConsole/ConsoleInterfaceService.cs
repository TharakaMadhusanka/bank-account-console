namespace BankConsole
{
    internal class ConsoleInterfaceService: IConsoleInterfaceService
    {
        public ConsoleInterfaceService() { }
        public void Start()
        {
            Console.WriteLine("Print Menu");
            Console.ReadKey();
        }
    }
}
