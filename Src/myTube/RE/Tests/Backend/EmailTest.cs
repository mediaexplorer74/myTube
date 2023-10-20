// Decompiled with JetBrains decompiler
// Type: myTube.Tests.Backend.EmailTest
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Cloud;
using System;
using TestFramework;

namespace myTube.Tests.Backend
{
  public class EmailTest
  {
    private string[] validEmails = new string[12]
    {
      "niceandsimple@example.com",
      "very.common@example.com",
      "a.little.lengthy.but.fine@dept.example.com",
      "disposable.style.email.with+symbol@example.com",
      "other.email-with-dash@example.com",
      "\"very.unusual.@.unusual.com\"@example.com",
      "\"very.(),:;<>[]\\\".VERY.\\\"very@\\\\ \\\"very\\\".unusual\"@strange.example.com",
      "#!$%&'*+-/=?^_`{}|~@example.org",
      "\"()<>[]:,;@\\\\\\\"!#$%&'*+-/=?^_`{}| ~.a\"@example.org",
      "\" \"@example.org",
      "üñîçøðé@example.com",
      "üñîçøðé@üñîçøðé.com"
    };
    private string[] invalidEmails = new string[12]
    {
      "Abc.example.com",
      "A@b@c@example.com",
      "a\"b(c)d,e:f;g<h>i[j\\k]l@example.com",
      "just\"not\"right@example.com",
      "this is\\\"not\allowed@example.com",
      "this\\ still\\\"not\\\\allowed@example",
      "john..doe@example.com",
      "john.doe@example..com",
      " hello@email.com",
      "hello@email.com ",
      " hello@email.com ",
      "quoatesInDomain@\"quotes.com\".com"
    };

    [TestMethod(DisplayName = "Valid Email Addresses", Parameters = "validEmails")]
    public void IsValidEmail(string email)
    {
      if (!Email.IsValidEmailAddress(email))
        throw new InvalidOperationException("The email " + email + " is not valid");
      Test.Log("The email " + email + " is valid!");
    }

    [TestMethod(DisplayName = "Invalid Email Addresses", Parameters = "invalidEmails")]
    public void IsInvalidEmail(string email)
    {
      if (Email.IsValidEmailAddress(email))
        throw new InvalidOperationException("The email " + email + " is not invalid");
      Test.Log("The email " + email + " is invalid!");
    }
  }
}
