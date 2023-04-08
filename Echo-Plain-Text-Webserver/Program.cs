using Microsoft.AspNetCore.Http.Extensions;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.Use(async (context, next) =>
{
    var req = context.Request;
    var res = context.Response;

    ConfigureResponseHeaders(res);

    res.ContentType = "text/plain";

    await WriteRequestDetailsAsync(req, res);

    async Task WriteSeparatorAsync(int numberOfSeparators = 2)
    {
        for (int i = 0; i < numberOfSeparators; i++)
        {
            await res.WriteAsync(Environment.NewLine);
        }
    }

    async Task WriteRequestDetailsAsync(HttpRequest req, HttpResponse res)
    {
        await WriteSectionAsync("Http method", req.Method);
        await WriteSectionAsync("Request Url (encoded)", req.GetEncodedUrl());
        await WriteHeadersAsync(req.Headers);
        await WriteBodyAsync(req.Body);

        async Task WriteSectionAsync(string title, string content)
        {
            await res.WriteAsync($"=== {title} ===");
            await WriteSeparatorAsync(1);
            await res.WriteAsync(content);
            await WriteSeparatorAsync();
        }

        async Task WriteHeadersAsync(IHeaderDictionary headers)
        {
            await res.WriteAsync("=== Headers ===");
            foreach (var header in headers)
            {
                await WriteSeparatorAsync(1);
                await res.WriteAsync($"{header.Key}: {header.Value}");
            }
            await WriteSeparatorAsync();
        }

        async Task WriteBodyAsync(Stream bodyStream)
        {
            using (var sr = new StreamReader(bodyStream))
            {
                var bodyAsString = await sr.ReadToEndAsync();
                if (!string.IsNullOrWhiteSpace(bodyAsString))
                {
                    await WriteSectionAsync("Body", bodyAsString);
                }
            }
        }
    }

    void ConfigureResponseHeaders(HttpResponse res)
    {
        res.Headers.Append("Access-Control-Allow-Origin", "*");
        res.Headers.Append("Access-Control-Allow-Headers", "*");
        res.Headers.Append("Access-Control-Allow-Methods", "*");
    }

    await next();
});

app.Run();
