using System;
using System.Diagnostics;
using System.Threading;

class MinecraftServerWrapper
{
    private readonly string _javaExecutable;
    private readonly string _serverJar;
    private readonly string _jvmArguments;
    private Process _serverProcess;
    private Thread _monitorThread;
    private bool _autoRestart;

    public MinecraftServerWrapper(Config config)
    {
        _javaExecutable = config.JavaExecutable;
        _serverJar = config.ServerJar;
        _jvmArguments = config.JvmArguments;
    }

    public void Start(bool autoRestart = false)
    {
        if (_serverProcess != null && !_serverProcess.HasExited)
        {
            Console.WriteLine("Server is already running.");
            return;
        }

        _autoRestart = autoRestart;

        var psi = new ProcessStartInfo
        {
            FileName = _javaExecutable,
            Arguments = $"{_jvmArguments} -jar {_serverJar} nogui",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        _serverProcess = new Process
        {
            StartInfo = psi,
            EnableRaisingEvents = true
        };

        _serverProcess.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
        _serverProcess.ErrorDataReceived += (s, e) => Console.WriteLine(e.Data);

        _serverProcess.Start();
        _serverProcess.BeginOutputReadLine();
        _serverProcess.BeginErrorReadLine();

        Console.WriteLine("Server started.");

        _monitorThread = new Thread(MonitorServerProcess);
        _monitorThread.Start();
    }

    private void MonitorServerProcess()
    {
        _serverProcess.WaitForExit();

        Console.WriteLine("Server stopped.");

        if (_autoRestart)
        {
            Console.WriteLine("Restarting server...");
            Start(_autoRestart);
        }
    }

    public void Stop()
    {
        if (_serverProcess == null || _serverProcess.HasExited)
        {
            Console.WriteLine("Server is not running.");
            return;
        }

        _autoRestart = false;
        _serverProcess.StandardInput.WriteLine("stop");
        _serverProcess.WaitForExit();
        _serverProcess.Close();
        _serverProcess = null;

        Console.WriteLine("Server stopped.");
    }

    public void ExecuteCommand(string command)
    {
        if (_serverProcess == null || _serverProcess.HasExited)
        {
            Console.WriteLine("Server is not running.");
            return;
        }

        _serverProcess.StandardInput.WriteLine(command);
    }

    public bool IsRunning()
    {
        return _serverProcess != null && !_serverProcess.HasExited;
    }
}
