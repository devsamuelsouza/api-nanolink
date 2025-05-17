using MySql.Data.MySqlClient;
using nanolink.Models;
using nanolink.Criptografy;

namespace nanolink.Controllers
{
    public static class UserController
    {
        private static readonly string Server = DotNetEnv.Env.GetString("DB_SERVER");
        private static readonly string Database = DotNetEnv.Env.GetString("DATABASE");
        private static readonly string User = DotNetEnv.Env.GetString("DB_USER");
        private static readonly string Password = DotNetEnv.Env.GetString("DB_PASSWORD");
        private static readonly string UrlConnection = $"Server={Server};Database={Database};User={User};Password={Password};";

        public static string AddUser(this UserModel user)
        {
            using (MySqlConnection connection = new MySqlConnection(UrlConnection))
            {
                try
                {
                    connection.Open();

                    user.Name = user.Name.ToLower();
                    user.Email = user.Email.ToLower();
                    user.User = user.User.ToLower();

                    if (user.Password == null)
                    {
                        return "Password invalido!";
                    }
                    else
                    {
                        user.Password = Criptografia.GerarHash(user.Password);
                    }

                    var consultaEmailandUser = new MySqlCommand("SELECT email_user, user FROM users WHERE email_user = @email OR user = @user", connection);
                    consultaEmailandUser.Parameters.AddWithValue("@email", user.Email);
                    consultaEmailandUser.Parameters.AddWithValue("@user", user.User);

                    var readerConsultaEmailandUser = consultaEmailandUser.ExecuteReader();
                    while (readerConsultaEmailandUser.Read())
                    {
                        if (user.Email == readerConsultaEmailandUser["email_user"].ToString() || user.User == readerConsultaEmailandUser["user"].ToString())
                        {
                            return "Email ou usuario ja existe!";
                        }
                    }
                    readerConsultaEmailandUser.Close();

                    var command = new MySqlCommand("INSERT INTO users(id_user, user, name_user, password_user, data_cadastro, sex_user, email_user) VALUES(@id_user, @user, @name_user, @password_user, NOW() , @sex_user, @email_user)", connection);
                    command.Parameters.AddWithValue("@id_user", user.Id);
                    command.Parameters.AddWithValue("@user", user.User);
                    command.Parameters.AddWithValue("@name_user", user.Name);
                    command.Parameters.AddWithValue("@password_user", user.Password);
                    command.Parameters.AddWithValue("@sex_user", user.Sex);
                    command.Parameters.AddWithValue("@email_user", user.Email);
                    var reader = command.ExecuteScalar();
                    return "Cadastrado";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return "Not Connected!";
                }
            }
        }

        public static string ValidaUser(UserModel user)
        {
            using (MySqlConnection connection = new MySqlConnection(UrlConnection))
            {
                try
                {
                    connection.Open();

                    user.User = user.User.ToLower();
                    user.Password = Criptografia.GerarHash(user.Password);

                    var command = new MySqlCommand("SELECT * FROM users WHERE user = @user AND password_user = @password", connection);
                    command.Parameters.AddWithValue("@user", user.User);
                    command.Parameters.AddWithValue("@password", user.Password);
                    var reader = command.ExecuteReader();
                    if (reader.Read() == false)
                    {
                        return "Invalid!";
                    }
                    else
                    {
                        return reader["id_user"].ToString();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return "Not Connected!";
                }
            }
        }
        public static object GetUrls(string id, HttpContext http)
        {
            using (var connection = new MySqlConnection(UrlConnection))
            {
                try
                {
                    connection.Open();
                    var Urls = new List<object>();


                    var command = new MySqlCommand("SELECT * FROM urls WHERE id_user = @id_user", connection);
                    command.Parameters.AddWithValue("@id_user", id);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Urls.Add(new
                        {
                            url_short = $"https://{http.Request.Host}/{reader["url_short"]}",
                            name = reader["name_url"],
                            url_long = reader["url_long"]

                        });
                    }
                    return Urls;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return "Not connected!";
                }
            }
        }
        public static object UserInfo(string id)
        {
            using (var connection = new MySqlConnection(UrlConnection))
            {
                try
                {
                    connection.Open();
                    var userInfo = new List<object>();
                    var command = new MySqlCommand("SELECT * FROM users WHERE id_user = @id", connection);
                    command.Parameters.AddWithValue("@id", id);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        userInfo.Add(new
                        {
                            id = reader["id_user"].ToString(),
                            user = reader["user"].ToString(),
                            name_user = reader["name_user"].ToString(),
                            email_user = reader["email_user"].ToString()
                        });
                    }
                    return userInfo;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return "Not connected!";
                }
            }
        }
        public static object EditarUser(UserModel user, string id)
        {
            using (var connection = new MySqlConnection(UrlConnection))
            {
                try
                {
                    connection.Open();
                    var userInfo = new List<object>();

                    user.Password = Criptografia.GerarHash(user.Password);

                    var command = new MySqlCommand("UPDATE users SET name_user = @new_name, password_user = @new_password WHERE id_user = @id_user", connection);
                    command.Parameters.AddWithValue("@new_name", user.Name);
                    command.Parameters.AddWithValue("@new_password", user.Password);
                    command.Parameters.AddWithValue("@id_user", id);
                    command.ExecuteScalar();

                    var command2 = new MySqlCommand("SELECT * FROM users WHERE id_user = @id", connection);
                    command2.Parameters.AddWithValue("@id", id);
                    var reader = command2.ExecuteReader();

                    while (reader.Read())
                    {
                        userInfo.Add(new
                        {
                            id = reader["id_user"].ToString(),
                            user = reader["user"].ToString(),
                            name_user = reader["name_user"].ToString(),
                            email_user = reader["email_user"].ToString()
                        });
                    }
                    return userInfo;
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return "Not connected!";
                }
            }
        }
        public static void DeletarUsuario(string id)
        {
            using (var connection = new MySqlConnection(UrlConnection))
            {
                try
                {
                    connection.Open();
                    var command = new MySqlCommand("DELETE FROM urls WHERE id_user = @id; DELETE FROM users WHERE id_user = @id", connection);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteScalar();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}