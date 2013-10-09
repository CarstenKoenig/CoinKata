using Xunit;

namespace CoinKata.Tests
{
    // Dieser Test SOLLTE eigentlich regelmäßig fehlschlagen - falls nicht in CoinChanger_Test.cs MaxTest im Property-Attribut anheben
    // VORSICHT: kein guter Unit-Test - soll nur das Problem veranschaulichen
    [Trait("Should", "Fail")]
    [Trait("Methode", "mit mutable / parallel Map")]
    public class CoinChanger_mutable_ParallelMap_Tests : CoinChanger_Tests<CoinChanger_mutable_ParallelMap>
    {
    }
}