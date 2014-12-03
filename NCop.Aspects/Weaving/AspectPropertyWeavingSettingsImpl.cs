﻿using NCop.Weaving;

namespace NCop.Aspects.Weaving
{
    public class AspectPropertyWeavingSettingsImpl : IAspectPropertyWeavingSettings
    {
        public IAspectRepository AspectRepository { get; set; }
        public IAspectArgsMapper AspectArgsMapper { get; set; }
        public IPropertyWeavingSettings WeavingSettings { get; set; }
        public ILocalBuilderRepository LocalBuilderRepository { get; set; }
        public IByRefArgumentsStoreWeaver ByRefArgumentsStoreWeaver { get; set; }
    }
}
