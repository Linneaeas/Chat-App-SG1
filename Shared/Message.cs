/*In summary, these classes define a simple messaging system where each message type has an Encode method to convert the message into a string, a Decode method to create a message from a string, and an Id method to return a unique identifier for the message type. The provided example specifically focuses on user registration and login messages.
*/

namespace Shared;


public abstract class Message
//Message is an abstract base class that declares two abstract methods: Encode() and Id(). Any class derived from Message must provide implementations for these methods.
{
    public abstract string Encode(); // Abstract method to encode the message as a string
    public abstract int Id(); //// Abstract method to get the message ID
}

public class RegisterUserMessage : Message
/*RegisterUserMessage is a class that inherits from Message.
It has properties Name and Password representing the user's name and password.
The Encode method returns a string representation of the message (in the format "Name:Password").
The Decode method is static and used to create a RegisterUserMessage from a string representation.
The Id method returns the message ID (in this case, 10).*/
{
    public string Name { get; set; }
    public string Password { get; set; }

    // Constructor to initialize Name and Password
    public RegisterUserMessage(string name, string password)
    {
        this.Name = name;
        this.Password = password;
    }

    // Implementation of Encode method for encoding the message as a string
    public override string Encode()
    {
        return $"{this.Name}:{this.Password}";
    }

    // Static method to decode a message string and create a RegisterUserMessage
    public static Message Decode(string message)
    {
        string[] split = message.Split(":");
        return new RegisterUserMessage(split[0], split[1]);
    }

    // Implementation of Id method to get the message ID (returns 10)
    public override int Id()
    {
        return 10;
    }
}



public class LoginMessage : Message
/*LoginMessage is similar to RegisterUserMessage in structure and functionality.
It represents a message for user login.
The Id method returns the message ID (in this case, 11).*/
{
    public string Name { get; set; }
    public string Password { get; set; }

    // Constructor to initialize Name and Password
    public LoginMessage(string name, string password)
    {
        this.Name = name;
        this.Password = password;
    }

    // Implementation of Encode method for encoding the message as a string
    public override string Encode()
    {
        return $"{this.Name}:{this.Password}";
    }

    // Static method to decode a message string and create a LoginMessage
    public static Message Decode(string message)
    {
        string[] split = message.Split(":");
        return new LoginMessage(split[0], split[1]);
    }

    // Implementation of Id method to get the message ID (returns 11)
    public override int Id()
    {
        return 11;
    }
}
