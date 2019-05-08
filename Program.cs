using System;
using System.ComponentModel;
using System.Diagnostics;

namespace dnkill {
    class Program {
        private static bool debug = false;
        static void Main (string[] args) {

            if (args.Length >= 2) {
                string targetProcName = args[0];
                string excludeTag = args[1];
                if (args.Length == 3 && args[2].Equals ("debug")) {
                    debug = true;
                }
                Process[] localByName = Process.GetProcessesByName (targetProcName);
                foreach (Process proc in localByName) {
                    bool kill = true;
                    log ($"Id: {proc.Id} Title: {proc.MainWindowTitle} Process Name: {proc.ProcessName} {proc.MainModule.FileName} {proc.Modules.Count}");
                    ProcessModuleCollection procModuleCollection = proc.Modules;
                    for (int i = 0; i < procModuleCollection.Count; i++) {
                        ProcessModule procModule = procModuleCollection[i];
                        log ($"-The moduleName is {procModule.ModuleName} {procModule.FileName}");
                        if (procModule.ModuleName.Contains (excludeTag) || procModule.FileName.Contains (excludeTag)) {
                            kill = false;
                           log ($"-Excluding because is using module {procModule.ModuleName} at {procModule.FileName}");
                        }
                    }
                    if (kill) {
                        log ($"Killing Id: {proc.Id} Title: {proc.MainWindowTitle} Process Name: {proc.ProcessName}");
                        proc.Kill();
                    } else {
                        log ($"NOT Killing Id: {proc.Id} Title: {proc.MainWindowTitle} Process Name: {proc.ProcessName}");
                    }

                }
            } else {
                 Console.Out.WriteLine  ("Please supply two arguments, the first is the process to find, and the second the keywords to exclude");
            }
        }

        private static void log (string msg) {
            if (debug) {
                Console.Out.WriteLine (msg);
            }
        }
    }
}