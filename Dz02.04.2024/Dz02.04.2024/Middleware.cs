using Microsoft.AspNetCore.Http;
using System.Numerics;

namespace Dz02._04._2024 {
    public class FromOneToTenMiddleware {
        private readonly RequestDelegate? _next;
        public FromOneToTenMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext context) {
            string? token = context.Request.Query["number"];
            try {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);
                if (number == 10) await context.Response.WriteAsync("Your number is ten");
                else {
                    string[] Ones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };
                    string existingResult = context.Session.GetString("number") ?? "", thousandResult = context.Session.GetString("thousand") ?? "", thousandProcess = context.Session.GetString("thousandProcess") ?? "true";
                    switch (number) {
                        case int n when n >= 20000 && thousandProcess == "true":
                            int two = int.Parse((n / 1000).ToString().Substring(1));
                            thousandResult += $" {Ones[two - 1]}";
                            context.Session.SetString("thousand", thousandResult);
                            break;
                        case int n when n >= 1000 && thousandProcess == "true":
                            context.Session.SetString("thousand", $" {Ones[n / 1000 - 1]}");
                            break;
                        case int n when n > 20:
                            string str = token!.Substring(token.Length - 1);
                            int lastNumber = int.Parse(str);
                            if (lastNumber != 0) existingResult += $" {Ones[lastNumber - 1]}";
                            else existingResult += $" ten";
                            await context.Response.WriteAsync("Your number is " + existingResult);
                            context.Session.Remove("number");
                            break;
                        default:
                            existingResult += $" {Ones[number - 1]}";
                            await context.Response.WriteAsync("Your number is " + existingResult);
                            context.Session.Remove("number");
                            break;
                    }
                }
            } catch (Exception) { await context.Response.WriteAsync("Incorrect parameter"); }
        }
    }
    public class FromElevenToNineteenMiddleware {
        private readonly RequestDelegate? _next;
        public FromElevenToNineteenMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext context) {
            string? token = context.Request.Query["number"];
            if (token == null) return;
            try {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);
                if (number < 11) await _next!.Invoke(context);
                string[] nums = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                string existingResult = context.Session.GetString("number") ?? "", thousandResult = context.Session.GetString("thousand") ?? "", thousandProcess = context.Session.GetString("thousandProcess") ?? "false";
                int lastNumber = Convert.ToInt32(token.Substring(token.Length - 2));
                switch (number) {
                    case int n when number >= 11_000 && number <= 19_000 && thousandProcess == "true":
                        thousandResult += $" {nums[(n / 1000) - 11]}";
                        context.Session.SetString("thousand", thousandResult);
                        break;
                    case int n when lastNumber >= 11 && lastNumber <= 19 && thousandProcess == "false":
                        existingResult += $" {nums[(n / 1000) - 11]}";
                        await context.Response.WriteAsync("Your number is " + existingResult);
                        context.Session.Remove("number");
                        break;
                    default:
                        await _next!.Invoke(context);
                        break;
                }
            } catch(Exception) { await context.Response.WriteAsync("Incorrect parameter!"); }
        }
    }
    public class FromTwentyToHundredMiddleware {
        private readonly RequestDelegate? _next;
        public FromTwentyToHundredMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext context) {
            string? token = context.Request.Query["number"];
            try {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);
                if (number < 20) await _next!.Invoke(context);
                else {
                    string[] nums = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
                    string existingResult = context.Session.GetString("number") ?? "", thousandResult = context.Session.GetString("thousand") ?? "", thousandProcess = context.Session.GetString("thousandProcess") ?? "false";
                    switch (number) {
                        case int n when n >= 20_000 && thousandProcess == "true":
                            thousandResult += $" {nums[(n / 1000) / 10 - 2]}";
                            context.Session.SetString("thousand", thousandResult);
                            if (n % 1000 != 0) await _next!.Invoke(context);
                            break;
                        case int n when n >= 20 && thousandProcess == "false":
                            if (token!.Substring(token.Length - 2, 1) != "0" && token.Substring(token.Length - 2, 1) != "1") {
                                int lastNumber = int.Parse(token.Substring(token.Length - 2));
                                existingResult += $" {nums[lastNumber / 10 - 2]}";
                                if (number % 10 == 0) {
                                    await context.Response.WriteAsync("Your number is " + existingResult);
                                    context.Session.Remove("number");
                                }
                                else {
                                    context.Session.SetString("number", existingResult);
                                    await _next!.Invoke(context);
                                }
                            }
                            else await _next!.Invoke(context);
                            break;
                        default:
                            await _next!.Invoke(context);
                            break;

                    }
                }
            } catch(Exception) { await context.Response.WriteAsync("Incorrect parameter!"); }
        }
    }
    public class FromHundredToThousandMiddleware {
        private readonly RequestDelegate? _next;
        public FromHundredToThousandMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext context) {
            string? token = context.Request.Query["number"];
            try {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);
                if (number < 100) await _next!.Invoke(context);
                else {
                    string[] nums = { "one hundred", "two hundred", "three hundred", "four hundred", "five hundred", "six hundred", "seven hundred", "eight hundred", "nine hundred" };
                    string existingResult = context.Session.GetString("number") ?? "", thousandResult = context.Session.GetString("thousand") ?? "", thousandProcess = context.Session.GetString("thousandProcess") ?? "false";
                    switch (number) {
                        case int n when n >= 100000 && thousandProcess == "true":
                            thousandResult += $" {nums[(n / 1000) - 1]}";
                            thousandResult = thousandResult.Replace("hundred", "");
                            context.Session.SetString("thousand", thousandResult);
                            if ((n / 1000) < 10) break;
                            else if ((n / 1000) % 100 != 0) await _next!.Invoke(context);
                            break;
                        case int n when n >= 100 && thousandProcess == "false":
                            string str = token!.Substring(token.Length - 3);
                            if (str.Substring(0, 1) == "0") await _next!.Invoke(context);
                            else {
                                int lastNumber = Convert.ToInt32(token.Substring(token.Length - 3));
                                existingResult += $" {nums[lastNumber / 100 - 1]}";
                                if (lastNumber % 100 == 0) {
                                    await context.Response.WriteAsync("Your number is " + existingResult);
                                    context.Session.Remove("number");
                                }
                                else {
                                    context.Session.SetString("number", existingResult);
                                    await _next!.Invoke(context);
                                }
                            }
                            break;
                        default:
                            await _next!.Invoke(context);
                            break;
                    }
                }
            }
            catch (Exception) { await context.Response.WriteAsync("Incorrect parameter!"); }
        }
    }
    public class FromThousandToHundredThousandMiddleware {
        private readonly RequestDelegate? _next;
        public FromThousandToHundredThousandMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext context) {
            string? token = context.Request.Query["number"];
            if (token == null) return;
            try {
                context.Session.Remove("number");
                context.Session.Remove("thousandProcess");
                context.Session.Remove("thousand");
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);
                if (number < 1000) await _next!.Invoke(context); 
                else {
                    context.Session.SetString("thousandProcess", "true");
                    string existingResult = context.Session.GetString("number") ?? "";
                    await _next!.Invoke(context);
                    string thousandResult = context.Session.GetString("thousand") ?? "";
                    existingResult += $"{thousandResult} thousand";
                    context.Session.SetString("thousandProcess", "false");
                    if (number % 1000 == 0) {
                        await context.Response.WriteAsync("Your number is " + existingResult);
                        context.Session.Remove("number");
                        context.Session.Remove("thousandProcess");
                        context.Session.Remove("thousand");
                    } else {
                        context.Session.SetString("number", existingResult);
                        await _next.Invoke(context);
                    }
                }
            }
            catch (Exception) { await context.Response.WriteAsync("Incorrect parameter!"); }
        }
    }
}