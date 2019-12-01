using Autofac;
using System.Reflection;

namespace OouiSignalRSample.IOC
{
    public class AppSetup
    {
        public IContainer CreateContainer()
        {
            var containerBuilder = new ContainerBuilder();
            RegisterDependencies(containerBuilder);
            return containerBuilder.Build();
        }

        protected virtual void RegisterDependencies(ContainerBuilder cb)
        {
            #region ViewModels
            cb.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(x => x.Name.EndsWith("ViewModel"));
            #endregion

            #region Services
            //cb.RegisterType<StoreService>().As<IStoreService>().SingleInstance();             
            #endregion
        }
    }

    public static class AppContainer
    {
        public static IContainer Container { get; set; }
    }
}