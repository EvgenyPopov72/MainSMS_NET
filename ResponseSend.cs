using System.Collections.Generic;
using System.Xml;

namespace MainSMS_NET
{
  /// <summary>
  /// Ответ на отправку сообщения
  /// </summary>
  public class ResponseSend
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
    /// Остаток на счете
    /// </summary>
    public string balance = "";
    
    /// <summary>
    /// Список номеров получателей в формате 79*********
    /// </summary>
    public List<string> recipients = new List<string>();

    /// <summary>
    /// Список id сообщений
    /// </summary>
    public List<string> message_ids = new List<string>();

    /// <summary>
    /// Количество отправленных смс 
    /// </summary>
    public string count = "";

    /// <summary>
    /// Количество частей из которых состоит сообщение 
    /// </summary>
    public string parts = "";

    /// <summary>
    /// Стоимость отправки 
    /// </summary>
    public string price = "";

    /// <summary>
    /// Режим тестирования
    /// </summary>
    public string test = "";

    /// <summary>
    /// Время отправки сообщения, если не заполнено то сообщение отправится немедленно
    /// </summary>
    public string run_at = "";

    /// <summary>
    /// Ответ сервера в формате xml
    /// </summary>
    public string response;

    /// <summary>
    /// Ответ сервера success - успех, error - неудача
    /// </summary>
    public string status;

    public ResponseSend(string _response)
    {
      try
      {
        this.response = _response;
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(this.response);
        if (xmlDocument.GetElementsByTagName("status")[0].FirstChild.Value == "success")
        {
          status = "success";
          balance = xmlDocument.GetElementsByTagName("balance")[0].FirstChild.Value;
          price = xmlDocument.GetElementsByTagName("price")[0].FirstChild.Value;
          parts = xmlDocument.GetElementsByTagName("parts")[0].FirstChild.Value;
          count = xmlDocument.GetElementsByTagName("count")[0].FirstChild.Value;
          if (xmlDocument.GetElementsByTagName("run-at").Count > 0)
            run_at = xmlDocument.GetElementsByTagName("run-at")[0].FirstChild.Value;
          test = xmlDocument.GetElementsByTagName("test")[0].FirstChild.Value;
          foreach (XmlNode xmlNode in xmlDocument.GetElementsByTagName("recipients")[0].ChildNodes)
            recipients.Add(xmlNode.ChildNodes[0].Value);
          foreach (XmlNode xmlNode in xmlDocument.GetElementsByTagName("messages-id")[0].ChildNodes)
            message_ids.Add(xmlNode.ChildNodes[0].Value);
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
