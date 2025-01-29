using Autofac;
using Vitalea.Dominio.InterfaceService;
using Vitalea.Dominio.Service;

namespace Vitalea.Dominio
{
    public class Injections : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ServiceUser>().As<IUserService>().SingleInstance();
            builder.RegisterType<ServiceLogin>().As<ILoginService>().SingleInstance();
            builder.RegisterType<ServiceEmail>().As<IEmailService>().SingleInstance();
        }
    }
}
