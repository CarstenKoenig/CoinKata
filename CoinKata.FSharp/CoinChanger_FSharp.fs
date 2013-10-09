namespace CoinKata.FSharp

open System
open CoinKata

type Münze = int
type Cents = int
type AnzahlMünzePaar = int * Münze
type Wechselgeld = AnzahlMünzePaar list

type CoinChanger_FSharp() = 

    let ausgabeUndRestbetragFürMünze (bisher : Wechselgeld, restBetrag : Cents) 
                                     (aktuelleMünze : Münze) =
        let (anzahl, restBetrag') = Math.DivRem(restBetrag, aktuelleMünze)
        let neuerWechsel = (anzahl, aktuelleMünze) :: bisher
        (neuerWechsel, restBetrag')

    let berechneWechselgeld münzen betrag =
        let initial = ([], betrag)
        münzen |> Seq.fold ausgabeUndRestbetragFürMünze initial
               |> fst

    interface ICoinChanger with
        member i.BerechneWechselgeld(verfügbareMünzen, betrag) = 
            berechneWechselgeld verfügbareMünzen betrag
            |> List.toSeq