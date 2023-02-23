using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FusionCore.Debug
{
    public class GitRepositoryInformation : IDisposable
    {
        private bool _disposed;
        private readonly Process _gitProcess;

        public GitRepositoryInformation(string path, string gitPath = null)
        {
            var processInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                FileName = Directory.Exists(gitPath) ? gitPath : "git.exe",
                CreateNoWindow = true,
                WorkingDirectory = (path != null && Directory.Exists(path)) ? path : Environment.CurrentDirectory
            };

            _gitProcess = new Process();
            _gitProcess.StartInfo = processInfo;
        }

        public string CommitHash
        {
            get { return RunCommand("rev-parse HEAD"); }
        }

        public string BranchName
        {
            get { return RunCommand("rev-parse --abbrev-ref HEAD"); }
        }

        public IEnumerable<string> Log
        {
            get
            {
                int skip = 0;
                while (true)
                {
                    string entry = RunCommand(string.Format("log --skip={0} -n1", skip++));
                    if (string.IsNullOrWhiteSpace(entry))
                    {
                        yield break;
                    }

                    yield return entry;
                }
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _gitProcess.Dispose();
            }
        }

        private string RunCommand(string args)
        {
            _gitProcess.StartInfo.Arguments = args;
            _gitProcess.Start();

            var output = _gitProcess.StandardOutput.ReadToEnd().Trim();

            _gitProcess.WaitForExit();

            return output;
        }
    }
}