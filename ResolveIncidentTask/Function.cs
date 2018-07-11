using System;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using PlagiarismIncidentSystem;

// Assembly attribute to enable the Lambda function's JSON state to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ResolveIncidentTask
{
    public class Function
    {
        private AmazonDynamoDBClient _client;
        private string _table;


        public Function()
        {
            _client = new AmazonDynamoDBClient(RegionEndpoint.APSoutheast2);
            _table = Environment.GetEnvironmentVariable("TABLE_NAME");
        }

        /// <summary>
        /// Function to resolve the incident and cpmplete the workflow.
        /// All state data is persisted.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public void FunctionHandler(IncidentState state, ILambdaContext context)
        {
            state.AdminActionRequired = false;
            state.IncidentResolved = true;
            state.ResolutionDate = DateTime.Now;

            Document incidentDocument = BuildIncidentDocument(state);

            var table = Table.LoadTable(_client, _table);
            table.PutItemAsync(incidentDocument);
        }

        private static Document BuildIncidentDocument(IncidentState state)
        {
            var examsList = new DynamoDBList();

            foreach (var exam in state.Exams)
            {
                var examMap = new Document
                {
                    {"ExamId", exam.ExamId},
                    {"ExamDate", exam.ExamDate},
                    {"Score", exam.Score}
                };

                examsList.Add(examMap);
            }

            var incidentDocument = new Document
            {
                ["IncidentId"] = state.IncidentId,
                ["StudentId"] = state.StudentId,
                ["IncidentDate"] = state.IncidentDate,
                ["AdminActionRequired"] = state.AdminActionRequired,
                ["IncidentResolved"] = state.IncidentResolved,
                ["ResolutionDate"] = state.ResolutionDate,
                ["Exams"] = examsList
            };
            return incidentDocument;
        }
    }
}
