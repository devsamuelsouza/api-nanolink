using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using nanolink.Models;

namespace nanolink.Controllers
{
    public class UrlController
    {
        private static readonly string Server = DotNetEnv.Env.GetString("DB_SERVER");
        private static readonly string Database = DotNetEnv.Env.GetString("DATABASE");
        private static readonly string User = DotNetEnv.Env.GetString("DB_USER");
        private static readonly string Password = DotNetEnv.Env.GetString("DB_PASSWORD");
        private static readonly string Port = DotNetEnv.Env.GetString("DB_PORT");
        private static readonly string UrlConnection = $"Server={Server};Port={Port};Database={Database};User={User};Password={Password};";

        public static string AddUrl(UrlModel url)
        {
            using (var connection = new MySqlConnection(UrlConnection))
            {
                try
                {
                    connection.Open();

                    var command = new MySqlCommand("INSERT INTO urls(url_short,name_url, id_user,url_long) VALUES(@url_short, @name_url, @id_user, @url_long)", connection);
                    command.Parameters.AddWithValue("@url_short", url.UrlCurta);
                    command.Parameters.AddWithValue("@name_url", url.NameUrl);
                    command.Parameters.AddWithValue("@id_user", url.IdUser);
                    command.Parameters.AddWithValue("@url_long", url.UrlLonga);
                    command.ExecuteScalar();
                    return "Ok!";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return "Not connected!";
                }
            }
        }

        public static string RemoveUrl(string url)
        {
            using (var connection = new MySqlConnection(UrlConnection))
            {
                try
                {
                    connection.Open();
                    var command = new MySqlCommand("DELETE FROM urls WHERE url_short = @url_short", connection);
                    command.Parameters.AddWithValue("@url_short", url);
                    command.ExecuteScalar();
                    return "Ok!";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return "Not connected!";
                }
            }
        }

        public static string UrlRedirect(string id)
        {
            using (var connection = new MySqlConnection(UrlConnection))
            {
                try
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT url_long FROM urls WHERE url_short = @id", connection);
                    command.Parameters.AddWithValue("@id", id);
                    var reader = command.ExecuteReader();
                    reader.Read();
                    return reader["url_long"].ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return "Not connected!";
                }
            }
        }

        public static string EditarUrl(UrlModel url)
        {
            using (var connection = new MySqlConnection(UrlConnection))
            {
                try
                {
                    connection.Open();
                    var command = new MySqlCommand("UPDATE urls SET name_url = @new_name, url_long = @url_long WHERE url_short = @url_short", connection);
                    command.Parameters.AddWithValue("@new_name", url.NameUrl);
                    command.Parameters.AddWithValue("@url_short", url.UrlCurta);
                    command.Parameters.AddWithValue("@url_long", url.UrlLonga);
                    command.ExecuteScalar();
                    return "Ok!";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return "Not connected!";
                }
            }
        }
    }
}