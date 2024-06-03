// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.BackEnd.Logging;
using Microsoft.Build.Exceptions;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Framework.Profiler;
using Microsoft.Build.Logging;
using Microsoft.Build.Shared;
using static Microsoft.Build.Experimental.BuildCheck.Infrastructure.BuildCheckManagerProvider;

namespace Microsoft.Build.Experimental.BuildCheck.Infrastructure;

internal class BuildCheckEventArgsDispatcher : EventArgsDispatcher
{
    private readonly BuildCheckEventHandlers _eventHandlers;

    public BuildCheckEventArgsDispatcher()
    {
        BuildCheckManager = new BuildCheckManager()
        _eventHandlers = new BuildCheckEventHandlers(loggingContextFactory, buildCheckManager);
    }

    public override void Dispatch(BuildEventArgs buildEvent)
    {
        // call base.Dispatch for the original event
        base.Dispatch(buildEvent);

        // BuildFinished
        // AnyEventRaised
        // extract new event and call base.Dispatch

        _eventHandlers.EventSource_AnyEventRaised(this, buildEvent);
    }
}
