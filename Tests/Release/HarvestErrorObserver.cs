using System;
using System.IO;
using UnityEngine;
using Verse;

// Independent Unity error capture for native MO runs; no logging framework dependency.
public sealed class HarvestErrorObserver : Mod
{
    private static readonly object Sync = new object();
    public HarvestErrorObserver(ModContentPack content) : base(content)
    {
        string path = System.Environment.GetEnvironmentVariable("RIMWORLD_AMJE_HARVEST_ERROR_LOG");
        string directory = System.Environment.GetEnvironmentVariable("RIMWORLD_AMJE_ERROR_DIRECTORY");
        if (!String.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
            path = Path.Combine(directory, "Unity-" + System.Diagnostics.Process.GetCurrentProcess().Id + ".log");
        }
        if (String.IsNullOrEmpty(path)) throw new InvalidOperationException("Harvest error log path is required.");
        File.WriteAllText(path, "[CAPTURE_READY]\n");
        Application.logMessageReceivedThreaded += delegate(string text, string stack, LogType type)
        {
            if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
                lock (Sync) File.AppendAllText(path, "[ERROR] " + text + "\n" + stack + "\n");
        };
    }
}
