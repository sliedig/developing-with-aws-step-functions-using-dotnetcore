using System;
using Amazon.Lambda.Core;
using PlagiarismIncidentSystem;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ScheduleExamTask
{
  public class Function
  {

    /// <summary>
    /// Function to schedule the next exam fr the student.
    /// Student cannot take more than 3 exams so throw customer exception
    /// if this business rule is met.
    /// </summary>
    /// <param name="state">Incident State object</param>
    /// <param name="context">Lambda Context</param>
    /// <returns></returns>
    public IncidentState FunctionHandler(IncidentState state, ILambdaContext context)
    {
      var exam = new Exam(Guid.NewGuid(), DateTime.Now.AddSeconds(10), 0);

      if (state.Exams != null && state.Exams.Count >= 3)
      {
        throw new StudentExceededAllowableExamRetries("Student cannot take more that 3 exams.");
      }

      state.Exams?.Add(exam);

      return state;
    }
  }
}
