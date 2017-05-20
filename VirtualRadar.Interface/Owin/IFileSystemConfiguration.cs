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
using VirtualRadar.Interface.WebSite;

namespace VirtualRadar.Interface.Owin
{
    /// <summary>
    /// The interface for the singleton object that can be used to configure and modify
    /// the behaviour of the <see cref="IFileSystemServer"/> middleware.
    /// </summary>
    public interface IFileSystemConfiguration : ISingleton<IFileSystemConfiguration>
    {
        /// <summary>
        /// Raised when an HTML file is loaded from a disk-bound file. Listeners can modify the HTML before
        /// it is sent.
        /// </summary>
        event EventHandler<TextContentEventArgs> HtmlLoadedFromFile;

        /// <summary>
        /// Adds a folder from which files can be served by the site.
        /// </summary>
        /// <param name="siteRoot"></param>
        void AddSiteRoot(SiteRoot siteRoot);

        /// <summary>
        /// Removes a site root that had been previously added to the site via <see cref="AddSiteRoot"/>.
        /// </summary>
        /// <param name="siteRoot"></param>
        void RemoveSiteRoot(SiteRoot siteRoot);

        /// <summary>
        /// Returns true if the site root is currently being used to serve files.
        /// </summary>
        /// <param name="siteRoot"></param>
        /// <param name="folderMustMatch"></param>
        /// <returns></returns>
        bool IsSiteRootActive(SiteRoot siteRoot, bool folderMustMatch);

        /// <summary>
        /// Returns a collection of all of the folders from which content will be served.
        /// </summary>
        /// <returns></returns>
        List<string> GetSiteRootFolders();

        /// <summary>
        /// Raises <see cref="HtmlLoadedFromFile"/>.
        /// </summary>
        /// <param name="args"></param>
        void RaiseHtmlLoadedFromFile(TextContentEventArgs args);
    }
}