using System;
using hr.core.helper;

namespace console.com {
    public class TestBaseHandler : BaseHandler
    {
        [TargetEvent(typeof(TestEventA))]
        public void handle(object sender, TestEventA args)
        {
           Console.WriteLine(args.Value);
        }

        [TargetEvent(typeof(TestEventB))]
        public void handle(object sender, TestEventB args)
        {
            Console.WriteLine(args.SomeNumber);
        }
    }
}