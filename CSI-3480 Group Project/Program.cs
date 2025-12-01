using System;
using System.IO;
using CSI_3480_Group_Project;

namespace CSI_3480_Group_Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is a password generator that hashes your password and saves it to be recalled later.\nType: 'generate' to generate a new password and save it, 'recall' to recall a password from a file, and 'end' to end your session.");
            Console.Write("\nEnter Command: ");
            string input = Console.ReadLine();
            string password;

            while (input != "end")
            {
                if (input == "generate")
                {
                    Console.WriteLine("Enter Password Length: ");
                    int length = int.Parse(Console.ReadLine());

                    Console.WriteLine("Use Special Characters? (y/n)");
                    bool useSpecial = Console.ReadLine().ToLower() == "y";

                    password = Generation.GeneratePassword(length, useSpecial);

                    Console.WriteLine("Generated Password: " + password);

                    Console.WriteLine("Enter a master password to encrypt with:");
                    string masterPassword = Console.ReadLine();

                    string encrypted = Encryption.Encrypt(password, masterPassword);
                    Console.WriteLine("Encrypted Version: " + encrypted);

                    Console.WriteLine("Please choose a file name for storage (excluding .txt at the end)");
                    string name = Console.ReadLine();

                    File.WriteAllText(name + ".txt", encrypted);
                    Console.WriteLine("Password saved to file: " + name + ".txt");
                }
                else if (input == "recall")
                {
                    Console.WriteLine("Enter the name of the file (excluding .txt) to be retrieved.");
                    string file = Console.ReadLine();

                    string encryptedPassword = File.ReadAllText(file + ".txt");
                    Console.WriteLine("Encrypted stored password: " + encryptedPassword);

                    Console.WriteLine("Enter your master password to decrypt:");
                    string masterPassword = Console.ReadLine();

                    try
                    {
                        string decrypted = Encryption.Decrypt(encryptedPassword, masterPassword);
                        Console.WriteLine("Decrypted Password: " + decrypted);
                    }
                    catch
                    {
                        Console.WriteLine("Error: Incorrect master password or corrupted file.");
                    }
                }
                else
                {
                    Console.WriteLine("Unknown Command.");
                }

                Console.Write("\nEnter Command: ");
                input = Console.ReadLine();
            }
        }
    }
}