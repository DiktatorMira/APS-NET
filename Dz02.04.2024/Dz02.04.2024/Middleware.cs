using Microsoft.AspNetCore.Http;

namespace Dz02._04._2024 {
    public class FromOneToTenMiddlewareMiddleware {
        private readonly RequestDelegate? _next;
        public FromOneToTenMiddlewareMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext context) {
            try {
                int number = Convert.ToInt32(context.Request.Query["number"]);
                number = Math.Abs(number);
                if (number == 10) await context.Response.WriteAsync("Number equals 10.");
                else {
                    if (number > 20) context.Session.SetString("number", number.ToString());
                    else await context.Response.WriteAsync($"Number equals {number}.");
                }
            } catch (Exception) { await context.Response.WriteAsync("Incorrect parameter!"); }
        }
    }
    public class FromElevenToNineteenMiddleware {
        private readonly RequestDelegate? _next;
        public FromElevenToNineteenMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext context) {
            try {
                int number = Convert.ToInt32(context.Request.Query["number"]);
                number = Math.Abs(number);
                if (number < 11 || number > 19) await _next!.Invoke(context);
                else await context.Response.WriteAsync($"Your number is {number}.");
            } catch(Exception) { await context.Response.WriteAsync("Incorrect parameter!"); }
        }
    }
    public class FromTwentyToHundredMiddleware {
        private readonly RequestDelegate? _next;
        public FromTwentyToHundredMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext context) {
            try {
                int number = Convert.ToInt32(context.Request.Query["number"]);
                number = Math.Abs(number);
                if (number < 20) await _next!.Invoke(context);
                else await context.Response.WriteAsync($"Your number is {number}.");
            } catch(Exception) { await context.Response.WriteAsync("Incorrect parameter!"); }
        }
    }
    public class FromHundredOneToThousandMiddleware {
        private readonly RequestDelegate? _next;
        public FromHundredOneToThousandMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext context) {
            try {
                int number = Convert.ToInt32(context.Request.Query["number"]);
                number = Math.Abs(number);
                if (number < 101) await _next!.Invoke(context);
                else await context.Response.WriteAsync($"Your number is {number}.");
            }
            catch (Exception) { await context.Response.WriteAsync("Incorrect parameter!"); }
        }
    }
    public class FromThousandOneToTenThousandMiddleware {
        private readonly RequestDelegate? _next;
        public FromThousandOneToTenThousandMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext context) {
            try {
                int number = Convert.ToInt32(context.Request.Query["number"]);
                number = Math.Abs(number);
                if (number < 1001) await _next!.Invoke(context);
                else await context.Response.WriteAsync($"Your number is {number}.");
            }
            catch (Exception) { await context.Response.WriteAsync("Incorrect parameter!"); }
        }
    }
    public class FromTenThousandOneToHundredThousandMiddleware {
        private readonly RequestDelegate? _next;
        public FromTenThousandOneToHundredThousandMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext context) {
            try {
                int number = Convert.ToInt32(context.Request.Query["number"]);
                number = Math.Abs(number);
                if (number < 10001) await _next!.Invoke(context);
                else if (number > 100000) await context.Response.WriteAsync($"Your number is bigger than 100000.");
                else await context.Response.WriteAsync($"Your number is {number}.");
            }
            catch (Exception) { await context.Response.WriteAsync("Incorrect parameter!"); }
        }
    }
}