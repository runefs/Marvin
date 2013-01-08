using Marvin;

namespace Mono.CSharp{
    public class RoleAssign : SimpleAssign{
        public RoleAssign(Expression target, Expression source) : base(target, source) { }
        public RoleAssign(Expression target, Expression source, Location loc) : base(target, source, loc) { }
        protected override Expression DoResolve(ResolveContext ec){
            var e = base.DoResolve(ec);
            if (e == null || e != this)
                return e;

            var fld = target as FieldExpr;
            if (fld == null){
                var access = target as MemberAccess;
                if (access != null)
                    fld = access.LeftExpression as FieldExpr;

                if (fld == null){
                    Expression simple = target as SimpleName;
                    simple = simple != null ? simple.Resolve(ec) : null;
                    fld = simple as FieldExpr;
                }
                if (fld == null)
                    return e;
            }
            if (!fld.IsRole)
                return e;

            //assigning to a role. Check that the contract is fullfilled
            var contractName = CSharpParser.GetCurrentRoleContractName(fld.DeclaringType.Name, fld.Name);
            if (CSharpParser.RoleContracts.ContainsKey(contractName)){
                if (source.Type == null)
                    source = source.Resolve(ec);
                var contract = CSharpParser.RoleContracts[contractName];
                if (!source.Type.FullfillsContract(contract)){
                    var index = -1;
                    var current_class = contract.Parent;
                    if (current_class.IsGeneric)
                    {
                        var typeParameters = current_class.TypeParameters;
                        for (int i = 0; index < 0 && i < typeParameters.Length; i++)
                        {
                            var param = typeParameters[i];
                            if (target.Type.Name == param.Name)
                            {
                                index = i;
                            }
                        }
                    }
                    if (index < 0)
                    {
                        ec.Report.Error(10009, loc, "Assignment to role '{0}' does not full fill contract", fld.Name);
                    }
                    else
                    {
                        current_class.Spec.AddTypesAndContracts(index, contract);
                    }
                }
            }

            return this;
        }
    }
}
