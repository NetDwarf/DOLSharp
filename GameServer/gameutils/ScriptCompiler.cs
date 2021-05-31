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
using log4net;

using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DOL.GS
{
	public class ScriptCompiler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private CodeDomProvider compiler;
		private CompilerParameters compilerParameters;

		private ScriptCompiler(string outputPath, IEnumerable<string> dllNamesOfReferences)
		{
			#if DEBUG
			var includeDebugInformation = true;
			#else
			var includeDebugInformation = false;
			#endif
			compilerParameters = new CompilerParameters(dllNamesOfReferences.ToArray(), outputPath, includeDebugInformation)
			{
				GenerateExecutable = false,
				GenerateInMemory = false,
				WarningLevel = 2,
				CompilerOptions = string.Format("/optimize /lib:.{0}lib", Path.DirectorySeparatorChar),
			};
			compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
		}

		public bool HasErrors { get; private set; } = false;

		public static ScriptCompiler CreateForCSharp(string outputPath, IEnumerable<string> dllNamesOfReferences)
		{
			var scriptCompiler = new ScriptCompiler(outputPath, dllNamesOfReferences)
			{
				compiler = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v4.0" } })
			};
			return scriptCompiler;
		}

		public static ScriptCompiler CreateForVBSharp(string outputPath, IEnumerable<string> dllNamesOfReferences)
		{
			var scriptCompiler = new ScriptCompiler(outputPath, dllNamesOfReferences)
			{
				compiler = new VBCodeProvider()
			};
			return scriptCompiler;
		}

		public Assembly Compile(IEnumerable<FileInfo> sourceFiles)
		{
			var sourceFilePaths = sourceFiles.Select(file => file.FullName).ToArray();
			var compilerResults = compiler.CompileAssemblyFromFile(compilerParameters, sourceFilePaths);
			GC.Collect();
			if (compilerResults.Errors.HasErrors)
			{
				HasErrors = true;
				PrintErrors(compilerResults);
			}
			return compilerResults.CompiledAssembly;
		}

		private void PrintErrors(CompilerResults compilerResults)
		{
			foreach (CompilerError err in compilerResults.Errors)
			{
				if (err.IsWarning) continue;

				var errorMessage = $"   {err.FileName} Line:{err.Line} Col:{err.Column}";

				if (log.IsErrorEnabled)
				{
					log.Error("Script compilation failed because: ");
					log.Error(err.ErrorText);
					log.Error(errorMessage);
				}
			}
		}
	}
}
