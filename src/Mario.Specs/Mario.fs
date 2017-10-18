module Mario.Specs

open Expecto
open Player

[<Tests>]
let iteration1 =
  testList "basic life" [
    testCase "starts small" <| fun _ ->
      Expect.equal player.size Small ""

    testCase "dies when hit" <| fun _ ->
      let subject =
        player
        |> hit
      Expect.equal subject.alive false ""
  ]

[<Tests>]
let iteration2 =
  testList "when picking up mushrooms" [
    testCase "carries mushroom" <| fun _ ->
      let subject =
        player
        |> pickupMushroom
      Expect.equal subject.bag (Some Mushroom) ""

    testCase "carries maximum of one mushroom" <| fun _ ->
      let subject =
        player
        |> pickupMushroom
        |> pickupMushroom
      Expect.equal subject.bag (Some Mushroom) ""

    testCase "loses mushroom and becomes small when hit" <| fun _ ->
      let subject =
        player
        |> pickupMushroom
        |> hit
      Expect.equal subject.bag None ""
      Expect.equal subject.size Small ""
  ]
      let subject = pickupMushroom player
      Expect.equal subject.bag (Some Mushroom) ""

    testCase "carries maximum of one mushroom" <| fun _ ->
      let subject =
        pickupMushroom player
        |> pickupMushroom
      Expect.equal subject.bag (Some Mushroom) ""

    testCase "loses mushroom and becomes small when hit" <| fun _ ->
      let subject =
        player
        |> pickupMushroom
        |> hit
      Expect.equal subject.bag None ""
      Expect.equal subject.size Small ""
  ]
