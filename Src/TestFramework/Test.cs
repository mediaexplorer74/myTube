// Decompiled with JetBrains decompiler
// Type: TestFramework.Test
// Assembly: TestFramework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D769B2DD-65AF-48DC-A0D8-37BA624BCB37
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\TestFramework.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TestFramework
{
  public static class Test
  {
    public static event EventHandler<TestLoggedEventArgs> Logged;

    public static List<TestMethodInfo> GetTestMethodInfo(Type type)
    {
      MethodInfo[] array = type.GetTypeInfo().DeclaredMethods.ToArray<MethodInfo>();
      List<TestMethodInfo> testMethodInfo1 = new List<TestMethodInfo>();
      foreach (MethodInfo element in array)
      {
        if (element.GetCustomAttribute(typeof (TestMethodAttribute), true) is TestMethodAttribute customAttribute)
        {
          if (customAttribute.ShouldBeSeparate && !string.IsNullOrEmpty(customAttribute.Parameters))
          {
            object obj1 = element.DeclaringType.GetRuntimeField(customAttribute.Parameters).GetValue((object) null);
            if (obj1 != null)
            {
              foreach (object obj2 in (IEnumerable) obj1)
              {
                TestMethodInfo testMethodInfo2 = new TestMethodInfo()
                {
                  Name = (customAttribute.DisplayName ?? element.Name) + " [" + obj2.ToString() + "]",
                  Description = customAttribute.Description,
                  MethodInfo = element,
                  Type = type,
                  Parameter = obj2
                };
                testMethodInfo1.Add(testMethodInfo2);
              }
            }
          }
          else
          {
            TestMethodInfo testMethodInfo3 = new TestMethodInfo()
            {
              Name = customAttribute.DisplayName ?? element.Name,
              Description = customAttribute.Description,
              MethodInfo = element,
              Type = type,
              ParameterSource = customAttribute.Parameters
            };
            testMethodInfo1.Add(testMethodInfo3);
          }
        }
      }
      return testMethodInfo1;
    }

    public static async Task<TestResult> Run(params object[] tests)
    {
      Test.Log("Running test on array");
      Test.Log("");
      List<Exception> exceptions = new List<Exception>();
      int totalCount = 0;
      object[] objArray = tests;
      for (int index = 0; index < objArray.Length; ++index)
      {
        object i = objArray[index];
        try
        {
          TestResult testResult = (TestResult) null;
          if (i is TestMethodInfo)
            testResult = await Test.Run(i as TestMethodInfo);
          else if (i is Type)
            testResult = await Test.Run(i as Type);
          if (testResult != null)
          {
            totalCount += testResult.NumberOfTests;
            if (testResult.Failure != null)
              exceptions.Add(testResult.Failure);
          }
        }
        catch (Exception ex)
        {
          exceptions.Add(ex);
        }
        i = (object) null;
      }
      objArray = (object[]) null;
      TestResult testResult1 = new TestResult()
      {
        NumberOfTests = totalCount
      };
      Test.Log("");
      Test.Log("======");
      if (exceptions.Count > 0)
      {
        int num = 0;
        foreach (Exception exception in exceptions)
        {
          if (exception is AggregateException)
            num += (exception as AggregateException).InnerExceptions.Count;
          else
            ++num;
        }
        AggregateException aggregateException = new AggregateException((IEnumerable<Exception>) exceptions);
        testResult1.Failure = (Exception) aggregateException;
        Test.Failure(num.ToString() + " failures out of " + (object) testResult1.NumberOfTests + " tests");
      }
      else
        Test.Success("All " + (object) testResult1.NumberOfTests + " tests passed!");
      Test.Log("");
      return testResult1;
    }

    public static async Task<TestResult> Run(Type type)
    {
      List<TestMethodInfo> testMethodInfo = Test.GetTestMethodInfo(type);
      Test.Log("Running test on type \"" + (object) type + "\"");
      Test.Log("");
      List<Exception> exceptions = new List<Exception>();
      int totalCount = 0;
      foreach (TestMethodInfo m in testMethodInfo)
      {
        try
        {
          TestResult testResult = await Test.Run(m);
          totalCount += testResult.NumberOfTests;
          if (testResult.Failure != null)
            exceptions.Add(testResult.Failure);
        }
        catch (Exception ex)
        {
          exceptions.Add(ex);
        }
      }
      TestResult testResult1 = new TestResult()
      {
        NumberOfTests = totalCount
      };
      Test.Log("");
      Test.Log("======");
      if (exceptions.Count > 0)
      {
        int num = 0;
        foreach (Exception exception in exceptions)
        {
          if (exception is AggregateException)
            num += (exception as AggregateException).InnerExceptions.Count;
          else
            ++num;
        }
        AggregateException aggregateException = new AggregateException((IEnumerable<Exception>) exceptions);
        testResult1.Failure = (Exception) aggregateException;
        Test.Failure(num.ToString() + " failures out of " + (object) testResult1.NumberOfTests + " tests");
      }
      else
        Test.Success("All " + (object) testResult1.NumberOfTests + " tests passed!");
      Test.Log("");
      return testResult1;
    }

    public static async Task<TestResult> Run(TestMethodInfo m)
    {
      try
      {
        return await Test.runInternal(m);
      }
      catch (Exception ex)
      {
        Test.Error("Failed to start test \"" + m.Name + "\"");
        return new TestResult()
        {
          NumberOfTests = 0,
          Failure = ex
        };
      }
    }

    private static async Task<TestResult> runInternal(TestMethodInfo m)
    {
      TestResult result = new TestResult()
      {
        NumberOfTests = 0
      };
      Test.Log("Running test \"" + m.Name + "\"");
      Test.Log("==============");
      Type type = m.MethodInfo.DeclaringType;
      List<Exception> exceptions = new List<Exception>();
      object obj;
      try
      {
        obj = Activator.CreateInstance(type);
        if (obj == null)
          throw new NullReferenceException("Could not create the object");
      }
      catch (Exception ex)
      {
        Test.Error(ex.ToString());
        Test.Log("Failed to create instance of test type. Type must have a default non-static constructor", LogType.Failure);
        throw;
      }
      try
      {
        IEnumerable parameters = (IEnumerable) null;
        object[] param = (object[]) null;
        if (m.Parameter != null)
          param = new object[1]{ m.Parameter };
        if (m.ParameterSource != null)
          parameters = type.GetRuntimeField(m.ParameterSource).GetValue(obj) as IEnumerable;
        if (parameters == null)
          result.NumberOfTests = 1;
        if (m.MethodInfo.ReturnType == typeof (Task))
        {
          if (parameters == null)
          {
            await (m.MethodInfo.Invoke(obj, param) as Task);
          }
          else
          {
            foreach (object parameters1 in parameters)
            {
              ++result.NumberOfTests;
              try
              {
                obj = Activator.CreateInstance(type);
                Task task;
                if (parameters1 is object[])
                  task = m.MethodInfo.Invoke(obj, parameters1 as object[]) as Task;
                else
                  task = m.MethodInfo.Invoke(obj, new object[1]
                  {
                    parameters1
                  }) as Task;
                await task;
              }
              catch (Exception ex)
              {
                Exception exception = !(ex is TargetInvocationException) || ex.InnerException == null ? ex : ex.InnerException;
                Test.Error(exception.GetType().Name + ": " + exception.Message);
                exceptions.Add(exception);
              }
              Test.Log("-");
            }
          }
        }
        else if (parameters == null)
        {
          m.MethodInfo.Invoke(obj, param);
        }
        else
        {
          foreach (object parameters2 in parameters)
          {
            ++result.NumberOfTests;
            try
            {
              obj = Activator.CreateInstance(type);
              if (parameters2 is object[])
                m.MethodInfo.Invoke(obj, parameters2 as object[]);
              else
                m.MethodInfo.Invoke(obj, new object[1]
                {
                  parameters2
                });
            }
            catch (Exception ex)
            {
              Exception exception = !(ex is TargetInvocationException) || ex.InnerException == null ? ex : ex.InnerException;
              Test.Error(exception.GetType().Name + ": " + exception.Message);
              exceptions.Add(exception);
            }
            Test.Log("-");
          }
        }
        parameters = (IEnumerable) null;
        param = (object[]) null;
      }
      catch (Exception ex)
      {
        Test.Error(ex.GetType().Name + ": " + ex.Message);
        Test.Failure("Test \"" + m.Name + "\" was unsuccessful.");
        result.Failure = ex;
      }
      if (exceptions.Count > 0)
      {
        Test.Error("Aggregate test failure");
        foreach (Exception exception in exceptions)
          ;
        AggregateException aggregateException = new AggregateException((IEnumerable<Exception>) exceptions);
        Test.Failure(exceptions.Count.ToString() + " failures out of " + (object) result.NumberOfTests + " tests in \"" + m.Name + "\"");
        result.Failure = (Exception) aggregateException;
      }
      if (result.Failure == null)
      {
        if (result.NumberOfTests == 1)
          Test.Success("Test \"" + m.Name + "\" completed successfully!");
        else
          Test.Success(result.NumberOfTests.ToString() + " tests in \"" + m.Name + "\" completed successfully!");
      }
      Test.Log("");
      return result;
    }

    private static IEnumerable GetParams(TestMethodInfo m)
    {
      if (m.ParameterSource == null)
        return (IEnumerable) null;
      object instance = Activator.CreateInstance(m.Type);
      return m.Type.GetRuntimeField(m.ParameterSource).GetValue(instance) as IEnumerable;
    }

    public static void Log(string text) => Test.Log(text, LogType.Log);

    private static void Log(string text, LogType type)
    {
      if (Test.Logged == null)
        return;
      Test.Logged((object) null, new TestLoggedEventArgs()
      {
        Text = text,
        Type = type
      });
    }

    public static void Error(string text) => Test.Log("[Error] " + text, LogType.Error);

    public static void Warn(string text) => Test.Log("Warning: " + text, LogType.Warning);

    private static void Failure(string text) => Test.Log(text, LogType.Failure);

    private static void Success(string text) => Test.Log(text, LogType.Success);
  }
}
