// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.BackEnd.Logging;
using Microsoft.Build.Experimental.BuildCheck;
using Microsoft.Build.Experimental.BuildCheck.Acquisition;
using Microsoft.Build.Experimental.BuildCheck.Utilities;
using Microsoft.Build.Framework;

namespace Microsoft.Build.Experimental.BuildCheck.Infrastructure;

internal sealed class BuildCheckConnectorLogger : ILogger
{
    private readonly IBuildCheckManager _buildCheckManager;
    private readonly Dictionary<Type, Action<BuildEventArgs>> _eventHandlers;

    internal BuildCheckConnectorLogger(IBuildCheckManager buildCheckManager)
    {
        _buildCheckManager = buildCheckManager;

        _eventHandlers = new()
        {
             { typeof(ProjectEvaluationFinishedEventArgs), (BuildEventArgs e) => _buildCheckManager.ProcessProjectEvaluationFinishedEventArgs((ProjectEvaluationFinishedEventArgs)e) },
             { typeof(ProjectEvaluationStartedEventArgs), (BuildEventArgs e) => _buildCheckManager.ProcessProjectEvaluationStartedEventArgs((ProjectEvaluationStartedEventArgs)e) },
             { typeof(ProjectStartedEventArgs), (BuildEventArgs e) => _buildCheckManager.StartProjectRequest(BuildCheckDataSource.EventArgs, e.BuildEventContext!) },
             { typeof(ProjectFinishedEventArgs), (BuildEventArgs e) => _buildCheckManager.EndProjectRequest(BuildCheckDataSource.EventArgs, e.BuildEventContext!) },
             { typeof(BuildCheckTracingEventArgs), (BuildEventArgs e) => _buildCheckManager.ProcessBuildCheckTracingEventArgs((BuildCheckTracingEventArgs)e) },
             { typeof(BuildCheckAcquisitionEventArgs), (BuildEventArgs e) => HandleAnalyzerAcquisition((BuildCheckAcquisitionEventArgs)e) },
             { typeof(TaskStartedEventArgs), (BuildEventArgs e) => _buildCheckManager.ProcessTaskStartedEventArgs((TaskStartedEventArgs)e) },
             { typeof(TaskFinishedEventArgs), (BuildEventArgs e) => _buildCheckManager.ProcessTaskFinishedEventArgs((TaskFinishedEventArgs)e) },
             { typeof(TaskParameterEventArgs), (BuildEventArgs e) => _buildCheckManager.ProcessTaskParameterEventArgs((TaskParameterEventArgs)e) },
        };
    }

    public LoggerVerbosity Verbosity { get; set; }

    public string? Parameters { get; set; }

    public void Initialize(IEventSource eventSource)
    {
        eventSource.AnyEventRaised += EventSource_AnyEventRaised;
        eventSource.BuildFinished += EventSource_BuildFinished;

        if (eventSource is IEventSource3 eventSource3)
        {
            eventSource3.IncludeTaskInputs();
        }

        if (eventSource is IEventSource4 eventSource4)
        {
            eventSource4.IncludeEvaluationPropertiesAndItems();
        }
    }

    public void EventSource_AnyEventRaised(object sender, BuildEventArgs e)
    {
        if (_eventHandlers.TryGetValue(e.GetType(), out Action<BuildEventArgs>? handler))
        {
            handler(e);
        }
    }

    public void EventSource_BuildFinished(object sender, BuildFinishedEventArgs e)
        => _buildCheckManager.ProcessBuildFinishedEventArgs(e);  

    public void Shutdown()
    {
    }

    private void HandleAnalyzerAcquisition(BuildCheckAcquisitionEventArgs e)
    {
        _buildCheckManager.ProcessAnalyzerAcquisition(e.ToAnalyzerAcquisitionData(), GetBuildEventContext(e));
    }

    private BuildEventContext GetBuildEventContext(BuildEventArgs e) => e.BuildEventContext
            ?? new BuildEventContext(
                    BuildEventContext.InvalidNodeId,
                    BuildEventContext.InvalidTargetId,
                    BuildEventContext.InvalidProjectContextId,
                    BuildEventContext.InvalidTaskId);
}
