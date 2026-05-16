param(
    [Parameter(Mandatory=$true)]
    [string]$InputFile,
    
    [Parameter(Mandatory=$false)]
    [string]$OutputFile,
    
    [Parameter(Mandatory=$false)]
    [switch]$Backup,
    
    [Parameter(Mandatory=$false)]
    [string]$ConfigFile
)

# Define conversion rules
$conversionRules = @(
    # Basic type conversions
    # Remove lines starting with /*
    @{ Pattern = '^\s*/\*.*$'; Replacement = '' },

    # Remove lines starting with */
    @{ Pattern = '^\s*\*/.*$'; Replacement = '' },
    @{ Pattern = '^\s*\*\*.*$'; Replacement = '' },
    @{ Pattern = '\bStringClass\b'; Replacement = 'string' },
    @{ Pattern = '\bstd::vector<([^>]+)>'; Replacement = 'List<$1>' },
    @{ Pattern = '\bstd::map<([^,]+),([^>]+)>'; Replacement = 'Dictionary<$1,$2>' },
    @{ Pattern = '\bstd::unordered_map<([^,]+),([^>]+)>'; Replacement = 'Dictionary<$1,$2>' },
    #@{ Pattern = '\bstd::shared_ptr<([^>]+)>'; Replacement = '$1' },
    #@{ Pattern = '\bstd::unique_ptr<([^>]+)>'; Replacement = '$1' },
    #@{ Pattern = '\bstd::make_shared<([^>]+)>'; Replacement = 'new $1' },
    #@{ Pattern = '\bstd::make_unique<([^>]+)>'; Replacement = 'new $1' },
    
    # Primitive type conversions
    @{ Pattern = '\bunsigned int\b'; Replacement = 'uint' },
    @{ Pattern = '\buint32_t\b",'; Replacement = 'uint' },
    @{ Pattern = '\bunsigned long\b'; Replacement = 'ulong' },
    @{ Pattern = '\blong long\b'; Replacement = 'long' },
    @{ Pattern = '\bunsigned char\b'; Replacement = 'byte' },
    
    # Include/using conversions
   # @{ Pattern = '#include\s+["<]([^">]+)[">]'; Replacement = '// #include "$1" - Review for C# equivalent' },
    #@{ Pattern = 'using namespace std;'; Replacement = '// using namespace std; - Not needed in C#' },
    
    # nullptr to null
    @{ Pattern = '\bnullptr\b'; Replacement = 'null' },
    
    # cout/cerr to Console
    @{ Pattern = 'std::cout\s*<<\s*(.+?)\s*<<\s*std::endl'; Replacement = 'Console.WriteLine($1)' },
    @{ Pattern = 'Debug_Say\(\((.*)\)\)'; Replacement = 'Console.WriteLine($1)' },
    @{ Pattern = 'WWDEBUG_SAY\(\((.*)\)\)'; Replacement = 'Console.WriteLine($1)' },
    @{ Pattern = 'std::cout\s*<<\s*(.+?);'; Replacement = 'Console.Write($1);' },
    @{ Pattern = 'std::cerr\s*<<\s*(.+?)\s*<<\s*std::endl'; Replacement = 'Console.Error.WriteLine($1)' },
    #@{ Pattern = '&'; Replacement = '' },
    #@{ Pattern = '\*'; Replacement = '' },
    @{ Pattern = 'const'; Replacement = '' },
    @{ Pattern = 'NULL'; Replacement = 'null' },
    @{ Pattern = '\bassert\s*\(([^)]+)\)'; Replacement = 'Debug.Assert($1)' },
    @{ Pattern = '\bint32_t\b'; Replacement = 'int' },
    @{ Pattern = '\buint32\b'; Replacement = 'uint' },
    @{ Pattern = '\( *void'; Replacement = '(' },
    @{ Pattern = 'READ_MICRO_CHUNK\(cload, (\S*), (\S*)\);*'; Replacement = "case `$1:`n`t`t`t`t`tcload.Read(ref `$2);`n`t`t`t`t`tbreak;" },
    @{ Pattern = 'READ_MICRO_CHUNK_WWSTRING\(cload, (\S*), (\S*)\);*'; Replacement = "case `$1:`n`t`t`t`t`tcload.ReadMicroChunkWWString(out `$2);`n`t`t`t`t`tbreak;" },
    @{ Pattern = 'WRITE_MICRO_CHUNK\(csave, (\S*), (\S*)\);*'; Replacement = 'csave.WriteMicro($1, $2);' },
    @{ Pattern = 'WRITE_MICRO_CHUNK_WWSTRING\(csave, (\S*), (\S*)\);*'; Replacement = "ArgumentNullException.ThrowIfNull(`$2);`n`t`t`t`t`tcsave.WriteMicroString(`$1, `$2);" },
    @{ Pattern = 'cload.Cur_Chunk_ID\(\)'; Replacement = 'cload.Cur_Chunk_ID' },
    @{ Pattern = 'cload.Cur_Micro_Chunk_ID\(\)'; Replacement = 'cload.Cur_Micro_Chunk_ID' },
    @{ Pattern = '^.*friend class.*$'; Replacement = '' },
    @{ Pattern = 'Get_Factory\(\)\s*;'; Replacement = 'Get_Factory() => _persistFactory;' },
    @{ Pattern = '^(\s*)char\s+Get_Type_Name\(\)'; Replacement = '$1string Get_Type_Name()' },
    @{ Pattern = '\bWWASSERT\b'; Replacement = 'Debug.Assert' },

    
    
    #@{ Pattern = ''; Replacement = '' },
    
    # Function syntax
    #@{ Pattern = '\bvoid\s+(\w+)\s*\(\s*\)'; Replacement = 'public void $1()' },
    #@{ Pattern = '\bint\s+(\w+)\s*\(\s*\)'; Replacement = 'public int $1()' },
    #@{ Pattern = '\bbool\s+(\w+)\s*\(\s*\)'; Replacement = 'public bool $1()' },
    
    # const references to in parameters (C# 7.2+)
    #@{ Pattern = 'const\s+(\w+)\s*&'; Replacement = 'in $1' },
    @{ Pattern = '\bconst\b'; Replacement = '' },
    
    # Pointer to reference (simplified - may need manual review)
    #@{ Pattern = '(\w+)\s*\*\s+(\w+)\s*='; Replacement = '$1 $2 =' },
    @{ Pattern = '->'; Replacement = '.' },
    
    # Boolean values
    #@{ Pattern = '\btrue\b'; Replacement = 'true' },
    #@{ Pattern = '\bfalse\b'; Replacement = 'false' },
    
    # Common STL to .NET
    #@{ Pattern = '\bstd::endl\b'; Replacement = '\n' },
    #@{ Pattern = '\bstd::pair<([^,]+),([^>]+)>'; Replacement = 'KeyValuePair<$1,$2>' },
    #@{ Pattern = '\bstd::queue<([^>]+)>'; Replacement = 'Queue<$1>' },
    #@{ Pattern = '\bstd::stack<([^>]+)>'; Replacement = 'Stack<$1>' },
    #@{ Pattern = '\bstd::list<([^>]+)>'; Replacement = 'LinkedList<$1>' },
    #@{ Pattern = '\bstd::set<([^>]+)>'; Replacement = 'HashSet<$1>' },
    
    # Array declarations
    @{ Pattern = '(\w+)\s+(\w+)\[(\d+)\]'; Replacement = '$1[] $2 = new $1[$3]' },
    
    # Class/struct declarations
    @{ Pattern = '^class\s+(\w+)\s*{'; Replacement = 'public class $1 {' },
    @{ Pattern = '^struct\s+(\w+)\s*{'; Replacement = 'public struct $1 {' },
    
    # Access modifiers
    @{ Pattern = '^public:'; Replacement = '    // public members' },
    @{ Pattern = '^private:'; Replacement = '    // private members' },
    @{ Pattern = '^protected:'; Replacement = '    // protected members' },

    @{ Pattern = '::'; Replacement = '.' },
    @{ Pattern = '^class\s+(\w+)\s*:\s*public\s+(\w+)'; Replacement = 'public class $1 : $2' },
    @{ Pattern = 'stricmp'; Replacement = 'string.Compare' },

    @{ Pattern = '\bDECLARE_EDITABLE'; Replacement = '//DECLARE_EDITABLE' },
    @{ Pattern = '\bEDITABLE_PARAM'; Replacement = '//EDITABLE_PARAM' },
    @{ Pattern = '\bFLOAT_UNITS_PARAM'; Replacement = '//FLOAT_UNITS_PARAM' },
    @{ Pattern = '\bFLOAT_EDITABLE_PARAM'; Replacement = '//FLOAT_EDITABLE_PARAM' },
    @{ Pattern = '\bFILENAME_PARAM'; Replacement = '//FILENAME_PARAM' },
    @{ Pattern = '\bINT_EDITABLE_PARAM'; Replacement = '//INT_EDITABLE_PARAM' },
    @{ Pattern = '\bOBSOLETE_MICRO_CHUNK'; Replacement = '//OBSOLETE_MICRO_CHUNK' },
    @{ Pattern = '\bNAMED_EDITABLE_PARAM'; Replacement = '//NAMED_EDITABLE_PARAM' },
    @{ Pattern = '\bNAMED_FLOAT_UNITS_PARAM'; Replacement = '//NAMED_FLOAT_UNITS_PARAM' },
    @{ Pattern = '\bGENERIC_EDITABLE_PARAM'; Replacement = '//GENERIC_EDITABLE_PARAM' },
    @{ Pattern = '\bENUM_PARAM'; Replacement = '//ENUM_PARAM' },
    

    @{ Pattern = 'DECLARE_DEFINITION_FACTORY\(\s*([A-Za-z_][A-Za-z0-9_]*)\s*,\s*(CLASSID_[A-Za-z0-9_]+)\s*,\s*"[^"]*"\s*\)\s*[A-Za-z0-9_]*;'; Replacement = 'public override uint Get_Class_ID() => ClassId.$2;' },
    @{ Pattern = 'SimplePersistFactoryClass<(\w+),\s*(\w+)>\s+\w+;'; Replacement = '[RegisterDefinition(ChunkId.$2)]' },

    @{ Pattern = '.*friend\s+class.*\r?\n?'; Replacement = '' }
)

# Load custom rules from config file if provided
if ($ConfigFile -and (Test-Path $ConfigFile)) {
    Write-Host "Loading custom rules from $ConfigFile..." -ForegroundColor Cyan
    $customRules = Get-Content $ConfigFile | ConvertFrom-Json
    foreach ($rule in $customRules.rules) {
        $conversionRules += @{ Pattern = $rule.pattern; Replacement = $rule.replacement }
    }
}

# Validate input file
if (-not (Test-Path $InputFile)) {
    Write-Error "Input file not found: $InputFile"
    exit 1
}

# Determine output file
if (-not $OutputFile) {
    $OutputFile = [System.IO.Path]::ChangeExtension($InputFile, ".cs")
}

# Create backup if requested
if ($Backup) {
    $backupFile = "$InputFile.bak"
    Copy-Item $InputFile $backupFile
    Write-Host "Backup created: $backupFile" -ForegroundColor Green
}

# Read input file
Write-Host "Reading from: $InputFile" -ForegroundColor Cyan
$content = Get-Content $InputFile -Raw

# Apply conversions
Write-Host "Applying conversion rules..." -ForegroundColor Yellow
$convertedContent = $content

$appliedRules = @()
foreach ($rule in $conversionRules) {
    $matches = [regex]::Matches($convertedContent, $rule.Pattern)
    if ($matches.Count -gt 0) {
        $convertedContent = [regex]::Replace($convertedContent, $rule.Pattern, $rule.Replacement)
        $appliedRules += "Applied: $($rule.Pattern) -> $($rule.Replacement) ($($matches.Count) replacements)"
    }
}

# Add C# file header if it doesn't exist
if ($convertedContent -notmatch "^using System") {
    $header = @"
using RenData.ChunkIO;
using RenData.SaveLoad;
using RenData.IDs;

namespace RenData;

"@
    $convertedContent = $header + $convertedContent
}

# Write output file
Write-Host "Writing to: $OutputFile" -ForegroundColor Cyan
#Set-Content -Path $OutputFile -Value $convertedContent
$convertedContent = $convertedContent -replace "`r?`n", "`r`n"
Set-Content -Path $OutputFile -Value $convertedContent -Encoding UTF8

# Report results
Write-Host "`nConversion Summary:" -ForegroundColor Green
Write-Host "===================" -ForegroundColor Green
foreach ($applied in $appliedRules) {
    Write-Host "  $applied" -ForegroundColor Gray
}

Write-Host "`nConversion complete!" -ForegroundColor Green
Write-Host "Output file: $OutputFile" -ForegroundColor Yellow
Write-Host "`nIMPORTANT: Please review the converted file manually." -ForegroundColor Magenta
Write-Host "Some conversions may require additional adjustments:" -ForegroundColor Magenta
Write-Host "  - Memory management (new/delete)" -ForegroundColor Red
Write-Host "  - Template specializations" -ForegroundColor Red
Write-Host "  - Multiple inheritance" -ForegroundColor Red
Write-Host "  - Preprocessor directives" -ForegroundColor Red
Write-Host "  - Operator overloading" -ForegroundColor Red
Write-Host "  - Friend functions" -ForegroundColor Red
