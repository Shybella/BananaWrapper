using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Timers;

class MinecraftServerWrapper
{
    private readonly string _javaExecutable;
    private readonly string _serverJar;
    private readonly string _jvmArguments;
    private Process _serverProcess;
    private Thread _monitorThread;
    private bool _autoRestart;
    private System.Timers.Timer _timer;
    private string[] _commands;
    private List<System.Timers.Timer> _loopingCommandTimers;

    public MinecraftServerWrapper(Config config)
    {
        _javaExecutable = config.JavaExecutable;
        _serverJar = config.ServerJar;
        _jvmArguments = config.JvmArguments;
        _loopingCommandTimers = new List<System.Timers.Timer>();

        foreach (var loopingCommand in config.Commands)
        {
            if (loopingCommand.Loop)
            {
                var timer = new System.Timers.Timer(loopingCommand.Interval * 1000); // convert seconds to milliseconds
                timer.Elapsed += (s, e) => ExecuteCommand(loopingCommand.Command);
                timer.AutoReset = true;
                timer.Start();
                _loopingCommandTimers.Add(timer);
            }
            else
            {
                if (loopingCommand.Delay > 0)
                {
                    var timer = new System.Timers.Timer(loopingCommand.Delay * 1000); // convert seconds to milliseconds
                    timer.Elapsed += (s, e) =>
                    {
                        ExecuteCommand(loopingCommand.Command);
                        timer.Stop();
                        timer.Dispose();
                    };
                    timer.AutoReset = false;
                    timer.Start();
                }
                else
                {
                    ExecuteCommand(loopingCommand.Command);
                }
            }
        }
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

        //start timer to execute commands 5 minutes later
        _timer = new System.Timers.Timer(5 * 60 * 1000); //5 minutes in milliseconds
        _timer.Elapsed += ExecuteCommands;
        _timer.AutoReset = false;
        _timer.Start();
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

        foreach (var timer in _loopingCommandTimers)
        {
            timer.Stop();
            timer.Dispose();
        }

        _loopingCommandTimers.Clear();

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
            Console.WriteLine("Attempted to run command, but server is not running. Command: " + command);
            return;
        }

        _serverProcess.StandardInput.WriteLine(command);
    }

    public bool IsRunning()
    {
        return _serverProcess != null && !_serverProcess.HasExited;
    }

    //executes commands from the _commands array
    private void ExecuteCommands(Object source, ElapsedEventArgs e)
    {
        if (_commands == null || _commands.Length == 0) return;

        foreach (string command in _commands)
        {
            ExecuteCommand(command);
        }
    }
}