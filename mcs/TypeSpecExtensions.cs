using System.Collections.Generic;
using System.Linq;
using Mono.CSharp;

namespace Marvin
{
    internal static class InterfaceExtensions
    {
        public static bool MethodExistInContract(this Interface contract, string methodName, Arguments arguments, ResolveContext ec)
        {
            if (contract == null)
                return false;

            var argumentTypes = arguments == null ? null : new TypeSpec[arguments.Count];
            if (arguments != null)
            {
                for (var i = 0; i < arguments.Count; i++)
                {
                    var argument = arguments[i];
                    if (argument.Type == null)
                        argument.Resolve(ec);
                    argumentTypes[i] = argument.Type;
                }
            }
            var potentialMethods = contract.Methods.OfType<Method>().Where
                (m =>
                 m.Name == methodName && m.ParameterTypes.Length == arguments.Count);
            var pm = (from potentialMethod in potentialMethods
                      where potentialMethod.ParameterTypes.AreAllAssignableFrom(argumentTypes)
                      select potentialMethod).SingleOrDefault();

            return pm != null;
        }
    }

    internal static class TypeSpecExtensions
    {
        private static readonly TypeSpecComparer Comparer = new TypeSpecComparer();
        internal class TypeSpecComparer : IEqualityComparer<TypeSpec>
        {
            public int Compare(TypeSpec x, TypeSpec y)
            {
                return x.Name.CompareTo(y.Name);
            }

            public bool Equals(TypeSpec x, TypeSpec y){
                if (x == null && y == null)
                    return true;
                if (x == null || y == null)
                    return false;
                return x.Name == y.Name;
            }

            public int GetHashCode(TypeSpec obj){
                return obj.GetHashCode();
            }
        }
        public static bool MethodExists(this IKVM.Reflection.Type self, string name, IKVM.Reflection.MethodSignature signature)
        {
            foreach (var method in self.__GetDeclaredMethods())
            {
                if (method.Name == name && method.MethodSignature.Equals(signature))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool FullfillsContract(this TypeSpec self,Interface contract){
            if(self.IsDynamic())
                return true;
            if(contract == null)
                return true;
            

            foreach(var method in contract.Methods.OfType<Method>()){
                var t = self.GetMetaInfo();
                var info = (IKVM.Reflection.MethodInfo) method.Spec.GetMetaInfo();
                if(!t.MethodExists(method.Name, info.MethodSignature))
                    return false;
            }
            return true;
        }

        public static bool AreAllAssignableFrom(this TypeSpec[] self, TypeSpec[] other){
            var noArguments =
                (other == null || other.Length == 0) &&
                (self == null || self.Length == 0);
            var argumentsMatch = (other != null) && (self != null) &&
                                 self.Select
                                     ((t, i) =>
                                      System.Tuple.Create(other[i], t)).All
                                     (t => t.Item2.IsAssignableFrom(t.Item1));
            return noArguments || argumentsMatch;
        }

        public static bool IsDynamic(this TypeSpec self){
            return self.BuiltinType == BuiltinTypeSpec.Type.Dynamic;
        }

        private static int GetSimpleTypeRank(TypeSpec t){
            switch (t.BuiltinType)
            {
                case BuiltinTypeSpec.Type.Double:
                    return 0;
                case BuiltinTypeSpec.Type.Float:
                    return 1;
                case BuiltinTypeSpec.Type.ULong:
                    return 2;
                case BuiltinTypeSpec.Type.Long:
                    return 3;
                case BuiltinTypeSpec.Type.UInt:
                    return 4;
                case BuiltinTypeSpec.Type.Int:
                    return 5;
                case BuiltinTypeSpec.Type.UShort:
                    return 6;
                case BuiltinTypeSpec.Type.Short:
                    return 7;
                case BuiltinTypeSpec.Type.SByte:
                    return 8;
                case BuiltinTypeSpec.Type.Byte:
                    return 9;
                default:
                    return int.MaxValue;
            }
        }

        public static bool IsAssignableFrom(this TypeSpec self, TypeSpec other){
            if (self.IsDynamic() || other.IsDynamic())
                return true;
            if (Comparer.Equals(self,other))
                return true;
            if (self.IsBaseOf(other))
                return true;
            if(self.IsInterfaceOf(other))
                return true;
            

            var selfRank = GetSimpleTypeRank(self);
            if (selfRank == int.MaxValue)
                return false;
            var otherRank = GetSimpleTypeRank(other);
            if (otherRank == int.MaxValue)
                return false;
            return selfRank <= otherRank;
        }

        public static bool IsBaseOf(this TypeSpec self, TypeSpec other)
        {
            if (other.BaseType == null)
                return false;
            if (Comparer.Equals(self, other.BaseType))
                return true;
            return self.IsBaseOf(other.BaseType);
        }

        public static bool IsInterfaceOf(this TypeSpec self, TypeSpec other)
        {
            if (other.Interfaces == null || other.Interfaces.Count == 0)
                return false;
            return other.Interfaces.Any(i => Comparer.Equals(self, i) || self.IsBaseOf(i));
        }

       
    }
}
