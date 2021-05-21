/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */
#if NETFRAMEWORK
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.CSharp;
using Microsoft.VisualBasic;
using log4net;

namespace DOL.GS
{
    public class DOLScriptCompiler : IScriptCompiler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

#if DEBUG
        bool includeDebugInformation = true;
#else
		bool includeDebugInformation = false;
#endif
        private CodeDomProvider compiler;
        private CompilerErrorCollection lastCompilationErrors;
        private List<string> referencedAssemblies = new List<string>();

        public DOLScriptCompiler()
        {
            compiler = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v4.0" } });
            referencedAssemblies.AddRange(GameServer.Instance.Configuration.ScriptAssemblies);
            referencedAssemblies.Add("System.Core.dll");
        }

        public bool HasErrors => lastCompilationErrors.HasErrors;

        private CompilerParameters DefaultParameters => new CompilerParameters(referencedAssemblies.ToArray())
        {
            IncludeDebugInformation = includeDebugInformation,
            GenerateExecutable = false,
            GenerateInMemory = false,
            WarningLevel = 2,
            CompilerOptions = string.Format($"/optimize /lib:{Path.Combine(".", "lib")}")
        };

        public void SetToVBSharp()
        {
            compiler = new VBCodeProvider();
        }

        public Assembly Compile(string outputPath, IEnumerable<FileInfo> sourceFiles)
        {
            var sourceFilePaths = sourceFiles.Select(file => file.FullName).ToArray();
            var compilerParameters = DefaultParameters;
            compilerParameters.OutputAssembly = outputPath;

            var compilerResults = compiler.CompileAssemblyFromFile(compilerParameters, sourceFilePaths);
            lastCompilationErrors = compilerResults.Errors;
            GC.Collect();
            return compilerResults.CompiledAssembly;
        }

        public Assembly CompileFromSource(string code)
        {
            var compilerParameters = DefaultParameters;
            compilerParameters.GenerateInMemory = true;

            var compilerResults = compiler.CompileAssemblyFromSource(compilerParameters, code);
            lastCompilationErrors = compilerResults.Errors;
            if (HasErrors) return null;
            return compilerResults.CompiledAssembly;
        }

        public IEnumerable<string> GetDetailedErrorMessages()
        {
            var errorMessages = new List<string>();
            foreach (CompilerError error in lastCompilationErrors)
            {
                if (error.IsWarning) continue;

                var errorMessage = $"   {error.FileName} Line:{error.Line} Col:{error.Column}";
                if (log.IsErrorEnabled)
                {
                    errorMessage = $"Script compilation failed because: \n{error.ErrorText}\n" + errorMessage;
                }
                errorMessages.Add(errorMessage);
            }
            return errorMessages;
        }

        public IEnumerable<string> GetErrorMessages()
        {
            var errorMessages = new List<string>();
            foreach (CompilerError error in lastCompilationErrors)
            {
                errorMessages.Add(error.ErrorText);
            }
            return errorMessages;
        }
    }
}
#endif
