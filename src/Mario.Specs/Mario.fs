module Mario.Specs

open Expecto
open Player
open System

let hitForTesting player =
  hit DateTime.MaxValue player

[<Tests>]
let iteration1 =
  testList "basic life" [
    testCase "starts small" <| fun _ ->
      Expect.equal player.State Small ""

    // testCase "dies when hit" <| fun _ ->
    //   let subject =
    //     player
    //     |> hitForTesting
    //   Expect.equal subject.Hearts Dead ""
  ]

[<Tests>]
let iteration2 =
  testList "when picking up mushrooms" [
    testCase "carries mushroom" <| fun _ ->
      let subject =
        player
        |> pickupMushroom
      Expect.equal subject.Bag (Some Mushroom) ""

    testCase "carries maximum of one mushroom" <| fun _ ->
      let subject =
        player
        |> pickupMushroom
        |> pickupMushroom
      Expect.equal subject.Bag (Some Mushroom) ""

    testCase "loses mushroom and becomes small when hit" <| fun _ ->
      let subject =
        player
        |> pickupMushroom
        |> hitForTesting
      Expect.equal subject.Bag None ""
      Expect.equal subject.State Small ""
  ]

[<Tests>]
let iteration3 =
  testList "lives" [
    testCase "starts with 3 lives" <| fun _ ->
      Expect.equal player.Hearts (Lives 3) ""

    testCase "loses 1 life when dying" <| fun _ ->
      let subject =
        player
        |> hitForTesting
      Expect.equal subject.Hearts (Lives 2) ""

    testCase "dead after losing 3 lives" <| fun _ ->
      let subject =
        player
        |> hitForTesting
        |> hitForTesting
        |> hitForTesting
      Expect.equal subject.Hearts Dead ""
  ]

[<Tests>]
let iteration4 =
  testList "finding lives" [
    testCase "finding lives increments lifespan" <| fun _ ->
      let subject =
        player
        |> findLife
      Expect.equal subject.Hearts (Lives 4) ""

    testCase "dead Marios can be resurrected finding lives" <| fun _ ->
      let subject =
        player
        |> hitForTesting
        |> hitForTesting
        |> hitForTesting
        |> findLife
      Expect.equal subject.Hearts (Lives 1) "does that even make sense?"
  ]

[<Tests>]
let iteration5 =
  testList "finding fire flowers" [
    testCase "finding a fire flower lets Mario grow" <| fun _ ->
      let subject =
        player
        |> findFireFlower
      Expect.equal subject.State Large ""

    testCase "a grown Mario hit by an enemy shrinks Mario and yields a mushroom" <| fun _ ->
      let subject =
        player
        |> findFireFlower
        |> hitForTesting
      Expect.equal subject.State Small ""
      Expect.equal subject.Bag (Some Mushroom) ""

    testCase "a grown Mario finding a mushroom keeps him grown" <| fun _ ->
      let subject =
        player
        |> findFireFlower
        |> pickupMushroom
      Expect.equal subject.State Large ""
      Expect.equal subject.Bag (Some Mushroom) "This is underspecified"

    testCase "a Mario carrying a mushroom finding a fire flower drops the mushroom" <| fun _ ->
      let subject =
        player
        |> pickupMushroom
        |> findFireFlower
      Expect.equal subject.State Large ""
      Expect.equal subject.Bag None "This is underspecified"
  ]

let iteration6 = exn // Skipped.

[<Tests>]
let iteration7 =
  testList "god mode" [
    testCase "starts mortal" <| fun _ ->
      Expect.equal player.State Small ""

    testCase "finding a star makes Mario immortal" <| fun _ ->
      let pickupTime = DateTime(2018, 3, 8, 14, 0, 0)
      let immortalUntil = pickupTime.AddSeconds 2.
      let hitTime = pickupTime

      let subject =
        player
        |> findStar pickupTime
        |> hit hitTime
        |> hit hitTime
        |> hit hitTime
      Expect.equal subject.Hearts (Lives 3) ""

      match subject.State with
      | MaybeImmortal i ->
        Expect.equal i.Until immortalUntil ""
      | _ -> failtest "Should be immortal"

    testCase "after 2 seconds Mario becomes mortal again" <| fun _ ->
      let pickupTime = DateTime(2018, 3, 8, 14, 0, 0)
      let hitTime = pickupTime.AddSeconds 3.

      let subject =
        player
        |> findStar pickupTime
        |> hit hitTime
        |> hit hitTime
        |> hit hitTime
      Expect.equal subject.Hearts Dead ""
  ]
