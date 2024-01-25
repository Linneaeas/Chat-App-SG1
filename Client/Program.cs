// // This code simulates a basic client program that connects to a server, registers a user, 
// // logs in, and attempts to register the same user again. 
// // The actual behavior of the program depends on the implementation details of the SocketConnection and the messages
// //  (e.g., RegisterUserMessage, LoginMessage) in the Shared namespace.
// 
// namespace Client;
// 
// class Program
// {
//     static void Main(string[] args)
//     {
//         // Create a connection using the SocketConnection class from the Shared namespace
//         Shared.IConnection connection = Shared.SocketConnection.Connect(
//             new byte[] { 127, 0, 0, 1 }, // IP address (127.0.0.1 - localhost)
//             27800 // Port number (27800)
//         );
// 
//         // Send a RegisterUserMessage through the connection
//         connection.Send(new Shared.RegisterUserMessage("Ironman", "stark123"));
//         // Send a LoginMessage through the connection
//         connection.Send(new Shared.LoginMessage("Ironman", "stark123"));
//         // Send another RegisterUserMessage through the connection
//         connection.Send(new Shared.RegisterUserMessage("Ironman", "stark123"));
//     }
// }

