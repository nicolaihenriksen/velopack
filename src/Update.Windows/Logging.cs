﻿using System;
using System.IO;
using NLog.Config;
using NLog.Targets;
using Squirrel.SimpleSplat;

namespace Squirrel.Update
{
    class SetupLogLogger : ILogger
    {
        public LogLevel Level { get; set; } = LogLevel.Info;

        private readonly NLog.Logger _log;

        public SetupLogLogger(UpdateAction action)
        {
            // NB: Trying to delete the app directory while we have Setup.log
            // open will actually crash the uninstaller if it's in the base directory
            bool logToTemp =
                action == UpdateAction.Unset ||
                action == UpdateAction.Uninstall ||
                action == UpdateAction.Setup ||
                action == UpdateAction.Install;

            var logDirectory = logToTemp ? Utility.GetDefaultTempBaseDirectory() : SquirrelRuntimeInfo.BaseDirectory;
            var logName = logToTemp ? "Squirrel.log" : $"Squirrel-{action}.log";
            var logArchiveName = logToTemp ? "Squirrel.archive{###}.log" : $"Squirrel-{action}.archive{{###}}.log";
            
            // https://gist.github.com/chrisortman/1092889
            SimpleConfigurator.ConfigureForTargetLogging(
                new FileTarget() {
                    FileName = Path.Combine(logDirectory, logName),
                    Layout = new NLog.Layouts.SimpleLayout("${longdate} [${level:uppercase=true}] - ${message}"),
                    ArchiveFileName = Path.Combine(logDirectory, logArchiveName),
                    ArchiveAboveSize = 1_000_000 /* 2 MB */,
                    ArchiveNumbering = ArchiveNumberingMode.Sequence,
                    ConcurrentWrites = true, // should allow multiple processes to use the same file
                    KeepFileOpen = true,
                    MaxArchiveFiles = 1 /* MAX 2mb of log data per "action" */,
                },
                NLog.LogLevel.Debug
            );

            _log = NLog.LogManager.GetLogger("SetupLogLogger");
        }

        public void Write(string message, LogLevel logLevel)
        {
            if (logLevel < Level) {
                return;
            }

            switch (logLevel) {
            case LogLevel.Debug:
                _log.Debug(message);
                break;
            case LogLevel.Info:
                _log.Info(message);
                break;
            case LogLevel.Warn:
                _log.Warn(message);
                break;
            case LogLevel.Error:
                _log.Error(message);
                break;
            case LogLevel.Fatal:
                _log.Fatal(message);
                break;
            default:
                _log.Info(message);
                break;
            }
        }
    }
}