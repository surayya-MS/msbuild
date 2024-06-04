// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.BackEnd.Logging;
using Microsoft.Build.Experimental.BuildCheck.Infrastructure;
using Microsoft.Build.Framework;
using Microsoft.Build.Shared;

namespace Microsoft.Build.Experimental.BuildCheck.Infrastructure;

internal class LoggingDispatcher : IBuildCheckEventContextDispatcher
{
    private ILoggingService _loggingService;

    public LoggingDispatcher(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    public void DispatchAsComment(
        BuildEventContext buildEventContext,
        MessageImportance importance,
        string messageResourceName,
        params object?[] messageArgs)
        => _loggingService.LogComment(buildEventContext, importance, messageResourceName, messageArgs);

    public void DispatchAsCommentFromText(BuildEventContext buildEventContext, MessageImportance importance, string message)
        => _loggingService.LogCommentFromText(buildEventContext, importance, message);

    public void DispatchAsErrorFromText(
        BuildEventContext buildEventContext,
        string? subcategoryResourceName,
        string? errorCode,
        string? helpKeyword,
        BuildEventFileInfo file,
        string message)
        => _loggingService.LogErrorFromText(buildEventContext, subcategoryResourceName, errorCode, helpKeyword, file, message);

    public void DispatchAsBuildEvent(BuildEventArgs buildEvent)
        => _loggingService.LogBuildEvent(buildEvent);
}
