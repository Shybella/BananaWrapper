# Minecraft Server Wrapper

Minecraft Server Wrapper is a simple console application that wraps around the Minecraft server process on Windows machines.

Minecraft Server Wrapper with Spectre.Console

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
5. Create a `config.json` file with default values by running:
```
dotnet run --project MinecraftServerWrapper -- init
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

The application reads configuration values from a `config.json` file in the root directory of the repository. Here are the available configuration options:

- `ConsoleTitle`: string - the title of the console window.
- `JavaExecutable`: string - the path to the Java executable used to start the Minecraft server.
- `ServerJar`: string - the filename of the Minecraft server jar to be executed.
- `JvmArguments`: string - the arguments passed to the Java virtual machine when starting the Minecraft server.

If the `config.json` file does not exist, it will be created with default values when running the `init` command as mentioned above.
