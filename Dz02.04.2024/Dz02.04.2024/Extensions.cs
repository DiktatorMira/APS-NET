namespace Dz02._04._2024 {
    public static class FromOneToTenMiddlewareExtensions{
        public static IApplicationBuilder UseFromOneToTen(this IApplicationBuilder builder) {
            return builder.UseMiddleware<FromOneToTenMiddleware>();
        }
    }
    public static class FromElevenToNineteenExtensions {
        public static IApplicationBuilder UseFromElevenToNineteen(this IApplicationBuilder builder) {
            return builder.UseMiddleware<FromElevenToNineteenMiddleware>();
        }
    }
    public static class FromTwentyToHundredExtensions {
        public static IApplicationBuilder UseFromTwentyToHundred(this IApplicationBuilder builder) {
            return builder.UseMiddleware<FromTwentyToHundredMiddleware>();
        }
    }
    public static class FromHundredToThousandExtensions {
        public static IApplicationBuilder UseFromHundredToThousand(this IApplicationBuilder builder) {
            return builder.UseMiddleware<FromHundredToThousandMiddleware>();
        }
    }
    public static class FromThousandToHundredThousandExtensions {
        public static IApplicationBuilder UseFromThousandToHundredThousand(this IApplicationBuilder builder) {
            return builder.UseMiddleware<FromThousandToHundredThousandMiddleware>();
        }
    }
}