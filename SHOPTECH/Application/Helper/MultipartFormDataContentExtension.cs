using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Helper
{
    public static class MultipartFormDataContentExtension
    {
        public static MultipartFormDataContent Create<T>(this MultipartFormDataContent multiContent, T t) where T: class
        {
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(t);
                if (value != null)
                {
                    // Xử lý các trường bình thường
                    if (property.PropertyType != typeof(IFormFile))
                    {
                        multiContent.Add(new StringContent(value.ToString() ?? string.Empty), property.Name);
                    }

                    // Xử lý file
                    else if (property.PropertyType == typeof(IFormFile))
                    {
                        var file = value as IFormFile;
                        if (file != null && file.Length > 0)
                        {
                            var fileContent = new StreamContent(file.OpenReadStream());
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                            multiContent.Add(fileContent, property.Name, file.FileName);
                        }
                    }
                }
            }

            return multiContent;
        }
    }
}
