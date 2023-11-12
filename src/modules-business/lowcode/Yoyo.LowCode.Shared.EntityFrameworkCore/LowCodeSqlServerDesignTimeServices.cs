// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Yoyo.LowCode
{
    public class LowCodeSqlServerDesignTimeServices : IDesignTimeServices
    {
        public virtual void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAnnotationCodeGenerator, LowCodeSqlServerAnnotationCodeGenerator>();
        }
    }

    public class LowCodeSqlServerAnnotationCodeGenerator : SqlServerAnnotationCodeGenerator
    {
        public LowCodeSqlServerAnnotationCodeGenerator([NotNull] AnnotationCodeGeneratorDependencies dependencies)
            : base(dependencies)
        {
        }

#if NETSTANDARD2_1
        public override IReadOnlyList<MethodCallCodeFragment> GenerateFluentApiCalls(IModel model, IDictionary<string, IAnnotation> annotations)
        {
            //return base.GenerateFluentApiCalls(model, annotations);

            var res = base.GenerateFluentApiCalls(model, annotations).ToList();

            var identitys = res
                .Where(o => o.Method == "UseIdentityColumns" || o.Method == "UseIdentityColumn")
                .ToList();

            foreach (var item in identitys)
            {
                var index = res.FindIndex(o => o == item);
                if (item.ChainedCall == null)
                {
                    res[index] = new MethodCallCodeFragment($"{item.Method}SqlServer", item.Arguments?.ToArray());
                }
                else
                {
                    res[index] = new MethodCallCodeFragment($"{item.Method}SqlServer", item.Arguments?.ToArray(), item.ChainedCall);
                }
            }

            return res;
        }

        public override IReadOnlyList<MethodCallCodeFragment> GenerateFluentApiCalls(IProperty property, IDictionary<string, IAnnotation> annotations)
        {
            var res = base.GenerateFluentApiCalls(property, annotations).ToList();

            var identitys = res
                .Where(o => o.Method == "UseIdentityColumns" || o.Method == "UseIdentityColumn")
                .ToList();

            foreach (var item in identitys)
            {
                var index = res.FindIndex(o => o == item);
                if (item.ChainedCall == null)
                {
                    res[index] = new MethodCallCodeFragment($"{item.Method}SqlServer", item.Arguments?.ToArray());
                }
                else
                {
                    res[index] = new MethodCallCodeFragment($"{item.Method}SqlServer", item.Arguments?.ToArray(), item.ChainedCall);
                }
            }

            return res;
        }
#elif NET5_0

        public override IReadOnlyList<MethodCallCodeFragment> GenerateFluentApiCalls(IModel model, IDictionary<string, IAnnotation> annotations)
        {
            //return base.GenerateFluentApiCalls(model, annotations);

            var res = base.GenerateFluentApiCalls(model, annotations).ToList();

            var identitys = res
                .Where(o => o.Method == "UseIdentityColumns" || o.Method == "UseIdentityColumn")
                .ToList();

            foreach (var item in identitys)
            {
                var index = res.FindIndex(o => o == item);
                if (item.ChainedCall == null)
                {
                    res[index] = new MethodCallCodeFragment($"{item.Method}SqlServer", item.Arguments?.ToArray());
                }
                else
                {
                    res[index] = new MethodCallCodeFragment($"{item.Method}SqlServer", item.Arguments?.ToArray(), item.ChainedCall);
                }
            }

            return res;
        }

        public override IReadOnlyList<MethodCallCodeFragment> GenerateFluentApiCalls(IProperty property, IDictionary<string, IAnnotation> annotations)
        {
            var res = base.GenerateFluentApiCalls(property, annotations).ToList();

            var identitys = res
                .Where(o => o.Method == "UseIdentityColumns" || o.Method == "UseIdentityColumn")
                .ToList();

            foreach (var item in identitys)
            {
                var index = res.FindIndex(o => o == item);
                if (item.ChainedCall == null)
                {
                    res[index] = new MethodCallCodeFragment($"{item.Method}SqlServer", item.Arguments?.ToArray());
                }
                else
                {
                    res[index] = new MethodCallCodeFragment($"{item.Method}SqlServer", item.Arguments?.ToArray(), item.ChainedCall);
                }
            }

            return res;
        }
#endif
    }
}
