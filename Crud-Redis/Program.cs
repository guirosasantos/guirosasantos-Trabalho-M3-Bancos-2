using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/get", (string key) =>
{
    var redis = app.Services.GetRequiredService<IConnectionMultiplexer>();
    var db = redis.GetDatabase();
    var value = db.StringGet(key);
    return Results.Ok((string)value);
})
.WithName("GetRedisData")
.WithOpenApi();

app.MapPost("/post", (string key, string value) =>
{
    var redis = app.Services.GetRequiredService<IConnectionMultiplexer>();
    var db = redis.GetDatabase();
    db.StringSet(key, value);
    return Results.Ok();
})
.WithName("SetRedisData")
.WithOpenApi();

app.MapPut("/put", (string key, string value) =>
{
    var redis = app.Services.GetRequiredService<IConnectionMultiplexer>();
    var db = redis.GetDatabase();
    db.StringSet(key, value);
    return Results.Ok();
})
.WithName("UpdateRedisData")
.WithOpenApi();

app.MapDelete("/delete", (string key) =>
{
    var redis = app.Services.GetRequiredService<IConnectionMultiplexer>();
    var db = redis.GetDatabase();
    db.KeyDelete(key);
    return Results.Ok();
})
.WithName("DeleteRedisData")
.WithOpenApi();

app.Run();
