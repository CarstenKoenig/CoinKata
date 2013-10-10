-- | CoinChange - Kata in Haskell
-- | für eine gegebene Liste von Münzwerten und einen Betrag soll dieser Betrag aus möglichst wenig
-- | Münzen zusammengestellt werden
-- | Eingabe:   [Münze], Betrag
-- | Ausgabe:   [Anzahl, Münze]
-- | Bedingung: Summe aus Anzahl*Münze = Betrag
-- |            Summe aus Anzahl ist minimal
module CoinChange where

import Data.List(intercalate, minimumBy)
import Data.Maybe(mapMaybe)
import Control.Applicative ( (<$>) )
import qualified Data.Map.Strict as M

import Test.QuickCheck (NonNegative(..))
import qualified Test.HUnit as T


-- * Typen und Typsynonyme

-- | Ein Betrag in Cent soll hier eine Ganzzahl sein
type Cents           = Integer

-- | eine Münze wird über ihren Wert in ct. identifiziert
type Muenze          = Cents

-- | Anzahl der verwendeten Münzen ist eine Ganzzahl
type Anzahl          = Integer

-- | das Wechselgeld gibt für jede Münze an, wie oft sie verwendet wird
newtype Wechselgeld  = Wechselgeld (M.Map Muenze Anzahl)

-- | so sollte eine Lösung für unser Problem aussehen
type Algorithmus     = [Muenze] -> Cents -> Maybe Wechselgeld
 
-- * Wechselgeldfunktionen

-- | Wechselgeld schön darstellen
instance Show Wechselgeld where
  show (Wechselgeld wg) 
    | M.null wg    = "<kein Wechselgeld>"
    | otherwise    = intercalate "; " . M.foldrWithKey' appEl [] $ wg
    where appEl muenze anz 
            | anz <= 0   = id -- nur Einträge mit positiven Anzahlen anzeigen
            | otherwise  = ((show anz ++ " x " ++ show muenze ++ "ct.") :)

instance Eq Wechselgeld where
  wg == wg' = show wg == show wg'

muenzenHinzufuegen :: Anzahl -> Muenze -> Wechselgeld -> Wechselgeld
muenzenHinzufuegen anz muenze (Wechselgeld wg) = Wechselgeld $ M.alter addiere muenze wg
  where addiere Nothing  = Just anz
        addiere (Just a) = Just $ anz + a

wechselgeldBetrag :: Wechselgeld -> Cents
wechselgeldBetrag (Wechselgeld wg) = M.foldrWithKey summiere 0 wg
  where summiere muenze anz = ((anz*muenze) +)

-- * einige nützliche Konstanten

-- | die in Deutschland/EURO-Raum üblichen Münzen in Euro-Cent
euroMuenzen :: [Muenze]
euroMuenzen =  [200, 100, 50, 25, 10, 5, 2, 1]

-- | nichts zurückgeben
nichts :: Wechselgeld
nichts = Wechselgeld M.empty

-- * BRUTE-FORCE Implementierung

-- | sucht nach der optimalen Lösung für das Wechselgeldproblem
-- | indem ALLE Möglichkeiten durchgetestet werden
bruteForce :: [Muenze] -> Cents -> Maybe Wechselgeld
bruteForce muenzen betrag = snd <$> bfAlgorithmus betrag muenzen
    where bfAlgorithmus restBetrag []                -- keine Münzen mehr zur auswahl
            | restBetrag == 0    = Just (0, nichts)  -- alles zurückgeben - dann ist das ok
            | otherwise          = Nothing           -- ansonsten könenn wir keine Lösung finden
          bfAlgorithmus restBetrag (muenze:muenzen') -- es gibt noch Münzen
            | restBetrag < 0     = Nothing           -- Restbetrag ist negativ (sollte nicht vorkommen) -> Keine Lösung
            | otherwise =                            -- sonst teste alle Möglichen Vielfachheiten (0 bis kleinste Vielfache der Münze < restBetrag) und wähle dasjenige gültige Ergebnis mit minimaler Münzanzahl
                findMinimum . mapMaybe versuche $ [0 .. restBetrag `div` muenze]
                where versuche anz = do              -- für den Test berechne rekursiv die Lösung des kleineren Subproblems und vereinige
                        (anz', wg) <- bfAlgorithmus (restBetrag - anz*muenze) muenzen'
                        return (anz + anz', muenzenHinzufuegen anz muenze wg)
                      findMinimum [] = Nothing
                      findMinimum vs = Just . minimumBy (\ (a,_) (b, _) -> compare a b) $ vs

-- * Tests

testeAlgorithmus :: [Muenze] -> Cents -> [(Muenze, Anzahl)] -> Algorithmus -> T.Test
testeAlgorithmus muenzen betrag erwartet alg = 
  T.TestLabel ("Muenzen: " ++ show muenzen ++ ", Betrag: " ++ show betrag ++ " sollte " ++ show wg ++ " ergeben") $
  T.TestCase  (T.assertEqual "falsches Ergebnis" expected result)
  where expected = Just wg
        result   = alg muenzen betrag
        wg       = Wechselgeld $ M.fromList erwartet

findeOptimaleLoesungFuerStandradfall :: Algorithmus -> T.Test
findeOptimaleLoesungFuerStandradfall = testeAlgorithmus [200, 100, 50, 20, 10, 5, 2, 1] 234 [(200, 1), (20, 1), (10, 1), (2, 2)]

findeOptimaleLoesungFuerNichtStandartfall :: Algorithmus -> T.Test
findeOptimaleLoesungFuerNichtStandartfall = testeAlgorithmus [25, 15, 1] 30 [(15,2)]

testsFuerAlgorithmus :: Algorithmus -> T.Test
testsFuerAlgorithmus alg = T.TestList [ findeOptimaleLoesungFuerStandradfall alg,  findeOptimaleLoesungFuerNichtStandartfall alg]

runTestsFuerAlgorithmus :: Algorithmus -> IO()
runTestsFuerAlgorithmus alg = do
  let tests = testsFuerAlgorithmus alg
  _ <- T.runTestTT tests
  return ()

algorithmusLiefertGueltigeLoesung :: Algorithmus -> [Muenze] -> NonNegative Integer -> Bool
algorithmusLiefertGueltigeLoesung alg muenzen (NonNegative betrag) = wgBetrag == Just betrag
  where wgBetrag = wechselgeldBetrag <$> alg muenzen betrag

-- * Main

main :: IO ()
main = do
    putStrLn "Teste bruteForce Algorithmus..(wird etwas dauern!)"
    runTestsFuerAlgorithmus bruteForce
    putStrLn "...done"
