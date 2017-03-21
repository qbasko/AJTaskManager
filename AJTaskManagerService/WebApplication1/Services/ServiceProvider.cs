using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace WebApplication1.Services
{
    public class ServiceProvider :IServiceProvider
    {
        private readonly UnityContainer _container;
        public ServiceProvider()
        {
            _container = new UnityContainer();

            _container.RegisterInstance(typeof(IUserService), new UserService());
            _container.RegisterInstance(typeof(IGroupService), new GroupService());
            _container.RegisterInstance(typeof (IToDoListService), new ToDoListService());
            _container.RegisterInstance(typeof(ITaskSubitemService), new TaskSubitemService());
            _container.RegisterInstance(typeof (ITaskItemService), new TaskItemService());
        }

        public object GetService(Type type)
        {
            return _container.Resolve(type);
        }
    }
}
