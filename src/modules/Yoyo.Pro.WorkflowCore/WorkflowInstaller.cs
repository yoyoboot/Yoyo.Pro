using System.Reflection;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Yoyo.Pro.WorkflowCore.workflow;

namespace Yoyo.Pro.WorkflowCore
{
    internal class WorkflowInstaller : IWindsorInstaller
    {
        private IAbpWorkflowRegistry _serviceSelector;

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            _serviceSelector = container.Resolve<IAbpWorkflowRegistry>();
            container.Kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }

        private void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            /* This code checks if registering component implements any IEventHandler<TEventData> interface, if yes,
             * gets all event handler interfaces and registers type to Event Bus for each handling event.
             */
            if (!typeof(IAbpWorkflow).GetTypeInfo().IsAssignableFrom(handler.ComponentModel.Implementation))
            {
                return;
            }

            var interfaces = handler.ComponentModel.Implementation.GetTypeInfo().GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (!typeof(IAbpWorkflow).GetTypeInfo().IsAssignableFrom(@interface))
                {
                    continue;
                }
                _serviceSelector.RegisterWorkflow(handler.ComponentModel.Implementation);
            }
        }
    }
}
