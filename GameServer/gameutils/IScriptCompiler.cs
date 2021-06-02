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
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DOL.GS
{
    /// <summary>
    /// Compiles scripts at runtime. C# is the default source language, but can be set to VB#.
    /// </summary>
    public interface IScriptCompiler
    {
        /// <summary>
        /// Indicates whether last compilation had errors or not.
        /// </summary>
        bool HasErrors { get; }

        /// <summary>
        /// Compiles source code from files at runtime.
        /// </summary>
        /// <param name="outputPath">Output path for the compiled assembly.</param>
        /// <param name="sourceFiles">Files containing source code.</param>
        Assembly Compile(string outputPath, IEnumerable<FileInfo> sourceFiles);

        /// <summary>
        /// Compiles source code from string at runtime.
        /// </summary>
        /// <param name="code">Source code as string.</param>
        Assembly CompileFromSource(string code);

        /// <summary>
        /// Get error messages with line and column. Every string in the collection represents a separate error.
        /// </summary>
        IEnumerable<string> GetDetailedErrorMessages();

        /// <summary>
        /// Get default error messages representing ErrorText only. Every string in the collection represents a separate error.
        /// </summary>
        IEnumerable<string> GetErrorMessages();

        /// <summary>
        /// Set language to compile to Visual Basic .NET.
        /// </summary>
        void SetToVBSharp();
    }
}