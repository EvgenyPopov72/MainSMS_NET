using System.Collections.Generic;
using System.Xml;

namespace MainSMS_NET
{
  /// <summary>
  /// Ответ на запрос информации по номерам
  /// 
  /// </summary>
  public class ResponseInfo
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
    /// Список ответов по запрашиваемым номерам
    /// 
    /// </summary>
    public List<PhoneInfo> info = new List<PhoneInfo>();
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

    public ResponseInfo(string _response)
    {
      try
      {
        this.response = _response;
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(this.response);
        if (xmlDocument.GetElementsByTagName("status")[0].FirstChild.Value == "success")
        {
          this.status = "success";
          foreach (XmlNode xmlNode1 in xmlDocument.GetElementsByTagName("info")[0].ChildNodes)
          {
            PhoneInfo phoneInfo = new PhoneInfo();
            foreach (XmlNode xmlNode2 in xmlNode1.ChildNodes)
            {
              if (xmlNode2.Name == "region")
                phoneInfo.region = xmlNode2.ChildNodes[0].Value;
              if (xmlNode2.Name == "phone")
                phoneInfo.phone = xmlNode2.ChildNodes[0].Value;
              if (xmlNode2.Name == "code")
                phoneInfo.code = xmlNode2.ChildNodes[0].Value;
              if (xmlNode2.Name == "name")
                phoneInfo.name = xmlNode2.ChildNodes[0].Value;
            }
            this.info.Add(phoneInfo);
          }
        }
        else
        {
          this.status = "error";
          if (xmlDocument.GetElementsByTagName("error").Count == 0 || xmlDocument.GetElementsByTagName("message").Count == 0)
          {
            this.message = "Неизвестная ошибка, возможно проблемы с соединением.";
            this.error = "-1";
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
