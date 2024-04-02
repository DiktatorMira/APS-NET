namespace Dz02._04._2024 {
    public static class FromOneToTenMiddlewareExtensions{
        public static IApplicationBuilder UseFromOneToTen(this IApplicationBuilder builder) {
            return builder.UseMiddleware<FromOneToTenMiddlewareMiddleware>();
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
    public static class FromHundredOneToThousandExtensions {
        public static IApplicationBuilder UseFromHundredOneToThousand(this IApplicationBuilder builder) {
            return builder.UseMiddleware<FromHundredOneToThousandMiddleware>();
        }
    }
    public static class FromThousandOneToTenThousandExtensions {
        public static IApplicationBuilder UseFromThousandOneToTenThousand(this IApplicationBuilder builder) {
            return builder.UseMiddleware<FromThousandOneToTenThousandMiddleware>();
        }
    }
    public static class FromTenThousandOneToHundredThousandExtensions {
        public static IApplicationBuilder UseFromTenThousandOneToHundredThousandExtensions(this IApplicationBuilder builder) {
            return builder.UseMiddleware<FromTenThousandOneToHundredThousandMiddleware>();
        }
    }
}