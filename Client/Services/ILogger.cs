using System;

namespace MatchmakingMonitor.Services
{
  public interface ILogger
  {
    void Info(string message);
    void Error(string message, Exception e);
  }
}