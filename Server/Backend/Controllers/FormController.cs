using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using Backend.Models;

namespace Backend.Controllers
{
  [RoutePrefix("form")]
  public class FormController : ApiController
  {
    [Route("submit")]
    public async Task<bool> Submit(Form form)
    {
      try
      {
        using (var fs = File.OpenWrite(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
          form.Mode == FormMode.Bug ? "bugs" : "features", $"{form.Mode}_{EncodeAsFileName(form.Title)}.txt")))
        using (var sw = new StreamWriter(fs, Encoding.UTF8))
        {
          await sw.WriteLineAsync(form.Title);
          await sw.WriteLineAsync();
          await sw.WriteLineAsync(form.Contact);
          await sw.WriteLineAsync();
          await sw.WriteLineAsync(form.Message);
        }
        return true;
      }
      catch
      {
        return false;
      }
    }

    private static string EncodeAsFileName(string text)
    {
      return Regex.Replace(text, "[" + Regex.Escape(
                                   new string(Path.GetInvalidFileNameChars())) + "]", " ");
    }
  }
}