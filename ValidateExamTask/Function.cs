using System;
using System.Linq;
using Amazon.Lambda.Core;
using PlagiarismIncidentSystem;

// Assembly attribute to enable the Lambda function's JSON state to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ValidateExamTask
{
    public class Function
    {

        /// <summary>
        /// Function to validate the exam result.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IncidentState FunctionHandler(IncidentState state, ILambdaContext context)
        {

            // Generating a ramdom score. This would otherwise be calling an external system.
            var lastExam = state.Exams.LastOrDefault();
            lastExam.Score = new Random().Next(0, 100);

            return state;
        }
    }
}
