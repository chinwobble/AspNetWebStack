﻿using System.Web.Http.Description;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using System;
using System.Linq;

namespace WebApiHelpPageWebHost.UnitTest.Helpers
{
    public static class ApiDescriptionHelpers
    {
        public static ApiDescription GetApiDescription(HttpConfiguration config, string controllerName, string actionName, params string[] parameterNames)
        {
            if (config == null)
            {
                config = new HttpConfiguration();
                config.Formatters.Clear();
                config.Formatters.Add(new XmlMediaTypeFormatter());
                config.Formatters.Add(new JsonMediaTypeFormatter());
                config.Routes.MapHttpRoute("Default", "{controller}");
            }
            HashSet<string> parameterSet = new HashSet<string>(parameterNames, StringComparer.OrdinalIgnoreCase);
            foreach (var apiDescription in config.Services.GetApiExplorer().ApiDescriptions)
            {
                HttpActionDescriptor actionDescriptor = apiDescription.ActionDescriptor;
                if (String.Equals(actionDescriptor.ControllerDescriptor.ControllerName, controllerName, StringComparison.OrdinalIgnoreCase) &&
                    String.Equals(actionDescriptor.ActionName, actionName, StringComparison.OrdinalIgnoreCase))
                {
                    HashSet<string> actionParameterSet = new HashSet<string>(actionDescriptor.GetParameters().Select(p => p.ParameterName), StringComparer.OrdinalIgnoreCase);
                    if (parameterSet.SetEquals(actionParameterSet))
                    {
                        return apiDescription;
                    }
                }
            }

            return null;
        }
    }
}