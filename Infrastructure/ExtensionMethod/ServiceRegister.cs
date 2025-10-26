using Infrastructure.FileStorage;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ExtensionMethod;

public static class ServiceRegister
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ILikeService, LikeService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IPostTagService, PostTagService>();
        services.AddScoped<IFollowService, FollowService>();
    }
}