# TCP Unit Converter
Simple TCP server and client for unit conversion. Supported commands:
| Command | Argument | Description  |
| :---: |:---:| :--- |
| `lb2kg` | number | from pounds to kilograms |
| `kg2lb` | number | from kilograms to pounds |
| `in2cm` | number | from inches to centimeters |
| `cm2in` | number | from centimeters to inches |
| `help` | - | list of supported commands |
| `quit` | - | close connection |

## Technologies
* C#
* .NET

## Usage
To start the TCP server, cd to the **server** folder and run:
```
dotnet server
```
You should see the message `Waiting for a connection`, indicating that the server is listening.

To use the client, cd to the **client** folder and use one of the commands. For example, to convert 100 kg to punds:
```
dotnet client kg2lb 100
```
The server should respond `100 kg is 220.4623 lbs`.
