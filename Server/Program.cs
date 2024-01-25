using System.Net;
using System.Net.Sockets;
using Shared;

namespace Server;

// This code represents the server-side implementation of a chat application. Key components include:

// -The Program class with a main loop accepting client connections and handling incoming messages.
// -An interface IConnectionHandler defining methods for accepting connections and handling message reads.
// -The SocketConnectionHandler class implementing IConnectionHandler, managing a server socket, accepting connections, and delegating message handling to specific handlers based on message IDs.
// -Interfaces IMessageHandler, RegisterHandler, and LoginHandler define contracts for handling different types of messages, with concrete implementations for handling registration and login messages.

class Program
{
    static void Main(string[] args)
    {
        // Create a connection handler using SocketConnectionHandler
        IConnectionHandler connectionHandler = new SocketConnectionHandler();

        // Main server loop
        while (true)
        {
            // Attempt to accept a new client connection
            Shared.IConnection? potentialClient = connectionHandler.Accept();

            // If a client has connected, print a message
            if (potentialClient != null)
            {
                Console.WriteLine("A client has connected!");
            }

            // Handle any incoming messages from connected clients
            connectionHandler.HandleReads();
        }
    }
}

// Interface defining the contract for a connection handler
public interface IConnectionHandler
{
    Shared.IConnection? Accept();
    void HandleReads();
}

// Implementation of IConnectionHandler using Socket for handling multiple connections
public class SocketConnectionHandler : IConnectionHandler
{
    private Socket serverSocket;
    private List<IConnection> connections;
    private Dictionary<int, IMessageHandler> handlers;

    // Constructor initializes server settings and message handlers
    public SocketConnectionHandler()
    {
        // Set up server IP address and port
        IPAddress iPAddress = new IPAddress(new byte[] { 127, 0, 0, 1 });
        IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 27800);

        // Create and configure the server socket
        this.serverSocket = new Socket(
            iPAddress.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp
        );
        this.serverSocket.Bind(iPEndPoint);
        this.serverSocket.Listen();

        // Initialize lists and dictionaries
        this.connections = new List<IConnection>();
        this.handlers = new Dictionary<int, IMessageHandler>();
        this.handlers[10] = new RegisterHandler(); // RegisterHandler for messages with ID 10
        this.handlers[11] = new LoginHandler();    // LoginHandler for messages with ID 11
    }

    // Accept a new client connection
    public Shared.IConnection? Accept()
    {
        // Check if there is a pending connection
        if (!this.serverSocket.Poll(50, SelectMode.SelectRead))
        {
            return null;
        }

        // Accept the connection and create a SocketConnection
        Socket clientSocket = this.serverSocket.Accept();
        IConnection connection = new Shared.SocketConnection(clientSocket);
        this.connections.Add(connection);

        // Return the new connection
        return connection;
    }

    // Handle incoming messages from connected clients
    public void HandleReads()
    {
        // Iterate through all active connections
        for (int i = 0; i < this.connections.Count; i++)
        {
            IConnection connection = this.connections[i];

            // Receive messages from the connection
            foreach (Shared.Message message in connection.Receive())
            {
                // Retrieve the appropriate handler based on the message ID and handle the message
                IMessageHandler handler = this.handlers[message.Id()];
                handler.Handle(connection, message);
            }
        }
    }
}

// Interface defining the contract for a message handler
public interface IMessageHandler
{
    void Handle(IConnection connection, Message message);
}

// Implementation of IMessageHandler for handling registration messages
public class RegisterHandler : IMessageHandler
{
    public void Handle(IConnection connection, Message message)
    {
        // Logic for handling registration messages
        Console.WriteLine("Do register logic!");
    }
}

// Implementation of IMessageHandler for handling login messages
public class LoginHandler : IMessageHandler
{
    public void Handle(IConnection connection, Message message)
    {
        // Cast the message to LoginMessage to access specific properties
        Shared.LoginMessage login = (Shared.LoginMessage)message;

        // Logic for handling login messages
        Console.WriteLine("Do login logic! Name: " + login.Name);
    }
}