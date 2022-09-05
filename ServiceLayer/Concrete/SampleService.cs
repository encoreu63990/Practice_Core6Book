using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Concrete
{
    public class SampleService : ISampleService
    {
        public bool IsMoreThan2(int value)
        {
            return value > 2;
        }
    }
}
