namespace RenData;

internal class LookupTableMgrClass
{
    private readonly static Dictionary<string, LookupTableClass> Tables = [];
    internal static LookupTableClass? Get_Table(string name,bool try_to_load = true)
    {
        // TODO: Implement
        if (!Tables.TryGetValue(name, out var table) && try_to_load)
        {
            table = new();
            Tables[name] = table;
        }
        return table;
    }

}
