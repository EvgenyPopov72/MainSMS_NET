using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        
        public List<string> Senders = new List<string>();

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
}