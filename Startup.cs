using System.Text;
using Dapper;
using IMDBApi_Assignment4.Helpers;
using IMDBApi_Assignment4.Middleware;
using IMDBApi_Assignment4.Repositories;
using IMDBApi_Assignment4.Repository;
using IMDBApi_Assignment4.Repository.Interface;
using IMDBApi_Assignment4.Services;
using IMDBApi_Assignment4.Services.Interface;
using IMDBApi_Assignment4.Validations;
using IMDBApi_Assignment4.Validations.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


namespace IMDBApi_Assignment4
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:ValidIssuer"],
                    ValidAudience = _configuration["Jwt:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]))
                };
            });


            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Error;
                });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IMDBApi", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddScoped<IActorService, ActorService>();
            services.AddScoped<IActorRepository, ActorRepository>();
            services.AddScoped<IProducerService, ProducerService>();
            services.AddScoped<IProducerRepository, ProducerRepository>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IActorValidation, ActorValidation>();
            services.AddScoped<IProducerValidation, ProducerValidation>();
            services.AddScoped<IGenreValidation, GenreValidation>();
            services.AddScoped<IReviewValidation, ReviewValidation>();
            services.AddScoped<IMovieValidation, MovieValidation>();
            services.AddScoped<IUserValidation, UserValidation>();
            services.AddSingleton<ISupabaseService, SupabaseService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IMDBApi v1"));
            }
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}