using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sky7.CSharpChips.Reflection {
    public struct AnnotatedMember<T, TA> where T : class {
        public TA Annotation { get; set; }
        public T Info { get; set; }
    }

    public struct AnnotatedType<TA> {
        public Type Type { get; set; }
        public TA Annotation { get; set; }
    }

    public static class AnnotatedMembersHelpers {
        public static IEnumerable<AnnotatedMember<MemberInfo, TA>> GetAnnotatedMembers<TA>(this Type type) where TA : Attribute {
            return type.GetMembers()
                .Select(
                    m => new AnnotatedMember<MemberInfo, TA>() {
                        Info = m,
                        Annotation = m.GetCustomAttribute<TA>()
                    }
                )
                .Where(am => am.Annotation != null);
        }

        public static IEnumerable<AnnotatedMember<PropertyInfo, TA>> GetAnnotatedProperties<TA>(this Type type) where TA : Attribute {
            return type.GetProperties()
                .Select(
                    m => new AnnotatedMember<PropertyInfo, TA>() {
                        Info = m,
                        Annotation = m.GetCustomAttribute<TA>()
                    }
                )
                .Where(am => am.Annotation != null);
        }

        public static IEnumerable<AnnotatedMember<MethodInfo, TA>> GetAnnotatedMethods<TA>(this Type type) where TA : Attribute {
            return type.GetMethods()
                .Select(
                    m => new AnnotatedMember<MethodInfo, TA>() {
                        Info = m,
                        Annotation = m.GetCustomAttribute<TA>()
                    }
                )
                .Where(am => am.Annotation != null);
        }

        public static IEnumerable<AnnotatedType<TA>> GetAnnotatedTypes<TA>(this Assembly assembly) where TA : Attribute {
            return assembly.GetTypes()
                .Select(
                    t => new AnnotatedType<TA>() {
                        Type = t,
                        Annotation = t.GetCustomAttribute<TA>()
                    }
                )
                .Where(at => at.Annotation != null);
        }
    }
}
