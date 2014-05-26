namespace MainSMS_NET
{
  /// <summary>
  /// Информация о номере телефона
  /// </summary>
  public struct PhoneInfo
  {
      /// <summary>
      /// Номер телефона в формате 79*********
      /// </summary>
      public string phone;
      
      /// <summary>
      /// Код оператора
      /// </summary>
      public string code;

      /// <summary>
      /// Регион
      /// </summary>
      public string region;

      /// <summary>
      /// Название оператора
      /// </summary>
      public string name;
  }
}
