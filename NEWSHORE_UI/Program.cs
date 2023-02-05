using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NEWSHORE_UI.Data;
using NEWSHORE_UI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI
{
  static class Constants
  {
    public const string RUTA_NIVEL_0 = "Rutas:Unicas";
    public const string RUTA_NIVEL_1 = "Rutas:Multiples";
    public const string RUTA_NIVEL_2 = "Rutas:MultipyRetorno";
  }
  public class Program
  {
    public static void Main(string[] args)
    {
      //CreateHostBuilder(args).Build().Run();
      IHost host = CreateHostBuilder(args).Build();
      CreateDbIfNotExists(host);
      host.Run();
    }

    private static void CreateDbIfNotExists(IHost host)
    {
      using (var scope = host.Services.CreateScope())
      {
        var services = scope.ServiceProvider;
        try
        {
          var context = services.GetRequiredService<JourneyysContext>();
          DbInitilizer.Initialize(context);
        }
        catch (Exception ex)
        {
          var logger = services.GetRequiredService<ILogger<Program>>();
          logger.LogError(ex, "Error al crear la DB.");
        }
      }
    }


    public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                  webBuilder.UseStartup<Startup>();
                });
  }
}
