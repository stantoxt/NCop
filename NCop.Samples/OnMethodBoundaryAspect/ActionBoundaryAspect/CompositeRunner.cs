﻿using NCop.Composite.Framework;
using NCop.Composite.Runtime;

namespace NCop.Samples.OnMethodBoundaryAspect.ActionBoundaryAspect
{
    public static class CompositeRunner
    {
        public static void Run() {
            IDeveloper developer = null;
            var container = new CompositeContainer(new CompositeRuntimeSettings {
                Types = new[] { typeof(IDeveloper) }
            });

            container.Configure();
            developer = container.Resolve<IDeveloper>();
            developer.Code();
        }
    }
}
