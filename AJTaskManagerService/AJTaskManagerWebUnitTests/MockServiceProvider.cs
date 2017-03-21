using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using WebApplication1.Services;

namespace AJTaskManagerWebUnitTests
{
    class MockServiceProvider : IServiceProvider
    {
        public string AccessToken
        {
            get; 
            set;
        }
        public UnityContainer Container;
        public MockServiceProvider()
        {
            Container = new UnityContainer();
        }

        public object GetService(Type typeOfService)
        {
            return Container.Resolve(typeOfService);
        }
    }
}
