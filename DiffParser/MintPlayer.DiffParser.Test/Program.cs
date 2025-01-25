using Microsoft.Extensions.DependencyInjection;
using MintPlayer.DiffParser.Abstractions;
using MintPlayer.DiffParser.Extensions;
using System.Diagnostics;

var services = new ServiceCollection()
    .AddDiffParser()
    .BuildServiceProvider();

var diffParser = services.GetRequiredService<IDiffParser>();
//// Received from github webhook
//var diff = """
//    @@ -1,4 +1,5 @@
//    -﻿Console.WriteLine("Hello, World!");
//    +﻿
//    +Console.WriteLine("Hello, World!");
//     Console.WriteLine("Goodbye, World!");

//     var name = Greeter.GetName();
//    @@ -7,8 +8,6 @@

//     var unusedVariable = "Pieterjan";

//    -
//    -
//     // errors
//     // bli bla blu

//    @@ -18,5 +17,3 @@ public static class Greeter
//     @@
//         public static string Greet(string name) => $"Hello {name}";
//     }
//    -
//    -
//    """;


var webhook1 = @"@@ -1,10 +1,8 @@\n ﻿<Project Sdk=\""Microsoft.NET.Sdk\"">\r\n-\r\n   <PropertyGroup>\r\n     <OutputType>Exe</OutputType>\r\n     <TargetFramework>net8.0</TargetFramework>\r\n     <ImplicitUsings>enable</ImplicitUsings>\r\n     <Nullable>enable</Nullable>\r\n   </PropertyGroup>\r\n-\r\n-</Project>\r\n+</Project>\n\\ No newline at end of file";
var webhook2 = @"@@ -1,4 +1,5 @@\n-﻿Console.WriteLine(\""Hello, World!\"");\r\n+﻿\r\n+Console.WriteLine(\""Hello, World!\"");\r\n Console.WriteLine(\""Goodbye, World!\"");\r\n \r\n var name = Greeter.GetName();\r\n@@ -7,16 +8,12 @@\n \r\n var unusedVariable = \""Pieterjan\"";\r\n \r\n-\r\n-\r\n // errors\r\n // bli bla blu\r\n \r\n public static class Greeter\r\n {\r\n     public static string? GetName() => \""Pieterjan\"";\r\n-@@\r\n+dd@@\r\n     public static string Greet(string name) => $\""Hello {name}\"";\r\n }\r\n-\r\n-\r";

// Received from api.github.com - The one above is actually the same
var api = "@@ -1,4 +1,5 @@\n-﻿Console.WriteLine(\"Hello, World!\");\r\n+﻿\r\n+Console.WriteLine(\"Hello, World!\");\r\n Console.WriteLine(\"Goodbye, World!\");\r\n \r\n var name = Greeter.GetName();\r\n@@ -7,8 +8,6 @@\n \r\n var unusedVariable = \"Pieterjan\";\r\n \r\n-\r\n-\r\n // errors\r\n // bli bla blu\r\n \r\n@@ -19,4 +18,3 @@ public static class Greeter\n     public static string Greet(string name) => $\"Hello {name}\";\r\n }\r\n \r\n-\r";

var result = diffParser.Parse(api);
Debugger.Break();
