﻿using NewsAPI.Helpers;
using NewsAPI.Interfaces;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace NewsAPI.Controllers
{
    public class UserSettingsController : ApiController
    {
        private readonly IAddressBookProxy proxy;
        private readonly IUserAuthenticator userAuthenticator;
        private readonly IAccount account;
        private readonly IUserSettings userSettings;

        public UserSettingsController(IUserSettings userSettings, IAccount account, IUserAuthenticator userAuthenticator, IAddressBookProxy proxy)
        {
            this.proxy = proxy;
            this.userAuthenticator = userAuthenticator;
            this.account = account;
            this.userSettings = userSettings;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            // Получаем логин пользователя
            var userLogin = userAuthenticator.AuthenticateUser(base.User);

            // Получаем userSettings текущего пользователя
            return userSettings.GetUserSettings(userLogin);           
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]JObject JOsettings)
        {
            // Преобразуем JObject в json-строку
            string json = string.Join("", Regex.Split(JOsettings.ToString(), @"(?:\r\n|\n|\r)"));

            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            // Получение пользователя из реквеста
            var userLogin = userAuthenticator.AuthenticateUser(base.User);

            // Создание объекта UserSettings
            var response = userSettings.PostUserSettings(userLogin, json);

            // проксируем перед возвратом результирующий набор, содержащий guid пользователя
            return proxy.ReturnPersonGuid(response);
        }

        [HttpOptions]
        public IHttpActionResult Options()
        {
            return Ok();
        }



    }
}