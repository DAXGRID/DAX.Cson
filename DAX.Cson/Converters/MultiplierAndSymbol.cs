using System;
using System.Collections.Generic;
using System.Linq;
using DAX.CIM.PhysicalNetworkModel;

namespace DAX.Cson.Converters
{
    class MultiplierAndSymbol
    {
        static readonly Dictionary<string, MultiplierAndSymbol> MultipliersAndUnits = GetMulli();

        static Dictionary<string, MultiplierAndSymbol> GetMulli()
        {
            var multipliers = typeof(UnitMultiplier).GetEnumValues().Cast<UnitMultiplier>();
            var units = typeof(UnitSymbol).GetEnumValues().Cast<UnitSymbol>();

            var combinations =
                (from multiplier in multipliers
                 from unit in units
                 let mulli = new MultiplierAndSymbol(multiplier, unit)
                 where !(mulli.UnitMultiplier == DAX.CIM.PhysicalNetworkModel.UnitMultiplier.m && mulli.UnitSymbol == null)
                       && !(mulli.UnitMultiplier == DAX.CIM.PhysicalNetworkModel.UnitMultiplier.none && mulli.UnitSymbol == null)
                       && !(mulli.UnitMultiplier == DAX.CIM.PhysicalNetworkModel.UnitMultiplier.m && mulli.UnitSymbol == DAX.CIM.PhysicalNetworkModel.UnitSymbol.none)
                 select mulli)
                .ToList();

            var dupes = combinations.GroupBy(c => c.Key)
                .Where(g => g.Count() > 1)
                .ToList();

            if (dupes.Any())
            {
                throw new ApplicationException($@"Sorry, but one or more ambiguous combinations of unit/multiplier was found:

{string.Join(Environment.NewLine, dupes.Select(g => $"    {g.Key}: {string.Join("; ", g)}"))}

The tuple defined by (UnitMultiplier, UnitSymbol) must be unambiguously identifiable from the concatenation of its string symbols");
            }

            return combinations.ToDictionary(m => m.Key);
        }

        public static MultiplierAndSymbol GetMultiplierAndUnit(string part)
        {
            try
            {
                return MultipliersAndUnits[part];
            }
            catch (KeyNotFoundException exception)
            {
                throw new KeyNotFoundException($"Could not find '{part}' in list of available multiplier/unit combinations: {string.Join(", ", MultipliersAndUnits.Keys)}", exception);
            }
        }

        public MultiplierAndSymbol(UnitMultiplier unitMultiplier, UnitSymbol unitSymbol)
        {
            UnitMultiplier = unitMultiplier;
            UnitSymbol = unitSymbol;

            Key = String.Concat(
                UnitMultiplier == DAX.CIM.PhysicalNetworkModel.UnitMultiplier.none ? "" : UnitMultiplier.ToString(),
                UnitSymbol == DAX.CIM.PhysicalNetworkModel.UnitSymbol.none ? "" : UnitSymbol.ToString()
            );
        }

        public UnitMultiplier? UnitMultiplier { get; }
        public UnitSymbol? UnitSymbol { get; }
        public string Key { get; }

        public override string ToString()
        {
            return $"(key: {Key}, multiplier: {UnitMultiplier?.ToString() ?? "NULL"}, unit: {UnitSymbol?.ToString() ?? "NULL"})";
        }
    }
}