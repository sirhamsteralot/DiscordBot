﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace DiscordBot.Scripting
{
    public static class ScriptManager
    {
        static readonly Dictionary<string, AssemblyName> AssemblyNames = new Dictionary<string, AssemblyName>();
        public static ScriptOptions op = ScriptOptions.Default
                                  .WithReferences("System.Runtime, Version = 4.0.0.0, Culture = neutral, PublicKeyToken = b03f5f7f11d50a3a")
                                  .WithReferences(typeof(List<>).Assembly, typeof(Enumerable).Assembly, typeof(string).Assembly, typeof(ScriptManager).Assembly, typeof(StringBuilder).Assembly)
                                  .WithImports("System", "System.Collections.Generic", "System.Timers", "System.Linq", "System.Text", "DiscordBot");

        public static async Task<object> ExecuteScript(string code)
        {
            var res = await CSharpScript.EvaluateAsync<object>(code, op);
            GC.Collect();

            return res?.ToString();
        }

        private static IEnumerable<Assembly> SelectAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location));
        }
    }
}
