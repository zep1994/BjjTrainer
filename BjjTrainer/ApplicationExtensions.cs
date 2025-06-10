namespace BjjTrainer
{
    public static class ApplicationExtensions
    {
        public static IServiceProvider Services(this Application application)
        {
            if (application.Handler?.MauiContext?.Services == null)
            {
                throw new InvalidOperationException("The application is not properly initialized. Services cannot be accessed.");
            }

            return application.Handler.MauiContext.Services;
        }
    }
}
