using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml;

namespace MainSMS_NET
{
    public class SendersInfo
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public string error = "";

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string message = "";

        /// <summary>
        /// Ответ сервера в формате xml
        /// </summary>
        public string response;

        /// <summary>
        /// Ответ сервера success - успех, error - неудача
        /// </summary>
        public string status;

        
        /// <summary>
        /// Список отправителей
        /// </summary>
        public List<string> Senders = new List<string>();

        /// <summary>
        /// Получение массива отправителей из строки ответа сервера
        /// </summary>
        /// <param name="_response"></param>
        public SendersInfo(string _response)
        {
            string pattern = @"(?<="")([^,\""]*)(?="")";
            Regex newReg = new Regex(pattern);
            MatchCollection matches = newReg.Matches(_response);
            
            for (int i = 0; i < matches.Count; i++)
            {
                Senders.Add(matches[i].Value);
            }
            if (matches.Count > 0)
                this.status = "success";
            else
                this.status = "error";
        }
    }

    /// <summary>
    /// Класс отправителя по умолчанию
    /// </summary>
    public class ResponceSenderDefault
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public string error = "";

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string message = "";

        /// <summary>
        /// Ответ сервера в формате xml
        /// </summary>
        public string response ="";

        /// <summary>
        /// Ответ сервера success - успех, error - неудача
        /// </summary>
        public string status = "";

        public string SenderDefault = "";

        public ResponceSenderDefault(string _response)
        {
            try
            {
                this.response = _response;
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(this.response);
                //if (xmlDocument.GetElementsByTagName("status")[0].FirstChild.Value == "success")
                if (xmlDocument.GetElementsByTagName("sender")[0].FirstChild.Value.Length > 0)
                {
                    this.status = "success";
                    this.SenderDefault = xmlDocument.GetElementsByTagName("sender")[0].FirstChild.Value;
                }
                else
                {
                    this.status = "error";
                    if (xmlDocument.GetElementsByTagName("error").Count == 0 || xmlDocument.GetElementsByTagName("message").Count == 0)
                    {
                        this.message = "Неизвестная ошибка, возможно проблемы с соединением.";
                    }
                    else
                    {
                        this.error = xmlDocument.GetElementsByTagName("error")[0].FirstChild.Value;
                        this.message = xmlDocument.GetElementsByTagName("message")[0].FirstChild.Value;
                    }
                }
            }
            catch
            {
                this.message = "Неизвестная ошибка, возможно проблемы с соединением.";
                this.error = "-1";
            }
        }
    }
}