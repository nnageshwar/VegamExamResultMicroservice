namespace FinalExamlResult.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            //app.MapGet("/", () => "Hello World!");

            // Enable static file serving (e.g., index.html)
            app.UseStaticFiles();

            // Optional: Map a custom route for root ("/") to serve index.html explicitly
            app.MapGet("/", () => Results.Redirect("/index.html"));


            app.Run();
        }
    }
}
