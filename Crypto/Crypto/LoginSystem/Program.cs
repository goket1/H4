using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using Npgsql;

namespace LoginSystem
{
    class Program
    {
        static int Menu(string message, int optionCount)
        {
            bool invalid = true;
            int selected = 0;

            while (invalid)
            {
                Console.Write($"{message}\n: ");
                string input = Console.ReadLine();
                invalid = !(int.TryParse(input, out selected) & selected > 0 & selected <= optionCount);
            }

            return selected;
        }

        static void Main(string[] args)
        {
            bool user_authenticated = false;
            Hash hash = new Hash();
            var cs = "Host=localhost;Username=postgres;Password=s$cret;Database=h4loginsystem";
            using (var con = new NpgsqlConnection(cs))
            {
                con.Open();

                using (var cmd = new NpgsqlCommand("SELECT version()", con))
                {
                    var version = cmd.ExecuteScalar();
                    Console.WriteLine($"Connected to postgreSQL version: {version}");
                }

                while (!user_authenticated)
                {
                    Console.Write("Username: ");
                    string username = Console.ReadLine();

                    Console.Write("Password: ");
                    string password = Console.ReadLine();

                    switch (Menu("Select\n1. Create user\n2. Login", 2))
                    {
                        case 1:
                            byte[] salt = hash.GenerateSalt();
                            byte[] hashed = hash.ComputePasswordHash(password, salt);
                            // Console.WriteLine($"Password: |{password}|\nSalt:{BitConverter.ToString(salt)}\nPassword hash: {BitConverter.ToString(hashed)}\nLen: {hashed.Length}");

                            using (var cmd = new NpgsqlCommand("CALL create_user(@username, @password, @salt)", con))
                            {
                                cmd.Parameters.AddWithValue("username", username);
                                cmd.Parameters.AddWithValue("password", hashed);
                                cmd.Parameters.AddWithValue("salt", salt);
                                ;
                                cmd.ExecuteNonQuery();
                            }

                            break;
                        case 2:
                            byte[] db_password = new byte[64];
                            byte[] db_salt = new byte[16];
                            int db_attempts = 0;

                            using (var cmd = new NpgsqlCommand("get_user", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("in_username", username);
                                using (var reader = cmd.ExecuteReader())
                                {
                                    Console.WriteLine();
                                    while (reader.Read())
                                    {
                                        reader.GetString(0);
                                        reader.GetBytes(1, 0, db_password, 0, 64);
                                        reader.GetBytes(2, 0, db_salt, 0, 16);
                                        db_attempts = reader.GetInt16(3);
                                    }
                                }
                            }

                            if (db_attempts > 5)
                            {
                                Console.WriteLine("User account locked due to too many login attempts");
                            }
                            else
                            {

                                byte[] hashFromUser =
                                    hash.ComputePasswordHash(password, db_salt);
                                // Console.WriteLine($"Password: |{password}|\nSalt:{BitConverter.ToString(db_salt)}\nPassword hash from user: {BitConverter.ToString(hashFromUser)}\nPassword hash in DB: {BitConverter.ToString(db_password)}\nLen: {hashFromUser.Length}");
                                if (hash.CheckAuthenticity(hashFromUser, db_password))
                                {
                                    Console.WriteLine("Password correct!");
                                    user_authenticated = true;
                                }
                                else
                                {
                                    Console.WriteLine("Password incorrect :(");
                                    db_attempts++;
                                    using (var cmd = new NpgsqlCommand("CALL wrong_password(@username, @attempts)",
                                        con))
                                    {
                                        cmd.Parameters.AddWithValue("username", username);
                                        cmd.Parameters.AddWithValue("attempts", db_attempts);
                                        ;
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                            }

                            break;
                    }
                }
            }
        }
    }
}