using Microsoft.AspNetCore.Mvc;
using nanolink.Controllers;
using nanolink.Criptografy;
using nanolink.Models;

namespace nanolink.Routes
{
    public static class Routes
    {
        public static void Rotas(this WebApplication app)
        {
            app.MapPost("/add-url", (HttpContext http, [FromBody] UrlModel? url) =>
            {
                if (http.Session.GetString("id") == null)
                {
                    return Results.Unauthorized();
                }
                else
                {
                    if (url == null)
                    {
                        return Results.NoContent();
                    }
                    url.IdUser = http.Session.GetString("id");
                    var info = UrlController.AddUrl(url);
                    if (info == "Not connected!")
                    {
                        return Results.InternalServerError();
                    }
                    else
                    {
                        return Results.Ok();
                    }
                }

            });

            //

            app.MapPost("/deletar-url", ([FromBody] UrlModel? url, HttpContext http) =>
            {
                if (http.Session.GetString("id") == null)
                {
                    return Results.Unauthorized();
                }
                else
                {
                    string info = UrlController.RemoveUrl(url.UrlCurta.ToString());
                    if (info == "Ok!")
                    {
                        return Results.Ok();
                    }
                    else
                    {
                        return Results.InternalServerError();
                    }
                }
            });

            //

            app.MapDelete("/deletar-usuario", (HttpContext http) =>
            {
                if (http.Session.GetString("id") == null)
                {
                    return Results.Unauthorized();
                }
                else
                {
                    UserController.DeletarUsuario(http.Session.GetString("id"));
                    http.Session.Clear();
                    return Results.Ok();
                }
            });

            //

            app.MapGet("/urls", (HttpContext http) =>
            {
                if (http.Session.GetString("id") == null)
                {
                    return Results.Unauthorized();
                }
                else
                {

                    object urls = UserController.GetUrls(http.Session.GetString("id"), http);
                    if (urls.ToString() == "Not connected!")
                    {
                        return Results.InternalServerError();
                    }
                    else
                    {
                        return Results.Ok(urls);
                    }
                }
            });

            //

            app.MapPost("/editar-url", (HttpContext http, UrlModel url) =>
            {
                if (http.Session.GetString("id") == null)
                {
                    return Results.Unauthorized();
                }
                else
                {
                    if (url == null)
                    {
                        return Results.NoContent();
                    }
                    else
                    {
                        var info = UrlController.EditarUrl(url);
                        if (info == "Not connected!")
                        {
                            return Results.InternalServerError();
                        }
                        else
                        {
                            return Results.Ok();
                        }
                    }
                }
            });

            //

            app.MapPost("/user-login", ([FromBody] UserModel user, HttpContext http) =>
            {
                if (user == null)
                {
                    return Results.NoContent();
                }
                else
                {
                    var info = UserController.ValidaUser(user);
                    if (info.ToString() == "Invalid!")
                    {
                        return Results.Unauthorized();
                    }
                    else if (info.ToString() == "Not Connected!")
                    {
                        return Results.InternalServerError();
                    }
                    else
                    {
                        user.Password = Criptografia.GerarHash(user.Password);
                        http.Session.SetString("id", info);
                        return Results.Ok(UserController.UserInfo(info));
                    }
                }
            });

            //

            app.MapPost("/cadastrar-usuario", (UserModel? user) =>
            {
                if (user == null)
                {
                    return Results.NoContent();
                }
                else
                {
                    var info = UserController.AddUser(user);
                    if (info == "Email ou usuario ja existe!")
                    {
                        return Results.Conflict(new
                        {
                            menssage = info
                        });
                    }
                    else if (info == "Password invalido!")
                    {
                        return Results.NotFound(new
                        {
                            info = "Password invalido!"
                        });
                    }
                    else if (info == "Not Connected!")
                    {
                        return Results.InternalServerError();
                    }
                    else
                    {
                        return Results.Ok();
                    }
                }
            });


            //

            app.MapGet("/close-session", (HttpContext http) =>
            {
                http.Session.Clear();
                return Results.Ok();
            });

            app.MapGet("/{id}", (string id) =>
            {
                var urlLonga = UrlController.UrlRedirect(id);
                if (urlLonga == "Not connected!")
                {
                    return Results.InternalServerError();
                }
                else
                {
                    return Results.Redirect(urlLonga);
                }
            });

            //

            app.MapPost("/editar-user", ([FromBody] UserModel user, HttpContext http) =>
            {
                if (http.Session.GetString("id") == null)
                {
                    return Results.Unauthorized();
                }
                else
                {
                    if (user == null)
                    {
                        return Results.Unauthorized();
                    }
                    else
                    {
                        var info = UserController.EditarUser(user, http.Session.GetString("id").ToString());
                        if (info.ToString() == "Not connected")
                        {
                            return Results.InternalServerError();
                        }
                        else
                        {
                            return Results.Ok(info);
                        }
                    }
                }
            });
        }
    }
}