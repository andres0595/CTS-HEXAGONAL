using Autofac;
using Vitalea.Dominio.InterfacesInfrastructure;
using Vitalea.Dominio.Models;
using Vitalea.Infraestructura.Configurations;
using Vitalea.Infraestructura.GenericRepository;
using Vitalea.Infraestructura.Repositories;
using Vitalea.Infraestructura.Security;

namespace Vitalea.Infraestructura
{
    public class Injections : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TokenValidationHandler>().AsSelf().SingleInstance();
            builder.RegisterType<ContextDB>().AsSelf().SingleInstance();
            builder.RegisterType<QuerySqlViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<SqlController>().AsSelf().SingleInstance();
            builder.RegisterType<TokenGenerator>().As<ITokenInfrastructure>().SingleInstance();
            builder.RegisterType<LoginRepository>().As<ILoginInfrastructure>().SingleInstance();
            builder.RegisterType<UserRepository>().As<IUserInfrastructure>().SingleInstance();
            builder.RegisterType<EncryptRepository>().As<IEncryptInfrastructure>().SingleInstance();
        }
    }
}
