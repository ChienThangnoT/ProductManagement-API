using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SE161774.ProductManagement.Api.Helpers
{
    public class KebabCaseNamingStrategy : NamingStrategy
    {
        protected override string ResolvePropertyName(string name)
        {
            return ConvertToKebabCase(name);
        }

        private string ConvertToKebabCase(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            return Regex.Replace(name, "(?<!^)([A-Z])", "-$1").ToLower();
        }
    }

    public class KebabCaseContractResolver : DefaultContractResolver
    {
        public KebabCaseContractResolver()
        {
            NamingStrategy = new KebabCaseNamingStrategy();
        }
    }
}
