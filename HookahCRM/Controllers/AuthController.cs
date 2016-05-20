using HookahCRM.DataModel;
using HookahCRM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace HookahCRM.Controllers
{
    public class UserViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class AuthController : BaseApiController
    {
        [ActionName("Login")]
        public IHttpActionResult Post(UserViewModel viewModel)
        {
            D_User foundUser = _session.QueryOver<D_User>().Where(x => x.Login == viewModel.Username).SingleOrDefault();

            if (foundUser != null)
            {
                if (!System.Web.Helpers.Crypto.VerifyHashedPassword(foundUser.Password, viewModel.Password))
                {
                    return Ok(new { success = false, message = "Неправильный логин пользователя или пароль" });
                }
            }
            else
            {
                return Ok(new { success = false, message = "Пользователь не найден" });
            }

            return Ok(new { success = true });
        }
    }
}