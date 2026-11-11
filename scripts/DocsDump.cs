// Generates the OpenAI.SystemOne documentation data files (docs/data/*.js) used by
// the static docs site, from the library's public API (via reflection) merged with
// its XML documentation file.
//
// Usage:
//   dotnet run scripts/DocsDump.cs -- <path-to-OpenAI.SystemOne.xml> <docs/data output dir>
//
// This tool is invoked automatically after every build of OpenAI.SystemOne (see the
// GenerateDocs target in the project file); it can also be run manually.
//
// .NET file-based app.
#:project ../src/OpenAI.SystemOne/OpenAI.SystemOne.csproj
#:property Nullable=enable

#pragma warning disable IL2070, IL2026, IL3050

using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Xml.Linq;

const string PackageId = "OpenAI.SystemOne";
const string PackageUrl = "https://www.nuget.org/packages/OpenAI.SystemOne";

if (args.Length < 2)
{
    Console.Error.WriteLine("Usage: DocsDump <OpenAI.SystemOne.xml> <docs/data output dir>");
    return 1;
}

var xmlPath = args[0];
var dataDir = args[1];
Directory.CreateDirectory(dataDir);

var library = typeof(OpenAI.SystemOne.ISystemOneClient).Assembly;
var version = library.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
              ?? library.GetName().Version?.ToString(3)
              ?? "1.0.0";

var doc = XDocument.Load(xmlPath);

var summaries = doc.Descendants("member")
    .ToDictionary(
        m => m.Attribute("name")?.Value ?? string.Empty,
        m => new MemberDoc(
            RenderDoc(m.Element("summary")),
            (m.Elements("param") ?? []).Select(p => RenderDoc(p) ?? string.Empty).ToArray(),
            RenderDoc(m.Element("returns"))));

var unionTypes = new HashSet<string>
{
    "OpenAI.SystemOne.SystemOneState",
    "OpenAI.SystemOne.SystemOneContent",
    "OpenAI.SystemOne.SystemOneKeepAlive",
};

var types = library.GetExportedTypes()
    .Where(t => t.IsVisible && !t.IsNested)
    .OrderBy(t => t.Namespace)
    .ThenBy(t => t.Name)
    .Select(DescribeType)
    .ToList();

// ---------- emit docs/data/api.js ----------

var namespacesJson = new JsonArray();
foreach (var group in types.GroupBy(t => t.Namespace))
{
    var typeNamesJson = new JsonArray();
    foreach (var t in group)
    {
        typeNamesJson.Add((JsonNode?)t.Name);
    }
    namespacesJson.Add(new JsonObject
    {
        ["name"] = group.Key,
        ["types"] = typeNamesJson,
    });
}

var versionsJson = new JsonArray { (JsonNode?)version };
var apiJson = new JsonObject
{
    ["versions"] = versionsJson,
    ["current"] = version,
    ["namespaces"] = namespacesJson,
    ["install"] = new JsonObject
    {
        ["id"] = PackageId,
        ["url"] = PackageUrl,
    },
};

File.WriteAllText(
    Path.Combine(dataDir, "api.js"),
    $"window.OS1_API = {apiJson};\n",
    new UTF8Encoding(false));

// ---------- emit docs/data/<Type>.js ----------

foreach (var type in types)
{
    var propertiesJson = new JsonArray();
    foreach (var prop in type.Properties)
    {
        propertiesJson.Add(new JsonObject
        {
            ["name"] = prop.Name,
            ["kind"] = prop.Kind,
            ["since"] = version,
            ["declaration"] = prop.Declaration,
            ["desc"] = prop.Summary ?? string.Empty,
            ["return"] = prop.ReturnType ?? string.Empty,
            ["href"] = Slugify(prop.Name) + ".html",
        });
    }

    var membersJson = new JsonArray();
    foreach (var method in type.Methods)
    {
        membersJson.Add(new JsonObject
        {
            ["name"] = method.Name,
            ["kind"] = method.Kind,
            ["since"] = version,
            ["declaration"] = method.Declaration,
            ["desc"] = method.Summary ?? string.Empty,
            ["return"] = method.ReturnType ?? string.Empty,
            ["href"] = Slugify(method.Name) + ".html",
            ["overline"] = method.ReturnType is null ? null : $"Returns: {method.ReturnType}",
            ["sections"] = MemberSections(method),
        });
    }

    var sectionsJson = new JsonArray();
    if (!string.IsNullOrEmpty(type.Summary))
    {
        sectionsJson.Add(new JsonObject
        {
            ["heading"] = "Remarks",
            ["html"] = $"<p>{type.Summary}</p>",
        });
    }

    var typeDataJson = new JsonObject
    {
        ["name"] = type.Name,
        ["namespace"] = type.Namespace,
        ["kind"] = type.Kind,
        ["since"] = version,
        ["declaration"] = type.Declaration,
        ["overline"] = $"Namespace: {type.Namespace} · Assembly: {PackageId}",
        ["summary"] = type.Summary ?? string.Empty,
        ["sections"] = sectionsJson,
        ["properties"] = propertiesJson,
        ["members"] = membersJson,
    };

    File.WriteAllText(
        Path.Combine(dataDir, $"{type.Name}.js"),
        $"window.OS1_TYPES = window.OS1_TYPES || {{}};\n"
        + $"window.OS1_TYPES[\"{type.Name}\"] = {typeDataJson};\n",
        new UTF8Encoding(false));
}

// ---------- update the <script> list in docs/index.html ----------

var indexPath = Path.Combine(dataDir, "..", "index.html");
if (File.Exists(indexPath))
{
    var scripts = new List<string>
    {
        "<script src=\"data/api.js\"></script>",
    };

    scripts.AddRange(types
        .Select(t => $"<script src=\"data/{t.Name}.js\"></script>"));

    var indexHtml = File.ReadAllText(indexPath);
    var updated = Regex.Replace(
        indexHtml,
        @"<!-- types:begin -->.*?<!-- types:end -->",
        "<!-- types:begin -->\n" + string.Join("\n", scripts) + "\n<!-- types:end -->",
        RegexOptions.Singleline);

    if (updated != indexHtml)
    {
        File.WriteAllText(indexPath, updated, new UTF8Encoding(false));
    }
}

Console.WriteLine($"Generated {types.Count} type pages in {dataDir}");
return 0;

// ---------- local functions ----------

TypeDoc DescribeType(Type type)
{
    var docId = $"T:{type.FullName!.Replace('+', '.')}";
    var kind = unionTypes.Contains(type.FullName!)
        ? "Union"
        : type.IsInterface
            ? "Interface"
            : typeof(Exception).IsAssignableFrom(type)
                ? "Exception"
                : "Class";

    var typeKeyword = type.IsInterface ? "interface" : kind == "Union" ? "union" : "class";

    return new TypeDoc(
        type.Name,
        type.Namespace!,
        kind,
        $"public {typeKeyword} {type.Name}",
        summaries.GetValueOrDefault(docId)?.Summary,
        DeclaredProperties(type),
        DeclaredMethods(type));
}

List<MemberDocOut> DeclaredProperties(Type type)
{
    var properties = new List<MemberDocOut>();

    foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(p => p.GetIndexParameters().Length == 0))
    {
        var docId = $"P:{type.FullName!.Replace('+', '.')}.{property.Name}";

        properties.Add(new MemberDocOut(
            Name: property.Name,
            Kind: "Property",
            ReturnType: PrettyType(property.PropertyType),
            Declaration: $"{PrettyType(property.PropertyType)} {property.Name} {{ {(property.CanWrite ? "get; set;" : "get;")} }}",
            Summary: ResolveInheritedDoc(type, docId, property.Name, "P"),
            Returns: null,
            Params: []));
    }

    foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
    {
        var docId = $"F:{type.FullName!.Replace('+', '.')}.{field.Name}";
        var summary = summaries.GetValueOrDefault(docId)?.Summary;
        var isConst = field.IsLiteral;

        properties.Add(new MemberDocOut(
            Name: field.Name,
            Kind: isConst ? "Constant" : "Field",
            ReturnType: PrettyType(field.FieldType),
            Declaration: isConst
                ? $"public const {PrettyType(field.FieldType)} {field.Name} = \"{field.GetRawConstantValue()}\""
                : $"public static {PrettyType(field.FieldType)} {field.Name}",
            Summary: summary,
            Returns: null,
            Params: []));
    }

    return properties;
}

List<MemberDocOut> DeclaredMethods(Type type)
{
    var methods = new List<MemberDocOut>();

    foreach (var constructor in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
    {
        var parameters = constructor.GetParameters();
        var signature = BuildSignature(type.Name, type, parameters, out var paramFullNames);
        var docId = $"M:{type.FullName!.Replace('+', '.')}.{type.Name}({string.Join(',', paramFullNames)})";
        var docData = summaries.GetValueOrDefault(docId);
        var paramDocs = PairParams(docData?.Params, parameters);

        methods.Add(new MemberDocOut(
            Name: SignatureName(type.Name, parameters),
            Kind: "Constructor",
            ReturnType: type.Name,
            Declaration: signature,
            Summary: docData?.Summary ?? $"Initializes a new instance of the {type.Name} class.",
            Returns: null,
            Params: paramDocs));
    }

    foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(m => !m.IsSpecialName))
    {
        var parameters = method.GetParameters();
        var genericSuffix = method.IsGenericMethodDefinition
            ? "``" + method.GetGenericArguments().Length
            : string.Empty;
        var signature = BuildSignature(method.Name + genericSuffix, method.DeclaringType!, parameters, out var paramFullNames);
        var docId = $"M:{type.FullName!.Replace('+', '.')}.{method.Name}{genericSuffix}({string.Join(',', paramFullNames)})";
        var docData = summaries.GetValueOrDefault(docId);
        var paramDocs = PairParams(docData?.Params, parameters);

        var returnType = method.ReturnType == typeof(void)
            ? null
            : PrettyType(method.ReturnType);

        methods.Add(new MemberDocOut(
            Name: SignatureName(method.Name, parameters),
            Kind: "Method",
            ReturnType: returnType,
            Declaration: signature,
            Summary: ResolveInheritedDoc(type, docId, method.Name, "M", paramFullNames),
            Returns: docData?.Returns,
            Params: paramDocs));
    }

    foreach (var op in type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(m => m.IsSpecialName && (m.Name is "op_Implicit" or "op_Explicit")))
    {
        var docId = $"M:{type.FullName!.Replace('+', '.')}.{op.Name}({op.GetParameters()[0].ParameterType.FullName})~{op.ReturnType.FullName}";
        var summary = summaries.GetValueOrDefault(docId)?.Summary;

        methods.Add(new MemberDocOut(
            Name: $"implicit operator {PrettyType(op.ReturnType)}",
            Kind: "Operator",
            ReturnType: PrettyType(op.ReturnType),
            Declaration: $"public static implicit operator {PrettyType(op.ReturnType)}({PrettyType(op.GetParameters()[0].ParameterType)} value)",
            Summary: summary,
            Returns: null,
            Params: []));
    }

    return methods;
}

string BuildSignature(string nameWithSuffix, Type declaringType, ParameterInfo[] parameters, out string[] paramFullNames)
{
    var name = nameWithSuffix.Split('`')[0];
    var genericCount = nameWithSuffix.Split("``").Length - 1;
    var genericSuffix = genericCount > 0
        ? "<" + string.Join(", ", Enumerable.Range(1, genericCount).Select(i => $"T{i}")) + ">"
        : string.Empty;

    var list = new List<string>();
    paramFullNames = new string[parameters.Length];

    for (var i = 0; i < parameters.Length; i++)
    {
        var parameter = parameters[i];
        paramFullNames[i] = XmlTypeName(parameter.ParameterType);
        list.Add($"{PrettyType(parameter.ParameterType)} {parameter.Name}{(parameter.HasDefaultValue ? " = " + FormatDefault(parameter) : string.Empty)}");
    }

    var prefix = declaringType.IsInterface
        ? $"{PrettyType(declaringType)}."
        : string.Empty;

    return $"{prefix}{name}{genericSuffix}({string.Join(", ", list)})";
}

string FormatDefault(ParameterInfo parameter)
{
    var value = parameter.DefaultValue;
    if (value is null)
    {
        return PrettyType(parameter.ParameterType) == "string" ? "null" : "default";
    }

    return value switch
    {
        string s => $"\"{s}\"",
        bool b => b ? "true" : "false",
        _ => value.ToString() ?? "default",
    };
}

static string SignatureName(string name, ParameterInfo[] parameters)
{
    if (parameters.Length == 0)
    {
        return name;
    }

    return $"{name}({string.Join(", ", parameters.Select(p => PrettyType(p.ParameterType)))})";
}

static MemberParamOut[] PairParams(string[]? descs, ParameterInfo[] parameters)
{
    if (descs is null || descs.Length == 0)
    {
        return [];
    }

    return parameters
        .Select((p, i) => new MemberParamOut(p.Name!, PrettyType(p.ParameterType), descs[i]))
        .ToArray();
}

string? ResolveInheritedDoc(Type type, string docId, string memberName, string memberKind, string[]? paramFullNames = null)
{
    var own = summaries.GetValueOrDefault(docId)?.Summary;
    if (!string.IsNullOrEmpty(own) && !own.Contains("inheritdoc", StringComparison.OrdinalIgnoreCase))
    {
        return own;
    }

    for (var baseType = type.BaseType; baseType is not null; baseType = baseType.BaseType)
    {
        var inheritedId = memberKind == "M" && paramFullNames is not null
            ? $"M:{baseType.FullName!.Replace('+', '.')}.{memberName}({string.Join(',', paramFullNames)})"
            : $"{memberKind}:{baseType.FullName!.Replace('+', '.')}.{memberName}";

        var inherited = summaries.GetValueOrDefault(inheritedId)?.Summary;
        if (!string.IsNullOrEmpty(inherited))
        {
            return inherited;
        }
    }

    return own;
}

static string? RenderDoc(XElement? element)
{
    if (element is null)
    {
        return null;
    }

    // Preserve the inner markup so <see>/<c>/<paramref> tags can be turned into readable text.
    var text = string.Concat(element.Nodes().Select(node => node.ToString(SaveOptions.DisableFormatting)));
    text = text.Replace("\r", " ").Replace("\n", " ");

    text = Regex.Replace(text, "<see\\s+cref=\"(?<c>[^\"]+)\"\\s*/>", match =>
    {
        var cref = match.Groups["c"].Value; // e.g. T:OpenAI.SystemOne.SystemOneResponse
        return cref.Split(':', '.')[^1];
    });
    text = Regex.Replace(text, "<see\\s+langword=\"(?<w>[\\w]+)\"\\s*/>", "<code>${w}</code>");
    text = Regex.Replace(text, "<c>(?<c>.*?)</c>", "<code>${c}</code>");
    text = Regex.Replace(text, "<paramref\\s+name=\"(?<p>[\\w]+)\"\\s*/>", "<code>${p}</code>");
    text = Regex.Replace(text, "</?[a-zA-Z][^>]*>", string.Empty); // strip any remaining tags

    var cleaned = string.Join(' ', text.Split(' ').Where(part => part.Trim().Length > 0));

    return cleaned.Length > 0 ? cleaned : null;
}

static string XmlTypeName(Type type)
{
    if (type.IsGenericType && !type.IsGenericTypeDefinition)
    {
        var definitionName = type.GetGenericTypeDefinition().FullName!.Split('`')[0];
        var args = string.Join(',', type.GetGenericArguments().Select(XmlTypeName));
        return $"{definitionName}{{{args}}}";
    }

    if (type.IsArray)
    {
        return XmlTypeName(type.GetElementType()!) + "[]";
    }

    return type.FullName ?? type.Name;
}

static string PrettyType(Type type)
{
    if (type == typeof(void))
    {
        return "void";
    }

    if (type.IsArray)
    {
        return PrettyType(type.GetElementType()!) + "[]";
    }

    if (type.IsGenericType)
    {
        var definition = type.GetGenericTypeDefinition();

        if (definition == typeof(Nullable<>))
        {
            return PrettyType(type.GetGenericArguments()[0]) + "?";
        }

        if (definition == typeof(Task<>))
        {
            return $"Task<{string.Join(", ", type.GetGenericArguments().Select(PrettyType))}>";
        }

        var name = definition.Name.Split('`')[0];
        return $"{name}<{string.Join(", ", type.GetGenericArguments().Select(PrettyType))}>";
    }

    var aliases = new Dictionary<Type, string>
    {
        [typeof(string)] = "string",
        [typeof(int)] = "int",
        [typeof(double)] = "double",
        [typeof(bool)] = "bool",
        [typeof(object)] = "object",
        [typeof(CancellationToken)] = "CancellationToken",
    };

    if (aliases.TryGetValue(type, out var alias))
    {
        return alias;
    }

    return type.Name;
}

static string Slugify(string text)
{
    var slug = Regex.Replace(text, "[^A-Za-z0-9]+", "-").ToLowerInvariant();
    return slug.Trim('-');
}

static string Escape(string? text) => string.IsNullOrEmpty(text)
    ? string.Empty
    : text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");

static string ParamTable(MemberParamOut[]? parameters)
{
    if (parameters is null || parameters.Length == 0)
    {
        return string.Empty;
    }

    var rows = string.Join("\n", parameters.Select(p =>
        $"<tr><td><code>{Escape(p.Name)}</code></td><td>{Escape(p.Type)}</td><td>{Escape(p.Desc)}</td></tr>"));

    return $"<table><tr><th>Name</th><th>Type</th><th>Description</th></tr>\n{rows}\n</table>";
}

static JsonArray MemberSections(MemberDocOut method)
{
    var sections = new JsonArray();

    if (!string.IsNullOrEmpty(method.Summary))
    {
        sections.Add(new JsonObject
        {
            ["heading"] = "Definition",
            ["html"] = $"<p>{method.Summary}</p>",
        });
    }

    var parameters = ParamTable(method.Params);
    if (parameters.Length > 0)
    {
        sections.Add(new JsonObject
        {
            ["heading"] = "Parameters",
            ["html"] = parameters,
        });
    }

    if (!string.IsNullOrEmpty(method.Returns))
    {
        sections.Add(new JsonObject
        {
            ["heading"] = "Returns",
            ["html"] = $"<p>{Escape(method.Returns)}</p>",
        });
    }

    return sections;
}

record MemberDoc(string? Summary, string[]? Params, string? Returns);

record TypeDoc(
    string Name,
    string Namespace,
    string Kind,
    string Declaration,
    string? Summary,
    List<MemberDocOut> Properties,
    List<MemberDocOut> Methods);

record MemberDocOut(
    string Name,
    string Kind,
    string? ReturnType,
    string Declaration,
    string? Summary,
    string? Returns,
    MemberParamOut[] Params);

record MemberParamOut(
    string Name,
    string Type,
    string Desc);
