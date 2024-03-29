﻿using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Threading.Tasks;
using System.Text;
using YamlDotNet.Serialization;

namespace APIAspNetCore5.Formatter
{
    public class YamlInputFormatter : TextInputFormatter
    {
        private readonly Deserializer _deserializer;

        public YamlInputFormatter(Deserializer serializer)
        {
            _deserializer = serializer;
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationYaml);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.TextYaml);
        }
        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var request = context.HttpContext.Request;

            using (var streamReader = context.ReaderFactory(request.Body, encoding))
            {
                var type = context.ModelType;

                try
                {
                    var model = _deserializer.Deserialize(streamReader, type);
                    return InputFormatterResult.SuccessAsync(model);
                }
                catch (Exception)
                {
                    return InputFormatterResult.FailureAsync();
                }
            }
        }
    }
}

// https://dejanstojanovic.net/aspnet/2018/september/custom-input-and-output-serializers-in-aspnet-core/