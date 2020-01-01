using System;
using console.com;
using hr.core.domain.Employees;
using hr.core.helper;

namespace console
{
    class Program
    {
        static void Main(string[] args)
        {
            var h = new TestBaseHandler();
            var h2 = new TestBaseHandler2();
        
            EventBroker.getInstance().Emit(new TestEventA("Testing"));
            EventBroker.getInstance().Emit(new TestEventB(12));
            Console.WriteLine(h2.RegisteredHandlers.Count);
        }
    }
}
