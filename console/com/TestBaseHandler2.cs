using System;
using hr.core.helper;

namespace console.com {
    public class TestBaseHandler2 : BaseHandler
    {
        [TargetEvent(typeof(TestEventB))]
        public void handle(object sender, TestEventB args)
        {
            Console.WriteLine(args.SomeNumber * 2);
        }
    }
}