// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Shouldly;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    public class Traits_Tests
    {
        [Fact]
        public void DebugEngine_RespondsToMSBuildDebugEngine()
        {
            var originalValue = Environment.GetEnvironmentVariable("MSBuildDebugEngine");
            var originalCapsValue = Environment.GetEnvironmentVariable("MSBUILDDEBUGENGINE");
            
            try
            {
                Environment.SetEnvironmentVariable("MSBuildDebugEngine", "1");
                Environment.SetEnvironmentVariable("MSBUILDDEBUGENGINE", null);
                
                var traits = new Traits();
                
                traits.DebugEngine.ShouldBeTrue();
            }
            finally
            {
                Environment.SetEnvironmentVariable("MSBuildDebugEngine", originalValue);
                Environment.SetEnvironmentVariable("MSBUILDDEBUGENGINE", originalCapsValue);
            }
        }

        [Fact]
        public void DebugEngine_RespondsToMSBUILDDEBUGENGINE()
        {
            var originalValue = Environment.GetEnvironmentVariable("MSBuildDebugEngine");
            var originalCapsValue = Environment.GetEnvironmentVariable("MSBUILDDEBUGENGINE");
            
            try
            {
                Environment.SetEnvironmentVariable("MSBuildDebugEngine", null);
                Environment.SetEnvironmentVariable("MSBUILDDEBUGENGINE", "1");
                
                var traits = new Traits();
                
                traits.DebugEngine.ShouldBeTrue();
            }
            finally
            {
                Environment.SetEnvironmentVariable("MSBuildDebugEngine", originalValue);
                Environment.SetEnvironmentVariable("MSBUILDDEBUGENGINE", originalCapsValue);
            }
        }

        [Fact]
        public void DebugEngine_FalseWhenNeitherVariableSet()
        {
            var originalValue = Environment.GetEnvironmentVariable("MSBuildDebugEngine");
            var originalCapsValue = Environment.GetEnvironmentVariable("MSBUILDDEBUGENGINE");
            
            try
            {
                Environment.SetEnvironmentVariable("MSBuildDebugEngine", null);
                Environment.SetEnvironmentVariable("MSBUILDDEBUGENGINE", null);
                
                var traits = new Traits();
                
                traits.DebugEngine.ShouldBeFalse();
            }
            finally
            {
                Environment.SetEnvironmentVariable("MSBuildDebugEngine", originalValue);
                Environment.SetEnvironmentVariable("MSBUILDDEBUGENGINE", originalCapsValue);
            }
        }
    }
}