using RestaurantsReview.Server.Controllers;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RestaurantsReview.SwaggerClientGenerator
{
	/// <summary>
	/// Generate Swagger REST client which is used on client to invoke methods in OpenAPI on server over the internet.
	/// </summary>
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("BlazorClientInfoGenerator start");
			GenerateBlazorClientInfo();
			Console.WriteLine("BlazorClientInfoGenerator completed");

			Console.WriteLine("SwaggerClientGenerator start");
			GenerateRestClient(true, true);
			Console.WriteLine("SwaggerClientGenerator completed");
		}

		static void WriteFileIfChanged(string filePath, string newContent)
		{
			string prev1 = File.Exists(filePath) ? File.ReadAllText(filePath, Encoding.UTF8) : null;
			if (prev1 != newContent)
			{
				File.WriteAllText(filePath, newContent, Encoding.UTF8);
			}
		}

		static string FindProjectDirectory()
		{
			string dir1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string projectFilePath = File.ReadAllText(Path.Combine(dir1, "ProjectFileLocation.txt")).Trim();
			return Path.GetDirectoryName(projectFilePath);
		}

		static void GenerateBlazorClientInfo()
		{
			string code = CreateBlazorClientInfoClass();
			WriteFileIfChanged(Path.Combine(FindProjectDirectory(), "..\\Client\\BlazorClientInfo.cs"), code);
		}

		static string CreateBlazorClientInfoClass()
		{
			return $@"namespace RestaurantsReview.Client
{{
	public static class BlazorClientInfo
	{{
		public const string Version = ""{Guid.NewGuid()}"";
	}}
}}
";
		}

		static void GenerateRestClient(bool cSharp, bool typeScript)
		{
			// we don't have to search for controller, we know it's type
			//var controllers = NSwag.Generation.WebApi.WebApiOpenApiDocumentGenerator.GetControllerClasses(typeof(RestaurantsReviewController).Assembly).ToList();

			var sett = new NSwag.Generation.WebApi.WebApiOpenApiDocumentGeneratorSettings()
			{
				IsAspNetCore = true
			};

			NSwag.Generation.WebApi.WebApiOpenApiDocumentGenerator gen = new NSwag.Generation.WebApi.WebApiOpenApiDocumentGenerator(sett);

			var doc1 = gen.GenerateForControllerAsync(typeof(RestaurantsReviewController)).Result;

			if (cSharp)
			{
				var sett2 = new NSwag.CodeGeneration.CSharp.CSharpClientGeneratorSettings();
				sett2.GenerateDtoTypes = true;
				sett2.ClientClassAccessModifier = "public";
				var codeSett = (NJsonSchema.CodeGeneration.CSharp.CSharpGeneratorSettings)sett2.CodeGeneratorSettings;
				codeSett.Namespace = "RestaurantsReview.ApiClient";
				NSwag.CodeGeneration.CSharp.CSharpClientGenerator gen2 = new NSwag.CodeGeneration.CSharp.CSharpClientGenerator(doc1, sett2);
				var csharpClientContent = gen2.GenerateFile();

				WriteFileIfChanged(Path.Combine(FindProjectDirectory(), "..\\Client\\Services\\RestaurantsReviewClient.cs"), csharpClientContent);
				Console.WriteLine("Generated C# client");
			}

			if (typeScript)
			{
				var sett3 = new NSwag.CodeGeneration.TypeScript.TypeScriptClientGeneratorSettings();
				sett3.GenerateDtoTypes = true;

				var codeSett = (NJsonSchema.CodeGeneration.TypeScript.TypeScriptGeneratorSettings)sett3.CodeGeneratorSettings;
				codeSett.Namespace = "RestaurantsReview.ApiClient";

				NSwag.CodeGeneration.TypeScript.TypeScriptClientGenerator gen3 = new NSwag.CodeGeneration.TypeScript.TypeScriptClientGenerator(doc1, sett3);

				var tsClientCode = gen3.GenerateFile();
				WriteFileIfChanged(Path.Combine(FindProjectDirectory(), "..\\Client\\Services\\RestaurantsReviewClient.ts"), tsClientCode);
				Console.WriteLine("Generated TS client");
			}
		}

	}
}
