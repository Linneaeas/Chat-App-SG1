using System.Net;
using System.Net.Sockets;

namespace Shared;

// This code defines a set of classes and interfaces for managing network connections in a chat application. It includes:

// -An interface IConnection with methods for sending and receiving messages.
// -A class SocketConnection implementing IConnection, managing a Socket for communication.
// -The SocketConnection class has methods to establish a connection, receive messages, and send messages.

// Interface defining the contract for a connection
public interface IConnection
{
    void Send(Message message);
    List<Message> Receive();
}

// Implementation of the IConnection interface using Socket
public class SocketConnection : IConnection
{
    private Socket socket;

    // Constructor taking an existing Socket for connection
    public SocketConnection(Socket socket)
    {
        this.socket = socket;
    }

    // Method for creating a new SocketConnection by connecting to a server
    public static SocketConnection Connect(byte[] ip, int port)
    {
        // Create an IPAddress and an IPEndPoint from the provided parameters
        IPAddress iPAddress = new IPAddress(ip);
        IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);

        // Create a new Socket and connect to the specified IP address and port
        Socket socket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(iPEndPoint);

        // Return a new SocketConnection with the created Socket
        return new SocketConnection(socket);
    }

    // Method for receiving messages from the server
    public List<Message> Receive()
    {
        List<Message> messages = new List<Message>();

        // Check if there is data to read from the socket
        if (this.socket.Available != 0)
        {
            // Create a buffer to store the received data
            byte[] buffer = new byte[1024];
            int read = this.socket.Receive(buffer);

            // Convert the received bytes to a UTF-8 string
            string content = System.Text.Encoding.UTF8.GetString(buffer, 0, read);

            // Split the message based on a separator ("|")
            string[] split = content.Split("|");

            // Loop through each part and process messages
            for (int i = 0; i < split.Length - 1; i++)
            {
                string packet = split[i];
                string stringId = packet.Substring(0, 2);
                string message = packet.Substring(2);

                // Depending on the identifier, create and decode the message, and add it to the list
                if (stringId == "10")
                {
                    messages.Add(RegisterUserMessage.Decode(message));
                }
                else if (stringId == "11")
                {
                    messages.Add(LoginMessage.Decode(message));
                }
            }
        }

        // Return the list of processed messages
        return messages;
    }

    // Method for sending a message to the server
    public void Send(Message message)
    {
        // Construct the string to be sent (ID + encoded message + separator)
        string toSend = message.Id() + message.Encode() + "|";

        // Convert the string to a byte array with UTF-8 encoding and send it via the socket
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(toSend);
        socket.Send(buffer);
    }
}
