using QuickJob.Notifications.DataModel.Entities.Mongo;

namespace QuickJob.Notifications.Core.Extensions;

public static class TemplateExtensions
{
    public static void SetVariables(this Template template, Dictionary<string, string> variables)
    {
        if (variables.Count == 0)
            return;
        
        foreach (var variable in variables) 
            template.Body = template.Body.Replace($"{{{{{variable.Key}}}}}", variable.Value);
    }
    
}