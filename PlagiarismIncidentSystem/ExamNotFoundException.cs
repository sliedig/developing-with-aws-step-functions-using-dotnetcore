using System;

namespace PlagiarismIncidentSystem
{
  public class ExamNotFoundException : Exception
  {
    public ExamNotFoundException()
    { 
    }
    public ExamNotFoundException(string message) : base(message)
    {
    }
  }
}
