using Hotel.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
//using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Hotel
{
    public static class ServiceExtensions
    {

        //هر متدی که درونش از 
        //(this IServiceCollection services) 
        //استفاده شده است می تواند در پروگرم  با زدن کمه ی 
        //service.ConfigureIdentity() 
        //اسم متد استفاده شود
        public static void  ConfigureIdentity(this IServiceCollection services) 
        {

            //این تنظیم معمولاً برای جلوگیری از ایجاد حساب کاربری با ایمیل تکراری استفاده می‌شود.
            var builer=services.AddIdentityCore<ApiUser>(x=>x.User.RequireUniqueEmail=true);

                builer= new IdentityBuilder(builer.UserType, typeof(IdentityRole), services);

            builer.AddEntityFrameworkStores<DataBaseContext>().AddDefaultTokenProviders();
        
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSetting = configuration.GetSection("Jwt");
            // var key = Environment.GetEnvironmentVariable("KEY");
            var key = jwtSetting.GetSection("key").Value;

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters//برای هر کار بر یک توکن جدید می سازد
                    {
                        ValidateIssuer = true,//اگر کسی یکتوکن غیر معتبر بسازد و بفرستد
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,//از کلید بالا در آن استفاده شده است
                        ValidIssuer = jwtSetting.GetSection("Issuer").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

                    };
                    o.Audience = jwtSetting.GetSection("Issuer").Value;
                });
        }
    }
}
