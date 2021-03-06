﻿// Copyright © 2017 onwards, Andrew Whewell
// All rights reserved.
//
// Redistribution and use of this software in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * Neither the name of the author nor the names of the program's contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHORS OF THE SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceFactory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualRadar.Interface;
using VirtualRadar.Interface.Owin;

namespace Test.VirtualRadar.Owin.Configuration
{
    /// <summary>
    /// Base class for text manipulator configuration tests.
    /// </summary>
    [TestClass]
    public abstract class ManipulatorConfigurationTests
    {
        protected class TextResponseManipulator : ITextResponseManipulator
        {
            public IDictionary<string, object> Environment { get; private set; }
            public TextContent TextContent { get; private set; }

            public void ManipulateTextResponse(IDictionary<string, object> environment, TextContent textContent)
            {
                Environment = environment;
                TextContent = textContent;
            }
        }

        protected interface IWrappingInterface : ITextResponseManipulator
        {
            int SomeUnrelatedValue { get; }
        }

        protected class WrappingManipulator : TextResponseManipulator, IWrappingInterface
        {
            public int SomeUnrelatedValue { get { return 1; } }
        }

        public TestContext TestContext { get; set; }
        protected IClassFactory _Snapshot;
        protected TextResponseManipulator _Manipulator;

        [TestInitialize]
        public void TestInitialise()
        {
            _Snapshot = Factory.TakeSnapshot();
            Factory.Register<IWrappingInterface, WrappingManipulator>();

            _Manipulator = new TextResponseManipulator();
            ExtraConfiguration();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Factory.RestoreSnapshot(_Snapshot);
        }

        protected abstract void ExtraConfiguration();
    }
}
