var path = @"C:\Users\hoang\.kube\config";
Console.WriteLine($"Testing kubeconfig at: {path}");
Console.WriteLine($"File exists: {File.Exists(path)}");

if (!File.Exists(path))
{
    Console.WriteLine("File not found!");
    return;
}

var yaml = File.ReadAllText(path);
Console.WriteLine($"\nYAML content length: {yaml.Length} bytes\n");

Console.WriteLine("=== Attempting to deserialize ===");
try
{
    var deserializer = new DeserializerBuilder().Build();
    var config = deserializer.Deserialize<Dictionary<string, object>>(yaml);
    
    Console.WriteLine($"Keys found: {string.Join(", ", config.Keys)}");
    
    if (config.ContainsKey("contexts"))
    {
        Console.WriteLine($"\nContexts key EXISTS!");
        var contextsObj = config["contexts"];
        Console.WriteLine($"Type: {contextsObj.GetType().FullName}");
        
        if (contextsObj is List<object> contexts)
        {
            Console.WriteLine($"Contexts count: {contexts.Count}");
            
            var names = contexts
                .OfType<Dictionary<object, object>>()
                .Where(c => c.ContainsKey("name"))
                .Select(c => c["name"]?.ToString())
                .Where(n => !string.IsNullOrEmpty(n))
                .ToList();
            
            Console.WriteLine($"\n✅ SUCCESS! Context names: {string.Join(", ", names)}");
        }
        else
        {
            Console.WriteLine($"❌ ERROR: contexts is {contextsObj.GetType()}, not List<object>");
        }
    }
    else
    {
        Console.WriteLine("\n❌ ERROR: No 'contexts' key found!");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"\n❌ EXCEPTION: {ex.Message}");
    Console.WriteLine($"Type: {ex.GetType().Name}");
    Console.WriteLine($"Stack:\n{ex.StackTrace}");
}
