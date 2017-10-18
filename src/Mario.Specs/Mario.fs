module Mario.Specs

open Expecto
open Player

[<Tests>]
let tests =
  testList "iteration 1" [
    testCase "starts small" <| fun _ ->
      Expect.equal player Small ""

    testCase "dies when hit" <| fun _ ->
      let dead = hit player
      Expect.equal dead Dead ""
  ]
