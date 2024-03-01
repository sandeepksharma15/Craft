using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Craft.Generators;

[Generator]
public class PermissionsSourceGenerator : IIncrementalGenerator
{
    private static void CreateModuleItems(byte moduleId, List<ModuleItem> moduleItems, string itemName, string actions, ref byte itemId)
    {
        switch (actions)
        {
            case "UserAction.CanCreate":
                moduleItems.Add(new ModuleItem(moduleId, itemId++, itemName, "Create"));
                break;

            case "UserAction.CanRead":
                moduleItems.Add(new ModuleItem(moduleId, itemId++, itemName, "Read"));
                break;

            case "UserAction.CanUpdate":
                moduleItems.Add(new ModuleItem(moduleId, itemId++, itemName, "Update"));
                break;

            case "UserAction.CanDelete":
                moduleItems.Add(new ModuleItem(moduleId, itemId++, itemName, "Delete"));
                break;

            case "UserAction.CanToggleActivate":
                moduleItems.Add(new ModuleItem(moduleId, itemId++, itemName, "ToggleActivate"));
                break;

            case "UserAction.CanDoCrud":
                moduleItems.Add(new ModuleItem(moduleId, itemId++, itemName, "Create"));
                moduleItems.Add(new ModuleItem(moduleId, itemId++, itemName, "Read"));
                moduleItems.Add(new ModuleItem(moduleId, itemId++, itemName, "Update"));
                moduleItems.Add(new ModuleItem(moduleId, itemId++, itemName, "Delete"));
                break;

            case "UserAction.CanDoAll":
                moduleItems.Add(new ModuleItem(moduleId, itemId++, itemName, "Create"));
                moduleItems.Add(new ModuleItem(moduleId, itemId++, itemName, "Read"));
                moduleItems.Add(new ModuleItem(moduleId, itemId++, itemName, "Update"));
                moduleItems.Add(new ModuleItem(moduleId, itemId++, itemName, "Delete"));
                moduleItems.Add(new ModuleItem(moduleId, itemId++, itemName, "ToggleActivate"));
                break;
        }
    }

    private static void Execute(SourceProductionContext context, ClassDeclarationSyntax classDeclarationSyntax)
    {
        // Generate Class Name
        const string nameSpaceName = "Craft.Security.Authorization";
        const string className = "Permissions";
        var fileName = $"{classDeclarationSyntax.Identifier.Text}.g.cs";

        var stringBuilder = new StringBuilder();

        // Genrate NameSpace
        stringBuilder.Append("namespace ").Append(nameSpaceName).AppendLine(";");
        stringBuilder.AppendLine();

        // Genrate Class Name
        stringBuilder.Append("public static partial class ").Append(className).AppendLine(" {");

        // Genrate Class Body
        foreach (var member in classDeclarationSyntax.Members)
        {
            if (member is not FieldDeclarationSyntax field)
                continue;

            // Get The Variable Type
            var variableType = field.Declaration.Type;

            if (variableType is not GenericNameSyntax genericNameSyntax || genericNameSyntax.Identifier.Text != "List")
                continue;

            // Get the List Type
            variableType = genericNameSyntax.TypeArgumentList.Arguments.First();

            if (variableType is not IdentifierNameSyntax identifierNameSyntax || identifierNameSyntax.Identifier.Text != "ModuleInfo")
                continue;

            // Get The Variable Initializer Value
            var variableValue = (ImplicitObjectCreationExpressionSyntax)field.Declaration.Variables.First().Initializer.Value;
            var variableInitializer = variableValue.Initializer;
            var appModules = variableInitializer.Expressions;

            foreach (var appModule in appModules)
            {
                byte moduleId = 0;
                string moduleName = string.Empty;
                List<ModuleItem> moduleItems = [];

                var appModuleInitializer = ((ObjectCreationExpressionSyntax)appModule).Initializer;

                foreach (var initItem in appModuleInitializer.Expressions)
                {
                    // If initItem Is An Assignment, Get The Value Of Left And Right Expression
                    if (initItem is not AssignmentExpressionSyntax assignmentExpressionSyntax)
                        continue;
                    if (assignmentExpressionSyntax.Left is not IdentifierNameSyntax leftIdentifierNameSyntax)
                        continue;

                    if (leftIdentifierNameSyntax.Identifier.Text == "Id")
                        moduleId = GetId(assignmentExpressionSyntax);
                    else if (leftIdentifierNameSyntax.Identifier.Text == "Name")
                        moduleName = ((LiteralExpressionSyntax)assignmentExpressionSyntax.Right).Token.ValueText;
                    else if (leftIdentifierNameSyntax.Identifier.Text == "Items")
                        moduleItems = GetModuleItems(assignmentExpressionSyntax, moduleId);
                }

                foreach (var moduleItem in moduleItems)
                    stringBuilder
                        .Append("    ")
                        .Append("public const short ")
                        .Append(moduleName.ToUpper())
                        .Append("_")
                        .Append(moduleItem.Name.ToUpper())
                        .Append("_")
                        .Append(moduleItem.ActionName.ToUpper())
                        .Append(" = ")
                        .Append(moduleItem.Id)
                        .AppendLine(";");
            }
        }

        // Genrate Class End
        stringBuilder.AppendLine("}");

        context.AddSource(fileName, stringBuilder.ToString());
    }

    private static byte GetId(AssignmentExpressionSyntax assignmentExpressionSyntax)
    {
        // Get The Value Of The Right Expression
        var idValue = ((LiteralExpressionSyntax)assignmentExpressionSyntax.Right).Token.ValueText;

        // Parse The Value To Byte
        return byte.Parse(idValue);
    }

    private static List<ModuleItem> GetModuleItems(AssignmentExpressionSyntax assignmentExpressionSyntax, byte moduleId)
    {
        var moduleItems = new List<ModuleItem>();

        var listInitializer = (ObjectCreationExpressionSyntax)assignmentExpressionSyntax.Right;

        foreach (var item in listInitializer.Initializer.Expressions)
        {
            var moduleItemInitializer = ((ImplicitObjectCreationExpressionSyntax)item).Initializer;
            var itemName = string.Empty;
            byte itemId = 0;

            foreach (var expr in moduleItemInitializer.Expressions)
            {
                if (expr is not AssignmentExpressionSyntax assignmentExpression)
                    continue;
                if (assignmentExpression.Left is not IdentifierNameSyntax leftIdentifierNameSyntax)
                    continue;

                if (leftIdentifierNameSyntax.Identifier.Text == "Id")
                    itemId = GetId(assignmentExpression);
                else if (leftIdentifierNameSyntax.Identifier.Text == "Name")
                    itemName = ((LiteralExpressionSyntax)assignmentExpression.Right).Token.ValueText;
                else if (leftIdentifierNameSyntax.Identifier.Text == "PossibleActions")
                {
                    string actions = ((MemberAccessExpressionSyntax)assignmentExpression.Right).ToString();
                    CreateModuleItems(moduleId, moduleItems, itemName, actions, ref itemId);
                }
            }
        }

        return moduleItems;
    }

    private static ClassDeclarationSyntax GetSemanticTarget(GeneratorSyntaxContext context)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;
        var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);
        var attributeSymbol = context.SemanticModel.Compilation
            .GetTypeByMetadataName("Craft.Generators.GeneratePermissionsAttribute");

        if (classSymbol is null || attributeSymbol is null)
            return null;

        foreach (var attributeData in classSymbol.GetAttributes())
            if (attributeSymbol.Equals(attributeData.AttributeClass, SymbolEqualityComparer.Default))
                return classDeclarationSyntax;

        return null;
    }

    private static bool IsSyntaxTarget(SyntaxNode node)
    {
        return node is ClassDeclarationSyntax classDeclarationSyntax &&
            classDeclarationSyntax.AttributeLists.Count > 0;
    }

    private static void PostInitializationOutput(IncrementalGeneratorPostInitializationContext context)
    {
        // Setup Constants
        const string nameSpaceName = "Craft.Generators";
        const string className = "GeneratePermissionsAttribute";
        var fileName = $"{className}.g.cs";

        var stringBuilder = new StringBuilder();

        // Generate Namespace
        stringBuilder
            .Append("namespace ")
            .Append(nameSpaceName)
            .AppendLine(";")
            .AppendLine();

        // Genrate Class Name
        stringBuilder
            .Append("internal class ")
            .Append(className)
            .AppendLine(" : System.Attribute")
            .AppendLine("{");

        // Genrate Class End
        stringBuilder.AppendLine("}");

        // Create The File
        context.AddSource(fileName, stringBuilder.ToString());
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classes = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: static (node, _) => IsSyntaxTarget(node),
            transform: static (ctx, _) => GetSemanticTarget(ctx))
            .Where(static (target) => target is not null);

        context.RegisterSourceOutput(classes,
            static (ctx, source) => Execute(ctx, source));

        context.RegisterPostInitializationOutput(static (ctx) => PostInitializationOutput(ctx));
    }

    private sealed class ModuleItem(byte moduleId, byte id, string name, string actionName)
    {
        public string ActionName { get; set; } = actionName;
        public string Id { get; set; } = $"0x{moduleId:X2}_{id:X2}";
        public string Name { get; set; } = name;
    }
}
