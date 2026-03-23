using System;
using System.Text.RegularExpressions;

namespace EFCore.BulkExtensions.SqlAdapters;

/// <summary>
/// Formats SQL identifiers for providers that require quoted names and case-sensitive matching.
/// </summary>
internal static class IdentifierFormatter
{
    private static readonly Regex SquareBracketIdentifierRegex = new(@"\[(?<identifier>[^\]]+)\]", RegexOptions.Compiled);

    /// <summary>
    /// Converts identifiers written as [Identifier] to quoted identifiers with provider-aware casing.
    /// </summary>
    public static string ConvertSquareBracketIdentifiers(string sql, BulkConfig bulkConfig, DbServerType dbServerType)
    {
        if (string.IsNullOrWhiteSpace(sql))
        {
            return sql;
        }

        return SquareBracketIdentifierRegex.Replace(sql, match =>
        {
            var identifier = match.Groups["identifier"].Value;
            return NormalizeAndQuoteSingle(identifier, bulkConfig, dbServerType);
        });
    }

    /// <summary>
    /// Normalizes and quotes a single identifier name.
    /// </summary>
    public static string NormalizeAndQuoteSingle(string rawName, BulkConfig bulkConfig, DbServerType dbServerType)
    {
        var normalized = NormalizeIdentifierName(rawName, bulkConfig, dbServerType);
        return Quote(normalized);
    }

    /// <summary>
    /// Normalizes a single identifier name without adding quotes.
    /// </summary>
    public static string NormalizeIdentifierName(string rawName, BulkConfig bulkConfig, DbServerType dbServerType)
    {
        var trimmed = rawName?.Trim() ?? string.Empty;
        var isExplicitQuoted = IsExplicitQuoted(trimmed);
        var unwrapped = Unwrap(trimmed);

        if (string.IsNullOrEmpty(unwrapped))
        {
            return unwrapped;
        }

        if (!bulkConfig.EnableIdentifierAutoCaseResolve || isExplicitQuoted || bulkConfig.IdentifierCaseStrategy == IdentifierCaseStrategy.Preserve)
        {
            return unwrapped;
        }

        string normalized = bulkConfig.IdentifierCaseStrategy switch
        {
            IdentifierCaseStrategy.Upper => unwrapped.ToUpperInvariant(),
            IdentifierCaseStrategy.Lower => unwrapped.ToLowerInvariant(),
            IdentifierCaseStrategy.Auto => NormalizeByProvider(unwrapped, dbServerType),
            _ => unwrapped
        };

        return normalized;
    }

    private static string NormalizeByProvider(string value, DbServerType dbServerType)
    {
        return dbServerType switch
        {
            DbServerType.Oracle => value.ToUpperInvariant(),
            DbServerType.PostgreSQL => value.ToLowerInvariant(),
            _ => value
        };
    }

    private static string Unwrap(string value)
    {
        if (value.Length >= 2)
        {
            if ((value[0] == '[' && value[^1] == ']') || (value[0] == '"' && value[^1] == '"'))
            {
                return value[1..^1];
            }
        }

        return value;
    }

    private static bool IsExplicitQuoted(string value)
    {
        return value.Length >= 2 && value[0] == '"' && value[^1] == '"';
    }

    private static string Quote(string value)
    {
        return $"\"{value.Replace("\"", "\"\"")}\"";
    }
}
