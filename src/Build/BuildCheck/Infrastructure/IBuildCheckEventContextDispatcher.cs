// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Shared;

namespace Microsoft.Build.Experimental.BuildCheck.Infrastructure;

internal interface IBuildCheckEventContextDispatcher
{
    void DispatchAsComment(BuildEventContext buildEventContext, MessageImportance importance, string messageResourceName, params object?[] messageArgs);

    void DispatchAsBuildEvent(BuildEventArgs buildEvent);

    void DispatchAsErrorFromText(BuildEventContext buildEventContext, string? subcategoryResourceName, string? errorCode, string? helpKeyword, BuildEventFileInfo file, string message);

    void DispatchAsCommentFromText(BuildEventContext buildEventContext, MessageImportance importance, string message);
}
