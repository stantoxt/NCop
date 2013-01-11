﻿using NCop.Aspects.Engine;
using NCop.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NCop.Aspects.Pointcuts
{
    public class AbstractPointcutAttribute : Attribute, IPointcutVisitor
    {
        private static readonly IEnumerable<IPointcut> Empty = Enumerable.Empty<IPointcut>();

        public virtual BindingFlags Flags {
            get {
                return BindingFlags.Instance | BindingFlags.Public;
            }
        }

        public IEnumerable<IPointcut> Visit(Type type) {
            return Visit(type.GetFields(Flags))
                     .SelfJoin(Visit(type.GetMethods(Flags)))
                        .SelfJoin(Visit(type.GetProperties(Flags)))
                            .SelfJoin(Visit(type.GetConstructors(Flags)));
        }

        public IPointcutCollection Match(Type type) {
            var pointcuts = Visit(type);

            return new PointcutCollection(pointcuts);
        }

        public virtual IEnumerable<IPointcut> Visit(FieldInfo[] fields) {
            return Empty;
        }

        public virtual IEnumerable<IPointcut> Visit(MethodInfo[] methods) {
            return Empty;
        }

        public virtual IEnumerable<IPointcut> Visit(ConstructorInfo[] ctors) {
            return Empty;
        }

        public virtual IEnumerable<IPointcut> Visit(PropertyInfo[] properties) {
            return Empty;
        }
    }
}