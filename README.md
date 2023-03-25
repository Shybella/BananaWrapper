# Minecraft Server Wrapper

Minecraft Server Wrapper is a simple console application that wraps around the Minecraft server process on Windows machines.

This is a console application that allows you to start and stop a Minecraft server from the command-line. It also provides an interface for issuing server commands.

## Auto-Restart

If the server stops running for any reason, the wrapper will automatically restart it. The `autoRestart` parameter in the `Start()` method controls this behavior.

## Max Tick Time

Make sure you set the max-tick-time in your server.properties file between 80000-120000. This ensures that the connection to the server stays stable.

## Installation

1. Clone or download this repository to your machine.
2. Install the latest version of the .NET SDK if not already installed ([download here](https://dotnet.microsoft.com/download)).
3. Open a terminal window and navigate to the root directory of the cloned/downloaded repository.
4. Run the following command to install dependencies:
```
dotnet restore
```

## Usage

Start the application from the terminal using the following command:
```
dotnet run --project MinecraftServerWrapper
```

Once the application is running, use the following commands to interact with the Minecraft server:

- `start`: starts the Minecraft server with auto restart.
- `stop`: stops the Minecraft server.
- `exit`: stops the Minecraft server (if running) and exits the application.
- `<command>`: executes the given command in the Minecraft server console.

## Configuration

Contents of config.js
```
{
  "ConsoleTitle": "Minecraft Server Wrapper",
  "JavaExecutable": "java",
  "ServerJar": "server.jar",
  "JvmArguments": "-Xmx2G -Xms2G",
  "Commands": [
    {
      "Command": "say This command will run after 10 seconds",
      "Loop": false,
      "Interval": 0,
      "Delay": 10
    },
    {
      "Command": "say This command will only execute once",
      "Loop": false,
      "Interval": 0,
      "Delay": 0
    },
    {
      "Command": "say This command will loop every 10 seconds",
      "Loop": true,
      "Interval": 10,
      "Delay": 0
    }
  ]
}
```
