using System.Xml;

namespace MainSMS_NET
{
  /// <summary>
  /// Ответ на запрос баланса
  /// 
  /// </summary>
  public class ResponseBalance
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
    /// Остаток на счете
    /// 
    /// </summary>
    public string balance = "";
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

    public ResponseBalance(string _response)
    {
        try
        {
            this.response = _response;
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(this.response);
            if (xmlDocument.GetElementsByTagName("status")[0].FirstChild.Value == "success")
            {
              this.status = "success";
              this.balance = xmlDocument.GetElementsByTagName("balance")[0].FirstChild.Value;
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
