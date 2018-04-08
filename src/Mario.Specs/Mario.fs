module Mario.Specs

open Expecto
open Player

[<Tests>]
let iteration1 =
  testList "basic life" [
    testCase "starts small" <| fun _ ->
      Expect.equal player.size Small ""

    // testCase "dies when hit" <| fun _ ->
    //   let subject =
    //     player
    //     |> hit
    //   Expect.equal subject.hearts Dead ""
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

[<Tests>]
let iteration3 =
  testList "lifes" [
    testCase "starts with 3 lifes" <| fun _ ->
      Expect.equal player.hearts (Lifes 3) ""

    testCase "loses 1 life when dying" <| fun _ ->
      let subject =
        player
        |> hit
      Expect.equal subject.hearts (Lifes 2) ""

    testCase "dead after losing 3 lifes" <| fun _ ->
      let subject =
        player
        |> hit
        |> hit
        |> hit
      Expect.equal subject.hearts Dead ""
  ]

[<Tests>]
let iteration4 =
  testList "finding lifes" [
    testCase "finding lifes increments lifespan" <| fun _ ->
      let subject =
        player
        |> findLife
      Expect.equal subject.hearts (Lifes 4) ""

    testCase "dead Marios can be resurrected finding lifes" <| fun _ ->
      let subject =
        player
        |> hit
        |> hit
        |> hit
        |> findLife
      Expect.equal subject.hearts (Lifes 1) "does that even make sense?"
  ]

[<Tests>]
let iteration5 =
  testList "finding fire flowers" [
    testCase "finding a fire flower lets Mario grow" <| fun _ ->
      let subject =
        player
        |> findFireFlower
      Expect.equal subject.size Size.Large ""

    testCase "a grown Mario hit by an enemy shrinks Mario and yields a mushroom" <| fun _ ->
      let subject =
        player
        |> findFireFlower
        |> hit
      Expect.equal subject.size Size.Small ""
      Expect.equal subject.bag (Some Mushroom) ""

    testCase "a grown Mario finding a mushroom keeps him grown" <| fun _ ->
      let subject =
        player
        |> findFireFlower
        |> pickupMushroom
      Expect.equal subject.size Size.Large ""
      Expect.equal subject.bag None "This is underspecified"

    testCase "a Mario carrying a mushroom finding a fire flower drops the mushroom" <| fun _ ->
      let subject =
        player
        |> pickupMushroom
        |> findFireFlower
      Expect.equal subject.size Size.Large ""
      Expect.equal subject.bag None "This is underspecified"
  ]
