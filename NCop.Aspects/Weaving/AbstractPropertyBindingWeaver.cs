﻿using NCop.Aspects.Extensions;
using NCop.Core.Extensions;
using NCop.Weaving;
using NCop.Weaving.Extensions;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using MA = System.Reflection.MethodAttributes;

namespace NCop.Aspects.Weaving
{
    internal abstract class AbstractPropertyBindingWeaver : IPropertyBindingWeaver, IBindingWeaver, IBindingTypeReflector
    {
        private Type baseType = null;
        protected PropertyInfo property = null;
        protected static int bindingCounter = 0;
        protected TypeBuilder typeBuilder = null;
        protected FieldBuilder fieldBuilder = null;
        protected readonly BindingSettings bindingSettings = null;
        protected readonly IMethodScopeWeaver getMethodScopeWeaver = null;
        protected readonly IMethodScopeWeaver setMethodScopeWeaver = null;
        protected readonly IAspectWeavingSettings aspectWeavingSettings = null;
        protected readonly MethodAttributes methodAttrs = MA.Public | MA.HideBySig | MA.Virtual;
        protected readonly CallingConventions callingConventions = CallingConventions.Standard | CallingConventions.HasThis;
        protected readonly FieldAttributes singletonFieldAttributes = FieldAttributes.Private | FieldAttributes.FamANDAssem | FieldAttributes.Static;
        protected readonly MethodAttributes ctorAttributes = MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;

        internal AbstractPropertyBindingWeaver(PropertyInfo property, BindingSettings bindingSettings, IAspectWeavingSettings aspectWeavingSettings, IMethodScopeWeaver getMethodScopeWeaver = null, IMethodScopeWeaver setMethodScopeWeaver = null) {
            this.property = property;
            this.bindingSettings = bindingSettings;
            this.getMethodScopeWeaver = getMethodScopeWeaver;
            this.setMethodScopeWeaver = setMethodScopeWeaver;
            this.aspectWeavingSettings = aspectWeavingSettings;
        }

        public FieldInfo WeavedType { get; set; }

        public FieldInfo Weave() {
            WeaveTypeBuilder();
            WeaveConstructors();

            if (WeaveGetMethod) {
                WeaveGetValueMethod();
            }

            if (WeaveSetMethod) {
                WeaveSetValueMethod();
            }

            return WeavedType = typeBuilder.CreateType()
                                           .GetField(fieldBuilder.Name, BindingFlags.NonPublic | BindingFlags.Static);
        }

        protected void WeaveTypeBuilder() {
            var declaringType = property.DeclaringType;
            var types = new[] { declaringType, property.PropertyType };

            baseType = typeof(AbstractPropertyBinding<,>).MakeGenericType(types);
            typeBuilder = baseType.DefineType("PropertyBinding_{0}".Fmt(Interlocked.Increment(ref bindingCounter)).ToUniqueName(), attributes: TypeAttributes.Public | TypeAttributes.Sealed);
        }

        protected virtual void WeaveConstructors() {
            var cctor = typeBuilder.DefineConstructor(ctorAttributes | MethodAttributes.Static, CallingConventions.Standard, Type.EmptyTypes);
            var cctorILGenerator = cctor.GetILGenerator();
            var defaultCtor = typeBuilder.DefineConstructor(ctorAttributes, CallingConventions.Standard | CallingConventions.HasThis, Type.EmptyTypes);
            var bindingTypeCtor = baseType.GetConstructor(Type.EmptyTypes);
            var defaultCtorGenerator = defaultCtor.GetILGenerator();

            fieldBuilder = typeBuilder.DefineField("singleton", typeBuilder, singletonFieldAttributes);

            defaultCtorGenerator.EmitLoadArg(0);
            defaultCtorGenerator.Emit(OpCodes.Call, bindingTypeCtor);
            defaultCtorGenerator.Emit(OpCodes.Ret);

            cctorILGenerator.Emit(OpCodes.Newobj, defaultCtor);
            cctorILGenerator.Emit(OpCodes.Stsfld, fieldBuilder);
            cctorILGenerator.Emit(OpCodes.Ret);
        }

        protected virtual MethodParameters ResolveParameterTypes(bool set = false) {
            Type[] parameters = null;
            var methodParameters = bindingSettings.ToBindingMethodParameters();

            if (!set) {
                return methodParameters;
            }

            parameters = new Type[methodParameters.Parameters.Length + 1];
            methodParameters.Parameters.CopyTo(parameters, 0);
            parameters[methodParameters.Parameters.Length] = property.PropertyType;
            methodParameters.Parameters = parameters;

            return methodParameters;
        }

        protected virtual void WeaveGetValueMethod() {
            ILGenerator ilGenerator = null;
            MethodBuilder methodBuilder = null;
            var methodParameters = ResolveParameterTypes();

            methodBuilder = typeBuilder.DefineMethod("GetValue", methodAttrs, callingConventions, methodParameters.ReturnType, methodParameters.Parameters);
            ilGenerator = methodBuilder.GetILGenerator();
            getMethodScopeWeaver.Weave(ilGenerator);
            ilGenerator.Emit(OpCodes.Ret);
        }

        protected virtual void WeaveSetValueMethod() {
            ILGenerator ilGenerator = null;
            MethodBuilder methodBuilder = null;
            var methodParameters = ResolveParameterTypes(true);

            methodBuilder = typeBuilder.DefineMethod("SetValue", methodAttrs, callingConventions, typeof(void), methodParameters.Parameters);
            ilGenerator = methodBuilder.GetILGenerator();
            setMethodScopeWeaver.Weave(ilGenerator);
            ilGenerator.Emit(OpCodes.Ret);
        }

        public virtual bool WeaveGetMethod {
            get {
                return false;
            }
        }

        public virtual bool WeaveSetMethod {
            get {
                return false;
            }
        }
    }
}
