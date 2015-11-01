using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using RemoteBotService;
using Ninject.Extensions.Conventions;

namespace XnaRemoteBot
{
    public class NinjectBinder : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            //Kernel.Bind(x => x.FromAssemblyContaining<IBatteryControlService>() // 1
            //    .SelectAllClasses() // Retrieve all non-abstract classes
            //    .Excluding<NetworkService>()
            //    .Excluding<FontService>()
            //    .BindAllInterfaces() // Binds the default interface to them;
            //    );
            // Battery control deactivated for now
            Kernel.Bind<IRoverElement, IBatteryControlService>().To<BatteryControlService>().InSingletonScope();
            Kernel.Bind<IRoverElement, ICompassService, INetworkListener>().To<CompassService>().InSingletonScope();
            Kernel.Bind<IRoverElement, IFontService>().To<FontService>().InSingletonScope();
            Kernel.Bind<IRoverElement, IHeadlightService>().To<HeadlightService>().InSingletonScope();
            Kernel.Bind<IRoverElement, IMjpegService>().To<MjpegService>().InSingletonScope();
            Kernel.Bind<IRoverElement, IAimService>().To<AimService>().InSingletonScope();
            Kernel.Bind<IRoverElement, IMotorService>().To<MotorService>().InSingletonScope();
            Kernel.Bind<IRoverElement, ICameraService>().To<CameraService>().InSingletonScope();
            Kernel.Bind<IRoverElement, INetworkService>().To<NetworkService>().InSingletonScope();
            Kernel.Bind<IControllerService>().To<ControllerService>().InSingletonScope();
        }
    }
}
