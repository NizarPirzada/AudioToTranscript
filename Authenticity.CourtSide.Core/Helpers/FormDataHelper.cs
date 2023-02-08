using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Authenticity.CourtSide.Core.Helpers
{
    public static class FormDataHelper
    {
        public static MultipartFormDataContent GetMultiparContent<T>(this T obj)
        {
            var properties = typeof(T).GetProperties();
            MultipartFormDataContent formData = new MultipartFormDataContent();
            foreach (var prop in properties)
            {
                if (prop.GetValue(obj) != null)
                {
                    if (typeof(IEnumerable<int>).IsAssignableFrom(prop.PropertyType) || typeof(IEnumerable<string>).IsAssignableFrom(prop.PropertyType))
                    {
                        foreach (var item in (IEnumerable<int>)prop.GetValue(obj))
                        {
                            formData.Add(new StringContent(item.ToString()), (prop.Name + "[]"));
                        }
                    }
                    else if (typeof(IEnumerable<object>).IsAssignableFrom(prop.PropertyType))
                    {
                        int i = 0;
                        foreach (object item in (IEnumerable<object>)prop.GetValue(obj))
                        {
                            foreach (var itemProp in item.GetType().GetProperties())
                            {
                                formData.Add(new StringContent(itemProp.GetValue(item).ToString()), ($"{prop.Name}[{i}].{itemProp.Name}"));
                            }
                            i++;
                        }
                    }
                    else if (typeof(Stream).IsAssignableFrom(prop.PropertyType))
                    {
                        HttpContent fieStreamContent = new StreamContent((Stream)prop.GetValue(obj));
                        formData.Add(fieStreamContent, prop.Name, ((FileStream)prop.GetValue(obj)).Name);
                    }
                    else
                    {
                        formData.Add(new StringContent(prop.GetValue(obj).ToString()), prop.Name);
                    }
                }
            }

            return formData;
        }
    }
}
