using System.Collections.Generic;
using System.Xml;

namespace MainSMS_NET
{
  public class ResponseStatus
  {
    /// <summary>
    /// Код ошибки
    /// 
    /// </summary>
    public string error = "";
    /// <summary>
    /// Сообщение об ошибке
    /// 
    /// </summary>
    public string message = "";
    /// <summary>
    /// Словарь соответствий id сообщения статусу доставки
    /// 
    /// </summary>
    public Dictionary<string, string> messages = new Dictionary<string, string>();
    /// <summary>
    /// Ответ сервера в формате xml
    /// 
    /// </summary>
    public string response;
    /// <summary>
    /// Ответ сервера success - успех, error - неудача
    /// 
    /// </summary>
    public string status;

    public ResponseStatus(string _response)
    {
      try
      {
        this.response = _response;
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(this.response);
        if (xmlDocument.GetElementsByTagName("status")[0].FirstChild.Value == "success")
        {
          this.status = "success";
          foreach (XmlNode xmlNode in xmlDocument.GetElementsByTagName("messages")[0].ChildNodes)
            this.messages[xmlNode.Name.Replace("id", "")] = xmlNode.ChildNodes[0].Value;
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
