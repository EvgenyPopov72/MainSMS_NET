using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace MainSMS_NET

{
  public class Mainsms
  {
    private string response_type = "xml";
    private DateTime null_date = new DateTime();
    private const string REQUEST_SUCCESS = "success";
    private const string REQUEST_ERROR = "error";
    private string project;
    private string api_key;
    private bool use_ssl;
    private bool is_test;
    private string api_url;

    /// <summary>
    /// Конструктор класса Mainsms
    /// 
    /// </summary>
    /// <param name="_project">Название проекта, смотрите http://mainsms.ru/office/api_account</param><param name="_api_key">Ключ проекта, смотрите http://mainsms.ru/office/api_account</param>
    public Mainsms(string _project, string _api_key)
    {
      this.project = _project;
      this.api_key = _api_key;
      this.use_ssl = false;
      this.is_test = false;
      this.api_url = this.use_ssl ? "https://mainsms.ru/api/mainsms/" : "http://mainsms.ru/api/mainsms/";
    }

    /// <summary>
    /// Конструктор класса Mainsms
    /// 
    /// </summary>
    /// <param name="_project">Название проекта, смотрите http://mainsms.ru/office/api_account</param><param name="_api_key">Ключ проекта, смотрите http://mainsms.ru/office/api_account</param><param name="_is_test">Используется для отладки</param>
    public Mainsms(string _project, string _api_key, bool _is_test)
    {
      this.project = _project;
      this.api_key = _api_key;
      this.use_ssl = false;
      this.is_test = _is_test;
      this.api_url = this.use_ssl ? "https://mainsms.ru/api/mainsms/" : "http://mainsms.ru/api/mainsms/";
    }

    /// <summary>
    /// Конструктор класса Mainsms
    /// 
    /// </summary>
    /// <param name="_project">Название проекта, смотрите http://mainsms.ru/office/api_account</param><param name="_api_key">Ключ проекта, смотрите http://mainsms.ru/office/api_account</param><param name="_is_test">Используется для отладки</param><param name="_use_ssl">Использовать протокол https</param>
    public Mainsms(string _project, string _api_key, bool _is_test, bool _use_ssl)
    {
      this.project = _project;
      this.api_key = _api_key;
      this.use_ssl = _use_ssl;
      this.is_test = _is_test;
      this.api_url = this.use_ssl ? "https://mainsms.ru/api/mainsms/" : "http://mainsms.ru/api/mainsms/";
    }

    private static string GetHash(HashAlgorithm hashString, string text)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(text);
      string str = "";
      foreach (byte num in hashString.ComputeHash(bytes))
        str = str + string.Format("{0:x2}", (object) num);
      return str;
    }

    private string fetch(string url, string data)
    {
      try
      {
        WebRequest webRequest = WebRequest.Create(url);
        webRequest.Method = "POST";
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        webRequest.ContentType = "application/x-www-form-urlencoded";
        webRequest.ContentLength = (long) bytes.Length;
        Stream requestStream = webRequest.GetRequestStream();
        requestStream.Write(bytes, 0, bytes.Length);
        requestStream.Close();
        WebResponse response = webRequest.GetResponse();
        //Console.WriteLine(((HttpWebResponse) response).StatusDescription);
        Stream responseStream = response.GetResponseStream();
        StreamReader streamReader = new StreamReader(responseStream);
        string str = streamReader.ReadToEnd();
        streamReader.Close();
        responseStream.Close();
        response.Close();
        return str;
      }
      catch
      {
        return "";
      }
    }

    /// <summary>
    /// Отправка сообщения
    /// 
    /// </summary>
    /// <param name="sender">Имя отправителя</param><param name="recipients">Номера получателей в любом формате через запятую</param><param name="message">Текст сообщения</param>
    /// <returns/>
    public ResponseSend send(string sender, string recipients, string message)
    {
      return send(sender, recipients, message, this.null_date);
    }

    /// <summary>
    /// Отправка сообщения
    /// 
    /// </summary>
    /// <param name="sender">Имя отправителя</param><param name="recipients">Номера получателей в любом формате через запятую</param><param name="message">Текст сообщения</param><param name="run_at">Время доставки сообщения, укажите если сообщение должно быть доставленно в определенное время.</param>
    /// <returns/>
    public ResponseSend send(string sender, string recipients, string message, DateTime run_at)
    {
        string str = "project=" + project + "&sender=" + sender + "&message=" + message + "&recipients=" + recipients +
                     "&test=" + (is_test ? "1" : "0") + "&format=" + response_type;
        string hash = Mainsms.GetHash((HashAlgorithm) new MD5CryptoServiceProvider(),
                                      Mainsms.GetHash((HashAlgorithm) new SHA1Managed(),
                                                      project + ";" + sender + ";" + message + ";" + recipients + ";" +
                                                      (is_test ? "1" : "0") + ";" + response_type + ";" +
                                                      (null_date == run_at
                                                           ? api_key
                                                           : run_at.ToString("d.M.yyyy H:m") + ";" + api_key)));
        return
            new ResponseSend(fetch(api_url + "message/send",
                                   str +
                                   (null_date == run_at
                                        ? "&sign=" + hash
                                        : "&run_at=" + run_at.ToString("d.M.yyyy H:m") + "&sign=" + hash)));
    }

    /// <summary>
    /// Запрос стоимости сообщения
    /// 
    /// </summary>
    /// <param name="recipients">Номера получателей в любом формате через запятую</param><param name="message">Текст сообщения</param>
    /// <returns/>
    public ResponsePrice price(string recipients, string message)
    {
        return
            new ResponsePrice(this.fetch(this.api_url + "message/price",
                                         "project=" + this.project + "&message=" + message + "&recipients=" + recipients +
                                         "&format=" + this.response_type + "&sign=" +
                                         Mainsms.GetHash((HashAlgorithm) new MD5CryptoServiceProvider(),
                                                         Mainsms.GetHash((HashAlgorithm) new SHA1Managed(),
                                                                         this.project + ";" + message + ";" + recipients +
                                                                         ";" + this.response_type + ";" + this.api_key))));
    }

    /// <summary>
    /// Запрос баланса
    /// 
    /// </summary>
    /// 
    /// <returns/>
    public ResponseBalance balance()
    {
        return
            new ResponseBalance(this.fetch(this.api_url + "message/balance",
                                           "project=" + this.project + "&format=" + this.response_type + "&sign=" +
                                           Mainsms.GetHash((HashAlgorithm) new MD5CryptoServiceProvider(),
                                                           Mainsms.GetHash((HashAlgorithm) new SHA1Managed(),
                                                                           this.project + ";" + this.response_type + ";" +
                                                                           this.api_key))));
    }

    /// <summary>
    /// Запрос статуса сообщений
    /// 
    /// </summary>
    /// <param name="messages_id">Статусы сообщений через запятую</param>
    /// <returns/>
    public ResponseStatus status(string messages_id)
    {
        return
            new ResponseStatus(this.fetch(this.api_url + "message/status",
                                          "project=" + this.project + "&messages_id=" + messages_id + "&format=" +
                                          this.response_type + "&sign=" +
                                          Mainsms.GetHash((HashAlgorithm) new MD5CryptoServiceProvider(),
                                                          Mainsms.GetHash((HashAlgorithm) new SHA1Managed(),
                                                                          this.project + ";" + messages_id + ";" +
                                                                          this.response_type + ";" + this.api_key))));
    }

    /// <summary>
    /// Информация о номерах
    /// 
    /// </summary>
    /// <param name="phones">Номера в любом формате, через запятую</param>
    /// <returns/>
    public ResponseInfo info(string phones)
    {
        return
            new ResponseInfo(this.fetch(this.api_url + "message/info",
                                        "project=" + this.project + "&phones=" + phones + "&format=" +
                                        this.response_type + "&sign=" +
                                        Mainsms.GetHash((HashAlgorithm) new MD5CryptoServiceProvider(),
                                                        Mainsms.GetHash((HashAlgorithm) new SHA1Managed(),
                                                                        this.project + ";" + phones + ";" +
                                                                        this.response_type + ";" + this.api_key))));
    }

    /// <summary>
    /// Запрос списка  доступных отправителей
    /// </summary>
    /// <returns></returns>
    public SendersInfo GetSenders()
    {
        SendersInfo aaa = null;
/*
        string aaaa = this.fetch(this.api_url + "sender/list",
                                 "project=" + this.project + "&sign=" +
                                 Mainsms.GetHash((HashAlgorithm) new MD5CryptoServiceProvider(),
                                                 Mainsms.GetHash((HashAlgorithm) new SHA1Managed(),
                                                                 this.project + ";" + this.api_key)));
*/
        return new SendersInfo(this.fetch(this.api_url + "sender/list",
                                          "project=" + this.project + "&sign=" +
                                          Mainsms.GetHash((HashAlgorithm) new MD5CryptoServiceProvider(),
                                                          Mainsms.GetHash((HashAlgorithm) new SHA1Managed(),
                                                                          this.project + ";" + this.api_key))));
    }
  }
}
