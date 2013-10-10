using Xunit;

namespace CoinKata.Tests
{
    [Trait("Methode", "brute-force")]
    public class CoinChanger_BruteForce_Tests : CoinChanger_Tests<CoinChanger_BruteForce>
    {
        public CoinChanger_BruteForce_Tests()
            : base(maxBetrag: 50)
        {
            
        }
    }
}